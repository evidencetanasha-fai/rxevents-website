using System;
using RxEvents.Data;

namespace RxEvents
{
    public partial class Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadServices();
            }
        }

        private void LoadServices()
        {
            try
            {
                ServiceRepository repository = new ServiceRepository();

                ServicesRepeater.DataSource = repository.GetActiveServices();
                ServicesRepeater.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<p style='color:red;'>Unable to load services: "
                    + ex.Message +
                    "</p>"
                );
            }
        }
    }
}