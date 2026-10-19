using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminAddGallery : Page
    {
        private const int MaxFileSize = 5 * 1024 * 1024;

        private int GalleryID
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["GalleryID"], out id)
                    ? id : 0;
            }
        }

        private bool IsEditMode
        {
            get { return GalleryID > 0; }
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
                LoadEvents();

                if (IsEditMode)
                {
                    PageHeadingLiteral.Text = "Edit Gallery Image";
                    SaveGalleryButton.Text = "Update Image";
                    GalleryImageValidator.ErrorMessage =
                        "Please choose a replacement image, or leave the upload empty to keep the current image.";

                    LoadGalleryDetails();
                }
                else
                {
                    GalleryStatusDropdown.SelectedValue = "1";
                }
            }
        }

        private void LoadEvents()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT EventID, EventName
                    FROM dbo.Events
                    ORDER BY EventName";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        GalleryEventDropdown.DataSource = reader;
                        GalleryEventDropdown.DataTextField = "EventName";
                        GalleryEventDropdown.DataValueField = "EventID";
                        GalleryEventDropdown.DataBind();
                    }
                }
            }

            GalleryEventDropdown.Items.Insert(
                0, new System.Web.UI.WebControls.ListItem(
                    "Select an event", ""));
        }

        private void LoadGalleryDetails()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT EventID, Caption, ImagePath, IsActive
                    FROM dbo.Gallery
                    WHERE GalleryID = @GalleryID";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@GalleryID", SqlDbType.Int)
                        .Value = GalleryID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowMessage("Gallery image not found.", false);
                            SaveGalleryButton.Enabled = false;
                            return;
                        }

                        GalleryEventDropdown.SelectedValue =
                            Convert.ToString(reader["EventID"]);

                        GalleryCaptionInput.Text =
                            reader["Caption"] == DBNull.Value
                                ? ""
                                : Convert.ToString(reader["Caption"]);

                        GalleryStatusDropdown.SelectedValue =
                            Convert.ToBoolean(reader["IsActive"])
                                ? "1" : "0";
                    }
                }
            }
        }

        protected void GalleryImageValidator_ServerValidate(
            object source,
            System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (!GalleryImageUpload.HasFile)
            {
                // New records require an image. Existing records can keep
                // their current image without uploading a replacement.
                args.IsValid = IsEditMode;
                return;
            }

            string extension = Path.GetExtension(
                GalleryImageUpload.FileName).ToLowerInvariant();

            bool validExtension =
                extension == ".jpg" ||
                extension == ".jpeg" ||
                extension == ".png" ||
                extension == ".webp";

            bool validSize =
                GalleryImageUpload.PostedFile.ContentLength > 0 &&
                GalleryImageUpload.PostedFile.ContentLength <= MaxFileSize;

            bool validContentType =
                GalleryImageUpload.PostedFile.ContentType != null &&
                GalleryImageUpload.PostedFile.ContentType.StartsWith(
                    "image/", StringComparison.OrdinalIgnoreCase);

            args.IsValid = validExtension && validSize && validContentType;

            if (!validExtension)
            {
                GalleryImageValidator.ErrorMessage =
                    "Only JPG, JPEG, PNG, and WEBP images are allowed.";
            }
            else if (!validSize)
            {
                GalleryImageValidator.ErrorMessage =
                    "The image must be larger than 0 bytes and no bigger than 5 MB.";
            }
            else if (!validContentType)
            {
                GalleryImageValidator.ErrorMessage =
                    "The uploaded file must be an image.";
            }
        }

        protected void SaveGalleryButton_Click(object sender, EventArgs e)
        {
            Page.Validate("GalleryForm");

            if (!Page.IsValid)
                return;

            int eventID;

            if (!int.TryParse(GalleryEventDropdown.SelectedValue, out eventID))
            {
                ShowMessage("Please select a valid event.", false);
                return;
            }

            string caption = GalleryCaptionInput.Text.Trim();

            if (caption.Length > 255)
            {
                ShowMessage("Caption cannot exceed 255 characters.", false);
                return;
            }

            string imagePath = null;
            string physicalPath = null;
            bool newFileSaved = false;

            try
            {
                // For edits without a new upload, retain the current path.
                if (IsEditMode && !GalleryImageUpload.HasFile)
                {
                    imagePath = GetCurrentImagePath();
                }

                if (GalleryImageUpload.HasFile)
                {
                    string extension = Path.GetExtension(
                        GalleryImageUpload.FileName).ToLowerInvariant();

                    string fileName = Guid.NewGuid().ToString("N") + extension;

                    string folderPath = Server.MapPath("~/Content/images/");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    physicalPath = Path.Combine(folderPath, fileName);

                    GalleryImageUpload.SaveAs(physicalPath);
                    newFileSaved = true;

                    imagePath = "Content/images/" + fileName;
                }

                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    ShowMessage("Please select an image to upload.", false);
                    return;
                }

                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    connection.Open();

                    string query = IsEditMode
                        ? @"
                            UPDATE dbo.Gallery
                            SET EventID = @EventID,
                                ImagePath = @ImagePath,
                                Caption = @Caption,
                                IsActive = @IsActive
                            WHERE GalleryID = @GalleryID"
                        : @"
                            INSERT INTO dbo.Gallery
                                (EventID, ImagePath, Caption, IsActive)
                            VALUES
                                (@EventID, @ImagePath, @Caption, @IsActive)";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@EventID", SqlDbType.Int)
                            .Value = eventID;

                        command.Parameters.Add(
                            "@ImagePath", SqlDbType.VarChar, 255)
                            .Value = imagePath;

                        command.Parameters.Add(
                            "@Caption", SqlDbType.VarChar, 255)
                            .Value = string.IsNullOrWhiteSpace(caption)
                                ? (object)DBNull.Value : caption;

                        command.Parameters.Add("@IsActive", SqlDbType.Bit)
                            .Value = GalleryStatusDropdown.SelectedValue == "1";

                        if (IsEditMode)
                        {
                            command.Parameters.Add("@GalleryID", SqlDbType.Int)
                                .Value = GalleryID;
                        }

                        int affected = command.ExecuteNonQuery();

                        if (IsEditMode && affected == 0)
                        {
                            if (newFileSaved && File.Exists(physicalPath))
                                File.Delete(physicalPath);

                            ShowMessage("Gallery image not found.", false);
                            return;
                        }
                    }
                }

                Response.Redirect("~/AdminGallery.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (SqlException)
            {
                if (newFileSaved && File.Exists(physicalPath))
                    File.Delete(physicalPath);

                ShowMessage(
                    "Unable to save the gallery image. Check the database connection and selected event.",
                    false);
            }
            catch (IOException)
            {
                ShowMessage(
                    "The image could not be saved. Check that the images folder is writable.",
                    false);
            }
            catch (Exception)
            {
                if (newFileSaved && File.Exists(physicalPath))
                    File.Delete(physicalPath);

                ShowMessage(
                    "An error occurred while saving the gallery image.",
                    false);
            }
        }

        private string GetCurrentImagePath()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT ImagePath
                    FROM dbo.Gallery
                    WHERE GalleryID = @GalleryID";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@GalleryID", SqlDbType.Int)
                        .Value = GalleryID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    return result == null || result == DBNull.Value
                        ? null
                        : Convert.ToString(result);
                }
            }
        }

        private void ShowMessage(string message, bool success)
        {
            GalleryFormMessage.Text = Server.HtmlEncode(message);

            GalleryFormMessage.ForeColor = success
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.Firebrick;
        }
    }
}