using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminReports : Page
    {
        private DateTime StartDate
        {
            get
            {
                DateTime date;
                if (DateTime.TryParseExact(
                    StartDateInput.Text, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date))
                    return date.Date;

                return DateTime.Today.AddMonths(-11).AddDays(
                    1 - DateTime.Today.Day);
            }
        }

        private DateTime EndDate
        {
            get
            {
                DateTime date;
                if (DateTime.TryParseExact(
                    EndDateInput.Text, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date))
                    return date.Date;

                return DateTime.Today;
            }
        }

        private int SelectedEventID
        {
            get
            {
                int id;
                return int.TryParse(
                    EventFilterDropdown.SelectedValue, out id) ? id : 0;
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
                StartDateInput.Text = StartDate.ToString("yyyy-MM-dd");
                EndDateInput.Text = DateTime.Today.ToString("yyyy-MM-dd");

                LoadEventFilter();
                LoadReport();
            }
        }

        private void LoadEventFilter()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT EventID, EventName
                    FROM dbo.Events
                    ORDER BY EventName";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        EventFilterDropdown.DataSource = reader;
                        EventFilterDropdown.DataTextField = "EventName";
                        EventFilterDropdown.DataValueField = "EventID";
                        EventFilterDropdown.DataBind();
                    }
                }
            }

            EventFilterDropdown.Items.Insert(
                0, new System.Web.UI.WebControls.ListItem(
                    "All Events", ""));
        }

        private void AddReportParameters(SqlCommand command)
        {
            command.Parameters.Add("@StartDate", SqlDbType.Date)
                .Value = StartDate;

            // Exclusive end date includes all times on the selected end date.
            command.Parameters.Add("@EndDateExclusive", SqlDbType.Date)
                .Value = EndDate.AddDays(1);

            command.Parameters.Add("@EventID", SqlDbType.Int)
                .Value = SelectedEventID;
        }

        private string BookingFilter()
        {
            return @"
                b.EventDate >= @StartDate
                AND b.EventDate < @EndDateExclusive
                AND (@EventID = 0 OR b.EventID = @EventID)";
        }

        private void LoadReport()
        {
            if (StartDate > EndDate)
            {
                ReportMessage.Text =
                    "Start date cannot be later than end date.";
                ReportMessage.ForeColor =
                    System.Drawing.Color.Firebrick;
                return;
            }

            ReportMessage.Text = "";

            LoadKpis();
            LoadMonthlyRevenue();
            LoadTopEvents();
            LoadBookingStatuses();
            LoadRecentBookings();
        }

        private void LoadKpis()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                connection.Open();

                string filter = BookingFilter();

                const string bookingQuery = @"
                    SELECT COUNT(*)
                    FROM dbo.Bookings b
                    WHERE " + "";

                using (SqlCommand command = new SqlCommand(
                    bookingQuery + filter, connection))
                {
                    AddReportParameters(command);
                    TotalBookingsValue.Text =
                        Convert.ToInt32(command.ExecuteScalar())
                            .ToString("N0");
                }

                const string revenueQuery = @"
                    SELECT ISNULL(SUM(p.Amount), 0)
                    FROM dbo.Payments p
                    INNER JOIN dbo.Bookings b
                        ON p.BookingID = b.BookingID
                    WHERE p.PaymentStatus = 'Paid'
                      AND " + "";

                using (SqlCommand command = new SqlCommand(
                    revenueQuery + filter, connection))
                {
                    AddReportParameters(command);

                    decimal revenue = Convert.ToDecimal(
                        command.ExecuteScalar());

                    TotalRevenueValue.Text =
                        "UGX " + revenue.ToString("N0");
                }

                const string customersQuery = @"
                    SELECT COUNT(DISTINCT b.UserID)
                    FROM dbo.Bookings b
                    WHERE b.UserID IS NOT NULL
                      AND " + "";

                using (SqlCommand command = new SqlCommand(
                    customersQuery + filter, connection))
                {
                    AddReportParameters(command);

                    TotalCustomersValue.Text =
                        Convert.ToInt32(command.ExecuteScalar())
                            .ToString("N0");
                }

                const string ratingQuery = @"
                    SELECT AVG(CAST(r.Rating AS DECIMAL(5,2)))
                    FROM dbo.Reviews r
                    INNER JOIN dbo.Bookings b
                        ON r.BookingID = b.BookingID
                    WHERE r.IsApproved = 1
                      AND " + "";

                using (SqlCommand command = new SqlCommand(
                    ratingQuery + filter, connection))
                {
                    AddReportParameters(command);
                    object result = command.ExecuteScalar();

                    AverageRatingValue.Text =
                        result == DBNull.Value || result == null
                            ? "0.0/5"
                            : Convert.ToDecimal(result)
                                .ToString("0.0") + "/5";
                }
            }
        }

        private void LoadMonthlyRevenue()
        {
            DataTable monthlyData = new DataTable();
            monthlyData.Columns.Add("MonthLabel", typeof(string));
            monthlyData.Columns.Add("DisplayValue", typeof(string));
            monthlyData.Columns.Add("BarHeight", typeof(int));

            DataTable raw = new DataTable();

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT YEAR(b.EventDate) AS ReportYear,
                           MONTH(b.EventDate) AS ReportMonth,
                           SUM(p.Amount) AS Revenue
                    FROM dbo.Payments p
                    INNER JOIN dbo.Bookings b
                        ON p.BookingID = b.BookingID
                    WHERE p.PaymentStatus = 'Paid'
                      AND b.EventDate >= @StartDate
                      AND b.EventDate < @EndDateExclusive
                      AND (@EventID = 0 OR b.EventID = @EventID)
                    GROUP BY YEAR(b.EventDate), MONTH(b.EventDate)
                    ORDER BY ReportYear, ReportMonth";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    AddReportParameters(command);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(raw);
                    }
                }
            }

            decimal maxRevenue = 0;

            foreach (DataRow row in raw.Rows)
            {
                maxRevenue = Math.Max(
                    maxRevenue, Convert.ToDecimal(row["Revenue"]));
            }

            // Display each month in the selected range.
            DateTime month = new DateTime(
                StartDate.Year, StartDate.Month, 1);

            DateTime lastMonth = new DateTime(
                EndDate.Year, EndDate.Month, 1);

            while (month <= lastMonth)
            {
                decimal revenue = 0;

                foreach (DataRow row in raw.Rows)
                {
                    if (Convert.ToInt32(row["ReportYear"]) == month.Year &&
                        Convert.ToInt32(row["ReportMonth"]) == month.Month)
                    {
                        revenue += Convert.ToDecimal(row["Revenue"]);
                        break;
                    }
                }

                int height = maxRevenue <= 0
                    ? 2
                    : Math.Max(2, (int)Math.Round(
                        revenue / maxRevenue * 100));

                DataRow newRow = monthlyData.NewRow();
                newRow["MonthLabel"] = month.ToString("MMM");
                newRow["DisplayValue"] = revenue >= 1000000
                    ? (revenue / 1000000).ToString("0.#") + "M"
                    : revenue >= 1000
                        ? (revenue / 1000).ToString("0.#") + "K"
                        : revenue.ToString("0");

                newRow["BarHeight"] = height;
                monthlyData.Rows.Add(newRow);

                month = month.AddMonths(1);
            }

            MonthlyRevenueRepeater.DataSource = monthlyData;
            MonthlyRevenueRepeater.DataBind();
        }

        private void LoadTopEvents()
        {
            DataTable events = new DataTable();

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT TOP 5 e.EventName,
                           COUNT(b.BookingID) AS BookingCount
                    FROM dbo.Bookings b
                    INNER JOIN dbo.Events e
                        ON b.EventID = e.EventID
                    WHERE " + "";

                string fullQuery = query + BookingFilter() + @"
                    GROUP BY e.EventName
                    ORDER BY COUNT(b.BookingID) DESC, e.EventName";

                using (SqlCommand command =
                    new SqlCommand(fullQuery, connection))
                {
                    AddReportParameters(command);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(events);
                    }
                }
            }

            events.Columns.Add("BarWidth", typeof(int));

            int maxCount = 0;

            foreach (DataRow row in events.Rows)
            {
                maxCount = Math.Max(
                    maxCount, Convert.ToInt32(row["BookingCount"]));
            }

            foreach (DataRow row in events.Rows)
            {
                row["BarWidth"] = maxCount == 0
                    ? 0
                    : Math.Max(4, (int)Math.Round(
                        Convert.ToInt32(row["BookingCount"]) /
                        (double)maxCount * 100));
            }

            TopEventsRepeater.DataSource = events;
            TopEventsRepeater.DataBind();

            TopEventsEmptyLabel.Visible = events.Rows.Count == 0;
        }

        private void LoadBookingStatuses()
        {
            DataTable statuses = new DataTable();

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT b.BookingStatus,
                           COUNT(*) AS BookingCount
                    FROM dbo.Bookings b
                    WHERE " + "";

                string fullQuery = query + BookingFilter() + @"
                    GROUP BY b.BookingStatus
                    ORDER BY b.BookingStatus";

                using (SqlCommand command =
                    new SqlCommand(fullQuery, connection))
                {
                    AddReportParameters(command);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(statuses);
                    }
                }
            }

            int total = 0;

            foreach (DataRow row in statuses.Rows)
            {
                total += Convert.ToInt32(row["BookingCount"]);
            }

            statuses.Columns.Add("Percentage", typeof(int));
            statuses.Columns.Add("StatusColor", typeof(string));

            foreach (DataRow row in statuses.Rows)
            {
                int count = Convert.ToInt32(row["BookingCount"]);

                row["Percentage"] = total == 0
                    ? 0
                    : (int)Math.Round(count * 100.0 / total);

                string status = Convert.ToString(row["BookingStatus"]);

                switch (status)
                {
                    case "Confirmed":
                        row["StatusColor"] = "#39b980";
                        break;
                    case "Pending":
                        row["StatusColor"] = "#8750df";
                        break;
                    case "Completed":
                        row["StatusColor"] = "#f3b52f";
                        break;
                    case "Cancelled":
                        row["StatusColor"] = "#f05e78";
                        break;
                    default:
                        row["StatusColor"] = "#999999";
                        break;
                }
            }

            BookingStatusRepeater.DataSource = statuses;
            BookingStatusRepeater.DataBind();

            BookingStatusEmptyLabel.Visible = statuses.Rows.Count == 0;
        }

        private void LoadRecentBookings()
        {
            DataTable bookings = new DataTable();

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT TOP 8
                        b.BookingReference,
                        COALESCE(
                            NULLIF(LTRIM(RTRIM(
                                ISNULL(u.FirstName, '') + ' ' +
                                ISNULL(u.LastName, ''))), ''),
                            b.GuestCustomerName,
                            'Guest Customer') AS CustomerName,
                        e.EventName,
                        b.EventDate,
                        b.TotalAmount,
                        b.BookingStatus
                    FROM dbo.Bookings b
                    LEFT JOIN dbo.Users u
                        ON b.UserID = u.UserID
                    INNER JOIN dbo.Events e
                        ON b.EventID = e.EventID
                    WHERE " + "";

                string fullQuery = query + BookingFilter() + @"
                    ORDER BY b.BookingDate DESC, b.BookingID DESC";

                using (SqlCommand command =
                    new SqlCommand(fullQuery, connection))
                {
                    AddReportParameters(command);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(bookings);
                    }
                }
            }

            RecentBookingsGridView.DataSource = bookings;
            RecentBookingsGridView.DataBind();
        }

        protected void GenerateReportButton_Click(
            object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void ClearReportFiltersButton_Click(
            object sender, EventArgs e)
        {
            StartDateInput.Text = DateTime.Today
                .AddMonths(-11).AddDays(1 - DateTime.Today.Day)
                .ToString("yyyy-MM-dd");

            EndDateInput.Text = DateTime.Today.ToString("yyyy-MM-dd");
            ReportTypeDropdown.SelectedIndex = 0;
            EventFilterDropdown.SelectedIndex = 0;

            LoadReport();
        }

        protected void ExportReportButton_Click(
            object sender, EventArgs e)
        {
            if (StartDate > EndDate)
            {
                ReportMessage.Text =
                    "Start date cannot be later than end date.";
                ReportMessage.ForeColor =
                    System.Drawing.Color.Firebrick;
                return;
            }

            DataTable data = new DataTable();

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                string query;

                switch (ReportTypeDropdown.SelectedValue)
                {
                    case "Bookings":
                        query = @"
                            SELECT b.BookingReference,
                                COALESCE(
                                    NULLIF(LTRIM(RTRIM(
                                        ISNULL(u.FirstName, '') + ' ' +
                                        ISNULL(u.LastName, ''))), ''),
                                    b.GuestCustomerName, 'Guest Customer')
                                    AS Customer,
                                e.EventName, b.EventDate,
                                b.GuestCount, b.TotalAmount,
                                b.BookingStatus
                            FROM dbo.Bookings b
                            LEFT JOIN dbo.Users u ON b.UserID = u.UserID
                            INNER JOIN dbo.Events e ON b.EventID = e.EventID
                            WHERE " + BookingFilter() + @"
                            ORDER BY b.EventDate";
                        break;

                    case "Revenue":
                        query = @"
                            SELECT b.BookingReference, e.EventName,
                                p.PaymentReference, p.Amount,
                                p.PaymentMethod, p.PaymentDate,
                                p.PaymentStatus
                            FROM dbo.Payments p
                            INNER JOIN dbo.Bookings b
                                ON p.BookingID = b.BookingID
                            INNER JOIN dbo.Events e
                                ON b.EventID = e.EventID
                            WHERE b.EventDate >= @StartDate
                              AND b.EventDate < @EndDateExclusive
                              AND (@EventID = 0 OR b.EventID = @EventID)
                            ORDER BY p.PaymentDate";
                        break;

                    case "Customers":
                        query = @"
                            SELECT DISTINCT u.UserID, u.FirstName,
                                u.LastName, u.Email, u.Phone
                            FROM dbo.Bookings b
                            INNER JOIN dbo.Users u ON b.UserID = u.UserID
                            WHERE " + BookingFilter() + @"
                            ORDER BY u.LastName, u.FirstName";
                        break;

                    case "Reviews":
                        query = @"
                            SELECT r.ReviewID,
                                u.FirstName + ' ' + u.LastName AS Customer,
                                e.EventName, r.Rating, r.Comment,
                                r.ReviewDate, r.IsApproved
                            FROM dbo.Reviews r
                            INNER JOIN dbo.Users u ON r.UserID = u.UserID
                            INNER JOIN dbo.Bookings b
                                ON r.BookingID = b.BookingID
                            INNER JOIN dbo.Events e ON r.EventID = e.EventID
                            WHERE r.ReviewDate >= @StartDate
                              AND r.ReviewDate < @EndDateExclusive
                              AND (@EventID = 0 OR r.EventID = @EventID)
                            ORDER BY r.ReviewDate DESC";
                        break;

                    default:
                        query = @"
                            SELECT b.BookingReference,
                                COALESCE(
                                    NULLIF(LTRIM(RTRIM(
                                        ISNULL(u.FirstName, '') + ' ' +
                                        ISNULL(u.LastName, ''))), ''),
                                    b.GuestCustomerName, 'Guest Customer')
                                    AS Customer,
                                e.EventName, b.EventDate,
                                b.TotalAmount, b.BookingStatus
                            FROM dbo.Bookings b
                            LEFT JOIN dbo.Users u ON b.UserID = u.UserID
                            INNER JOIN dbo.Events e ON b.EventID = e.EventID
                            WHERE " + BookingFilter() + @"
                            ORDER BY b.EventDate";
                        break;
                }

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    AddReportParameters(command);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            StringBuilder csv = new StringBuilder();

            for (int column = 0; column < data.Columns.Count; column++)
            {
                if (column > 0) csv.Append(",");
                csv.Append(Csv(data.Columns[column].ColumnName));
            }

            csv.AppendLine();

            foreach (DataRow row in data.Rows)
            {
                for (int column = 0; column < data.Columns.Count; column++)
                {
                    if (column > 0) csv.Append(",");
                    csv.Append(Csv(row[column]));
                }

                csv.AppendLine();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AddHeader(
                "Content-Disposition",
                "attachment;filename=RxEventsReport.csv");

            Response.Write("\uFEFF" + csv.ToString());
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
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