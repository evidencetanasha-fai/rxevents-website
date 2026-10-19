
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;
using System.IO;

namespace RxEvents
{
    public partial class AdminBookings : System.Web.UI.Page
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
                LoadEventFilter();
                LoadBookingStatistics();
                BindBookings();
                ClearBookingDetails();
            }
        }

        // --------------------------------------------------
        // SUMMARY CARDS
        // --------------------------------------------------
        private void LoadBookingStatistics()
        {
            string query = @"
                SELECT
                    COUNT(*) AS TotalBookings,
                    SUM(CASE WHEN LOWER(BookingStatus) = 'confirmed'
                        THEN 1 ELSE 0 END) AS ConfirmedBookings,
                    SUM(CASE WHEN LOWER(BookingStatus) = 'completed'
                        THEN 1 ELSE 0 END) AS CompletedBookings,
                    SUM(CASE WHEN LOWER(BookingStatus) = 'cancelled'
                        THEN 1 ELSE 0 END) AS CancelledBookings
                FROM Bookings;";

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
                        TotalBookingsCount.Text =
                            Convert.ToInt32(reader["TotalBookings"])
                                .ToString("N0");

                        ConfirmedBookingsCount.Text =
                            Convert.ToInt32(reader["ConfirmedBookings"])
                                .ToString("N0");

                        CompletedBookingsCount.Text =
                            Convert.ToInt32(reader["CompletedBookings"])
                                .ToString("N0");

                        CancelledBookingsCount.Text =
                            Convert.ToInt32(reader["CancelledBookings"])
                                .ToString("N0");
                    }
                }
            }
        }

        // --------------------------------------------------
        // EVENT FILTER
        // --------------------------------------------------
        private void LoadEventFilter()
        {
            string query = @"
                SELECT DISTINCT e.EventID, e.EventName
                FROM Events e
                INNER JOIN Bookings b ON b.EventID = e.EventID
                ORDER BY e.EventName;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    EventFilter.DataSource = reader;
                    EventFilter.DataTextField = "EventName";
                    EventFilter.DataValueField = "EventID";
                    EventFilter.DataBind();
                }
            }

            EventFilter.Items.Insert(
                0, new ListItem("All Events", ""));
        }

        // --------------------------------------------------
        // BIND BOOKINGS GRID
        // --------------------------------------------------
        private void BindBookings()
        {
            string query = @"
                SELECT
                    b.BookingID,
                    b.BookingReference,
                    b.UserID,
                    b.EventID,
                    b.VenueID,
                    b.PackageID,
                    b.EventDate,
                    b.EventTime,
                    b.GuestCount,
                    b.TotalAmount,
                    b.BookingStatus,
                    u.FirstName + ' ' + u.LastName AS CustomerName,
                    u.Phone AS CustomerPhone,
                    u.Email AS CustomerEmail,
                    e.EventName,
                    e.EventImage,
                    v.VenueName,
                    v.Location AS VenueLocation,
                    p.PackageName,
                    ISNULL(pay.PaidAmount, 0) AS PaidAmount,
                    ISNULL(pay.PaymentStatus, 'Unpaid') AS PaymentStatus
                FROM Bookings b
                INNER JOIN Users u ON b.UserID = u.UserID
                INNER JOIN Events e ON b.EventID = e.EventID
                LEFT JOIN Venues v ON b.VenueID = v.VenueID
                LEFT JOIN Packages p ON b.PackageID = p.PackageID
                OUTER APPLY
                (
                    SELECT
                        SUM(CASE
                            WHEN LOWER(PaymentStatus) IN
                                ('paid', 'successful', 'completed')
                            THEN Amount ELSE 0
                        END) AS PaidAmount,
                        MAX(PaymentStatus) AS PaymentStatus
                    FROM Payments
                    WHERE BookingID = b.BookingID
                ) pay
                WHERE
                    (@Search = ''
                     OR CAST(b.BookingID AS VARCHAR(20)) LIKE '%' + @Search + '%'
                     OR ISNULL(b.BookingReference, '') LIKE '%' + @Search + '%'
                     OR u.FirstName + ' ' + u.LastName LIKE '%' + @Search + '%')
                    AND (@Status = ''
                         OR LOWER(b.BookingStatus) = LOWER(@Status))
                    AND (@EventID = 0 OR b.EventID = @EventID)
                    AND (@StartDate IS NULL OR b.EventDate >= @StartDate)
                    AND (@EndDate IS NULL OR b.EventDate < DATEADD(day, 1, @EndDate))
                ORDER BY b.BookingDate DESC, b.BookingID DESC;";

            DateTime? startDate = ParseDate(StartDateTextBox.Text);
            DateTime? endDate = ParseDate(EndDateTextBox.Text);

            if (startDate.HasValue && endDate.HasValue &&
                startDate.Value.Date > endDate.Value.Date)
            {
                BookingsMessage.Text =
                    "The start date cannot be later than the end date.";
                return;
            }

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Search",
                    SqlDbType.VarChar, 200).Value =
                    SearchBookingsTextBox.Text.Trim();

                command.Parameters.Add("@Status",
                    SqlDbType.VarChar, 50).Value =
                    StatusFilter.SelectedValue;

                command.Parameters.Add("@EventID",
                    SqlDbType.Int).Value =
                    string.IsNullOrEmpty(EventFilter.SelectedValue)
                    ? 0
                    : Convert.ToInt32(EventFilter.SelectedValue);

                command.Parameters.Add("@StartDate",
                    SqlDbType.Date).Value =
                    startDate.HasValue
                    ? (object)startDate.Value.Date
                    : DBNull.Value;

                command.Parameters.Add("@EndDate",
                    SqlDbType.Date).Value =
                    endDate.HasValue
                    ? (object)endDate.Value.Date
                    : DBNull.Value;

                DataTable table = new DataTable();

                using (SqlDataAdapter adapter =
                       new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }

                BookingsGrid.DataSource = table;
                BookingsGrid.DataBind();

                BookingResultsLabel.Text =
                    table.Rows.Count == 0
                    ? "No bookings found."
                    : "Showing " + table.Rows.Count +
                      " matching booking(s).";
            }
        }

        private DateTime? ParseDate(string value)
        {
            DateTime date;

            if (DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
            {
                return date.Date;
            }

            return null;
        }

        // --------------------------------------------------
        // SEARCH AND FILTERS
        // --------------------------------------------------
        protected void SearchBookingsButton_Click(
            object sender, EventArgs e)
        {
            BookingsGrid.PageIndex = 0;
            BookingsMessage.Text = "";
            BindBookings();
            ClearBookingDetails();
        }

        protected void BookingFilters_Changed(
            object sender, EventArgs e)
        {
            BookingsGrid.PageIndex = 0;
            BookingsMessage.Text = "";
            BindBookings();
            ClearBookingDetails();
        }

        protected void ClearBookingFiltersButton_Click(
            object sender, EventArgs e)
        {
            SearchBookingsTextBox.Text = "";
            StatusFilter.SelectedIndex = 0;
            EventFilter.SelectedIndex = 0;
            StartDateTextBox.Text = "";
            EndDateTextBox.Text = "";

            BookingsGrid.PageIndex = 0;
            BookingsMessage.Text = "";

            BindBookings();
            ClearBookingDetails();
        }

        // --------------------------------------------------
        // PAGINATION
        // --------------------------------------------------
        protected void BookingsGrid_PageIndexChanging(
            object sender, GridViewPageEventArgs e)
        {
            BookingsGrid.PageIndex = e.NewPageIndex;
            BindBookings();
            ClearBookingDetails();
        }

        // --------------------------------------------------
        // STATUS COLORS
        // --------------------------------------------------
        protected void BookingsGrid_RowDataBound(
            object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            // Status is column index 6.
            if (e.Row.Cells.Count <= 6)
                return;

            string status = e.Row.Cells[6].Text.Trim();

            switch (status.ToLowerInvariant())
            {
                case "confirmed":
                    e.Row.Cells[6].ForeColor =
                        System.Drawing.Color.Green;
                    break;

                case "completed":
                    e.Row.Cells[6].ForeColor =
                        System.Drawing.Color.FromArgb(13, 185, 76);
                    break;

                case "pending":
                    e.Row.Cells[6].ForeColor =
                        System.Drawing.Color.FromArgb(225, 165, 0);
                    break;

                case "cancelled":
                case "rejected":
                    e.Row.Cells[6].ForeColor =
                        System.Drawing.Color.Red;
                    break;
            }
        }

        // --------------------------------------------------
        // SELECTED BOOKING DETAILS
        // --------------------------------------------------
        protected void BookingsGrid_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (BookingsGrid.SelectedDataKey == null)
                return;

            int bookingID = Convert.ToInt32(
                BookingsGrid.SelectedDataKey.Value);

            string query = @"
                SELECT
                    b.BookingID,
                    b.BookingReference,
                    b.EventDate,
                    b.EventTime,
                    b.TotalAmount,
                    b.BookingStatus,
                    u.FirstName + ' ' + u.LastName AS CustomerName,
                    u.Phone AS CustomerPhone,
                    u.Email AS CustomerEmail,
                    e.EventName,
                    e.EventImage,
                    v.VenueName,
                    v.Location AS VenueLocation,
                    p.PackageName,
                    ISNULL(pay.PaidAmount, 0) AS PaidAmount,
                    ISNULL(pay.PaymentStatus, 'Unpaid') AS PaymentStatus
                FROM Bookings b
                INNER JOIN Users u ON b.UserID = u.UserID
                INNER JOIN Events e ON b.EventID = e.EventID
                LEFT JOIN Venues v ON b.VenueID = v.VenueID
                LEFT JOIN Packages p ON b.PackageID = p.PackageID
                OUTER APPLY
                (
                    SELECT
                        SUM(CASE
                            WHEN LOWER(PaymentStatus) IN
                                ('paid', 'successful', 'completed')
                            THEN Amount ELSE 0
                        END) AS PaidAmount,
                        MAX(PaymentStatus) AS PaymentStatus
                    FROM Payments
                    WHERE BookingID = b.BookingID
                ) pay
                WHERE b.BookingID = @BookingID;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add("@BookingID",
                    SqlDbType.Int).Value = bookingID;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SelectedBookingReference.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["BookingReference"]));

                        SelectedCustomerName.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["CustomerName"]));

                        SelectedCustomerPhone.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["CustomerPhone"]));

                        SelectedCustomerEmail.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["CustomerEmail"]));

                        SelectedEventName.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["EventName"]));

                        DateTime eventDate =
                            Convert.ToDateTime(reader["EventDate"]);

                        string eventTime = "";

                        if (reader["EventTime"] != DBNull.Value)
                        {
                            TimeSpan time =
                                (TimeSpan)reader["EventTime"];

                            eventTime = DateTime.Today
                                .Add(time)
                                .ToString("h:mm tt");
                        }

                        SelectedEventDate.Text =
                            eventDate.ToString("dd/MM/yyyy") +
                            (eventTime == "" ? "" : " | " + eventTime);

                        SelectedVenueName.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["VenueName"]));

                        SelectedPackageName.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["PackageName"]));

                        SelectedTotalAmount.Text =
                            Convert.ToDecimal(reader["TotalAmount"])
                                .ToString("N0");

                        SelectedPaidAmount.Text =
                            Convert.ToDecimal(reader["PaidAmount"])
                                .ToString("N0");

                        SelectedPaymentStatus.Text =
                            Server.HtmlEncode(
                                Convert.ToString(reader["PaymentStatus"]));

                        string status =
                            Convert.ToString(reader["BookingStatus"]);

                        SelectedBookingStatus.Text =
                            Server.HtmlEncode(status);

                        string normalizedStatus =
                            status.ToLowerInvariant();

                        SelectedBookingStatus.CssClass =
                            "booking-status-badge " +
                            (normalizedStatus == "confirmed"
                                ? "status-confirmed"
                            : normalizedStatus == "completed"
                                ? "status-completed"
                            : normalizedStatus == "cancelled" ||
                              normalizedStatus == "rejected"
                                ? "status-cancelled"
                                : "status-pending");

                        string imagePath =
                            Convert.ToString(reader["EventImage"]);

                        if (!string.IsNullOrWhiteSpace(imagePath))
                        {
                            imagePath = imagePath.Trim();

                            // Accept only local website paths.
                            if (!imagePath.StartsWith("http",
                                StringComparison.OrdinalIgnoreCase) &&
                                !imagePath.StartsWith("//"))
                            {
                                if (imagePath.StartsWith("~/"))
                                {
                                    BookingEventImage.ImageUrl =
                                        ResolveUrl(imagePath);
                                }
                                else
                                {
                                    BookingEventImage.ImageUrl =
                                        ResolveUrl(
                                            "~/" + imagePath.TrimStart('/'));
                                }

                                BookingEventImage.Visible = true;
                            }
                            else
                            {
                                BookingEventImage.Visible = false;
                            }
                        }
                        else
                        {
                            BookingEventImage.Visible = false;
                        }

                        ViewFullBookingLink.NavigateUrl =
                            "~/AdminBookings.aspx?bookingId=" +
                            bookingID;
                    }
                }
            }
        }

        private void ClearBookingDetails()
        {
            SelectedBookingReference.Text = "Select a booking";
            SelectedBookingStatus.Text = "";
            SelectedCustomerName.Text = "—";
            SelectedCustomerPhone.Text = "";
            SelectedCustomerEmail.Text = "";
            SelectedEventName.Text = "—";
            SelectedEventDate.Text = "—";
            SelectedVenueName.Text = "—";
            SelectedPackageName.Text = "—";
            SelectedTotalAmount.Text = "0";
            SelectedPaidAmount.Text = "0";
            SelectedPaymentStatus.Text = "—";

            BookingEventImage.Visible = false;
            SelectedBookingStatus.CssClass = "booking-status-badge";
        }

        // --------------------------------------------------
        // EXPORT FILTERED BOOKINGS TO CSV
        // --------------------------------------------------

        protected void ExportBookingsButton_Click(
            object sender, EventArgs e)
        {
            // Reuse the same filters as the bookings grid.
            DateTime? startDate = ParseDate(StartDateTextBox.Text);
            DateTime? endDate = ParseDate(EndDateTextBox.Text);

            if (startDate.HasValue && endDate.HasValue &&
                startDate.Value.Date > endDate.Value.Date)
            {
                BookingsMessage.Text =
                    "The start date cannot be later than the end date.";
                return;
            }

            string query = @"
        SELECT
            b.BookingReference,
            COALESCE(
                NULLIF(LTRIM(RTRIM(
                    u.FirstName + ' ' + u.LastName
                )), ''),
                b.GuestCustomerName,
                'Guest Customer'
            ) AS CustomerName,
            e.EventName,
            b.EventDate,
            b.EventTime,
            COALESCE(
                v.VenueName,
                b.VenueAddress,
                'Not specified'
            ) AS VenueName,
            p.PackageName,
            b.GuestCount,
            b.TotalAmount,
            b.BookingStatus,
            b.BookingDate
        FROM dbo.Bookings b
        LEFT JOIN dbo.Users u ON b.UserID = u.UserID
        INNER JOIN dbo.Events e ON b.EventID = e.EventID
        LEFT JOIN dbo.Venues v ON b.VenueID = v.VenueID
        INNER JOIN dbo.Packages p ON b.PackageID = p.PackageID
        WHERE
            (@Search = ''
             OR CAST(b.BookingID AS VARCHAR(20))
                    LIKE '%' + @Search + '%'
             OR ISNULL(b.BookingReference, '')
                    LIKE '%' + @Search + '%'
             OR COALESCE(
                    NULLIF(LTRIM(RTRIM(
                        u.FirstName + ' ' + u.LastName
                    )), ''),
                    b.GuestCustomerName, ''
                ) LIKE '%' + @Search + '%')
            AND (@Status = ''
                 OR LOWER(b.BookingStatus) = LOWER(@Status))
            AND (@EventID = 0 OR b.EventID = @EventID)
            AND (@StartDate IS NULL
                 OR b.EventDate >= @StartDate)
            AND (@EndDate IS NULL
                 OR b.EventDate < DATEADD(day, 1, @EndDate))
        ORDER BY b.BookingDate DESC, b.BookingID DESC;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@Search", SqlDbType.VarChar, 200).Value =
                    SearchBookingsTextBox.Text.Trim();

                command.Parameters.Add(
                    "@Status", SqlDbType.VarChar, 50).Value =
                    StatusFilter.SelectedValue;

                command.Parameters.Add(
                    "@EventID", SqlDbType.Int).Value =
                    string.IsNullOrEmpty(EventFilter.SelectedValue)
                        ? 0
                        : Convert.ToInt32(EventFilter.SelectedValue);

                command.Parameters.Add(
                    "@StartDate", SqlDbType.Date).Value =
                    startDate.HasValue
                        ? (object)startDate.Value.Date
                        : DBNull.Value;

                command.Parameters.Add(
                    "@EndDate", SqlDbType.Date).Value =
                    endDate.HasValue
                        ? (object)endDate.Value.Date
                        : DBNull.Value;

                DataTable table = new DataTable();

                using (SqlDataAdapter adapter =
                       new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }

                StringBuilder csv = new StringBuilder();

                csv.AppendLine(
                    "Booking Reference,Customer,Event,Date,Time,Venue,Package,Guests,Total Amount,Status,Booking Date");

                foreach (DataRow row in table.Rows)
                {
                    csv.AppendLine(
                        Csv(row["BookingReference"]) + "," +
                        Csv(row["CustomerName"]) + "," +
                        Csv(row["EventName"]) + "," +
                        Csv(Convert.ToDateTime(row["EventDate"])
                            .ToString("dd/MM/yyyy")) + "," +
                        Csv(row["EventTime"]) + "," +
                        Csv(row["VenueName"]) + "," +
                        Csv(row["PackageName"]) + "," +
                        Csv(row["GuestCount"]) + "," +
                        Csv(row["TotalAmount"]) + "," +
                        Csv(row["BookingStatus"]) + "," +
                        Csv(Convert.ToDateTime(row["BookingDate"])
                            .ToString("dd/MM/yyyy HH:mm")));
                }

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "text/csv";
                Response.ContentEncoding = Encoding.UTF8;

                Response.AddHeader(
                    "Content-Disposition",
                    "attachment;filename=RxEventsBookings.csv");

                Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                Response.Write(csv.ToString());
                Response.Flush();

                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private string Csv(object value)
        {
            string text = value == null || value == DBNull.Value
                ? ""
                : Convert.ToString(value,
                    CultureInfo.InvariantCulture);

            // Prevent spreadsheet formula injection.
            if (text.Length > 0 &&
                (text[0] == '=' || text[0] == '+' ||
                 text[0] == '-' || text[0] == '@'))
            {
                text = "'" + text;
            }

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}
