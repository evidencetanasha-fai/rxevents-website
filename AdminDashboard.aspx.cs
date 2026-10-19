
using System;
using System.Data.SqlClient;
using System.Globalization;
using RxEvents.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace RxEvents
{
    public partial class AdminDashboard : System.Web.UI.Page
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
                LoadDashboardStatistics();
                LoadBookingsChart();
                LoadRevenueChart();
                LoadUpcomingEvent();
                LoadRecentBookings();
            }
        }

        private void LoadDashboardStatistics()
        {
            string query = @"
                SELECT
                    (SELECT COUNT(*)
                     FROM Bookings) AS TotalBookings,

                    (SELECT COUNT(*)
 FROM Bookings
 WHERE EventDate >= CAST(GETDATE() AS DATE)
   AND BookingStatus NOT IN ('Cancelled', 'Rejected')
) AS TotalEvents,

                    (SELECT COUNT(*)
                     FROM Users
                     WHERE RoleID = 2) AS TotalCustomers,

                    (SELECT ISNULL(SUM(Amount), 0)
                     FROM Payments
                     WHERE LOWER(PaymentStatus) IN
                           ('paid', 'successful', 'completed'))
                     AS TotalRevenue;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TotalBookingsValue.Text =
                            Convert.ToInt32(
                                reader["TotalBookings"])
                            .ToString("N0");

                        TotalEventsValue.Text =
                            Convert.ToInt32(
                                reader["TotalEvents"])
                            .ToString("N0");

                        TotalCustomersValue.Text =
                            Convert.ToInt32(
                                reader["TotalCustomers"])
                            .ToString("N0");

                        decimal revenue =
                            Convert.ToDecimal(
                                reader["TotalRevenue"]);

                        TotalRevenueValue.Text =
                            revenue.ToString(
                                "N0",
                                CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        protected void RecentBookingsGrid_RowDataBound(
            object sender,
            System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType !=
                System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                return;
            }

            int statusColumnIndex = 6;

            if (e.Row.Cells.Count <= statusColumnIndex)
            {
                return;
            }

            string status =
                e.Row.Cells[statusColumnIndex].Text.Trim();

            switch (status.ToLowerInvariant())
            {
                case "confirmed":
                case "completed":
                    e.Row.Cells[statusColumnIndex].ForeColor =
                        System.Drawing.Color.FromArgb(13, 185, 76);
                    break;

                case "pending":
                    e.Row.Cells[statusColumnIndex].ForeColor =
                        System.Drawing.Color.FromArgb(225, 165, 0);
                    break;

                case "cancelled":
                    e.Row.Cells[statusColumnIndex].ForeColor =
                        System.Drawing.Color.Red;
                    break;
            }
        }

        private DateTime GetPeriodStart(string period)
        {
            DateTime today = DateTime.Today;

            switch (period)
            {
                case "This month":
                    return new DateTime(today.Year, today.Month, 1);

                case "This year":
                    return new DateTime(today.Year, 1, 1);

                default: // This week, starting Monday
                    int daysSinceMonday =
                        ((int)today.DayOfWeek + 6) % 7;

                    return today.AddDays(-daysSinceMonday);
            }
        }

        private void LoadBookingsChart()
        {
            DateTime startDate =
                GetPeriodStart(BookingsPeriod.SelectedValue);

            DateTime endDate = DateTime.Today.AddDays(1);

            string query = @"
        SELECT CAST(EventDate AS DATE) AS ChartDate,
               COUNT(*) AS Total
        FROM Bookings
        WHERE EventDate >= @StartDate
          AND EventDate < @EndDate
          AND BookingStatus <> 'Cancelled'
        GROUP BY CAST(EventDate AS DATE)
        ORDER BY ChartDate;";

            Dictionary<DateTime, int> totals =
                new Dictionary<DateTime, int>();

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totals[Convert.ToDateTime(reader["ChartDate"])] =
                            Convert.ToInt32(reader["Total"]);
                    }
                }
            }

            // The existing markup contains seven columns labelled Mon-Sun.
            // Update each bar with the count for its corresponding day.
            for (int i = 0; i < 7; i++)
            {
                DateTime date = startDate.AddDays(i);

                int count = totals.ContainsKey(date)
                    ? totals[date]
                    : 0;

                // The chart bars will be converted to server controls in Step 2.
            }
        }

        private void LoadRevenueChart()
        {
            DateTime startDate =
                GetPeriodStart(RevenuePeriod.SelectedValue);

            DateTime endDate = DateTime.Today.AddDays(1);

            string query = @"
        SELECT CAST(PaymentDate AS DATE) AS ChartDate,
               SUM(Amount) AS Total
        FROM Payments
        WHERE PaymentDate >= @StartDate
          AND PaymentDate < @EndDate
          AND LOWER(PaymentStatus) IN
              ('paid', 'successful', 'completed')
        GROUP BY CAST(PaymentDate AS DATE)
        ORDER BY ChartDate;";

            Dictionary<DateTime, decimal> totals =
                new Dictionary<DateTime, decimal>();

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totals[Convert.ToDateTime(reader["ChartDate"])] =
                            Convert.ToDecimal(reader["Total"]);
                    }
                }
            }

            // The existing markup must be changed to server controls
            // before the returned amounts can set each bar's height.
        }

        protected void BookingsPeriod_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            LoadBookingsChart();
        }

        protected void RevenuePeriod_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            LoadRevenueChart();
        }

        private void LoadUpcomingEvent()
        {
            string query = @"
        SELECT TOP 1
            B.BookingID,
            B.EventDate,
            B.EventTime,
            B.BookingStatus,
            E.EventName,
            E.EventImage,
            V.VenueName
        FROM Bookings B
        INNER JOIN Events E
            ON B.EventID = E.EventID
        LEFT JOIN Venues V
            ON B.VenueID = V.VenueID
        WHERE B.EventDate >= @Today
          AND B.BookingStatus NOT IN ('Cancelled', 'Rejected')
        ORDER BY B.EventDate ASC, B.EventTime ASC;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Today", DateTime.Today);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UpcomingEventName.Text =
                            reader["EventName"].ToString();

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

                        UpcomingEventDate.Text =
                            eventDate.ToString("dd/MM/yyyy") +
                            (string.IsNullOrEmpty(eventTime)
                                ? ""
                                : " | " + eventTime);

                        UpcomingEventVenue.Text =
                            reader["VenueName"] == DBNull.Value
                                ? "Venue not specified"
                                : reader["VenueName"].ToString();

                        UpcomingEventStatus.Text =
                            reader["BookingStatus"].ToString();


                        string imagePath = reader["EventImage"] == DBNull.Value
                            ? ""
                            : reader["EventImage"].ToString().Trim();

                        if (!string.IsNullOrWhiteSpace(imagePath))
                        {
                            if (!imagePath.StartsWith("~/") &&
                                !imagePath.StartsWith("/"))
                            {
                                imagePath = "~/Content/images/" + imagePath;
                            }

                            UpcomingEventImage.ImageUrl = ResolveUrl(imagePath);
                            UpcomingEventImage.Visible = true;
                        }
                        else
                        {
                            UpcomingEventImage.Visible = false;
                        }

                        UpcomingEventImage.AlternateText =
                            reader["EventName"].ToString();

                        UpcomingEventStatus.CssClass =
                            reader["BookingStatus"].ToString()
                                .Equals("Confirmed",
                                    StringComparison.OrdinalIgnoreCase)
                            ? "event-status-confirmed"
                            : "event-status-pending";
                    }
                    else
                    {
                        UpcomingEventName.Text = "No upcoming events";
                        UpcomingEventDate.Text =
                            "There are no upcoming bookings.";
                        UpcomingEventVenue.Text = "";
                        UpcomingEventStatus.Text = "";
                        UpcomingEventImage.Visible = false;
                    }
                }
            }
        }

        private void LoadRecentBookings()
        {
            string query = @"
        SELECT TOP 5
            B.BookingReference AS BookingID,
            LTRIM(RTRIM(
                ISNULL(U.FirstName, '') + ' ' +
                ISNULL(U.LastName, '')
            )) AS CustomerName,
            E.EventName,
            B.EventDate AS BookingDate,
            ISNULL(V.VenueName, B.VenueAddress) AS Venue,
            B.TotalAmount AS Amount,
            B.BookingStatus AS Status
        FROM Bookings B
        INNER JOIN Users U
            ON B.UserID = U.UserID
        INNER JOIN Events E
            ON B.EventID = E.EventID
        LEFT JOIN Venues V
            ON B.VenueID = V.VenueID
        ORDER BY B.BookingDate DESC, B.BookingID DESC;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                System.Data.DataTable bookings =
                    new System.Data.DataTable();

                adapter.Fill(bookings);

                RecentBookingsGrid.DataSource = bookings;
                RecentBookingsGrid.DataBind();

                BookingsEmptyMessage.Visible =
                    bookings.Rows.Count == 0;
            }
        }


    }
}
