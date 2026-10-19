using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminCustomers : Page
    {
        private const int PageSize = 7;

        protected int CurrentCustomerPage
        {
            get
            {
                return ViewState["CurrentCustomerPage"] == null
                    ? 1 : Convert.ToInt32(ViewState["CurrentCustomerPage"]);
            }
            set { ViewState["CurrentCustomerPage"] = value; }
        }

        private int TotalCustomerPages
        {
            get
            {
                return ViewState["TotalCustomerPages"] == null
                    ? 1 : Convert.ToInt32(ViewState["TotalCustomerPages"]);
            }
            set { ViewState["TotalCustomerPages"] = value; }
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
                LoadCustomerStatistics();
                LoadCustomers();
            }
        }

        // Finds the customer role by name instead of assuming its numeric ID.
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

        private void LoadCustomerStatistics()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                connection.Open();

                int customerRoleID = GetCustomerRoleID(connection);

                const string query = @"
                    SELECT
                        COUNT(*) AS TotalCustomers,
                        SUM(CASE
                            WHEN CreatedDate >= @MonthStart
                             AND CreatedDate < @NextMonth
                            THEN 1 ELSE 0 END) AS NewCustomers,
                        SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END)
                            AS ActiveCustomers,
                        SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END)
                            AS InactiveCustomers
                    FROM dbo.Users
                    WHERE RoleID = @RoleID;";

                DateTime now = DateTime.Now;
                DateTime monthStart = new DateTime(now.Year, now.Month, 1);
                DateTime nextMonth = monthStart.AddMonths(1);

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@RoleID", SqlDbType.Int)
                        .Value = customerRoleID;

                    command.Parameters.Add("@MonthStart", SqlDbType.DateTime)
                        .Value = monthStart;

                    command.Parameters.Add("@NextMonth", SqlDbType.DateTime)
                        .Value = nextMonth;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TotalCustomersValue.Text =
                                Convert.ToString(reader["TotalCustomers"]);

                            NewCustomersValue.Text =
                                Convert.ToString(reader["NewCustomers"]);

                            ActiveCustomersValue.Text =
                                Convert.ToString(reader["ActiveCustomers"]);

                            InactiveCustomersValue.Text =
                                Convert.ToString(reader["InactiveCustomers"]);
                        }
                    }
                }
            }
        }

        private void LoadCustomers()
        {
            string search = CustomerSearchTextBox.Text.Trim();
            string status = CustomerStatusFilter.SelectedValue;
            string joinDate = CustomerJoinDateFilter.SelectedValue;
            string sort = CustomerSortDropdown.SelectedValue;

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                connection.Open();

                int customerRoleID = GetCustomerRoleID(connection);

                DateTime now = DateTime.Now;
                DateTime monthStart = new DateTime(now.Year, now.Month, 1);
                DateTime nextMonth = monthStart.AddMonths(1);
                DateTime thirtyDaysAgo = now.Date.AddDays(-30);
                DateTime yearStart = new DateTime(now.Year, 1, 1);

                string filter = @"
                    u.RoleID = @RoleID
                    AND (
                        @Search = ''
                        OR u.FirstName LIKE @SearchPattern
                        OR u.LastName LIKE @SearchPattern
                        OR u.Email LIKE @SearchPattern
                        OR ISNULL(u.Phone, '') LIKE @SearchPattern
                    )
                    AND (@Status = '' OR u.IsActive = @IsActive)
                    AND (
                        @JoinDate = ''
                        OR (@JoinDate = 'month'
                            AND u.CreatedDate >= @MonthStart
                            AND u.CreatedDate < @NextMonth)
                        OR (@JoinDate = '30days'
                            AND u.CreatedDate >= @ThirtyDaysAgo)
                        OR (@JoinDate = 'year'
                            AND u.CreatedDate >= @YearStart
                            AND u.CreatedDate < @NextYear)
                    )";

                const string countQuery = @"
                    SELECT COUNT(*)
                    FROM dbo.Users u
                    WHERE ";

                using (SqlCommand countCommand =
                    new SqlCommand(countQuery + filter, connection))
                {
                    AddCustomerFilterParameters(
                        countCommand, customerRoleID, search, status,
                        joinDate, monthStart, nextMonth,
                        thirtyDaysAgo, yearStart, yearStart.AddYears(1));

                    int totalRecords =
                        Convert.ToInt32(countCommand.ExecuteScalar());

                    TotalCustomerPages = Math.Max(
                        1, (int)Math.Ceiling(totalRecords / (double)PageSize));

                    if (CurrentCustomerPage > TotalCustomerPages)
                    {
                        CurrentCustomerPage = TotalCustomerPages;
                    }

                    string orderBy;

                    switch (sort)
                    {
                        case "oldest":
                            orderBy = "u.CreatedDate ASC, u.UserID ASC";
                            break;
                        case "name":
                            orderBy = "u.FirstName ASC, u.LastName ASC";
                            break;
                        case "bookings":
                            orderBy = "ISNULL(b.TotalBookings, 0) DESC, u.UserID DESC";
                            break;
                        case "spending":
                            orderBy = "ISNULL(p.TotalSpent, 0) DESC, u.UserID DESC";
                            break;
                        default:
                            orderBy = "u.CreatedDate DESC, u.UserID DESC";
                            break;
                    }

                    string query = @"
                        SELECT
                            u.UserID,
                            u.FirstName,
                            u.LastName,
                            u.Email,
                            u.Phone,
                            u.ProfilePhoto,
                            u.CreatedDate,
                            u.IsActive,
                            ISNULL(b.TotalBookings, 0) AS TotalBookings,
                            ISNULL(p.TotalSpent, 0) AS TotalSpent
                        FROM dbo.Users u
                        LEFT JOIN
                        (
                            SELECT UserID, COUNT(*) AS TotalBookings
                            FROM dbo.Bookings
                            WHERE UserID IS NOT NULL
                            GROUP BY UserID
                        ) b ON b.UserID = u.UserID
                        LEFT JOIN
                        (
                            SELECT bk.UserID, SUM(pay.Amount) AS TotalSpent
                            FROM dbo.Bookings bk
                            INNER JOIN dbo.Payments pay
                                ON pay.BookingID = bk.BookingID
                            WHERE pay.PaymentStatus = 'Paid'
                              AND bk.UserID IS NOT NULL
                            GROUP BY bk.UserID
                        ) p ON p.UserID = u.UserID
                        WHERE " + filter + @"
                        ORDER BY " + orderBy + @"
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize ROWS ONLY;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        AddCustomerFilterParameters(
                            command, customerRoleID, search, status,
                            joinDate, monthStart, nextMonth,
                            thirtyDaysAgo, yearStart, yearStart.AddYears(1));

                        command.Parameters.Add("@Offset", SqlDbType.Int)
                            .Value = (CurrentCustomerPage - 1) * PageSize;

                        command.Parameters.Add("@PageSize", SqlDbType.Int)
                            .Value = PageSize;

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(command))
                        {
                            DataTable customers = new DataTable();
                            adapter.Fill(customers);

                            CustomersGridView.DataSource = customers;
                            CustomersGridView.DataBind();
                        }
                    }

                    int firstRecord = totalRecords == 0
                        ? 0 : (CurrentCustomerPage - 1) * PageSize + 1;

                    int lastRecord = Math.Min(
                        CurrentCustomerPage * PageSize, totalRecords);

                    CustomerPaginationSummary.Text =
                        "Showing " + firstRecord + " to " + lastRecord +
                        " of " + totalRecords + " customers";

                    BindCustomerPagination();
                }
            }
        }

        private void AddCustomerFilterParameters(
            SqlCommand command,
            int roleID,
            string search,
            string status,
            string joinDate,
            DateTime monthStart,
            DateTime nextMonth,
            DateTime thirtyDaysAgo,
            DateTime yearStart,
            DateTime nextYear)
        {
            command.Parameters.Add("@RoleID", SqlDbType.Int).Value = roleID;

            command.Parameters.Add("@Search", SqlDbType.VarChar, 100)
                .Value = search;

            command.Parameters.Add("@SearchPattern", SqlDbType.VarChar, 202)
                .Value = "%" + search + "%";

            command.Parameters.Add("@Status", SqlDbType.VarChar, 1)
                .Value = status;

            command.Parameters.Add("@IsActive", SqlDbType.Bit)
                .Value = status == "1";

            command.Parameters.Add("@JoinDate", SqlDbType.VarChar, 10)
                .Value = joinDate;

            command.Parameters.Add("@MonthStart", SqlDbType.DateTime)
                .Value = monthStart;

            command.Parameters.Add("@NextMonth", SqlDbType.DateTime)
                .Value = nextMonth;

            command.Parameters.Add("@ThirtyDaysAgo", SqlDbType.DateTime)
                .Value = thirtyDaysAgo;

            command.Parameters.Add("@YearStart", SqlDbType.DateTime)
                .Value = yearStart;

            command.Parameters.Add("@NextYear", SqlDbType.DateTime)
                .Value = nextYear;
        }

        private void BindCustomerPagination()
        {
            DataTable pages = new DataTable();
            pages.Columns.Add("PageNumber", typeof(int));

            for (int page = 1; page <= TotalCustomerPages; page++)
            {
                pages.Rows.Add(page);
            }

            CustomerPaginationRepeater.DataSource = pages;
            CustomerPaginationRepeater.DataBind();

            PreviousCustomerPageButton.Enabled = CurrentCustomerPage > 1;
            NextCustomerPageButton.Enabled =
                CurrentCustomerPage < TotalCustomerPages;
        }

        protected string GetCustomerImageUrl(object profilePhoto)
        {
            string path = profilePhoto == null ||
                          profilePhoto == DBNull.Value
                ? ""
                : Convert.ToString(profilePhoto).Trim();

            if (string.IsNullOrWhiteSpace(path))
            {
                return ResolveUrl("~/Content/images/customer-placeholder.png");
            }

            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            path = path.Replace("\\", "/").TrimStart('/');

            if (!path.Contains("/"))
            {
                path = "Content/images/" + path;
            }

            return ResolveUrl("~/" + path);
        }

        protected void ApplyCustomerFiltersButton_Click(
            object sender, EventArgs e)
        {
            CurrentCustomerPage = 1;
            LoadCustomerStatistics();
            LoadCustomers();
        }

        protected void ClearCustomerFiltersButton_Click(
            object sender, EventArgs e)
        {
            CustomerSearchTextBox.Text = "";
            CustomerStatusFilter.SelectedIndex = 0;
            CustomerJoinDateFilter.SelectedIndex = 0;
            CustomerSortDropdown.SelectedValue = "recent";
            CurrentCustomerPage = 1;

            LoadCustomerStatistics();
            LoadCustomers();
        }

        protected void PreviousCustomerPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentCustomerPage > 1)
            {
                CurrentCustomerPage--;
            }

            LoadCustomers();
        }

        protected void NextCustomerPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentCustomerPage < TotalCustomerPages)
            {
                CurrentCustomerPage++;
            }

            LoadCustomers();
        }

        protected void CustomerPaginationRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "GoToPage")
            {
                int pageNumber;

                if (int.TryParse(
                    Convert.ToString(e.CommandArgument), out pageNumber) &&
                    pageNumber >= 1 &&
                    pageNumber <= TotalCustomerPages)
                {
                    CurrentCustomerPage = pageNumber;
                    LoadCustomers();
                }
            }
        }

        protected void CustomersGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            int userID;

            if (!int.TryParse(Convert.ToString(e.CommandArgument), out userID))
            {
                return;
            }

            if (e.CommandName == "ViewCustomer" ||
                e.CommandName == "EditCustomer")
            {
                Response.Redirect(
                    "~/AdminAddCustomer.aspx?UserID=" + userID);
            }
            else if (e.CommandName == "ToggleCustomer")
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    connection.Open();

                    int customerRoleID = GetCustomerRoleID(connection);

                    const string query = @"
                        UPDATE dbo.Users
                        SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                        WHERE UserID = @UserID
                          AND RoleID = @RoleID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@UserID", SqlDbType.Int)
                            .Value = userID;

                        command.Parameters.Add("@RoleID", SqlDbType.Int)
                            .Value = customerRoleID;

                        command.ExecuteNonQuery();
                    }
                }

                LoadCustomerStatistics();
                LoadCustomers();
            }
        }

        protected void ExportCustomersButton_Click(
            object sender, EventArgs e)
        {
            string search = CustomerSearchTextBox.Text.Trim();
            string status = CustomerStatusFilter.SelectedValue;
            string joinDate = CustomerJoinDateFilter.SelectedValue;

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                connection.Open();

                int customerRoleID = GetCustomerRoleID(connection);

                DateTime now = DateTime.Now;
                DateTime monthStart = new DateTime(now.Year, now.Month, 1);

                string query = @"
                    SELECT
                        u.UserID,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Phone,
                        u.CreatedDate,
                        u.IsActive,
                        ISNULL(b.TotalBookings, 0) AS TotalBookings,
                        ISNULL(p.TotalSpent, 0) AS TotalSpent
                    FROM dbo.Users u
                    LEFT JOIN
                    (
                        SELECT UserID, COUNT(*) AS TotalBookings
                        FROM dbo.Bookings
                        WHERE UserID IS NOT NULL
                        GROUP BY UserID
                    ) b ON b.UserID = u.UserID
                    LEFT JOIN
                    (
                        SELECT bk.UserID, SUM(pay.Amount) AS TotalSpent
                        FROM dbo.Bookings bk
                        INNER JOIN dbo.Payments pay
                            ON pay.BookingID = bk.BookingID
                        WHERE pay.PaymentStatus = 'Paid'
                          AND bk.UserID IS NOT NULL
                        GROUP BY bk.UserID
                    ) p ON p.UserID = u.UserID
                    WHERE u.RoleID = @RoleID
                      AND (@Search = ''
                           OR u.FirstName LIKE @SearchPattern
                           OR u.LastName LIKE @SearchPattern
                           OR u.Email LIKE @SearchPattern
                           OR ISNULL(u.Phone, '') LIKE @SearchPattern)
                      AND (@Status = '' OR u.IsActive = @IsActive)
                      AND (
                          @JoinDate = ''
                          OR (@JoinDate = 'month'
                              AND u.CreatedDate >= @MonthStart
                              AND u.CreatedDate < @NextMonth)
                          OR (@JoinDate = '30days'
                              AND u.CreatedDate >= @ThirtyDaysAgo)
                          OR (@JoinDate = 'year'
                              AND u.CreatedDate >= @YearStart
                              AND u.CreatedDate < @NextYear)
                      )
                    ORDER BY u.CreatedDate DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddCustomerFilterParameters(
                        command, customerRoleID, search, status, joinDate,
                        monthStart, monthStart.AddMonths(1),
                        now.Date.AddDays(-30),
                        new DateTime(now.Year, 1, 1),
                        new DateTime(now.Year + 1, 1, 1));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder csv = new StringBuilder();

                        csv.AppendLine(
                            "User ID,First Name,Last Name,Email,Phone,Total Bookings,Total Spent (UGX),Status,Join Date");

                        while (reader.Read())
                        {
                            csv.Append(Csv(reader["UserID"])).Append(",");
                            csv.Append(Csv(reader["FirstName"])).Append(",");
                            csv.Append(Csv(reader["LastName"])).Append(",");
                            csv.Append(Csv(reader["Email"])).Append(",");
                            csv.Append(Csv(reader["Phone"])).Append(",");
                            csv.Append(Csv(reader["TotalBookings"])).Append(",");
                            csv.Append(Csv(Convert.ToDecimal(reader["TotalSpent"])
                                .ToString("0.00", CultureInfo.InvariantCulture)))
                                .Append(",");
                            csv.Append(Csv(Convert.ToBoolean(reader["IsActive"])
                                ? "Active" : "Inactive")).Append(",");
                            csv.AppendLine(Csv(Convert.ToDateTime(
                                reader["CreatedDate"]).ToString("dd MMM yyyy")));
                        }

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "text/csv";
                        Response.ContentEncoding = Encoding.UTF8;
                        Response.AddHeader(
                            "Content-Disposition",
                            "attachment;filename=RxEventsCustomers.csv");

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