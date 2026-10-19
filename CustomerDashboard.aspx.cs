using System;

namespace RxEvents
{
    public partial class CustomerDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomerDetails();
            }
        }

        private void LoadCustomerDetails()
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

            CustomerNameLabel.Text =
                Session["FirstName"] != null
                    ? Session["FirstName"].ToString()
                    : "Customer";
        }
    }
}