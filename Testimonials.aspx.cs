using System;
using RxEvents.Data;

namespace RxEvents
{
    public partial class Testimonials : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTestimonials();
            }
        }

        private void LoadTestimonials()
        {
            try
            {
                ReviewRepository repository =
                    new ReviewRepository();

                TestimonialsRepeater.DataSource =
                    repository.GetApprovedReviews();

                TestimonialsRepeater.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<p style='color:red;'>Unable to load testimonials: "
                    + ex.Message +
                    "</p>"
                );
            }
        }

        protected string GetStars(int rating)
        {
            string stars = "";

            for (int i = 1; i <= 5; i++)
            {
                if (i <= rating)
                {
                    stars += "★";
                }
                else
                {
                    stars += "☆";
                }
            }

            return stars;
        }
    }
}