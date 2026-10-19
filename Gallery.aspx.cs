using System;
using RxEvents.Data;

namespace RxEvents
{
    public partial class Gallery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGallery();
            }
        }

        private void LoadGallery()
        {
            try
            {
                GalleryRepository repository =
                    new GalleryRepository();

                GalleryRepeater.DataSource =
                    repository.GetActiveGallery();

                GalleryRepeater.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<p style='color:red;'>Unable to load gallery: "
                    + ex.Message +
                    "</p>"
                );
            }
        }
    }
}