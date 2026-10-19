using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminAddCustomer : Page
    {
        private int CustomerID
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["UserID"], out id)
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
                if (CustomerID > 0)
                {
                    PageHeadingLiteral.Text = "Edit Customer";
                    SaveCustomerButton.Text = "Update Customer";

                    PasswordPanel.Visible = false;
                    PasswordRequiredValidator.Enabled = false;
                    PasswordStrengthValidator.Enabled = false;

                    LoadCustomerDetails();
                }
                else
                {
                    CustomerStatusDropdown.SelectedValue = "1";
                }
            }
        }

        private int GetCustomerRoleID(SqlConnection connection)
        {
            const string query = @"
                SELECT TOP 1 RoleID
                FROM dbo.Roles
                WHERE RoleName = @RoleName";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@RoleName", SqlDbType.VarChar, 50)
                    .Value = "Customer";

                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "The Customer role was not found in dbo.Roles.");
                }

                return Convert.ToInt32(result);
            }
        }

        private void LoadCustomerDetails()
        {
            try
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    connection.Open();

                    int customerRoleID = GetCustomerRoleID(connection);

                    const string query = @"
                        SELECT FirstName, LastName, Email, Phone, IsActive
                        FROM dbo.Users
                        WHERE UserID = @UserID AND RoleID = @RoleID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@UserID", SqlDbType.Int)
                            .Value = CustomerID;

                        command.Parameters.Add("@RoleID", SqlDbType.Int)
                            .Value = customerRoleID;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                ShowMessage("Customer not found.", false);
                                SaveCustomerButton.Enabled = false;
                                return;
                            }

                            FirstNameInput.Text =
                                Convert.ToString(reader["FirstName"]);

                            LastNameInput.Text =
                                Convert.ToString(reader["LastName"]);

                            EmailInput.Text =
                                Convert.ToString(reader["Email"]);

                            PhoneInput.Text =
                                reader["Phone"] == DBNull.Value
                                    ? ""
                                    : Convert.ToString(reader["Phone"]);

                            CustomerStatusDropdown.SelectedValue =
                                Convert.ToBoolean(reader["IsActive"])
                                    ? "1" : "0";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ShowMessage(
                    "Unable to load customer details. Please check the database connection.",
                    false);
            }
        }

        protected void SaveCustomerButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string firstName = FirstNameInput.Text.Trim();
            string lastName = LastNameInput.Text.Trim();
            string email = EmailInput.Text.Trim();
            string phone = PhoneInput.Text.Trim();
            bool isActive = CustomerStatusDropdown.SelectedValue == "1";

            string password = PasswordInput.Text;
            string newImagePath = null;
            string uploadedFilePath = null;

            if (firstName.Length > 50 || lastName.Length > 50 ||
                email.Length > 100 || phone.Length > 20)
            {
                ShowMessage("One or more fields exceed the allowed length.", false);
                return;
            }

            if (CustomerID == 0 && string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("A temporary password is required.", false);
                return;
            }

            try
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    connection.Open();

                    int customerRoleID = GetCustomerRoleID(connection);

                    // Check that the email is not already used by another account.
                    const string duplicateQuery = @"
                        SELECT COUNT(*)
                        FROM dbo.Users
                        WHERE Email = @Email AND UserID <> @UserID";

                    using (SqlCommand duplicateCommand =
                        new SqlCommand(duplicateQuery, connection))
                    {
                        duplicateCommand.Parameters.Add(
                            "@Email", SqlDbType.VarChar, 100).Value = email;

                        duplicateCommand.Parameters.Add(
                            "@UserID", SqlDbType.Int).Value = CustomerID;

                        if (Convert.ToInt32(
                            duplicateCommand.ExecuteScalar()) > 0)
                        {
                            ShowMessage(
                                "An account with this email address already exists.",
                                false);
                            return;
                        }
                    }

                    // Editing: load the existing photo so it can be retained.
                    if (CustomerID > 0)
                    {
                        const string imageQuery = @"
                            SELECT ProfilePhoto
                            FROM dbo.Users
                            WHERE UserID = @UserID AND RoleID = @RoleID";

                        using (SqlCommand imageCommand =
                            new SqlCommand(imageQuery, connection))
                        {
                            imageCommand.Parameters.Add(
                                "@UserID", SqlDbType.Int).Value = CustomerID;

                            imageCommand.Parameters.Add(
                                "@RoleID", SqlDbType.Int).Value = customerRoleID;

                            object result = imageCommand.ExecuteScalar();

                            if (result == null)
                            {
                                ShowMessage("Customer not found.", false);
                                return;
                            }

                            if (result != DBNull.Value)
                            {
                                newImagePath = Convert.ToString(result);
                            }
                        }
                    }

                    // Validate and save an optional profile photo.
                    if (ProfilePhotoInput.HasFile)
                    {
                        string extension = Path.GetExtension(
                            ProfilePhotoInput.FileName).ToLowerInvariant();

                        string[] allowedExtensions =
                        {
                            ".jpg", ".jpeg", ".png", ".webp"
                        };

                        if (Array.IndexOf(allowedExtensions, extension) < 0)
                        {
                            ShowMessage(
                                "Upload a JPG, JPEG, PNG, or WEBP image.",
                                false);
                            return;
                        }

                        if (ProfilePhotoInput.PostedFile.ContentLength >
                            5 * 1024 * 1024)
                        {
                            ShowMessage(
                                "The profile photo must be 5 MB or smaller.",
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

                        uploadedFilePath =
                            Path.Combine(imageFolder, fileName);

                        ProfilePhotoInput.SaveAs(uploadedFilePath);

                        newImagePath = "Content/images/" + fileName;
                    }

                    if (CustomerID > 0)
                    {
                        const string updateQuery = @"
                            UPDATE dbo.Users
                            SET FirstName = @FirstName,
                                LastName = @LastName,
                                Email = @Email,
                                Phone = @Phone,
                                ProfilePhoto = @ProfilePhoto,
                                IsActive = @IsActive
                            WHERE UserID = @UserID AND RoleID = @RoleID";

                        using (SqlCommand command =
                            new SqlCommand(updateQuery, connection))
                        {
                            AddCustomerParameters(
                                command, firstName, lastName, email,
                                phone, newImagePath, isActive);

                            command.Parameters.Add(
                                "@UserID", SqlDbType.Int).Value = CustomerID;

                            command.Parameters.Add(
                                "@RoleID", SqlDbType.Int).Value = customerRoleID;

                            if (command.ExecuteNonQuery() == 0)
                            {
                                DeleteUploadedFile(uploadedFilePath);
                                ShowMessage("Customer not found.", false);
                                return;
                            }
                        }
                    }
                    else
                    {
                        string passwordHash = HashPassword(password);

                        const string insertQuery = @"
                            INSERT INTO dbo.Users
                                (RoleID, FirstName, LastName, Email, Phone,
                                 PasswordHash, ProfilePhoto, IsActive)
                            VALUES
                                (@RoleID, @FirstName, @LastName, @Email, @Phone,
                                 @PasswordHash, @ProfilePhoto, @IsActive)";

                        using (SqlCommand command =
                            new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.Add(
                                "@RoleID", SqlDbType.Int).Value = customerRoleID;

                            AddCustomerParameters(
                                command, firstName, lastName, email,
                                phone, newImagePath, isActive);

                            command.Parameters.Add(
                                "@PasswordHash", SqlDbType.VarChar, 255)
                                .Value = passwordHash;

                            command.ExecuteNonQuery();
                        }
                    }
                }

                Response.Redirect("~/AdminCustomers.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (SqlException ex)
            {
                DeleteUploadedFile(uploadedFilePath);

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    ShowMessage(
                        "This email address is already registered.", false);
                }
                else
                {
                    ShowMessage(
                        "Unable to save the customer. Please check the database and try again.",
                        false);
                }
            }
            catch (Exception)
            {
                DeleteUploadedFile(uploadedFilePath);

                ShowMessage(
                    "An error occurred while saving the customer. Please try again.",
                    false);
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];

            using (RandomNumberGenerator random =
                RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            byte[] hash;

            using (Rfc2898DeriveBytes deriveBytes =
                new Rfc2898DeriveBytes(password, salt, 100000))
            {
                hash = deriveBytes.GetBytes(32);
            }

            return Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        private void AddCustomerParameters(
            SqlCommand command,
            string firstName,
            string lastName,
            string email,
            string phone,
            string profilePhoto,
            bool isActive)
        {
            command.Parameters.Add(
                "@FirstName", SqlDbType.VarChar, 50).Value = firstName;

            command.Parameters.Add(
                "@LastName", SqlDbType.VarChar, 50).Value = lastName;

            command.Parameters.Add(
                "@Email", SqlDbType.VarChar, 100).Value = email;

            command.Parameters.Add(
                "@Phone", SqlDbType.VarChar, 20).Value =
                string.IsNullOrWhiteSpace(phone)
                    ? (object)DBNull.Value : phone;

            command.Parameters.Add(
                "@ProfilePhoto", SqlDbType.VarChar, 255).Value =
                string.IsNullOrWhiteSpace(profilePhoto)
                    ? (object)DBNull.Value : profilePhoto;

            command.Parameters.Add(
                "@IsActive", SqlDbType.Bit).Value = isActive;
        }

        private void DeleteUploadedFile(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (IOException)
                {
                    // Keep the original database error.
                }
                catch (UnauthorizedAccessException)
                {
                    // Keep the original database error.
                }
            }
        }

        private void ShowMessage(string message, bool success)
        {
            CustomerMessage.Text = Server.HtmlEncode(message);
            CustomerMessage.ForeColor = success
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.Firebrick;
        }
    }
}