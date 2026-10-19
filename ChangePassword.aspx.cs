using System;
using System.Security.Cryptography;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckCustomerAccess();
            }
        }


        private void CheckCustomerAccess()
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx");
            }
        }


        protected void ChangePasswordButton_Click(
            object sender,
            EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!Page.IsValid)
            {
                return;
            }


            string currentPassword =
                CurrentPasswordTextBox.Text;

            string newPassword =
                NewPasswordTextBox.Text;

            string confirmPassword =
                ConfirmPasswordTextBox.Text;


            // Check whether the new passwords match

            if (newPassword != confirmPassword)
            {
                MessageLabel.Text =
                    "New password and confirmation password do not match.";

                MessageLabel.CssClass =
                    "rx-change-message error";

                return;
            }


            // Prevent the same password being used

            if (currentPassword == newPassword)
            {
                MessageLabel.Text =
                    "Your new password must be different from your current password.";

                MessageLabel.CssClass =
                    "rx-change-message error";

                return;
            }


            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);


                UserRepository repository =
                    new UserRepository();


                User user =
                    repository.GetUserById(userId);


                if (user == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }


                // Verify current password

                if (!VerifyPassword(
                    currentPassword,
                    user.PasswordHash))
                {
                    MessageLabel.Text =
                        "Your current password is incorrect.";

                    MessageLabel.CssClass =
                        "rx-change-message error";

                    return;
                }


                // Create new password hash

                string newPasswordHash =
                    HashPassword(newPassword);


                // Save new password

                bool updated =
                    repository.UpdatePassword(
                        userId,
                        newPasswordHash);


                if (updated)
                {
                    CurrentPasswordTextBox.Text = "";
                    NewPasswordTextBox.Text = "";
                    ConfirmPasswordTextBox.Text = "";

                    MessageLabel.Text =
                        "Your password has been changed successfully.";

                    MessageLabel.CssClass =
                        "rx-change-message success";
                }
                else
                {
                    MessageLabel.Text =
                        "Unable to change your password. Please try again.";

                    MessageLabel.CssClass =
                        "rx-change-message error";
                }
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "An error occurred while changing your password.";

                MessageLabel.CssClass =
                    "rx-change-message error";
            }
        }


        // ==========================================
        // PASSWORD HASHING
        // ==========================================

        private string HashPassword(string password)
        {
            byte[] salt =
                new byte[16];


            using (RandomNumberGenerator generator =
                   RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
            }


            using (Rfc2898DeriveBytes pbkdf2 =
                   new Rfc2898DeriveBytes(
                       password,
                       salt,
                       100000))
            {
                byte[] hash =
                    pbkdf2.GetBytes(32);


                return Convert.ToBase64String(salt)
                       + ":"
                       + Convert.ToBase64String(hash);
            }
        }


        // ==========================================
        // PASSWORD VERIFICATION
        // ==========================================

        private bool VerifyPassword(
            string password,
            string storedPassword)
        {
            try
            {
                string[] parts =
                    storedPassword.Split(':');


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
                    byte[] hash =
                        pbkdf2.GetBytes(32);


                    return SlowEquals(
                        hash,
                        storedHash);
                }
            }
            catch
            {
                return false;
            }
        }


        // ==========================================
        // CONSTANT-TIME COMPARISON
        // ==========================================

        private bool SlowEquals(
            byte[] first,
            byte[] second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }


            int result = 0;


            for (int i = 0;
                 i < first.Length;
                 i++)
            {
                result |= first[i] ^ second[i];
            }


            return result == 0;
        }
    }
}