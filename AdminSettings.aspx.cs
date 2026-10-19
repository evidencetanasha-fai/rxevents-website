
using System;
using System.Data;
using System.Data.SqlClient;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminSettings : System.Web.UI.Page
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
                LoadAccountSettings();
            }
        }

        private void LoadAccountSettings()
        {
            string query = @"
                SELECT FirstName, LastName, Email, Phone, DisplayMode
                FROM Users
                WHERE UserID = @UserID AND RoleID = 1;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value =
                    Convert.ToInt32(Session["UserID"]);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        FirstNameTextBox.Text =
                            Convert.ToString(reader["FirstName"]);

                        LastNameTextBox.Text =
                            Convert.ToString(reader["LastName"]);

                        EmailTextBox.Text =
                            Convert.ToString(reader["Email"]);

                        PhoneTextBox.Text =
                            reader["Phone"] == DBNull.Value
                            ? ""
                            : Convert.ToString(reader["Phone"]);

                        string displayMode =
                            reader["DisplayMode"] == DBNull.Value
                            ? "Light"
                            : Convert.ToString(reader["DisplayMode"]);

                        if (DisplayModeDropdown.Items.FindByValue(
                            displayMode) != null)
                        {
                            DisplayModeDropdown.SelectedValue =
                                displayMode;
                        }
                    }
                    else
                    {
                        SettingsMessage.Text =
                            "Administrator account was not found.";
                    }
                }
            }
        }

        protected void SaveAccountButton_Click(
            object sender, EventArgs e)
        {
            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email))
            {
                SettingsMessage.Text =
                    "First name, last name, and email are required.";
                return;
            }

            string query = @"
                UPDATE Users
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    Phone = @Phone
                WHERE UserID = @UserID AND RoleID = 1;";

            try
            {
                using (SqlConnection connection =
                       databaseHelper.GetConnection())
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@FirstName",
                        SqlDbType.VarChar, 100).Value = firstName;

                    command.Parameters.Add("@LastName",
                        SqlDbType.VarChar, 100).Value = lastName;

                    command.Parameters.Add("@Email",
                        SqlDbType.VarChar, 255).Value = email;

                    command.Parameters.Add("@Phone",
                        SqlDbType.VarChar, 30).Value =
                        string.IsNullOrWhiteSpace(phone)
                        ? (object)DBNull.Value
                        : phone;

                    command.Parameters.Add("@UserID",
                        SqlDbType.Int).Value =
                        Convert.ToInt32(Session["UserID"]);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["FirstName"] = firstName;
                        Session["LastName"] = lastName;

                        SettingsMessage.Text =
                            "Account settings saved successfully.";
                    }
                    else
                    {
                        SettingsMessage.Text =
                            "No administrator account was updated.";
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    SettingsMessage.Text =
                        "This email address is already registered.";
                }
                else
                {
                    SettingsMessage.Text =
                        "Unable to save settings. Please try again.";
                }
            }
        }


        protected void SaveAppearanceButton_Click(
            object sender, EventArgs e)
        {
            string displayMode = DisplayModeDropdown.SelectedValue;

            if (displayMode != "Light" && displayMode != "Dark")
            {
                SettingsMessage.Text = "Please select a valid display mode.";
                return;
            }

            string query = @"
        UPDATE Users
        SET DisplayMode = @DisplayMode
        WHERE UserID = @UserID AND RoleID = 1;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DisplayMode",
                    SqlDbType.VarChar, 20).Value = displayMode;

                command.Parameters.Add("@UserID",
                    SqlDbType.Int).Value =
                    Convert.ToInt32(Session["UserID"]);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Response.Redirect(Request.RawUrl);
                    return;
                }

                SettingsMessage.Text =
                    "No administrator account was updated.";
            }
        }

    }
}
