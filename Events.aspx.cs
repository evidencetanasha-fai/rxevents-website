using System;
using RxEvents.Data;

namespace RxEvents
{
    public partial class Events : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEventCategories();
            }
        }

        private void LoadEventCategories()
        {
            try
            {
                EventCategoryRepository repository =
                    new EventCategoryRepository();

                EventsRepeater.DataSource =
                    repository.GetActiveEventCategories();

                EventsRepeater.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<p style='color:red;'>Unable to load events: "
                    + ex.Message +
                    "</p>"
                );
            }
        }

        protected string GetInitial(object categoryName)
        {
            if (categoryName == null)
            {
                return "";
            }

            string name = categoryName.ToString();

            if (string.IsNullOrWhiteSpace(name))
            {
                return "";
            }

            return name.Substring(0, 1).ToUpper();
        }
    }
}