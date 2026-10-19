using System;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Customer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomerProfile();
            }
        }

        private void LoadCustomerProfile()
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                UserRepository repository =
                    new UserRepository();

                User user =
                    repository.GetUserById(userId);

                if (user == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                // Load profile photo
                if (!string.IsNullOrWhiteSpace(user.ProfilePhoto))
                {
                    NavbarProfileImage.ImageUrl =
                        ResolveUrl(
                            "~/Content/images/" +
                            user.ProfilePhoto);
                }
                else
                {
                    NavbarProfileImage.ImageUrl =
                        ResolveUrl(
                            "~/Content/images/rx-events-logo.png");
                }
            }
            catch (Exception)
            {
                NavbarProfileImage.ImageUrl =
                    ResolveUrl(
                        "~/Content/images/rx-events-logo.png");
            }
        }


        protected string GetNavClass(string pageName)
        {
            string currentPage =
                System.IO.Path.GetFileName(Request.Path);

            if (currentPage.Equals(
                pageName,
                StringComparison.OrdinalIgnoreCase))
            {
                return "rx-dashboard-nav-item active";
            }

            return "rx-dashboard-nav-item";
        }
        protected string GetThemeClass()
        {
            if (Session["UserID"] == null)
            {
                return "theme-light";
            }

            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                UserRepository repository = new UserRepository();
                User user = repository.GetUserById(userId);

                if (user != null &&
                    string.Equals(
                        user.DisplayMode,
                        "Dark",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return "theme-dark";
                }
            }
            catch (Exception)
            {
                // Use light mode if the preference cannot be loaded.
            }

            return "theme-light";
        }
    }
}