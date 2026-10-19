
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminAddEvent : System.Web.UI.Page
    {
        private readonly DatabaseHelper databaseHelper =
            new DatabaseHelper();

        private const string UploadFolder = "~/Content/images/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 1)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCategories();

                string eventIdValue =
                    Request.QueryString["EventID"];

                if (!string.IsNullOrWhiteSpace(eventIdValue))
                {
                    int eventID;

                    if (int.TryParse(eventIdValue, out eventID))
                    {
                        LoadEvent(eventID);
                    }
                    else
                    {
                        EventMessage.Text = "Invalid event ID.";
                        SaveEventButton.Enabled = false;
                    }
                }
            }
        }

        // ------------------------------------------
        // LOAD ACTIVE CATEGORIES
        // ------------------------------------------
        private void LoadCategories()
        {
            string query = @"
                SELECT CategoryID, CategoryName
                FROM EventCategories
                WHERE IsActive = 1
                ORDER BY CategoryName;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                DataTable categories = new DataTable();
                adapter.Fill(categories);

                CategoryDropdown.DataSource = categories;
                CategoryDropdown.DataTextField = "CategoryName";
                CategoryDropdown.DataValueField = "CategoryID";
                CategoryDropdown.DataBind();
            }

            CategoryDropdown.Items.Insert(
                0, new System.Web.UI.WebControls.ListItem(
                    "-- Select Category --", ""));
        }

        // ------------------------------------------
        // LOAD AN EXISTING EVENT FOR EDITING
        // ------------------------------------------
        private void LoadEvent(int eventID)
        {
            string query = @"
                SELECT EventID, CategoryID, EventName,
                       Description, EventImage, BasePrice, IsActive
                FROM Events
                WHERE EventID = @EventID;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@EventID", SqlDbType.Int).Value = eventID;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        EventMessage.Text = "Event not found.";
                        SaveEventButton.Enabled = false;
                        return;
                    }

                    EventNameInput.Text =
                        Convert.ToString(reader["EventName"]);

                    DescriptionInput.Text =
                        reader["Description"] == DBNull.Value
                            ? ""
                            : Convert.ToString(reader["Description"]);

                    BasePriceInput.Text =
                        Convert.ToDecimal(reader["BasePrice"])
                            .ToString(
                                System.Globalization.CultureInfo.InvariantCulture);

                    string categoryID =
                        Convert.ToString(reader["CategoryID"]);

                    if (CategoryDropdown.Items.FindByValue(categoryID) != null)
                    {
                        CategoryDropdown.SelectedValue = categoryID;
                    }

                    EventStatusDropdown.SelectedValue =
                        Convert.ToBoolean(reader["IsActive"])
                            ? "1" : "0";

                    PageHeadingLiteral.Text = "Edit Event";
                    SaveEventButton.Text = "Save Changes";
                }
            }
        }

        // ------------------------------------------
        // CREATE OR UPDATE EVENT
        // ------------------------------------------
        protected void SaveEventButton_Click(
            object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            string eventName = EventNameInput.Text.Trim();
            string description = DescriptionInput.Text.Trim();

            int categoryID;
            decimal basePrice;

            if (!int.TryParse(
                    CategoryDropdown.SelectedValue, out categoryID))
            {
                EventMessage.Text = "Please select an event category.";
                return;
            }

            if (!decimal.TryParse(
                    BasePriceInput.Text.Trim(),
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out basePrice) || basePrice < 0)
            {
                EventMessage.Text =
                    "Enter a valid non-negative base price.";
                return;
            }

            bool isActive =
                EventStatusDropdown.SelectedValue == "1";

            string existingImage = null;
            int eventID = 0;
            bool isEditing = int.TryParse(
                Request.QueryString["EventID"], out eventID);

            if (isEditing)
            {
                existingImage = GetExistingImage(eventID);

                if (existingImage == null)
                {
                    EventMessage.Text = "The event could not be found.";
                    return;
                }
            }

            string savedImagePath = existingImage;

            try
            {
                if (EventImageInput.HasFile)
                {
                    string extension = Path.GetExtension(
                        EventImageInput.FileName).ToLowerInvariant();

                    string[] allowedExtensions =
                    {
                        ".jpg", ".jpeg", ".png", ".webp"
                    };

                    if (Array.IndexOf(
                        allowedExtensions, extension) < 0)
                    {
                        EventMessage.Text =
                            "Upload a JPG, JPEG, PNG, or WEBP image.";
                        return;
                    }

                    if (EventImageInput.PostedFile.ContentLength >
                        5 * 1024 * 1024)
                    {
                        EventMessage.Text =
                            "The image must be 5 MB or smaller.";
                        return;
                    }

                    string folderPath =
                        Server.MapPath(UploadFolder);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName =
                        Guid.NewGuid().ToString("N") + extension;

                    EventImageInput.SaveAs(
                        Path.Combine(folderPath, fileName));

                    savedImagePath =
                        "Content/images/" + fileName;
                }

                if (isEditing)
                {
                    UpdateEvent(
                        eventID, categoryID, eventName,
                        description, savedImagePath,
                        basePrice, isActive);
                }
                else
                {
                    InsertEvent(
                        categoryID, eventName, description,
                        savedImagePath, basePrice, isActive);
                }

                Response.Redirect(
                    ResolveUrl("~/AdminEvents.aspx"), false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                EventMessage.Text =
                    "The event could not be saved. Check the database connection, " +
                    "category selection, and image folder permissions.";
            }
        }

        // ------------------------------------------
        // GET EXISTING IMAGE PATH
        // ------------------------------------------
        private string GetExistingImage(int eventID)
        {
            string query = @"
                SELECT EventImage
                FROM Events
                WHERE EventID = @EventID;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@EventID", SqlDbType.Int).Value = eventID;

                connection.Open();

                object result = command.ExecuteScalar();

                if (result == null)
                    return null;

                return result == DBNull.Value
                    ? ""
                    : Convert.ToString(result);
            }
        }

        // ------------------------------------------
        // INSERT NEW EVENT
        // ------------------------------------------
        private void InsertEvent(
            int categoryID,
            string eventName,
            string description,
            string imagePath,
            decimal basePrice,
            bool isActive)
        {
            string query = @"
                INSERT INTO Events
                    (CategoryID, EventName, Description,
                     EventImage, BasePrice, IsActive, DateAdded)
                VALUES
                    (@CategoryID, @EventName, @Description,
                     @EventImage, @BasePrice, @IsActive, GETDATE());";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                AddEventParameters(
                    command, categoryID, eventName, description,
                    imagePath, basePrice, isActive);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // ------------------------------------------
        // UPDATE EXISTING EVENT
        // ------------------------------------------
        private void UpdateEvent(
            int eventID,
            int categoryID,
            string eventName,
            string description,
            string imagePath,
            decimal basePrice,
            bool isActive)
        {
            string query = @"
                UPDATE Events
                SET CategoryID = @CategoryID,
                    EventName = @EventName,
                    Description = @Description,
                    EventImage = @EventImage,
                    BasePrice = @BasePrice,
                    IsActive = @IsActive
                WHERE EventID = @EventID;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                AddEventParameters(
                    command, categoryID, eventName, description,
                    imagePath, basePrice, isActive);

                command.Parameters.Add(
                    "@EventID", SqlDbType.Int).Value = eventID;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // ------------------------------------------
        // COMMON SQL PARAMETERS
        // ------------------------------------------
        private void AddEventParameters(
            SqlCommand command,
            int categoryID,
            string eventName,
            string description,
            string imagePath,
            decimal basePrice,
            bool isActive)
        {
            command.Parameters.Add(
                "@CategoryID", SqlDbType.Int).Value = categoryID;

            command.Parameters.Add(
                "@EventName", SqlDbType.VarChar, 200).Value = eventName;

            command.Parameters.Add(
                "@Description", SqlDbType.VarChar, -1).Value =
                string.IsNullOrWhiteSpace(description)
                    ? (object)DBNull.Value
                    : description;

            command.Parameters.Add(
                "@EventImage", SqlDbType.VarChar, 500).Value =
                string.IsNullOrWhiteSpace(imagePath)
                    ? (object)DBNull.Value
                    : imagePath;

            command.Parameters.Add(
                "@BasePrice", SqlDbType.Decimal).Value = basePrice;

            command.Parameters["@BasePrice"].Precision = 12;
            command.Parameters["@BasePrice"].Scale = 2;

            command.Parameters.Add(
                "@IsActive", SqlDbType.Bit).Value = isActive;
        }
    }
}
