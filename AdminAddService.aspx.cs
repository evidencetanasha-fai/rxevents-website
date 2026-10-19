using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminAddService : Page
    {
        private int ServiceID
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["ServiceID"], out id)
                    ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int roleID;

            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                !int.TryParse(Convert.ToString(Session["RoleID"]), out roleID) ||
                roleID != 1)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (ServiceID > 0)
                {
                    PageHeadingLiteral.Text = "Edit Service";
                    SaveServiceButton.Text = "Update Service";
                    LoadServiceDetails();
                }
                else
                {
                    ServiceStatusDropdown.SelectedValue = "1";
                }
            }
        }

        private void LoadServiceDetails()
        {
            try
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    const string query = @"
                        SELECT ServiceName, Category, Description,
                               Price, ImagePath, IsActive
                        FROM dbo.Services
                        WHERE ServiceID = @ServiceID";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@ServiceID", SqlDbType.Int)
                            .Value = ServiceID;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                ShowMessage("Service not found.", false);
                                SaveServiceButton.Enabled = false;
                                return;
                            }

                            ServiceNameInput.Text =
                                Convert.ToString(reader["ServiceName"]);

                            string category = reader["Category"] == DBNull.Value
                                ? ""
                                : Convert.ToString(reader["Category"]);

                            // Preserve existing categories not listed in the dropdown.
                            if (!string.IsNullOrWhiteSpace(category) &&
                                ServiceCategoryDropdown.Items.FindByValue(category) == null)
                            {
                                ServiceCategoryDropdown.Items.Add(
                                    new System.Web.UI.WebControls.ListItem(
                                        category, category));
                            }

                            ServiceCategoryDropdown.SelectedValue = category;

                            ServiceDescriptionInput.Text =
                                reader["Description"] == DBNull.Value
                                    ? ""
                                    : Convert.ToString(reader["Description"]);

                            ServicePriceInput.Text =
                                Convert.ToDecimal(reader["Price"])
                                    .ToString("0.00", CultureInfo.InvariantCulture);

                            ServiceStatusDropdown.SelectedValue =
                                Convert.ToBoolean(reader["IsActive"]) ? "1" : "0";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ShowMessage(
                    "Unable to load service details. Please check the database connection.",
                    false);
            }
        }

        protected void SaveServiceButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string serviceName = ServiceNameInput.Text.Trim();
            string category = ServiceCategoryDropdown.SelectedValue;
            string description = ServiceDescriptionInput.Text.Trim();
            bool isActive = ServiceStatusDropdown.SelectedValue == "1";

            decimal price;

            if (!decimal.TryParse(
                    ServicePriceInput.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out price) ||
                price < 0 ||
                price > 9999999999.99m)
            {
                ShowMessage("Please enter a valid non-negative price.", false);
                return;
            }

            if (description.Length > 500)
            {
                ShowMessage(
                    "The description cannot exceed 500 characters.",
                    false);
                return;
            }

            string newImagePath = null;
            string uploadedFilePath = null;

            try
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    connection.Open();

                    // ServiceName is unique in the database.
                    const string duplicateQuery = @"
                        SELECT COUNT(*)
                        FROM dbo.Services
                        WHERE ServiceName = @ServiceName
                          AND ServiceID <> @ServiceID";

                    using (SqlCommand duplicateCommand =
                        new SqlCommand(duplicateQuery, connection))
                    {
                        duplicateCommand.Parameters.Add(
                            "@ServiceName", SqlDbType.VarChar, 150)
                            .Value = serviceName;

                        duplicateCommand.Parameters.Add(
                            "@ServiceID", SqlDbType.Int)
                            .Value = ServiceID;

                        if (Convert.ToInt32(
                            duplicateCommand.ExecuteScalar()) > 0)
                        {
                            ShowMessage(
                                "A service with this name already exists.",
                                false);
                            return;
                        }
                    }

                    string existingImagePath = null;

                    if (ServiceID > 0)
                    {
                        const string imageQuery = @"
                            SELECT ImagePath
                            FROM dbo.Services
                            WHERE ServiceID = @ServiceID";

                        using (SqlCommand imageCommand =
                            new SqlCommand(imageQuery, connection))
                        {
                            imageCommand.Parameters.Add(
                                "@ServiceID", SqlDbType.Int).Value = ServiceID;

                            object result = imageCommand.ExecuteScalar();

                            if (result == null)
                            {
                                ShowMessage("Service not found.", false);
                                return;
                            }

                            if (result != DBNull.Value)
                            {
                                existingImagePath = Convert.ToString(result);
                            }
                        }
                    }

                    newImagePath = existingImagePath;

                    if (ServiceImageInput.HasFile)
                    {
                        string extension = Path.GetExtension(
                            ServiceImageInput.FileName).ToLowerInvariant();

                        string[] allowedExtensions =
                        {
                            ".jpg", ".jpeg", ".png", ".webp"
                        };

                        if (Array.IndexOf(allowedExtensions, extension) < 0)
                        {
                            ShowMessage(
                                "Please upload a JPG, JPEG, PNG, or WEBP image.",
                                false);
                            return;
                        }

                        const int maxFileSize = 5 * 1024 * 1024;

                        if (ServiceImageInput.PostedFile.ContentLength >
                            maxFileSize)
                        {
                            ShowMessage(
                                "The image must be 5 MB or smaller.",
                                false);
                            return;
                        }

                        string imageFolder =
                            Server.MapPath("~/Content/images/");

                        if (!Directory.Exists(imageFolder))
                        {
                            Directory.CreateDirectory(imageFolder);
                        }

                        string fileName =
                            Guid.NewGuid().ToString("N") + extension;

                        uploadedFilePath = Path.Combine(imageFolder, fileName);

                        ServiceImageInput.SaveAs(uploadedFilePath);

                        newImagePath = "Content/images/" + fileName;
                    }

                    if (ServiceID > 0)
                    {
                        const string updateQuery = @"
                            UPDATE dbo.Services
                            SET ServiceName = @ServiceName,
                                Category = @Category,
                                Description = @Description,
                                Price = @Price,
                                ImagePath = @ImagePath,
                                IsActive = @IsActive
                            WHERE ServiceID = @ServiceID";

                        using (SqlCommand command =
                            new SqlCommand(updateQuery, connection))
                        {
                            AddServiceParameters(
                                command, serviceName, category,
                                description, price, newImagePath, isActive);

                            command.Parameters.Add(
                                "@ServiceID", SqlDbType.Int).Value = ServiceID;

                            if (command.ExecuteNonQuery() == 0)
                            {
                                ShowMessage("Service not found.", false);
                                return;
                            }
                        }
                    }
                    else
                    {
                        const string insertQuery = @"
                            INSERT INTO dbo.Services
                                (ServiceName, Category, Description,
                                 Price, ImagePath, IsActive)
                            VALUES
                                (@ServiceName, @Category, @Description,
                                 @Price, @ImagePath, @IsActive)";

                        using (SqlCommand command =
                            new SqlCommand(insertQuery, connection))
                        {
                            AddServiceParameters(
                                command, serviceName, category,
                                description, price, newImagePath, isActive);

                            command.ExecuteNonQuery();
                        }
                    }
                }

                Response.Redirect("~/AdminServices.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (SqlException ex)
            {
                // Remove the newly uploaded file if saving failed.
                DeleteUploadedFile(uploadedFilePath);

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    ShowMessage(
                        "A service with this name already exists.", false);
                }
                else
                {
                    ShowMessage(
                        "Unable to save the service. Please check the database and try again.",
                        false);
                }
            }
            catch (Exception)
            {
                DeleteUploadedFile(uploadedFilePath);

                ShowMessage(
                    "An error occurred while saving the service. Please try again.",
                    false);
            }
        }

        private void AddServiceParameters(
            SqlCommand command,
            string serviceName,
            string category,
            string description,
            decimal price,
            string imagePath,
            bool isActive)
        {
            command.Parameters.Add(
                "@ServiceName", SqlDbType.VarChar, 150)
                .Value = serviceName;

            command.Parameters.Add(
                "@Category", SqlDbType.VarChar, 100)
                .Value = string.IsNullOrWhiteSpace(category)
                    ? (object)DBNull.Value : category;

            command.Parameters.Add(
                "@Description", SqlDbType.VarChar, 500)
                .Value = string.IsNullOrWhiteSpace(description)
                    ? (object)DBNull.Value : description;

            SqlParameter priceParameter =
                command.Parameters.Add("@Price", SqlDbType.Decimal);

            priceParameter.Precision = 12;
            priceParameter.Scale = 2;
            priceParameter.Value = price;

            command.Parameters.Add(
                "@ImagePath", SqlDbType.VarChar, 255)
                .Value = string.IsNullOrWhiteSpace(imagePath)
                    ? (object)DBNull.Value : imagePath;

            command.Parameters.Add(
                "@IsActive", SqlDbType.Bit).Value = isActive;
        }

        private void DeleteUploadedFile(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) &&
                File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (IOException)
                {
                    // Do not hide the original database error.
                }
                catch (UnauthorizedAccessException)
                {
                    // Do not hide the original database error.
                }
            }
        }

        private void ShowMessage(string message, bool success)
        {
            ServiceMessage.Text = Server.HtmlEncode(message);
            ServiceMessage.ForeColor = success
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.Firebrick;
        }
    }
}