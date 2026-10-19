using System;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class PrivacySecurity : System.Web.UI.Page
    {
        private readonly UserRepository userRepository =
            new UserRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSecurityInformation();
            }
        }

        private void LoadSecurityInformation()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                User user = userRepository.GetUserById(userId);

                if (user == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                SecurityEmailLabel.Text = user.Email;

                SecurityMemberSinceLabel.Text =
                    user.CreatedDate.ToString("MMMM yyyy");
            }
            catch (Exception)
            {
                SecurityEmailLabel.Text = "Unavailable";
                SecurityMemberSinceLabel.Text = "Unavailable";
            }
        }
    }
}