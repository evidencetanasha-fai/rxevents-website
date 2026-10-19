
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminWebSettings : System.Web.UI.Page
    {
        private readonly DatabaseHelper databaseHelper =
            new DatabaseHelper();

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
                LoadWebSettings();
            }
        }

        private void LoadWebSettings()
        {
            Dictionary<string, string> settings =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

            string query = @"
                SELECT SettingName, SettingValue
                FROM Settings;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name =
                            Convert.ToString(reader["SettingName"]);

                        string value =
                            reader["SettingValue"] == DBNull.Value
                            ? ""
                            : Convert.ToString(reader["SettingValue"]);

                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            settings[name] = value;
                        }
                    }
                }
            }

            WebsiteNameTextBox.Text =
                GetSetting(settings, "WebsiteName");

            CompanyEmailTextBox.Text =
                GetSetting(settings, "CompanyEmail");

            CompanyPhoneTextBox.Text =
                GetSetting(settings, "CompanyPhone");

            CompanyAddressTextBox.Text =
                GetSetting(settings, "CompanyAddress");

            WorkingHoursTextBox.Text =
                GetSetting(settings, "WorkingHours");

            FacebookTextBox.Text =
                GetSetting(settings, "Facebook");

            InstagramTextBox.Text =
                GetSetting(settings, "Instagram");

            XTextBox.Text = GetSetting(settings, "X");

            LinkedInTextBox.Text =
                GetSetting(settings, "LinkedIn");

            AboutTextBox.Text =
                GetSetting(settings, "AboutText");

            FooterTextBox.Text =
                GetSetting(settings, "FooterText");

            SetImagePreview(LogoPreview,
                GetSetting(settings, "LogoPath"));

            SetImagePreview(FaviconPreview,
                GetSetting(settings, "FaviconPath"));
        }

        private string GetSetting(
            Dictionary<string, string> settings,
            string name)
        {
            string value;

            return settings.TryGetValue(name, out value)
                ? value
                : "";
        }

        private void SetImagePreview(
            System.Web.UI.WebControls.Image image,
            string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                image.Visible = false;
                return;
            }

            image.ImageUrl = ResolveUrl(path);
            image.Visible = true;
        }

        protected void SaveWebSettingsButton_Click(
            object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                    WebsiteNameTextBox.Text))
            {
                WebSettingsMessage.Text =
                    "Website name is required.";
                return;
            }

            try
            {
                SaveSetting("WebsiteName",
                    WebsiteNameTextBox.Text.Trim());

                SaveSetting("CompanyEmail",
                    CompanyEmailTextBox.Text.Trim());

                SaveSetting("CompanyPhone",
                    CompanyPhoneTextBox.Text.Trim());

                SaveSetting("CompanyAddress",
                    CompanyAddressTextBox.Text.Trim());

                SaveSetting("WorkingHours",
                    WorkingHoursTextBox.Text.Trim());

                SaveSetting("Facebook",
                    FacebookTextBox.Text.Trim());

                SaveSetting("Instagram",
                    InstagramTextBox.Text.Trim());

                SaveSetting("X",
                    XTextBox.Text.Trim());

                SaveSetting("LinkedIn",
                    LinkedInTextBox.Text.Trim());

                SaveSetting("AboutText",
                    AboutTextBox.Text.Trim());

                SaveSetting("FooterText",
                    FooterTextBox.Text.Trim());

                string logoPath = SaveUploadedImage(
                    LogoUpload, "logos");

                if (logoPath != null)
                {
                    SaveSetting("LogoPath", logoPath);
                }

                string faviconPath = SaveUploadedImage(
                    FaviconUpload, "favicons");

                if (faviconPath != null)
                {
                    SaveSetting("FaviconPath", faviconPath);
                }

                WebSettingsMessage.Text =
                    "Website settings saved successfully.";

                LoadWebSettings();
            }
            catch (Exception)
            {
                WebSettingsMessage.Text =
                    "Unable to save settings. Check the database and uploaded files, then try again.";
            }
        }

        private void SaveSetting(string name, string value)
        {
            string query = @"
                IF EXISTS (
                    SELECT 1 FROM Settings
                    WHERE SettingName = @Name
                )
                BEGIN
                    UPDATE Settings
                    SET SettingValue = @Value,
                        UpdatedDate = GETDATE()
                    WHERE SettingName = @Name;
                END
                ELSE
                BEGIN
                    INSERT INTO Settings
                        (SettingName, SettingValue,
                         Description, UpdatedDate)
                    VALUES
                        (@Name, @Value,
                         'Website configuration', GETDATE());
                END";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Name",
                    SqlDbType.VarChar, 100).Value = name;

                command.Parameters.Add("@Value",
                    SqlDbType.VarChar, -1).Value =
                    (object)value ?? DBNull.Value;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private string SaveUploadedImage(
            System.Web.UI.WebControls.FileUpload upload,
            string subfolder)
        {
            if (upload == null || !upload.HasFile)
            {
                return null;
            }

            string extension =
                Path.GetExtension(upload.FileName).ToLowerInvariant();

            string[] allowedExtensions =
                { ".jpg", ".jpeg", ".png", ".webp", ".ico" };

            if (Array.IndexOf(allowedExtensions, extension) < 0)
            {
                throw new InvalidOperationException(
                    "Unsupported image file type.");
            }

            if (upload.PostedFile.ContentLength > 2 * 1024 * 1024)
            {
                throw new InvalidOperationException(
                    "Image must not exceed 2 MB.");
            }

            string folder = Server.MapPath(
                "~/Content/images/" + subfolder + "/");

            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString("N")
                + extension;

            string fullPath = Path.Combine(folder, fileName);

            upload.SaveAs(fullPath);

            return "~/Content/images/" + subfolder
                + "/" + fileName;
        }
    }
}
