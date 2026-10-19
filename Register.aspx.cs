using System;
using System.Security.Cryptography;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            // Server-side ASP.NET validation
            if (!Page.IsValid)
            {
                return;
            }

            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string password = PasswordTextBox.Text;

            try
            {
                UserRepository repository = new UserRepository();

                // Check if email already exists
                if (repository.EmailExists(email))
                {
                    MessageLabel.Text =
                        "An account with this email already exists.";

                    return;
                }

                // Create new customer
                User user = new User();

                user.RoleID = 2;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.Phone = phone;
                user.PasswordHash = HashPassword(password);

                bool registered = repository.RegisterUser(user);

                if (registered)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    MessageLabel.Text =
                        "Registration failed. Please try again.";
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text =
                    "An error occurred while creating your account.";
            }
        }


        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];

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
                byte[] hash = pbkdf2.GetBytes(32);

                return Convert.ToBase64String(salt)
                       + ":"
                       + Convert.ToBase64String(hash);
            }
        }


        private void ClearForm()
        {
            FirstNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            EmailTextBox.Text = "";
            PhoneTextBox.Text = "";
            PasswordTextBox.Text = "";
            ConfirmPasswordTextBox.Text = "";
        }
    }
}