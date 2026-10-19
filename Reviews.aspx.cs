using System;
using System.Collections.Generic;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Reviews : System.Web.UI.Page
    {
        private readonly ReviewRepository reviewRepository =
            new ReviewRepository();

        private readonly BookingRepository bookingRepository =
            new BookingRepository();

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
                LoadReviewStatistics();
                LoadReviews();
                LoadReviewableBookings();
            }
        }

        private void LoadReviews()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                List<Review> reviews =
                    reviewRepository.GetReviewsByUser(userId);

                ReviewsRepeater.DataSource = reviews;
                ReviewsRepeater.DataBind();

                NoReviewsPanel.Visible = reviews.Count == 0;
            }
            catch (Exception)
            {
                ReviewsMessageLabel.Text =
                    "Unable to load your reviews at this time.";

                ReviewsMessageLabel.CssClass =
                    "rx-reviews-message error";

                NoReviewsPanel.Visible = true;
            }
        }

        private void LoadReviewStatistics()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                Dictionary<string, object> statistics =
                    reviewRepository.GetReviewStatistics(userId);

                int totalReviews =
                    Convert.ToInt32(statistics["TotalReviews"]);

                TotalReviewsLabel.Text =
                    totalReviews.ToString();

                AverageRatingLabel.Text =
                    totalReviews > 0
                        ? CalculateAverageRating(statistics).ToString("0.0")
                        : "0.0";

                FiveStarLabel.Text =
                    Convert.ToInt32(statistics["FiveStar"]).ToString();

                FourStarLabel.Text =
                    Convert.ToInt32(statistics["FourStar"]).ToString();

                ThreeStarLabel.Text =
                    Convert.ToInt32(statistics["ThreeStar"]).ToString();

                TwoStarLabel.Text =
                    Convert.ToInt32(statistics["TwoStar"]).ToString();

                OneStarLabel.Text =
                    Convert.ToInt32(statistics["OneStar"]).ToString();
            }
            catch (Exception)
            {
                AverageRatingLabel.Text = "0.0";
                TotalReviewsLabel.Text = "0";
                FiveStarLabel.Text = "0";
                FourStarLabel.Text = "0";
                ThreeStarLabel.Text = "0";
                TwoStarLabel.Text = "0";
                OneStarLabel.Text = "0";
            }
        }

        private double CalculateAverageRating(
            Dictionary<string, object> statistics)
        {
            int fiveStar =
                Convert.ToInt32(statistics["FiveStar"]);

            int fourStar =
                Convert.ToInt32(statistics["FourStar"]);

            int threeStar =
                Convert.ToInt32(statistics["ThreeStar"]);

            int twoStar =
                Convert.ToInt32(statistics["TwoStar"]);

            int oneStar =
                Convert.ToInt32(statistics["OneStar"]);

            int total =
                fiveStar +
                fourStar +
                threeStar +
                twoStar +
                oneStar;

            if (total == 0)
            {
                return 0;
            }

            int totalPoints =
                (fiveStar * 5) +
                (fourStar * 4) +
                (threeStar * 3) +
                (twoStar * 2) +
                oneStar;

            return (double)totalPoints / total;
        }

        private void LoadReviewableBookings()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                List<Booking> bookings =
                    bookingRepository.GetReviewableBookings(userId);

                ReviewBookingDropDown.Items.Clear();

                ReviewBookingDropDown.Items.Add(
                    new System.Web.UI.WebControls.ListItem(
                        "Select an event",
                        ""));

                foreach (Booking booking in bookings)
                {
                    ReviewBookingDropDown.Items.Add(
                        new System.Web.UI.WebControls.ListItem(
                            booking.EventName,
                            booking.BookingID.ToString()));
                }
            }
            catch (Exception)
            {
                ReviewFormMessageLabel.Text =
                    "Unable to load your completed events.";
            }
        }

        protected void WriteReviewButton_Click(
            object sender,
            EventArgs e)
        {
            LoadReviewableBookings();

            ReviewFormMessageLabel.Text = "";

            WriteReviewPopup.Visible = true;
        }

        protected void CloseReviewButton_Click(
            object sender,
            EventArgs e)
        {
            WriteReviewPopup.Visible = false;

            ReviewFormMessageLabel.Text = "";

            ReviewBookingDropDown.SelectedIndex = 0;
            ReviewRatingDropDown.SelectedIndex = 0;
            ReviewCommentTextBox.Text = "";
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
        protected void SubmitReviewButton_Click(
    object sender,
    EventArgs e)
        {
            if (!Page.IsValid)
            {
                WriteReviewPopup.Visible = true;
                return;
            }

            try
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                int userId =
                    Convert.ToInt32(Session["UserID"]);

                int bookingId =
                    Convert.ToInt32(
                        ReviewBookingDropDown.SelectedValue);

                int rating =
                    Convert.ToInt32(
                        ReviewRatingDropDown.SelectedValue);

                string comment =
                    ReviewCommentTextBox.Text.Trim();

                /*
                 * Get the customer's eligible bookings again.
                 * This prevents a customer from submitting
                 * a booking that does not belong to them.
                 */
                List<Booking> bookings =
                    bookingRepository.GetReviewableBookings(userId);

                Booking selectedBooking =
                    bookings.Find(
                        b => b.BookingID == bookingId);

                if (selectedBooking == null)
                {
                    ReviewFormMessageLabel.Text =
                        "The selected event is not available for review.";

                    ReviewFormMessageLabel.CssClass =
                        "rx-review-form-message";

                    WriteReviewPopup.Visible = true;
                    return;
                }

                Review review = new Review
                {
                    UserID = userId,
                    EventID = selectedBooking.EventID,
                    BookingID = selectedBooking.BookingID,
                    Rating = rating,
                    Comment = comment,
                    IsApproved = false
                };

                bool added =
                    reviewRepository.AddReview(review);

                if (added)
                {
                    WriteReviewPopup.Visible = false;

                    ReviewBookingDropDown.SelectedIndex = 0;
                    ReviewRatingDropDown.SelectedIndex = 0;
                    ReviewCommentTextBox.Text = "";

                    ReviewsMessageLabel.Text =
                        "Your review has been submitted and is awaiting approval.";

                    ReviewsMessageLabel.CssClass =
                        "rx-reviews-message";

                    LoadReviews();
                    LoadReviewStatistics();
                    LoadReviewableBookings();
                }
                else
                {
                    ReviewFormMessageLabel.Text =
                        "Unable to submit your review. Please try again.";

                    WriteReviewPopup.Visible = true;
                }
            }
            catch (Exception)
            {
                ReviewFormMessageLabel.Text =
                    "Something went wrong while submitting your review.";

                WriteReviewPopup.Visible = true;
            }
        }
    }
}