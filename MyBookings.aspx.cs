
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class MyBookings : System.Web.UI.Page
    {
        private readonly BookingRepository bookingRepository =
            new BookingRepository();

        private DataTable AllBookings
        {
            get
            {
                return ViewState["AllBookings"] as DataTable;
            }
            set
            {
                ViewState["AllBookings"] = value;
            }
        }

        private string ActiveTab
        {
            get
            {
                return ViewState["ActiveTab"] as string ?? "All";
            }
            set
            {
                ViewState["ActiveTab"] = value;
            }
        }

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
                LoadBookings();
            }
        }

        private void LoadBookings()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                AllBookings =
                    bookingRepository.GetCustomerBookingHistory(userId);

                UpdateTabCounts();
                BindBookings();
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to load your bookings right now. Please try again.";
                BookingsRepeater.DataSource = null;
                BookingsRepeater.DataBind();
            }
        }

        private void UpdateTabCounts()
        {
            DataTable bookings = AllBookings;

            int all = 0;
            int upcoming = 0;
            int completed = 0;
            int cancelled = 0;

            if (bookings != null)
            {
                foreach (DataRow row in bookings.Rows)
                {
                    all++;

                    string status =
                        row["BookingStatus"].ToString();

                    if (status == "Cancelled")
                    {
                        cancelled++;
                    }
                    else if (status == "Completed")
                    {
                        completed++;
                    }
                    else if (status == "Pending" ||
                             status == "Confirmed")
                    {
                        DateTime eventDate =
                            Convert.ToDateTime(row["EventDate"]);

                        if (eventDate.Date >= DateTime.Today)
                        {
                            upcoming++;
                        }
                    }
                }
            }

            AllCountLabel.Text = all.ToString();
            UpcomingCountLabel.Text = upcoming.ToString();
            CompletedCountLabel.Text = completed.ToString();
            CancelledCountLabel.Text = cancelled.ToString();
        }

        protected void StatusTab_Command(
            object sender, CommandEventArgs e)
        {
            ActiveTab = e.CommandArgument.ToString();

            AllTab.CssClass = "booking-tab";
            UpcomingTab.CssClass = "booking-tab";
            CompletedTab.CssClass = "booking-tab";
            CancelledTab.CssClass = "booking-tab";

            switch (ActiveTab)
            {
                case "Upcoming":
                    UpcomingTab.CssClass =
                        "booking-tab booking-tab-active";
                    break;

                case "Completed":
                    CompletedTab.CssClass =
                        "booking-tab booking-tab-active";
                    break;

                case "Cancelled":
                    CancelledTab.CssClass =
                        "booking-tab booking-tab-active";
                    break;

                default:
                    ActiveTab = "All";
                    AllTab.CssClass =
                        "booking-tab booking-tab-active";
                    break;
            }

            BindBookings();
        }

        protected void SearchButton_Click(
            object sender, EventArgs e)
        {
            BindBookings();
        }

        private void BindBookings()
        {
            DataTable source = AllBookings;

            if (source == null)
            {
                BookingsRepeater.DataSource = null;
                BookingsRepeater.DataBind();
                return;
            }

            DataTable filtered = source.Clone();

            string search = SearchTextBox.Text.Trim();
            string statusFilter = StatusFilter.SelectedValue;

            foreach (DataRow row in source.Rows)
            {
                string status = row["BookingStatus"].ToString();

                bool matchesTab = true;

                if (ActiveTab == "Upcoming")
                {
                    DateTime eventDate =
                        Convert.ToDateTime(row["EventDate"]);

                    matchesTab =
                        (status == "Pending" ||
                         status == "Confirmed") &&
                        eventDate.Date >= DateTime.Today;
                }
                else if (ActiveTab == "Completed")
                {
                    matchesTab = status == "Completed";
                }
                else if (ActiveTab == "Cancelled")
                {
                    matchesTab = status == "Cancelled";
                }

                bool matchesStatus =
                    statusFilter == "All" ||
                    status == statusFilter;

                bool matchesSearch =
                    string.IsNullOrWhiteSpace(search) ||
                    row["EventName"].ToString().IndexOf(
                        search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    row["BookingReference"].ToString().IndexOf(
                        search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    row["PackageName"].ToString().IndexOf(
                        search, StringComparison.OrdinalIgnoreCase) >= 0;

                if (matchesTab && matchesStatus && matchesSearch)
                {
                    filtered.ImportRow(row);
                }
            }

            BookingsRepeater.DataSource = filtered;
            BookingsRepeater.DataBind();

            MessageLabel.Text = filtered.Rows.Count == 0
                ? "No bookings match your search."
                : "";
        }


        protected string GetStatusCss(
            object statusValue, object eventDateValue)
        {
            string status = statusValue == null
                ? ""
                : statusValue.ToString();

            if (status == "Cancelled")
                return "booking-status status-cancelled";

            if (status == "Completed")
                return "booking-status status-completed";

            if (status == "Pending")
                return "booking-status status-pending";

            if (status == "Confirmed")
            {
                DateTime eventDate =
                    Convert.ToDateTime(eventDateValue);

                if (eventDate.Date < DateTime.Today)
                    return "booking-status status-completed";

                return "booking-status status-upcoming";
            }

            return "booking-status";
        }


        protected void ViewDetails_Command(
            object sender, CommandEventArgs e)
        {
            int bookingId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out bookingId))
            {
                MessageLabel.Text = "Invalid booking selection.";
                return;
            }

            // Recheck ownership before opening a booking.
            DataTable bookings = AllBookings;

            bool belongsToCustomer = false;

            if (bookings != null)
            {
                foreach (DataRow row in bookings.Rows)
                {
                    if (Convert.ToInt32(row["BookingID"]) == bookingId)
                    {
                        belongsToCustomer = true;
                        break;
                    }
                }
            }

            if (!belongsToCustomer)
            {
                MessageLabel.Text = "Booking not found.";
                return;
            }

            Response.Redirect(
                "BookingDetails.aspx?bookingId=" +
                bookingId.ToString(CultureInfo.InvariantCulture),
                false);

            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
