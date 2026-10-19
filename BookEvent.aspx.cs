using RxEvents.Data;
using RxEvents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace RxEvents
{
    public partial class BookEvent : System.Web.UI.Page
    {
        private readonly RxEvents.Data.DatabaseHelper databaseHelper =
            new RxEvents.Data.DatabaseHelper();
        private readonly PackageRepository packageRepository =
    new PackageRepository();
        private readonly WishlistRepository wishlistRepository =
    new WishlistRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadEventCategories();
                LoadVenues();
                SetDefaultDate();
            }
        }

        // ==========================================
        // LOAD EVENT CATEGORIES
        // ==========================================

        private void LoadEventCategories()
        {
            EventCategoryDropDown.Items.Clear();

            EventCategoryDropDown.Items.Add(
                new ListItem(
                    "Select event type",
                    ""));

            string query = @"
                SELECT
                    CategoryID,
                    CategoryName
                FROM EventCategories
                WHERE IsActive = 1
                ORDER BY CategoryName ASC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EventCategoryDropDown.Items.Add(
                            new ListItem(
                                reader["CategoryName"].ToString(),
                                reader["CategoryID"].ToString()));
                    }
                }
            }
        }

        // ==========================================
        // LOAD EVENTS BY CATEGORY
        // ==========================================

        private void LoadEventsByCategory(int categoryId)
        {
            EventDropDown.Items.Clear();

            EventDropDown.Items.Add(
                new ListItem(
                    "Select event",
                    ""));

            string query = @"
                SELECT
                    EventID,
                    EventName,
                    BasePrice,
                    EventImage
                FROM Events
                WHERE CategoryID = @CategoryID
                  AND IsActive = 1
                ORDER BY EventName ASC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@CategoryID",
                    categoryId);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EventDropDown.Items.Add(
                            new ListItem(
                                reader["EventName"].ToString(),
                                reader["EventID"].ToString()));
                    }
                }
            }
        }

        // ==========================================
        // LOAD VENUES
        // ==========================================

        private void LoadVenues()
        {
            VenueDropDown.Items.Clear();

            VenueDropDown.Items.Add(
                new ListItem(
                    "Select venue",
                    ""));

            string query = @"
                SELECT
                    VenueID,
                    VenueName,
                    Location,
                    Capacity,
                    Price
                FROM Venues
                WHERE IsActive = 1
                ORDER BY VenueName ASC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string venueName =
                            reader["VenueName"].ToString();

                        string location =
                            reader["Location"].ToString();

                        string displayName =
                            venueName +
                            " — " +
                            location;

                        VenueDropDown.Items.Add(
                            new ListItem(
                                displayName,
                                reader["VenueID"].ToString()));
                    }
                }
                VenueDropDown.Items.Add(
    new ListItem(
        "My Own Venue / Home",
        "OWN"));
            }
        }

        // ==========================================
        // EVENT CATEGORY CHANGED
        // ==========================================

        protected void EventCategoryDropDown_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                EventCategoryDropDown.SelectedValue))
            {
                EventDropDown.Items.Clear();

                EventDropDown.Items.Add(
                    new ListItem(
                        "Select event",
                        ""));

                ClearEventPreview();

                return;
            }

            int categoryId =
                Convert.ToInt32(
                    EventCategoryDropDown.SelectedValue);

            LoadEventsByCategory(categoryId);

            ClearEventPreview();
        }

        // ==========================================
        // EVENT CHANGED
        // ==========================================

        protected void EventDropDown_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                EventDropDown.SelectedValue))
            {
                ClearEventPreview();
                return;
            }

            int eventId =
                Convert.ToInt32(
                    EventDropDown.SelectedValue);

            LoadEventPreview(eventId);
        }

        // ==========================================
        // LOAD EVENT PREVIEW
        // ==========================================

        private void LoadEventPreview(int eventId)
        {
            string query = @"
                SELECT
                    EventName,
                    EventImage,
                    BasePrice
                FROM Events
                WHERE EventID = @EventID
                  AND IsActive = 1";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@EventID",
                    eventId);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string eventName =
                            reader["EventName"].ToString();

                        decimal basePrice =
                            Convert.ToDecimal(
                                reader["BasePrice"]);

                        EventPreviewNameLabel.Text =
                            eventName;

                        PricePreviewLabel.Text =
                            "Starting from UGX " +
                            basePrice.ToString(
                                "N0",
                                CultureInfo.InvariantCulture);

                        string image =
                            reader["EventImage"] == DBNull.Value
                                ? ""
                                : reader["EventImage"].ToString();

                        if (!string.IsNullOrWhiteSpace(image))
                        {
                            EventPreviewImage.ImageUrl =
                                ResolveUrl(
                                    "~/Content/images/" +
                                    image);
                        }
                        else
                        {
                            EventPreviewImage.ImageUrl =
                                ResolveUrl(
                                    "~/Content/images/rx-events-logo.png");
                        }
                    }
                }
            }
        }

        // ==========================================
        // DEFAULT DATE
        // ==========================================

        private void SetDefaultDate()
        {
            EventDateTextBox.Text =
                DateTime.Today
                    .AddDays(1)
                    .ToString("yyyy-MM-dd");
        }

        // ==========================================
        // CLEAR EVENT PREVIEW
        // ==========================================

        private void ClearEventPreview()
        {
            EventPreviewImage.ImageUrl =
                ResolveUrl(
                    "~/Content/images/rx-events-logo.png");

            EventPreviewNameLabel.Text =
                "Choose an event";

            PricePreviewLabel.Text =
                "Starting from UGX 0";
        }

        // ==========================================
        // CLEAR FORM
        // ==========================================

        protected void ClearFormButton_Click(
            object sender,
            EventArgs e)
        {
            EventCategoryDropDown.SelectedIndex = 0;

            EventDropDown.Items.Clear();

            EventDropDown.Items.Add(
                new ListItem(
                    "Select event",
                    ""));

            VenueDropDown.SelectedIndex = 0;

            EventDateTextBox.Text =
                DateTime.Today
                    .AddDays(1)
                    .ToString("yyyy-MM-dd");

            EventTimeTextBox.Text = "";

            GuestCountTextBox.Text = "";

            AdditionalNotesTextBox.Text = "";

            BookEventMessageLabel.Text = "";

            ClearEventPreview();
        }

        // ==========================================
        // NEXT: SELECT PACKAGE
        // ==========================================

        protected void NextPackageButton_Click(
            object sender,
            EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            DateTime eventDate;

            if (!DateTime.TryParse(
                EventDateTextBox.Text,
                out eventDate))
            {
                BookEventMessageLabel.Text =
                    "Please enter a valid event date.";

                return;
            }

            if (eventDate.Date <= DateTime.Today)
            {
                BookEventMessageLabel.Text =
                    "Please select a future event date.";

                return;
            }

            TimeSpan eventTime;

            if (!TimeSpan.TryParse(
                EventTimeTextBox.Text,
                out eventTime))
            {
                BookEventMessageLabel.Text =
                    "Please enter a valid event time.";

                return;
            }

            int guestCount;

            if (!int.TryParse(
                GuestCountTextBox.Text,
                out guestCount) ||
                guestCount <= 0)
            {
                BookEventMessageLabel.Text =
                    "Please enter a valid guest count.";

                return;
            }

            // Save Step 1 information temporarily.
            Session["BookEvent_EventID"] =
                Convert.ToInt32(
                    EventDropDown.SelectedValue);

            if (VenueDropDown.SelectedValue == "OWN")
            {
                Session["BookEvent_VenueID"] = null;
                Session["BookEvent_VenueAddress"] =
                    VenueAddressTextBox.Text.Trim();
            }
            else
            {
                Session["BookEvent_VenueID"] =
                    Convert.ToInt32(VenueDropDown.SelectedValue);

                Session["BookEvent_VenueAddress"] = null;
            }

            Session["BookEvent_Date"] =
                eventDate;

            Session["BookEvent_Time"] =
                eventTime;

            Session["BookEvent_GuestCount"] =
                guestCount;

            Session["BookEvent_Notes"] =
                AdditionalNotesTextBox.Text.Trim();

            /*
             * Step 2 will be connected here.
             *
             * We are deliberately not redirecting yet
             * because BookEventPackage.aspx will be
             * created in the next step.
             */

            BookEventMessageLabel.Text =
                "Event details saved. Package selection is next.";
            Step1Panel.Visible = false;
            Step2Panel.Visible = true;

            LoadPackages();
        }
        protected void VenueDropDown_SelectedIndexChanged(
    object sender,
    EventArgs e)
        {
            if (VenueDropDown.SelectedValue == "OWN")
            {
                OwnVenueAddressPanel.Visible = true;
                VenueAddressValidator.Enabled = true;
            }
            else
            {
                OwnVenueAddressPanel.Visible = false;
                VenueAddressValidator.Enabled = false;
                VenueAddressTextBox.Text = "";
            }
        }
        private void LoadPackages()
        {
            PackagesRepeater.DataSource =
                packageRepository.GetActivePackages();

            PackagesRepeater.DataBind();
        }
        private void LoadWishlistFolders()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            var folders =
                wishlistRepository.GetFoldersByUser(userId);

            WishlistFoldersRepeater.DataSource = folders;
            WishlistFoldersRepeater.DataBind();

            NoWishlistFoldersPanel.Visible =
                folders == null || folders.Count == 0;
        }
        protected void PackagesRepeater_ItemCommand(
    object source,
    RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectPackage")
            {
                int packageId =
                    Convert.ToInt32(e.CommandArgument);

                Session["BookEvent_PackageID"] = packageId;
                PackagesRepeater.DataSource =
    packageRepository.GetActivePackages();

                PackagesRepeater.DataBind();

                PackageSelectionMessageLabel.Text =
                    "Package selected successfully. You can continue to the next step.";
            }
        }
        protected void BackToDetailsButton_Click(
    object sender,
    EventArgs e)
        {
            Step2Panel.Visible = false;
            Step1Panel.Visible = true;
        }
        protected void NextAdditionalInfoButton_Click(
    object sender,
    EventArgs e)
        {
            if (Session["BookEvent_PackageID"] == null)
            {
                PackageSelectionMessageLabel.Text =
                    "Please select a package before continuing.";

                Step2Panel.Visible = true;
                Step3Panel.Visible = false;

                return;
            }

            Step2Panel.Visible = false;
            Step3Panel.Visible = true;

            LoadWishlistFolders();
        }
        protected void WishlistFoldersRepeater_ItemCommand(
    object source,
    RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectInspiration")
            {
                int folderId =
                    Convert.ToInt32(e.CommandArgument);

                Session["BookEvent_WishlistFolderID"] =
                    folderId;

                LoadWishlistFolders();

                InspirationMessageLabel.Text =
                    "Your inspiration folder has been selected successfully.";
            }
        }
        protected string GetPackageImageUrl(object imagePath)
        {
            if (imagePath == null ||
                imagePath == DBNull.Value ||
                string.IsNullOrWhiteSpace(imagePath.ToString()))
            {
                return ResolveUrl(
                    "~/Content/images/rx-events-logo.png");
            }

            string path = imagePath.ToString().Trim();

            if (path.StartsWith("~/"))
            {
                return ResolveUrl(path);
            }

            if (path.StartsWith("/"))
            {
                return ResolveUrl("~" + path);
            }

            return ResolveUrl(
                "~/Content/images/" + path);
        }
        protected string GetPackageCardClass(object packageId)
        {
            if (Session["BookEvent_PackageID"] == null)
            {
                return "rx-book-package-card";
            }

            int selectedPackageId =
                Convert.ToInt32(Session["BookEvent_PackageID"]);

            int currentPackageId =
                Convert.ToInt32(packageId);

            if (selectedPackageId == currentPackageId)
            {
                return "rx-book-package-card selected";
            }

            return "rx-book-package-card";
        }
        protected void BackToPackageButton_Click(
    object sender,
    EventArgs e)
        {
            Step3Panel.Visible = false;
            Step2Panel.Visible = true;

            LoadPackages();
        }
        protected void NextReviewButton_Click(
    object sender,
    EventArgs e)
        {
            Session["BookEvent_EventTheme"] =
                EventThemeTextBox.Text.Trim();

            Session["BookEvent_DecorationStyle"] =
                DecorationStyleDropDown.SelectedValue;

            Session["BookEvent_Entertainment"] =
                EntertainmentTextBox.Text.Trim();

            Session["BookEvent_DietaryRequirements"] =
                DietaryRequirementsTextBox.Text.Trim();

            Session["BookEvent_SpecialRequests"] =
                SpecialRequestsTextBox.Text.Trim();

            LoadReviewDetails();

            Step4Panel.Visible = false;
            Step5Panel.Visible = true;
        }
        protected string GetWishlistCoverImageUrl(object coverImage)
        {
            if (coverImage == null ||
                coverImage == DBNull.Value ||
                string.IsNullOrWhiteSpace(coverImage.ToString()))
            {
                return ResolveUrl(
                    "~/Content/images/rx-events-logo.png");
            }

            string path = coverImage.ToString().Trim();

            if (path.StartsWith("~/"))
            {
                return ResolveUrl(path);
            }

            if (path.StartsWith("/"))
            {
                return ResolveUrl("~" + path);
            }

            return ResolveUrl(
                "~/Content/images/" + path);
        }
        protected string GetInspirationFolderCardClass(
    object folderId)
        {
            if (Session["BookEvent_WishlistFolderID"] == null)
            {
                return "rx-book-inspiration-card";
            }

            int selectedFolderId =
                Convert.ToInt32(
                    Session["BookEvent_WishlistFolderID"]);

            int currentFolderId =
                Convert.ToInt32(folderId);

            if (selectedFolderId == currentFolderId)
            {
                return "rx-book-inspiration-card selected";
            }

            return "rx-book-inspiration-card";
        }
        protected void SkipInspirationButton_Click(
    object sender,
    EventArgs e)
        {
            Session["BookEvent_WishlistFolderID"] = null;

            Step3Panel.Visible = false;
            Step4Panel.Visible = true;
        }
        protected void BackToInspirationButton_Click(
    object sender,
    EventArgs e)
        {
            Step4Panel.Visible = false;
            Step3Panel.Visible = true;

            LoadWishlistFolders();
        }
private void LoadReviewDetails()
        {
            // EVENT DETAILS
            if (Session["BookEvent_EventID"] != null)
            {
                int eventId =
                    Convert.ToInt32(Session["BookEvent_EventID"]);

                string eventQuery = @"
            SELECT
                e.EventName,
                ec.CategoryName
            FROM Events e
            INNER JOIN EventCategories ec
                ON e.CategoryID = ec.CategoryID
            WHERE e.EventID = @EventID";

                using (SqlConnection connection =
                       databaseHelper.GetConnection())
                using (SqlCommand command =
                       new SqlCommand(eventQuery, connection))
                {
                    command.Parameters.AddWithValue("@EventID", eventId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ReviewEventNameLabel.Text =
                                reader["EventName"].ToString();

                            ReviewCategoryLabel.Text =
                                reader["CategoryName"].ToString();
                        }
                    }
                }
            }
            else
            {
                ReviewEventNameLabel.Text = "Not selected";
                ReviewCategoryLabel.Text = "Not selected";
            }

            // DATE, TIME AND GUESTS
            ReviewDateLabel.Text =
                Session["BookEvent_Date"] is DateTime eventDate
                    ? eventDate.ToString("dd MMM yyyy")
                    : "Not selected";

            ReviewTimeLabel.Text =
                Session["BookEvent_Time"] is TimeSpan eventTime
                    ? DateTime.Today.Add(eventTime).ToString("hh:mm tt")
                    : "Not selected";

            ReviewGuestsLabel.Text =
                Session["BookEvent_GuestCount"]?.ToString()
                ?? "Not specified";

            // VENUE
            if (Session["BookEvent_VenueID"] != null)
            {
                int venueId =
                    Convert.ToInt32(Session["BookEvent_VenueID"]);

                string venueQuery = @"
            SELECT VenueName, Location
            FROM Venues
            WHERE VenueID = @VenueID";

                using (SqlConnection connection =
                       databaseHelper.GetConnection())
                using (SqlCommand command =
                       new SqlCommand(venueQuery, connection))
                {
                    command.Parameters.AddWithValue("@VenueID", venueId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ReviewVenueLabel.Text =
                                reader["VenueName"].ToString();

                            string location =
                                reader["Location"].ToString();

                            string enteredAddress =
                                Session["BookEvent_VenueAddress"]?.ToString();

                            ReviewVenueAddressLabel.Text =
                                string.IsNullOrWhiteSpace(enteredAddress)
                                    ? location
                                    : enteredAddress;
                        }
                    }
                }
            }
            else if (Session["BookEvent_VenueAddress"] != null)
            {
                ReviewVenueLabel.Text = "My Own Venue / Home";

                ReviewVenueAddressLabel.Text =
                    Session["BookEvent_VenueAddress"].ToString();
            }
            else
            {
                ReviewVenueLabel.Text = "Not selected";
                ReviewVenueAddressLabel.Text = "";
            }

            // INSPIRATION FOLDER
            if (Session["BookEvent_WishlistFolderID"] != null)
            {
                int folderId =
                    Convert.ToInt32(Session["BookEvent_WishlistFolderID"]);

                int userId = Convert.ToInt32(Session["UserID"]);

                var folder =
                    wishlistRepository.GetFolderById(folderId, userId);

                if (folder != null)
                {
                    ReviewWishlistFolderLabel.Text = folder.FolderName;

                    ReviewWishlistStatusLabel.Text =
                        folder.ImageCount + " inspiration images";
                }
                else
                {
                    ReviewWishlistFolderLabel.Text = "Folder unavailable";
                    ReviewWishlistStatusLabel.Text =
                        "Please return to Step 3 and select another folder.";
                }
            }
            else
            {
                ReviewWishlistFolderLabel.Text = "No inspiration folder";
                ReviewWishlistStatusLabel.Text =
                    "You chose to continue without an inspiration folder.";
            }

            // CUSTOMIZATION
            ReviewThemeLabel.Text =
                Session["BookEvent_EventTheme"]?.ToString() ?? "Not specified";

            ReviewDecorationLabel.Text =
                Session["BookEvent_DecorationStyle"]?.ToString() ?? "Not specified";

            ReviewEntertainmentLabel.Text =
                Session["BookEvent_Entertainment"]?.ToString() ?? "Not specified";

            ReviewDietaryLabel.Text =
                Session["BookEvent_DietaryRequirements"]?.ToString() ?? "Not specified";

            ReviewSpecialRequestsLabel.Text =
                Session["BookEvent_SpecialRequests"]?.ToString() ?? "None";

            // PACKAGE
            ReviewPackageNameLabel.Text = "Not selected";
            ReviewPackageTypeLabel.Text = "";
            ReviewPackageDescriptionLabel.Text = "";
            ReviewPackagePriceLabel.Text = "0";
            ReviewTotalLabel.Text = "0";

            if (Session["BookEvent_PackageID"] != null)
            {
                int packageId =
                    Convert.ToInt32(Session["BookEvent_PackageID"]);

                Package package =
                    packageRepository.GetActivePackages()
                        .FirstOrDefault(p => p.PackageID == packageId);

                if (package != null)
                {
                    ReviewPackageNameLabel.Text = package.PackageName;
                    ReviewPackageTypeLabel.Text = package.PackageType;
                    ReviewPackageDescriptionLabel.Text = package.Description;
                    ReviewPackagePriceLabel.Text = package.Price.ToString("N0");
                    ReviewTotalLabel.Text = package.Price.ToString("N0");

                    ReviewPackageImage.ImageUrl =
                        GetPackageImageUrl(package.ImagePath);
                }
            }
        }

        protected void BackToCustomizeButton_Click(
    object sender,
    EventArgs e)
        {
            Step5Panel.Visible = false;
            Step4Panel.Visible = true;
        }

        protected void ConfirmBookingButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Verify the customer session.
                if (Session["UserID"] == null ||
                    Session["RoleID"] == null ||
                    Convert.ToInt32(Session["RoleID"]) != 2)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                // 2. Verify required booking details.
                if (Session["BookEvent_EventID"] == null ||
                    Session["BookEvent_PackageID"] == null ||
                    Session["BookEvent_Date"] == null ||
                    Session["BookEvent_Time"] == null ||
                    Session["BookEvent_GuestCount"] == null)
                {
                    ReviewMessageLabel.Text =
                        "Your booking details are incomplete. Please start again.";
                    return;
                }

                int userId = Convert.ToInt32(Session["UserID"]);
                int eventId = Convert.ToInt32(Session["BookEvent_EventID"]);
                int packageId = Convert.ToInt32(Session["BookEvent_PackageID"]);

                int? venueId = Session["BookEvent_VenueID"] == null
                    ? (int?)null
                    : Convert.ToInt32(Session["BookEvent_VenueID"]);

                int? wishlistFolderId =
                    Session["BookEvent_WishlistFolderID"] == null
                        ? (int?)null
                        : Convert.ToInt32(Session["BookEvent_WishlistFolderID"]);

                // 3. Get the selected package and its price.
                PackageRepository packageRepository = new PackageRepository();

                Package selectedPackage = packageRepository
                    .GetActivePackages()
                    .Find(p => p.PackageID == packageId);

                if (selectedPackage == null)
                {
                    ReviewMessageLabel.Text =
                        "The selected package is no longer available. Please select another package.";
                    return;
                }

                // 4. Combine customization details into AdditionalNotes.
                List<string> notes = new List<string>();

                string existingNotes =
                    Convert.ToString(Session["BookEvent_Notes"]);

                if (!string.IsNullOrWhiteSpace(existingNotes))
                    notes.Add("Additional notes: " + existingNotes);

                string theme = Convert.ToString(Session["BookEvent_EventTheme"]);
                string decoration = Convert.ToString(Session["BookEvent_DecorationStyle"]);
                string entertainment = Convert.ToString(Session["BookEvent_Entertainment"]);
                string dietary = Convert.ToString(Session["BookEvent_DietaryRequirements"]);
                string specialRequests = Convert.ToString(Session["BookEvent_SpecialRequests"]);

                if (!string.IsNullOrWhiteSpace(theme))
                    notes.Add("Event theme: " + theme);

                if (!string.IsNullOrWhiteSpace(decoration))
                    notes.Add("Decoration style: " + decoration);

                if (!string.IsNullOrWhiteSpace(entertainment))
                    notes.Add("Entertainment: " + entertainment);

                if (!string.IsNullOrWhiteSpace(dietary))
                    notes.Add("Dietary requirements: " + dietary);

                if (!string.IsNullOrWhiteSpace(specialRequests))
                    notes.Add("Special requests: " + specialRequests);

                string combinedNotes = string.Join(
                    Environment.NewLine, notes);

                // 5. Create the booking.
                Booking booking = new Booking
                {
                    BookingReference = "RX-" +
                        Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),

                    UserID = userId,
                    EventID = eventId,
                    VenueID = venueId,
                    PackageID = packageId,
                    EventDate = Convert.ToDateTime(Session["BookEvent_Date"]),
                    EventTime = (TimeSpan)Session["BookEvent_Time"],
                    GuestCount = Convert.ToInt32(Session["BookEvent_GuestCount"]),
                    TotalAmount = selectedPackage.Price,
                    AdditionalNotes = string.IsNullOrWhiteSpace(combinedNotes)
                        ? null
                        : combinedNotes,
                    WishlistFolderID = wishlistFolderId,
                    VenueAddress = venueId == null
                        ? Convert.ToString(Session["BookEvent_VenueAddress"])
                        : null
                };

                BookingRepository bookingRepository = new BookingRepository();

                int bookingId = bookingRepository.CreateBooking(booking);

                if (bookingId <= 0)
                {
                    ReviewMessageLabel.Text =
                        "We couldn't save your booking. Please try again.";
                    return;
                }

                // Keep the saved booking details for the payment step.
                Session["BookEvent_BookingID"] = bookingId;
                Session["BookEvent_BookingReference"] = booking.BookingReference;

                // Redirect after the booking has been saved.
                Response.Redirect(
                    "Payment.aspx?bookingId=" + bookingId, false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ReviewMessageLabel.Text =
                    "Something went wrong while saving your booking. Please try again.";
            }
        }

        protected void NextCustomizeEventButton_Click(
    object sender,
    EventArgs e)
        {
            Step3Panel.Visible = false;
            Step4Panel.Visible = true;
        }

        protected string GetProgressStepClass(int step)
        {
            if (step == 1 && Step1Panel.Visible)
                return "rx-book-step active";

            if (step == 2 && Step2Panel.Visible)
                return "rx-book-step active";

            if (step == 3 && Step3Panel.Visible)
                return "rx-book-step active";

            if (step == 4 && Step4Panel.Visible)
                return "rx-book-step active";

            if (step == 5 && Step5Panel.Visible)
                return "rx-book-step active";

            return "rx-book-step";
        }

    }
}