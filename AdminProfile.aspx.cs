
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminProfile : Page
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
                LoadAdminProfile();
                LoadProfileStatistics();
            }
        }

        private void LoadAdminProfile()
        {

            string query = @"
    SELECT
        FirstName,
        LastName,
        Email,
        Phone,
        CreatedDate,
        IsActive,
        DisplayMode,
        ProfilePhoto
    FROM Users
    WHERE UserID = @UserID AND RoleID = 1;";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int)
                    .Value = Convert.ToInt32(Session["UserID"]);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Response.Redirect("~/Login.aspx");
                        return;
                    }

                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    string fullName = (firstName + " " + lastName).Trim();

                    ProfileFullName.Text = Server.HtmlEncode(fullName);
                    DetailFullName.Text = Server.HtmlEncode(fullName);

                    DetailEmail.Text = Server.HtmlEncode(
                        reader["Email"].ToString());

                    DetailPhone.Text =
                        reader["Phone"] == DBNull.Value ||
                        string.IsNullOrWhiteSpace(reader["Phone"].ToString())
                        ? "Not provided"
                        : Server.HtmlEncode(reader["Phone"].ToString());

                    DetailMemberSince.Text =
                        reader["CreatedDate"] == DBNull.Value
                        ? "Not available"
                        : Convert.ToDateTime(reader["CreatedDate"])
                            .ToString("MMMM yyyy");

                    bool isActive = reader["IsActive"] != DBNull.Value &&
                        Convert.ToBoolean(reader["IsActive"]);

                    DetailAccountStatus.Text =
                        isActive ? "Active" : "Inactive";

                    string initials = "";

                    if (!string.IsNullOrWhiteSpace(firstName))
                        initials += firstName.Substring(0, 1);

                    if (!string.IsNullOrWhiteSpace(lastName))
                        initials += lastName.Substring(0, 1);

                    ProfileInitials.Text =
                        Server.HtmlEncode(initials.ToUpperInvariant());

                    string displayMode =
                        reader["DisplayMode"] == DBNull.Value
                        ? "Light"
                        : reader["DisplayMode"].ToString();

                    if (DisplayModeDropdown.Items.FindByValue(displayMode) != null)
                    {
                        DisplayModeDropdown.SelectedValue = displayMode;
                    }
                    string photoPath = reader["ProfilePhoto"] == DBNull.Value
    ? ""
    : reader["ProfilePhoto"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(photoPath))
                    {
                        ProfileAvatarImage.ImageUrl = ResolveUrl(GetPhotoUrl(photoPath));
                        ProfileAvatarImage.Visible = true;
                        ProfileInitials.Visible = false;
                    }
                    else
                    {
                        ProfileAvatarImage.Visible = false;
                        ProfileInitials.Visible = true;
                    }
                }
            }
        }
        private string GetPhotoUrl(string photo)
        {
            if (photo.StartsWith("~/"))
                return photo;

            if (photo.StartsWith("/"))
                return "~" + photo;

            return "~/" + photo.TrimStart('/');
        }

        private void LoadProfileStatistics()
        {

            string query = @"
    SELECT
        (SELECT COUNT(*) FROM Bookings) AS BookingCount,
        (SELECT COUNT(*)
         FROM Bookings
         WHERE BookingStatus NOT IN ('Cancelled', 'Rejected')
        ) AS EventCount;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ProfileBookingsCount.Text =
                            Convert.ToInt32(reader["BookingCount"])
                                .ToString("N0");

                        ProfileEventsCount.Text =
                            Convert.ToInt32(reader["EventCount"])
                                .ToString("N0");
                    }
                }
            }

            ProfileReportsCount.Text = "—";
        }

        protected void SavePreferencesButton_Click(
            object sender, EventArgs e)
        {
            string displayMode = DisplayModeDropdown.SelectedValue;

            if (displayMode != "Light" &&
                displayMode != "Dark" &&
                displayMode != "System")
            {
                PreferencesMessage.Text = "Invalid display preference.";
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
                command.Parameters.Add("@DisplayMode", SqlDbType.VarChar, 20)
                    .Value = displayMode;

                command.Parameters.Add("@UserID", SqlDbType.Int)
                    .Value = Convert.ToInt32(Session["UserID"]);

                connection.Open();

                int rowsUpdated = command.ExecuteNonQuery();

                PreferencesMessage.Text = rowsUpdated > 0
                    ? "Preference saved successfully."
                    : "Could not save the preference.";
            }
        }

        protected void OpenChangePasswordPopup_Click(
            object sender, EventArgs e)
        {
            ChangePasswordOverlay.Visible = true;
            PasswordMessage.Text = "";
        }

        protected void ClosePasswordPopup_Click(
            object sender, EventArgs e)
        {
            ChangePasswordOverlay.Visible = false;

            CurrentPasswordTextBox.Text = "";
            NewPasswordTextBox.Text = "";
            ConfirmPasswordTextBox.Text = "";
            PasswordMessage.Text = "";
        }

        protected void SavePasswordButton_Click(object sender, EventArgs e)
        {
            ChangePasswordOverlay.Visible = true;

            PasswordMessage.Text =
                "Password update is not connected yet. We need to verify your current password and use your existing password-hashing method before saving.";
        }

    }
}
