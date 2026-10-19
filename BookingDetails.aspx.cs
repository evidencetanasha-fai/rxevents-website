
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using RxEvents.Data;

namespace RxEvents
{
    public partial class BookingDetails : System.Web.UI.Page
    {
        private readonly DatabaseHelper databaseHelper =
            new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadBookingDetails();
            }
        }

        private void LoadBookingDetails()
        {
            int bookingId;

            if (!int.TryParse(
                Request.QueryString["bookingId"], out bookingId) ||
                bookingId <= 0)
            {
                ShowError("Invalid booking reference.");
                return;
            }

            string query = @"
                SELECT
                    b.BookingReference,
                    b.EventDate,
                    b.EventTime,
                    b.VenueAddress,
                    b.GuestCount,
                    b.TotalAmount,
                    b.BookingStatus,
                    b.BookingDate,
                    b.AdditionalNotes,
                    e.EventName,
                    p.PackageName
                FROM Bookings b
                INNER JOIN Events e
                    ON b.EventID = e.EventID
                INNER JOIN Packages p
                    ON b.PackageID = p.PackageID
                WHERE b.BookingID = @BookingID
                  AND b.UserID = @UserID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@BookingID", SqlDbType.Int).Value = bookingId;

                command.Parameters.Add(
                    "@UserID", SqlDbType.Int).Value =
                    Convert.ToInt32(Session["UserID"]);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        ShowError(
                            "Booking not found, or you do not have permission to view it.");
                        return;
                    }

                    BookingReferenceLabel.Text =
                        Server.HtmlEncode(
                            reader["BookingReference"].ToString());

                    EventNameLabel.Text =
                        Server.HtmlEncode(
                            reader["EventName"].ToString());

                    DateTime eventDate =
                        Convert.ToDateTime(reader["EventDate"]);

                    EventDateLabel.Text =
                        eventDate.ToString(
                            "dd MMM yyyy",
                            CultureInfo.InvariantCulture);

                    TimeSpan eventTime =
                        (TimeSpan)reader["EventTime"];

                    EventTimeLabel.Text =
                        DateTime.Today.Add(eventTime).ToString(
                            "hh:mm tt",
                            CultureInfo.InvariantCulture);

                    VenueLabel.Text =
                        Server.HtmlEncode(
                            reader["VenueAddress"] == DBNull.Value
                                ? "Not specified"
                                : reader["VenueAddress"].ToString());

                    PackageLabel.Text =
                        Server.HtmlEncode(
                            reader["PackageName"].ToString());

                    GuestCountLabel.Text =
                        reader["GuestCount"].ToString();

                    AmountLabel.Text =
                        "UGX " + Convert.ToDecimal(
                            reader["TotalAmount"]).ToString(
                                "N0",
                                CultureInfo.InvariantCulture);

                    BookingStatusLabel.Text =
                        Server.HtmlEncode(
                            reader["BookingStatus"].ToString());

                    BookingDateLabel.Text =
                        Convert.ToDateTime(
                            reader["BookingDate"]).ToString(
                                "dd MMM yyyy, hh:mm tt",
                                CultureInfo.InvariantCulture);

                    NotesLabel.Text =
                        Server.HtmlEncode(
                            reader["AdditionalNotes"] == DBNull.Value ||
                            string.IsNullOrWhiteSpace(
                                reader["AdditionalNotes"].ToString())
                                ? "No additional notes"
                                : reader["AdditionalNotes"].ToString());

                    BookingPanel.Visible = true;
                }
            }
        }

        private void ShowError(string message)
        {
            MessageLabel.Text = Server.HtmlEncode(message);
            BookingPanel.Visible = false;
        }

        protected void BackButton_Click(
            object sender, EventArgs e)
        {
            Response.Redirect("MyBookings.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
