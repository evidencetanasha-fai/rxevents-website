using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminStaff : Page
    {
        private const int PageSize = 8;

        protected int CurrentStaffPage
        {
            get
            {
                return ViewState["CurrentStaffPage"] == null
                    ? 1 : Convert.ToInt32(ViewState["CurrentStaffPage"]);
            }
            set { ViewState["CurrentStaffPage"] = value; }
        }

        private int TotalStaffPages
        {
            get
            {
                return ViewState["TotalStaffPages"] == null
                    ? 1 : Convert.ToInt32(ViewState["TotalStaffPages"]);
            }
            set { ViewState["TotalStaffPages"] = value; }
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

            // The current schema has no Department column.
            DepartmentFilter.Enabled = false;

            if (!IsPostBack)
            {
                LoadStaff();
            }
        }

        private void LoadStaff()
        {
            string search = StaffSearchTextBox.Text.Trim();
            string status = StaffStatusFilter.SelectedValue;
            string position = StaffRoleFilter.SelectedValue;

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string filter = @"
                    (@Search = ''
                     OR FirstName LIKE @SearchPattern
                     OR LastName LIKE @SearchPattern
                     OR ISNULL(Email, '') LIKE @SearchPattern
                     OR ISNULL(Phone, '') LIKE @SearchPattern)
                    AND (@Status = '' OR IsActive = @IsActive)
                    AND (@Position = '' OR ISNULL(Position, '') = @Position)";

                connection.Open();

                int totalRecords;

                using (SqlCommand countCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM dbo.Staff WHERE " + filter,
                    connection))
                {
                    AddFilterParameters(
                        countCommand, search, status, position);

                    totalRecords =
                        Convert.ToInt32(countCommand.ExecuteScalar());
                }

                TotalStaffPages = Math.Max(
                    1, (int)Math.Ceiling(totalRecords / (double)PageSize));

                if (CurrentStaffPage > TotalStaffPages)
                {
                    CurrentStaffPage = TotalStaffPages;
                }

                const string query = @"
                    SELECT StaffID, FirstName, LastName, Email, Phone,
                           Position, HireDate, IsActive
                    FROM dbo.Staff
                    WHERE
                        (@Search = ''
                         OR FirstName LIKE @SearchPattern
                         OR LastName LIKE @SearchPattern
                         OR ISNULL(Email, '') LIKE @SearchPattern
                         OR ISNULL(Phone, '') LIKE @SearchPattern)
                        AND (@Status = '' OR IsActive = @IsActive)
                        AND (@Position = '' OR ISNULL(Position, '') = @Position)
                    ORDER BY StaffID DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddFilterParameters(
                        command, search, status, position);

                    command.Parameters.Add("@Offset", SqlDbType.Int)
                        .Value = (CurrentStaffPage - 1) * PageSize;

                    command.Parameters.Add("@PageSize", SqlDbType.Int)
                        .Value = PageSize;

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        DataTable staff = new DataTable();
                        adapter.Fill(staff);

                        // Adapt the actual Staff columns to the existing markup.
                        staff.Columns.Add("Department", typeof(string));
                        staff.Columns.Add("StaffRole", typeof(string));
                        staff.Columns.Add("ProfilePhoto", typeof(string));

                        foreach (DataRow row in staff.Rows)
                        {
                            row["Department"] = "Not assigned";
                            row["StaffRole"] =
                                row["Position"] == DBNull.Value ||
                                string.IsNullOrWhiteSpace(
                                    Convert.ToString(row["Position"]))
                                    ? "Not assigned"
                                    : Convert.ToString(row["Position"]);

                            row["ProfilePhoto"] = DBNull.Value;
                        }

                        StaffGridView.DataSource = staff;
                        StaffGridView.DataBind();
                    }
                }

                int firstRecord = totalRecords == 0
                    ? 0 : (CurrentStaffPage - 1) * PageSize + 1;

                int lastRecord = Math.Min(
                    CurrentStaffPage * PageSize, totalRecords);

                StaffPaginationSummary.Text =
                    "Showing " + firstRecord + " to " + lastRecord +
                    " of " + totalRecords + " staff members";

                BindStaffPagination();
            }
        }

        private void AddFilterParameters(
            SqlCommand command,
            string search,
            string status,
            string position)
        {
            command.Parameters.Add("@Search", SqlDbType.VarChar, 100)
                .Value = search;

            command.Parameters.Add("@SearchPattern", SqlDbType.VarChar, 202)
                .Value = "%" + search + "%";

            command.Parameters.Add("@Status", SqlDbType.VarChar, 1)
                .Value = status;

            command.Parameters.Add("@IsActive", SqlDbType.Bit)
                .Value = status == "1";

            command.Parameters.Add("@Position", SqlDbType.VarChar, 100)
                .Value = position;
        }

        private void BindStaffPagination()
        {
            DataTable pages = new DataTable();
            pages.Columns.Add("PageNumber", typeof(int));

            for (int page = 1; page <= TotalStaffPages; page++)
            {
                pages.Rows.Add(page);
            }

            StaffPaginationRepeater.DataSource = pages;
            StaffPaginationRepeater.DataBind();

            PreviousStaffPageButton.Enabled = CurrentStaffPage > 1;
            NextStaffPageButton.Enabled = CurrentStaffPage < TotalStaffPages;
        }

        protected string GetStaffImageUrl(object profilePhoto)
        {
            return ResolveUrl("~/Content/images/customer-placeholder.png");
        }

        protected void ApplyStaffFiltersButton_Click(
            object sender, EventArgs e)
        {
            CurrentStaffPage = 1;
            LoadStaff();
        }

        protected void ClearStaffFiltersButton_Click(
            object sender, EventArgs e)
        {
            StaffSearchTextBox.Text = "";
            DepartmentFilter.SelectedIndex = 0;
            StaffRoleFilter.SelectedIndex = 0;
            StaffStatusFilter.SelectedIndex = 0;
            CurrentStaffPage = 1;

            LoadStaff();
        }

        protected void PreviousStaffPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentStaffPage > 1)
            {
                CurrentStaffPage--;
            }

            LoadStaff();
        }

        protected void NextStaffPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentStaffPage < TotalStaffPages)
            {
                CurrentStaffPage++;
            }

            LoadStaff();
        }

        protected void StaffPaginationRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            int pageNumber;

            if (e.CommandName == "GoToPage" &&
                int.TryParse(Convert.ToString(e.CommandArgument),
                    out pageNumber) &&
                pageNumber >= 1 &&
                pageNumber <= TotalStaffPages)
            {
                CurrentStaffPage = pageNumber;
                LoadStaff();
            }
        }

        protected void StaffGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            int staffID;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument), out staffID))
            {
                return;
            }

            if (e.CommandName == "ViewStaff" ||
                e.CommandName == "EditStaff")
            {
                Response.Redirect(
                    "~/AdminAddStaff.aspx?StaffID=" + staffID);
            }
            else if (e.CommandName == "ToggleStaff")
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    const string query = @"
                        UPDATE dbo.Staff
                        SET IsActive =
                            CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                        WHERE StaffID = @StaffID;";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@StaffID", SqlDbType.Int)
                            .Value = staffID;

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                LoadStaff();
            }
        }

        protected void ExportStaffButton_Click(object sender, EventArgs e)
        {
            string search = StaffSearchTextBox.Text.Trim();
            string status = StaffStatusFilter.SelectedValue;
            string position = StaffRoleFilter.SelectedValue;

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT StaffID, FirstName, LastName, Email, Phone,
                           Position, HireDate, IsActive
                    FROM dbo.Staff
                    WHERE
                        (@Search = ''
                         OR FirstName LIKE @SearchPattern
                         OR LastName LIKE @SearchPattern
                         OR ISNULL(Email, '') LIKE @SearchPattern
                         OR ISNULL(Phone, '') LIKE @SearchPattern)
                        AND (@Status = '' OR IsActive = @IsActive)
                        AND (@Position = '' OR ISNULL(Position, '') = @Position)
                    ORDER BY StaffID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddFilterParameters(
                        command, search, status, position);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder csv = new StringBuilder();

                        csv.AppendLine(
                            "Staff ID,First Name,Last Name,Email,Phone,Position,Hire Date,Status");

                        while (reader.Read())
                        {
                            csv.Append(Csv(reader["StaffID"])).Append(",");
                            csv.Append(Csv(reader["FirstName"])).Append(",");
                            csv.Append(Csv(reader["LastName"])).Append(",");
                            csv.Append(Csv(reader["Email"])).Append(",");
                            csv.Append(Csv(reader["Phone"])).Append(",");
                            csv.Append(Csv(reader["Position"])).Append(",");
                            csv.Append(Csv(
                                reader["HireDate"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(reader["HireDate"])
                                        .ToString("dd MMM yyyy"))).Append(",");
                            csv.AppendLine(Csv(
                                Convert.ToBoolean(reader["IsActive"])
                                    ? "Active" : "Inactive"));
                        }

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "text/csv";
                        Response.ContentEncoding = Encoding.UTF8;
                        Response.AddHeader(
                            "Content-Disposition",
                            "attachment;filename=RxEventsStaff.csv");

                        Response.Write("\uFEFF" + csv.ToString());
                        Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }

        private string Csv(object value)
        {
            string text = value == null || value == DBNull.Value
                ? ""
                : Convert.ToString(value);

            if (!string.IsNullOrEmpty(text) &&
                (text.StartsWith("=") || text.StartsWith("+") ||
                 text.StartsWith("-") || text.StartsWith("@")))
            {
                text = "'" + text;
            }

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}