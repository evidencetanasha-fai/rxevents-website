using System;
using System.Security.Cryptography;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text;

            try
            {
                UserRepository repository = new UserRepository();

                var user = repository.GetUserByEmail(email);

                if (user == null)
                {
                    MessageLabel.Text = "Invalid email or password.";
                    return;
                }

                if (!user.IsActive)
                {
                    MessageLabel.Text =
                        "Your account is currently inactive.";
                    return;
                }

                if (!VerifyPassword(password, user.PasswordHash))
                {
                    MessageLabel.Text = "Invalid email or password.";
                    return;
                }

                // Store logged-in user's information
                Session["UserID"] = user.UserID;
                Session["RoleID"] = user.RoleID;
                Session["FirstName"] = user.FirstName;
                Session["LastName"] = user.LastName;
                Session["Email"] = user.Email;
                Session["RoleName"] = user.RoleName;

                // Redirect according to role
                if (user.RoleID == 1)
                {
                    // Admin
                    Response.Redirect("AdminDashboard.aspx");
                }
                else if (user.RoleID == 2)
                {
                    // Customer
                    Response.Redirect("CustomerDashboard.aspx");
                }
                else
                {
                    MessageLabel.Text =
                        "Your account has an invalid role.";
                }
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to login. Please try again.";
            }
        }

        private bool VerifyPassword(
            string password,
            string storedPassword)
        {
            try
            {
                string[] parts = storedPassword.Split(':');

                if (parts.Length != 2)
                {
                    return false;
                }

                byte[] salt =
                    Convert.FromBase64String(parts[0]);

                byte[] storedHash =
                    Convert.FromBase64String(parts[1]);

                using (Rfc2898DeriveBytes pbkdf2 =
                       new Rfc2898DeriveBytes(
                           password,
                           salt,
                           100000))
                {
                    byte[] hash = pbkdf2.GetBytes(32);

                    return SlowEquals(hash, storedHash);
                }
            }
            catch
            {
                return false;
            }
        }

        private bool SlowEquals(
            byte[] first,
            byte[] second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            int result = 0;

            for (int i = 0; i < first.Length; i++)
            {
                result |= first[i] ^ second[i];
            }

            return result == 0;
        }
    }
}