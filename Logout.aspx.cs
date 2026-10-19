
using System;
using System.Web;

namespace RxEvents
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Cache.SetCacheability(
                HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            Response.Redirect("~/Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
