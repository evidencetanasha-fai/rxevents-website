using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminAddStaff : System.Web.UI.Page
    {
        private int StaffID
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["StaffID"], out id)
                    ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int roleID;

            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                !int.TryParse(
                    Convert.ToString(Session["RoleID"]), out roleID) ||
                roleID != 1)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (StaffID > 0)
                {
                    PageHeadingLiteral.Text = "Edit Staff";
                    SaveStaffButton.Text = "Update Staff";
                    LoadStaffDetails();
                }
                else
                {
                    StaffStatusDropdown.SelectedValue = "1";
                }
            }
        }

        private void LoadStaffDetails()
        {
            try
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    const string query = @"
                        SELECT FirstName, LastName, Email, Phone,
                               Position, HireDate, IsActive
                        FROM dbo.Staff
                        WHERE StaffID = @StaffID";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@StaffID", SqlDbType.Int)
                            .Value = StaffID;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                ShowMessage("Staff member not found.", false);
                                SaveStaffButton.Enabled = false;
                                return;
                            }

                            FirstNameInput.Text =
                                Convert.ToString(reader["FirstName"]);

                            LastNameInput.Text =
                                Convert.ToString(reader["LastName"]);

                            StaffEmailInput.Text =
                                reader["Email"] == DBNull.Value
                                    ? ""
                                    : Convert.ToString(reader["Email"]);

                            StaffPhoneInput.Text =
                                reader["Phone"] == DBNull.Value
                                    ? ""
                                    : Convert.ToString(reader["Phone"]);

                            string position =
                                reader["Position"] == DBNull.Value
                                    ? ""
                                    : Convert.ToString(reader["Position"]);

                            if (!string.IsNullOrWhiteSpace(position) &&
                                StaffPositionDropdown.Items.FindByValue(
                                    position) == null)
                            {
                                StaffPositionDropdown.Items.Add(
                                    new System.Web.UI.WebControls.ListItem(
                                        position, position));
                            }

                            StaffPositionDropdown.SelectedValue = position;

                            HireDateInput.Text =
                                reader["HireDate"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(reader["HireDate"])
                                        .ToString("yyyy-MM-dd");

                            StaffStatusDropdown.SelectedValue =
                                Convert.ToBoolean(reader["IsActive"])
                                    ? "1" : "0";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ShowMessage(
                    "Unable to load staff details. Please check the database connection.",
                    false);
            }
        }

        protected void SaveStaffButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string firstName = FirstNameInput.Text.Trim();
            string lastName = LastNameInput.Text.Trim();
            string email = StaffEmailInput.Text.Trim();
            string phone = StaffPhoneInput.Text.Trim();
            string position = StaffPositionDropdown.SelectedValue;

            bool isActive = StaffStatusDropdown.SelectedValue == "1";

            if (firstName.Length > 50 ||
                lastName.Length > 50 ||
                email.Length > 100 ||
                phone.Length > 20 ||
                position.Length > 100)
            {
                ShowMessage(
                    "One or more fields exceed the allowed length.", false);
                return;
            }

            object hireDate = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(HireDateInput.Text))
            {
                DateTime parsedDate;

                if (!DateTime.TryParseExact(
                    HireDateInput.Text.Trim(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedDate))
                {
                    ShowMessage("Please enter a valid hire date.", false);
                    return;
                }

                if (parsedDate.Date > DateTime.Today)
                {
                    ShowMessage(
                        "Hire date cannot be in the future.", false);
                    return;
                }

                hireDate = parsedDate.Date;
            }

            try
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    connection.Open();

                    // Email is optional, but if supplied it must be unique
                    // among staff records.
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        const string duplicateQuery = @"
                            SELECT COUNT(*)
                            FROM dbo.Staff
                            WHERE Email = @Email
                              AND StaffID <> @StaffID";

                        using (SqlCommand duplicateCommand =
                            new SqlCommand(duplicateQuery, connection))
                        {
                            duplicateCommand.Parameters.Add(
                                "@Email", SqlDbType.VarChar, 100)
                                .Value = email;

                            duplicateCommand.Parameters.Add(
                                "@StaffID", SqlDbType.Int)
                                .Value = StaffID;

                            if (Convert.ToInt32(
                                duplicateCommand.ExecuteScalar()) > 0)
                            {
                                ShowMessage(
                                    "Another staff member already uses this email address.",
                                    false);
                                return;
                            }
                        }
                    }

                    string query;

                    if (StaffID > 0)
                    {
                        query = @"
                            UPDATE dbo.Staff
                            SET FirstName = @FirstName,
                                LastName = @LastName,
                                Email = @Email,
                                Phone = @Phone,
                                Position = @Position,
                                HireDate = @HireDate,
                                IsActive = @IsActive
                            WHERE StaffID = @StaffID";
                    }
                    else
                    {
                        query = @"
                            INSERT INTO dbo.Staff
                                (FirstName, LastName, Email, Phone,
                                 Position, HireDate, IsActive)
                            VALUES
                                (@FirstName, @LastName, @Email, @Phone,
                                 @Position, @HireDate, @IsActive)";
                    }

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(
                            "@FirstName", SqlDbType.VarChar, 50)
                            .Value = firstName;

                        command.Parameters.Add(
                            "@LastName", SqlDbType.VarChar, 50)
                            .Value = lastName;

                        command.Parameters.Add(
                            "@Email", SqlDbType.VarChar, 100)
                            .Value = string.IsNullOrWhiteSpace(email)
                                ? (object)DBNull.Value : email;

                        command.Parameters.Add(
                            "@Phone", SqlDbType.VarChar, 20)
                            .Value = string.IsNullOrWhiteSpace(phone)
                                ? (object)DBNull.Value : phone;

                        command.Parameters.Add(
                            "@Position", SqlDbType.VarChar, 100)
                            .Value = string.IsNullOrWhiteSpace(position)
                                ? (object)DBNull.Value : position;

                        command.Parameters.Add(
                            "@HireDate", SqlDbType.Date)
                            .Value = hireDate;

                        command.Parameters.Add(
                            "@IsActive", SqlDbType.Bit)
                            .Value = isActive;

                        if (StaffID > 0)
                        {
                            command.Parameters.Add(
                                "@StaffID", SqlDbType.Int)
                                .Value = StaffID;
                        }

                        int affectedRows = command.ExecuteNonQuery();

                        if (StaffID > 0 && affectedRows == 0)
                        {
                            ShowMessage("Staff member not found.", false);
                            return;
                        }
                    }
                }

                Response.Redirect("~/AdminStaff.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    ShowMessage(
                        "A duplicate record was detected. Please check the email address.",
                        false);
                }
                else
                {
                    ShowMessage(
                        "Unable to save the staff member. Please check the database and try again.",
                        false);
                }
            }
            catch (Exception)
            {
                ShowMessage(
                    "An error occurred while saving the staff member.",
                    false);
            }
        }

        private void ShowMessage(string message, bool success)
        {
            StaffMessage.Text = Server.HtmlEncode(message);
            StaffMessage.ForeColor = success
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.Firebrick;
        }
    }
}