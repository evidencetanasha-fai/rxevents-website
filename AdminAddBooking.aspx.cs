
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminAddBooking : System.Web.UI.Page
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
                LoadEvents();
                LoadVenues();
                LoadPackages();

                EventDateInput.Attributes["min"] =
                    DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        private void LoadEvents()
        {
            LoadDropdown(
                EventDropdown,
                @"SELECT EventID, EventName
                  FROM Events
                  WHERE IsActive = 1
                  ORDER BY EventName;",
                "EventName",
                "EventID",
                "-- Select Event --");
        }

        private void LoadVenues()
        {
            LoadDropdown(
                VenueDropdown,
                @"SELECT VenueID, VenueName
                  FROM Venues
                  WHERE IsActive = 1
                  ORDER BY VenueName;",
                "VenueName",
                "VenueID",
                "-- Select Venue (Optional) --");
        }

        private void LoadPackages()
        {
            LoadDropdown(
                PackageDropdown,
                @"SELECT PackageID, PackageName
                  FROM Packages
                  WHERE IsActive = 1
                  ORDER BY PackageName;",
                "PackageName",
                "PackageID",
                "-- Select Package --");
        }

        private void LoadDropdown(
            DropDownList dropdown,
            string query,
            string textField,
            string valueField,
            string defaultText)
        {
            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                dropdown.DataSource = table;
                dropdown.DataTextField = textField;
                dropdown.DataValueField = valueField;
                dropdown.DataBind();

                dropdown.Items.Insert(
                    0, new ListItem(defaultText, ""));
            }
        }

        protected void EventDropdown_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (EventDropdown.SelectedValue == "")
                return;

            string query = @"
                SELECT BasePrice
                FROM Events
                WHERE EventID = @EventID
                  AND IsActive = 1;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@EventID", SqlDbType.Int).Value =
                    Convert.ToInt32(EventDropdown.SelectedValue);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    decimal price = Convert.ToDecimal(result);

                    TotalAmountInput.Text =
                        price.ToString(
                            "0.00",
                            CultureInfo.InvariantCulture);
                }
            }
        }

        protected void PackageDropdown_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (PackageDropdown.SelectedValue == "")
                return;

            string query = @"
                SELECT Price
                FROM Packages
                WHERE PackageID = @PackageID
                  AND IsActive = 1;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@PackageID", SqlDbType.Int).Value =
                    Convert.ToInt32(PackageDropdown.SelectedValue);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    decimal price = Convert.ToDecimal(result);

                    TotalAmountInput.Text =
                        price.ToString(
                            "0.00",
                            CultureInfo.InvariantCulture);
                }
            }
        }

        protected void SaveBookingButton_Click(
            object sender, EventArgs e)
        {
            BookingMessage.Text = "";

            // A guest booking has no UserID.
            // A registered customer may be linked to an existing account.
            int? userID = null;

            string customerName =
                CustomerNameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(customerName))
            {
                ShowError("Please enter the customer's name.");
                return;
            }

            if (customerName.Length > 200)
            {
                ShowError(
                    "The customer's name cannot exceed 200 characters.");
                return;
            }

            int eventID;
            int packageID;
            int venueID = 0;
            int guestCount;
            decimal totalAmount;
            DateTime eventDate;
            TimeSpan eventTime;

            if (!int.TryParse(
                    EventDropdown.SelectedValue, out eventID) ||
                !int.TryParse(
                    PackageDropdown.SelectedValue, out packageID))
            {
                ShowError("Please select an event and package.");
                return;
            }

            bool hasVenue =
                !string.IsNullOrWhiteSpace(
                    VenueDropdown.SelectedValue);

            if (hasVenue &&
                !int.TryParse(
                    VenueDropdown.SelectedValue, out venueID))
            {
                ShowError("Please select a valid venue.");
                return;
            }

            if (!DateTime.TryParseExact(
                    EventDateInput.Text,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out eventDate))
            {
                ShowError("Please enter a valid event date.");
                return;
            }

            if (eventDate.Date < DateTime.Today)
            {
                ShowError("The event date cannot be in the past.");
                return;
            }

            if (!TimeSpan.TryParse(
                    EventTimeInput.Text, out eventTime) ||
                eventTime < TimeSpan.Zero ||
                eventTime >= TimeSpan.FromDays(1))
            {
                ShowError("Please enter a valid event time.");
                return;
            }

            if (!int.TryParse(
                    GuestCountInput.Text, out guestCount) ||
                guestCount <= 0)
            {
                ShowError(
                    "The number of guests must be greater than zero.");
                return;
            }

            if (!decimal.TryParse(
                    TotalAmountInput.Text,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out totalAmount) ||
                totalAmount < 0)
            {
                ShowError(
                    "Please enter a valid, non-negative total amount.");
                return;
            }

            string status = BookingStatusDropdown.SelectedValue;

            if (status != "Pending" && status != "Confirmed")
            {
                ShowError("Please select a valid booking status.");
                return;
            }

            string notes = BookingNotesInput.Text.Trim();
            string venueAddress = VenueAddressInput.Text.Trim();

            if (notes.Length > 1000)
            {
                ShowError(
                    "Additional notes cannot exceed 1000 characters.");
                return;
            }

            if (venueAddress.Length > 500)
            {
                ShowError(
                    "The venue address cannot exceed 500 characters.");
                return;
            }

            // If an active customer account exists with this exact name,
            // link the booking to it. Otherwise, keep UserID as NULL.
            using (SqlConnection customerConnection =
                   databaseHelper.GetConnection())
            using (SqlCommand customerCommand =
                   new SqlCommand(@"
                       SELECT TOP 1 UserID
                       FROM Users
                       WHERE RoleID = 2
                         AND IsActive = 1
                         AND LTRIM(RTRIM(
                             FirstName + ' ' + LastName
                         )) = @CustomerName
                       ORDER BY UserID;",
                       customerConnection))
            {
                customerCommand.Parameters.Add(
                    "@CustomerName",
                    SqlDbType.VarChar, 200).Value = customerName;

                customerConnection.Open();

                object result = customerCommand.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    userID = Convert.ToInt32(result);
                }
            }

            // Create the booking in a database transaction.
            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction(
                           IsolationLevel.Serializable))
                {
                    try
                    {
                        // Validate event, package and optional customer.
                        string validationQuery = @"
                            SELECT CASE
                                WHEN EXISTS (
                                    SELECT 1
                                    FROM Events
                                    WHERE EventID = @EventID
                                      AND IsActive = 1
                                )
                                AND EXISTS (
                                    SELECT 1
                                    FROM Packages
                                    WHERE PackageID = @PackageID
                                      AND IsActive = 1
                                )
                                AND (
                                    @UserID IS NULL
                                    OR EXISTS (
                                        SELECT 1
                                        FROM Users
                                        WHERE UserID = @UserID
                                          AND RoleID = 2
                                          AND IsActive = 1
                                    )
                                )
                                THEN 1
                                ELSE 0
                            END;";

                        using (SqlCommand validationCommand =
                               new SqlCommand(
                                   validationQuery,
                                   connection,
                                   transaction))
                        {
                            validationCommand.Parameters.Add(
                                "@UserID", SqlDbType.Int).Value =
                                userID.HasValue
                                    ? (object)userID.Value
                                    : DBNull.Value;

                            validationCommand.Parameters.Add(
                                "@EventID", SqlDbType.Int).Value =
                                eventID;

                            validationCommand.Parameters.Add(
                                "@PackageID", SqlDbType.Int).Value =
                                packageID;

                            int isValid = Convert.ToInt32(
                                validationCommand.ExecuteScalar());

                            if (isValid != 1)
                            {
                                transaction.Rollback();

                                ShowError(
                                    "The selected event or package is no longer available, or the linked customer account is inactive.");

                                return;
                            }
                        }

                        // Validate the optional venue.
                        if (hasVenue)
                        {
                            using (SqlCommand venueCommand =
                                   new SqlCommand(
                                       @"SELECT COUNT(*)
                                         FROM Venues
                                         WHERE VenueID = @VenueID
                                           AND IsActive = 1;",
                                       connection,
                                       transaction))
                            {
                                venueCommand.Parameters.Add(
                                    "@VenueID", SqlDbType.Int).Value =
                                    venueID;

                                if (Convert.ToInt32(
                                    venueCommand.ExecuteScalar()) != 1)
                                {
                                    transaction.Rollback();

                                    ShowError(
                                        "The selected venue is unavailable.");

                                    return;
                                }
                            }
                        }

                        string bookingReference =
                            GenerateBookingReference(
                                connection, transaction);

                        string insertQuery = @"
                            INSERT INTO dbo.Bookings
                            (
                                BookingReference,
                                UserID,
                                GuestCustomerName,
                                EventID,
                                VenueID,
                                PackageID,
                                EventDate,
                                EventTime,
                                GuestCount,
                                TotalAmount,
                                BookingStatus,
                                AdditionalNotes,
                                VenueAddress
                            )
                            VALUES
                            (
                                @BookingReference,
                                @UserID,
                                @GuestCustomerName,
                                @EventID,
                                @VenueID,
                                @PackageID,
                                @EventDate,
                                @EventTime,
                                @GuestCount,
                                @TotalAmount,
                                @BookingStatus,
                                @AdditionalNotes,
                                @VenueAddress
                            );";

                        using (SqlCommand command =
                               new SqlCommand(
                                   insertQuery,
                                   connection,
                                   transaction))
                        {
                            command.Parameters.Add(
                                "@BookingReference",
                                SqlDbType.VarChar, 30).Value =
                                bookingReference;

                            command.Parameters.Add(
                                "@UserID", SqlDbType.Int).Value =
                                userID.HasValue
                                    ? (object)userID.Value
                                    : DBNull.Value;

                            command.Parameters.Add(
                                "@GuestCustomerName",
                                SqlDbType.VarChar, 200).Value =
                                customerName;

                            command.Parameters.Add(
                                "@EventID", SqlDbType.Int).Value =
                                eventID;

                            command.Parameters.Add(
                                "@VenueID", SqlDbType.Int).Value =
                                hasVenue
                                    ? (object)venueID
                                    : DBNull.Value;

                            command.Parameters.Add(
                                "@PackageID", SqlDbType.Int).Value =
                                packageID;

                            command.Parameters.Add(
                                "@EventDate", SqlDbType.Date).Value =
                                eventDate.Date;

                            command.Parameters.Add(
                                "@EventTime", SqlDbType.Time).Value =
                                eventTime;

                            command.Parameters.Add(
                                "@GuestCount", SqlDbType.Int).Value =
                                guestCount;

                            SqlParameter amountParameter =
                                command.Parameters.Add(
                                    "@TotalAmount", SqlDbType.Decimal);

                            amountParameter.Precision = 12;
                            amountParameter.Scale = 2;
                            amountParameter.Value = totalAmount;

                            command.Parameters.Add(
                                "@BookingStatus",
                                SqlDbType.VarChar, 30).Value =
                                status;

                            command.Parameters.Add(
                                "@AdditionalNotes",
                                SqlDbType.VarChar, 1000).Value =
                                string.IsNullOrWhiteSpace(notes)
                                    ? (object)DBNull.Value
                                    : notes;

                            command.Parameters.Add(
                                "@VenueAddress",
                                SqlDbType.VarChar, 500).Value =
                                string.IsNullOrWhiteSpace(venueAddress)
                                    ? (object)DBNull.Value
                                    : venueAddress;

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // The transaction may already have ended.
                        }

                        ShowError(
                            "The booking could not be saved. Please check the database configuration and selected records, then try again.");

                        return;
                    }
                }
            }

            // Redirect only after the transaction succeeds.
            Response.Redirect(
                "~/AdminBookings.aspx?created=1",
                false);

            Context.ApplicationInstance.CompleteRequest();
        }

        private string GenerateBookingReference(
            SqlConnection connection,
            SqlTransaction transaction)
        {
            string reference;

            do
            {
                reference = "RXB-" +
                    DateTime.Now.ToString("yyyyMMdd") + "-" +
                    Guid.NewGuid().ToString("N")
                        .Substring(0, 6).ToUpperInvariant();

                using (SqlCommand command =
                       new SqlCommand(
                           @"SELECT COUNT(*)
                             FROM dbo.Bookings
                             WHERE BookingReference = @Reference;",
                           connection,
                           transaction))
                {
                    command.Parameters.Add(
                        "@Reference",
                        SqlDbType.VarChar, 30).Value = reference;

                    if (Convert.ToInt32(
                        command.ExecuteScalar()) == 0)
                    {
                        break;
                    }
                }
            }
            while (true);

            return reference;
        }

        private void ShowError(string message)
        {
            BookingMessage.Text = Server.HtmlEncode(message);
        }
    }
}
