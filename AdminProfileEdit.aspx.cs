
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminProfileEdit : Page
    {
        private readonly DatabaseHelper databaseHelper =
            new DatabaseHelper();

        private int CurrentUserId
        {
            get
            {
                return Convert.ToInt32(Session["UserID"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToString(Session["RoleID"]) != "1")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            string query = @"
                SELECT FirstName, LastName, Email, Phone, ProfilePhoto
                FROM Users
                WHERE UserID = @UserID AND RoleID = 1;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int)
                    .Value = CurrentUserId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Response.Redirect("~/Login.aspx");
                        return;
                    }

                    FirstNameTextBox.Text =
                        reader["FirstName"].ToString();

                    LastNameTextBox.Text =
                        reader["LastName"].ToString();

                    EmailTextBox.Text =
                        reader["Email"].ToString();

                    PhoneTextBox.Text =
                        reader["Phone"] == DBNull.Value
                        ? ""
                        : reader["Phone"].ToString();

                    string photo = reader["ProfilePhoto"] == DBNull.Value
                        ? ""
                        : reader["ProfilePhoto"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(photo))
                    {
                        CurrentProfilePhoto.ImageUrl =
                            ResolveUrl(GetPhotoUrl(photo));

                        CurrentProfilePhoto.Visible = true;
                    }
                }
            }
        }

        protected void SaveProfileButton_Click(
            object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email))
            {
                ShowMessage("Please complete all required fields.", false);
                return;
            }

            string uploadedPhotoPath = null;

            if (ProfilePhotoUpload.HasFile)
            {
                string extension = Path.GetExtension(
                    ProfilePhotoUpload.FileName).ToLowerInvariant();

                string[] allowedExtensions =
                    { ".jpg", ".jpeg", ".png", ".webp" };

                if (Array.IndexOf(allowedExtensions, extension) < 0)
                {
                    ShowMessage(
                        "Upload a JPG, JPEG, PNG, or WEBP image.", false);
                    return;
                }

                if (ProfilePhotoUpload.PostedFile.ContentLength > 2 * 1024 * 1024)
                {
                    ShowMessage(
                        "The profile photo must be 2 MB or smaller.", false);
                    return;
                }

                if (ProfilePhotoUpload.PostedFile.ContentLength == 0)
                {
                    ShowMessage("The selected image is empty.", false);
                    return;
                }

                string folder = Server.MapPath(
                    "~/Content/images/profiles/");

                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString("N") + extension;
                string fullPath = Path.Combine(folder, fileName);

                try
                {
                    ProfilePhotoUpload.SaveAs(fullPath);
                    uploadedPhotoPath =
                        "Content/images/profiles/" + fileName;
                }
                catch
                {
                    ShowMessage(
                        "The profile photo could not be saved. Check folder permissions.",
                        false);
                    return;
                }
            }

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            {
                connection.Open();

                // Ensure another account does not already use this email.
                string emailCheck = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Email = @Email AND UserID <> @UserID;";

                using (SqlCommand checkCommand =
                       new SqlCommand(emailCheck, connection))
                {
                    checkCommand.Parameters.Add("@Email", SqlDbType.VarChar, 255)
                        .Value = email;

                    checkCommand.Parameters.Add("@UserID", SqlDbType.Int)
                        .Value = CurrentUserId;

                    if (Convert.ToInt32(checkCommand.ExecuteScalar()) > 0)
                    {
                        ShowMessage(
                            "That email address is already in use.", false);
                        return;
                    }
                }

                string updateQuery;

                if (uploadedPhotoPath == null)
                {
                    updateQuery = @"
                        UPDATE Users
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email,
                            Phone = @Phone
                        WHERE UserID = @UserID AND RoleID = 1;";
                }
                else
                {
                    updateQuery = @"
                        UPDATE Users
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email,
                            Phone = @Phone,
                            ProfilePhoto = @ProfilePhoto
                        WHERE UserID = @UserID AND RoleID = 1;";
                }

                using (SqlCommand updateCommand =
                       new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.Add("@FirstName",
                        SqlDbType.VarChar, 100).Value = firstName;

                    updateCommand.Parameters.Add("@LastName",
                        SqlDbType.VarChar, 100).Value = lastName;

                    updateCommand.Parameters.Add("@Email",
                        SqlDbType.VarChar, 255).Value = email;

                    updateCommand.Parameters.Add("@Phone",
                        SqlDbType.VarChar, 30).Value =
                            string.IsNullOrWhiteSpace(phone)
                            ? (object)DBNull.Value
                            : phone;

                    updateCommand.Parameters.Add("@UserID",
                        SqlDbType.Int).Value = CurrentUserId;

                    if (uploadedPhotoPath != null)
                    {
                        updateCommand.Parameters.Add("@ProfilePhoto",
                            SqlDbType.VarChar, 255).Value =
                                uploadedPhotoPath;
                    }

                    int rowsUpdated = updateCommand.ExecuteNonQuery();

                    if (rowsUpdated == 0)
                    {
                        ShowMessage(
                            "No administrator account was updated.", false);
                        return;
                    }
                }
            }

            Session["FirstName"] = firstName;
            Session["LastName"] = lastName;

            Response.Redirect("~/AdminProfile.aspx");
        }

        private string GetPhotoUrl(string photo)
        {
            if (photo.StartsWith("~/"))
                return photo;

            if (photo.StartsWith("/"))
                return "~" + photo;

            return "~/" + photo.TrimStart('/');
        }

        private void ShowMessage(string message, bool success)
        {
            MessageLabel.Text = Server.HtmlEncode(message);
            MessageLabel.ForeColor = success
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }
    }
}
