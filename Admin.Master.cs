
using RxEvents.Data;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace RxEvents
{
    public partial class AdminMaster : MasterPage
    {
        private readonly DatabaseHelper databaseHelper =
    new DatabaseHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToString(Session["RoleID"]) != "1")
            {
                Response.Redirect("~/Login.aspx", true);
                return;
            }

            if (!IsPostBack)
            {
                LoadAdminProfile();
                SetPageTitle();
                HighlightCurrentPage();
                ApplyDisplayMode();
            }
        }


        private void LoadAdminProfile()
        {
            string firstName = Convert.ToString(Session["FirstName"]);
            string lastName = Convert.ToString(Session["LastName"]);

            string fullName = (firstName + " " + lastName).Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                fullName = "Administrator";
            }

            AdminName.Text = Server.HtmlEncode(fullName);

            string initials = "";

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                initials += firstName.Substring(0, 1);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                initials += lastName.Substring(0, 1);
            }

            AdminInitials.Text = string.IsNullOrWhiteSpace(initials)
                ? "AD"
                : initials.ToUpperInvariant();

            // Load the saved profile photo from the database.
            string photoPath = "";

            string query = @"
        SELECT ProfilePhoto
        FROM Users
        WHERE UserID = @UserID AND RoleID = 1;";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value =
                    Convert.ToInt32(Session["UserID"]);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    photoPath = Convert.ToString(result).Trim();
                }
            }

            if (!string.IsNullOrWhiteSpace(photoPath))
            {
                AdminProfilePhoto.ImageUrl = ResolveUrl(GetPhotoUrl(photoPath));
                AdminProfilePhoto.Visible = true;
                AdminInitials.Visible = false;
            }
            else
            {
                AdminProfilePhoto.Visible = false;
                AdminInitials.Visible = true;
            }
        }

        private string GetPhotoUrl(string photo)
        {
            if (photo.StartsWith("~/"))
                return photo;

            if (photo.StartsWith("/"))
                return "~" + photo;

            return "~/" + photo.TrimStart('/');
        }


        private void SetPageTitle()
        {
            string pageName =
                System.IO.Path.GetFileName(
                    Request.AppRelativeCurrentExecutionFilePath);

            string title = "Dashboard";

            switch (pageName)
            {
                case "AdminDashboard.aspx":
                    title = "Dashboard";
                    break;

                case "AdminBookings.aspx":
                    title = "Bookings";
                    break;

                case "AdminEvents.aspx":
                    title = "Events";
                    break;

                case "AdminPackages.aspx":
                    title = "Packages";
                    break;

                case "AdminServices.aspx":
                    title = "Services";
                    break;

                case "AdminCustomers.aspx":
                    title = "Customers";
                    break;

                case "AdminPayments.aspx":
                    title = "Payments";
                    break;

                case "AdminStaff.aspx":
                    title = "Staff";
                    break;

                case "AdminReviews.aspx":
                    title = "Reviews";
                    break;

                case "AdminGallery.aspx":
                    title = "Gallery";
                    break;

                case "AdminReports.aspx":
                    title = "Reports";
                    break;

                case "AdminSettings.aspx":
                    title = "Settings";
                    break;

                case "AdminWebSettings.aspx":
                    title = "Website Settings";
                    break;
            }

            PageTitleLiteral.Text = title;
            HeaderTitleLiteral.Text = title;
        }
        private void HighlightCurrentPage()
        {
            string currentPage = System.IO.Path.GetFileName(
                Request.AppRelativeCurrentExecutionFilePath
            );

            // Remove active styling from all navigation links.
            System.Web.UI.WebControls.HyperLink[] navigationLinks =
            {
        DashboardLink,
        BookingsLink,
        EventsLink,
        PackagesLink,
        ServicesLink,
        CustomersLink,
        PaymentsLink,
        StaffLink,
        ReviewsLink,
        GalleryLink,
        ReportsLink,
        SettingsLink,
        WebSettingsLink
    };

            foreach (var link in navigationLinks)
            {
                link.CssClass = "admin-nav-link";
            }

            // Highlight the link for the current page.
            switch (currentPage.ToLowerInvariant())
            {
                case "admindashboard.aspx":
                    DashboardLink.CssClass = "admin-nav-link active";
                    break;

                case "adminbookings.aspx":
                    BookingsLink.CssClass = "admin-nav-link active";
                    break;

                case "adminevents.aspx":
                    EventsLink.CssClass = "admin-nav-link active";
                    break;

                case "adminpackages.aspx":
                    PackagesLink.CssClass = "admin-nav-link active";
                    break;

                case "adminservices.aspx":
                    ServicesLink.CssClass = "admin-nav-link active";
                    break;

                case "admincustomers.aspx":
                    CustomersLink.CssClass = "admin-nav-link active";
                    break;

                case "adminpayments.aspx":
                    PaymentsLink.CssClass = "admin-nav-link active";
                    break;

                case "adminstaff.aspx":
                    StaffLink.CssClass = "admin-nav-link active";
                    break;

                case "adminreviews.aspx":
                    ReviewsLink.CssClass = "admin-nav-link active";
                    break;

                case "admingallery.aspx":
                    GalleryLink.CssClass = "admin-nav-link active";
                    break;

                case "adminreports.aspx":
                    ReportsLink.CssClass = "admin-nav-link active";
                    break;

                case "adminsettings.aspx":
                    SettingsLink.CssClass = "admin-nav-link active";
                    break;

                case "adminwebsettings.aspx":
                    WebSettingsLink.CssClass = "admin-nav-link active";
                    break;
            }
        }

        private void ApplyDisplayMode()
        {
            string displayMode = "Light";

            if (Session["UserID"] != null)
            {
                string query = @"
            SELECT DisplayMode
            FROM Users
            WHERE UserID = @UserID AND RoleID = 1;";

                using (SqlConnection connection =
                       databaseHelper.GetConnection())
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value =
                        Convert.ToInt32(Session["UserID"]);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        displayMode = Convert.ToString(result);
                    }
                }
            }

            AdminBody.Attributes["class"] =
                string.Equals(displayMode, "Dark",
                    StringComparison.OrdinalIgnoreCase)
                ? "admin-dark-mode"
                : "admin-light-mode";
        }

    }
}
