using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminReviews : Page
    {
        private const int PageSize = 8;

        protected int CurrentReviewPage
        {
            get
            {
                return ViewState["CurrentReviewPage"] == null
                    ? 1 : Convert.ToInt32(ViewState["CurrentReviewPage"]);
            }
            set { ViewState["CurrentReviewPage"] = value; }
        }

        private int TotalReviewPages
        {
            get
            {
                return ViewState["TotalReviewPages"] == null
                    ? 1 : Convert.ToInt32(ViewState["TotalReviewPages"]);
            }
            set { ViewState["TotalReviewPages"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int roleID;

            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                !int.TryParse(Convert.ToString(Session["RoleID"]), out roleID) ||
                roleID != 1)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadEventFilter();
                LoadReviews();
            }
        }

        private void LoadEventFilter()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT EventID, EventName
                    FROM dbo.Events
                    ORDER BY EventName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ReviewEventFilter.DataSource = reader;
                        ReviewEventFilter.DataTextField = "EventName";
                        ReviewEventFilter.DataValueField = "EventID";
                        ReviewEventFilter.DataBind();
                    }
                }
            }

            ReviewEventFilter.Items.Insert(
                0, new ListItem("All Events", ""));
        }

        private void LoadReviews()
        {
            string search = ReviewSearchTextBox.Text.Trim();
            string rating = RatingFilter.SelectedValue;
            string status = ReviewStatusFilter.SelectedValue;
            string eventID = ReviewEventFilter.SelectedValue;

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                connection.Open();

                const string filter = @"
                    (@Search = ''
                     OR u.FirstName LIKE @SearchPattern
                     OR u.LastName LIKE @SearchPattern
                     OR u.Email LIKE @SearchPattern
                     OR e.EventName LIKE @SearchPattern
                     OR ISNULL(r.Comment, '') LIKE @SearchPattern)
                    AND (@Rating = 0 OR r.Rating = @Rating)
                    AND (@Status = ''
                         OR (@Status = 'Approved' AND r.IsApproved = 1)
                         OR (@Status = 'Pending' AND r.IsApproved = 0))
                    AND (@EventID = 0 OR r.EventID = @EventID)";

                string fromClause = @"
                    FROM dbo.Reviews r
                    INNER JOIN dbo.Users u ON r.UserID = u.UserID
                    INNER JOIN dbo.Events e ON r.EventID = e.EventID
                    WHERE ";

                int totalRecords;

                using (SqlCommand countCommand = new SqlCommand(
                    "SELECT COUNT(*) " + fromClause + filter, connection))
                {
                    AddReviewFilterParameters(
                        countCommand, search, rating, status, eventID);

                    totalRecords =
                        Convert.ToInt32(countCommand.ExecuteScalar());
                }

                TotalReviewPages = Math.Max(
                    1, (int)Math.Ceiling(totalRecords / (double)PageSize));

                if (CurrentReviewPage > TotalReviewPages)
                {
                    CurrentReviewPage = TotalReviewPages;
                }

                string query = @"
                    SELECT
                        r.ReviewID,
                        r.UserID,
                        r.EventID,
                        r.BookingID,
                        r.Rating,
                        r.Comment AS ReviewText,
                        r.ReviewDate,
                        r.IsApproved,
                        u.FirstName + ' ' + u.LastName AS CustomerName,
                        u.Email AS CustomerEmail,
                        e.EventName,
                        CASE WHEN r.IsApproved = 1
                             THEN 'Approved' ELSE 'Pending' END AS Status
                    " + fromClause + filter + @"
                    ORDER BY r.ReviewDate DESC, r.ReviewID DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddReviewFilterParameters(
                        command, search, rating, status, eventID);

                    command.Parameters.Add("@Offset", SqlDbType.Int)
                        .Value = (CurrentReviewPage - 1) * PageSize;

                    command.Parameters.Add("@PageSize", SqlDbType.Int)
                        .Value = PageSize;

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        DataTable reviews = new DataTable();
                        adapter.Fill(reviews);

                        reviews.Columns.Add(
                            "CustomerInitials", typeof(string));

                        foreach (DataRow row in reviews.Rows)
                        {
                            string first = Convert.ToString(
                                row["CustomerName"]);

                            string[] parts = first.Split(
                                new char[] { ' ' },
                                StringSplitOptions.RemoveEmptyEntries);

                            string initials = "";

                            if (parts.Length > 0)
                                initials += parts[0].Substring(0, 1);

                            if (parts.Length > 1)
                                initials += parts[parts.Length - 1]
                                    .Substring(0, 1);

                            row["CustomerInitials"] = initials.ToUpperInvariant();
                        }

                        ReviewsGridView.DataSource = reviews;
                        ReviewsGridView.DataBind();
                    }
                }

                int firstRecord = totalRecords == 0
                    ? 0 : (CurrentReviewPage - 1) * PageSize + 1;

                int lastRecord = Math.Min(
                    CurrentReviewPage * PageSize, totalRecords);

                ReviewPaginationSummary.Text =
                    "Showing " + firstRecord + " to " + lastRecord +
                    " of " + totalRecords + " reviews";

                BindReviewPagination();
            }
        }

        private void AddReviewFilterParameters(
            SqlCommand command,
            string search,
            string rating,
            string status,
            string eventID)
        {
            int ratingValue = 0;
            int eventValue = 0;

            int.TryParse(rating, out ratingValue);
            int.TryParse(eventID, out eventValue);

            command.Parameters.Add("@Search", SqlDbType.VarChar, 100)
                .Value = search;

            command.Parameters.Add("@SearchPattern", SqlDbType.VarChar, 202)
                .Value = "%" + search + "%";

            command.Parameters.Add("@Rating", SqlDbType.Int)
                .Value = ratingValue;

            command.Parameters.Add("@Status", SqlDbType.VarChar, 20)
                .Value = status;

            command.Parameters.Add("@EventID", SqlDbType.Int)
                .Value = eventValue;
        }

        protected string GetStars(object ratingValue)
        {
            int rating;

            if (!int.TryParse(Convert.ToString(ratingValue), out rating))
                rating = 0;

            rating = Math.Max(0, Math.Min(5, rating));

            return new string('★', rating) +
                   new string('☆', 5 - rating);
        }

        protected string GetReviewStatusCss(object statusValue)
        {
            string status = Convert.ToString(statusValue);

            if (status == "Approved")
                return "reviews-status-approved";

            if (status == "Rejected")
                return "reviews-status-rejected";

            return "reviews-status-pending";
        }

        private void BindReviewPagination()
        {
            DataTable pages = new DataTable();
            pages.Columns.Add("PageNumber", typeof(int));

            for (int page = 1; page <= TotalReviewPages; page++)
            {
                pages.Rows.Add(page);
            }

            ReviewPaginationRepeater.DataSource = pages;
            ReviewPaginationRepeater.DataBind();

            PreviousReviewPageButton.Enabled = CurrentReviewPage > 1;
            NextReviewPageButton.Enabled =
                CurrentReviewPage < TotalReviewPages;
        }

        protected void ApplyReviewFiltersButton_Click(
            object sender, EventArgs e)
        {
            CurrentReviewPage = 1;
            LoadReviews();
        }

        protected void ClearReviewFiltersButton_Click(
            object sender, EventArgs e)
        {
            ReviewSearchTextBox.Text = "";
            RatingFilter.SelectedIndex = 0;
            ReviewStatusFilter.SelectedIndex = 0;
            ReviewEventFilter.SelectedIndex = 0;
            CurrentReviewPage = 1;

            LoadReviews();
        }

        protected void PreviousReviewPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentReviewPage > 1)
                CurrentReviewPage--;

            LoadReviews();
        }

        protected void NextReviewPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentReviewPage < TotalReviewPages)
                CurrentReviewPage++;

            LoadReviews();
        }

        protected void ReviewPaginationRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            int pageNumber;

            if (e.CommandName == "GoToPage" &&
                int.TryParse(Convert.ToString(e.CommandArgument), out pageNumber) &&
                pageNumber >= 1 && pageNumber <= TotalReviewPages)
            {
                CurrentReviewPage = pageNumber;
                LoadReviews();
            }
        }

        protected void ReviewsGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            int reviewID;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument), out reviewID))
                return;

            if (e.CommandName == "ViewReview")
            {
                ShowReviewDetails(reviewID);
            }
            else if (e.CommandName == "ApproveReview")
            {
                SetReviewApproval(reviewID, true);
            }
            else if (e.CommandName == "RejectReview")
            {
                // The database has no Rejected status.
                ReviewsMessage.Text =
                    "Your current Reviews table supports Approved and Pending only.";
                ReviewsMessage.ForeColor =
                    System.Drawing.Color.Firebrick;
            }
        }

        private void ShowReviewDetails(int reviewID)
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT r.Comment, r.Rating, r.IsApproved,
                           u.FirstName, u.LastName, e.EventName
                    FROM dbo.Reviews r
                    INNER JOIN dbo.Users u ON r.UserID = u.UserID
                    INNER JOIN dbo.Events e ON r.EventID = e.EventID
                    WHERE r.ReviewID = @ReviewID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ReviewID", SqlDbType.Int)
                        .Value = reviewID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string comment = reader["Comment"] == DBNull.Value
                                ? "(No written comment)"
                                : Convert.ToString(reader["Comment"]);

                            string customer =
                                Convert.ToString(reader["FirstName"]) + " " +
                                Convert.ToString(reader["LastName"]);

                            string eventName = Convert.ToString(
                                reader["EventName"]);

                            string status = Convert.ToBoolean(
                                reader["IsApproved"])
                                ? "Approved" : "Pending";

                            ReviewsMessage.Text = Server.HtmlEncode(
                                "Review by " + customer + " for " +
                                eventName + " | Rating: " +
                                Convert.ToString(reader["Rating"]) +
                                "/5 | Status: " + status +
                                " | Comment: " + comment);

                            ReviewsMessage.ForeColor =
                                System.Drawing.Color.DarkGreen;
                        }
                        else
                        {
                            ReviewsMessage.Text = "Review not found.";
                        }
                    }
                }
            }
        }

        private void SetReviewApproval(int reviewID, bool approved)
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    UPDATE dbo.Reviews
                    SET IsApproved = @IsApproved
                    WHERE ReviewID = @ReviewID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@IsApproved", SqlDbType.Bit)
                        .Value = approved;

                    command.Parameters.Add("@ReviewID", SqlDbType.Int)
                        .Value = reviewID;

                    connection.Open();
                    int affected = command.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        ReviewsMessage.Text = "Review not found.";
                        ReviewsMessage.ForeColor =
                            System.Drawing.Color.Firebrick;
                        return;
                    }
                }
            }

            ReviewsMessage.Text = approved
                ? "Review approved successfully."
                : "Review set to pending.";

            ReviewsMessage.ForeColor = System.Drawing.Color.DarkGreen;

            LoadReviews();
        }

        protected void ExportReviewsButton_Click(
            object sender, EventArgs e)
        {
            string search = ReviewSearchTextBox.Text.Trim();
            string rating = RatingFilter.SelectedValue;
            string status = ReviewStatusFilter.SelectedValue;
            string eventID = ReviewEventFilter.SelectedValue;

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT r.ReviewID,
                           u.FirstName + ' ' + u.LastName AS CustomerName,
                           u.Email,
                           e.EventName,
                           r.Rating,
                           r.Comment,
                           r.ReviewDate,
                           CASE WHEN r.IsApproved = 1
                                THEN 'Approved' ELSE 'Pending' END AS Status
                    FROM dbo.Reviews r
                    INNER JOIN dbo.Users u ON r.UserID = u.UserID
                    INNER JOIN dbo.Events e ON r.EventID = e.EventID
                    WHERE
                        (@Search = ''
                         OR u.FirstName LIKE @SearchPattern
                         OR u.LastName LIKE @SearchPattern
                         OR u.Email LIKE @SearchPattern
                         OR e.EventName LIKE @SearchPattern
                         OR ISNULL(r.Comment, '') LIKE @SearchPattern)
                        AND (@Rating = 0 OR r.Rating = @Rating)
                        AND (@Status = ''
                             OR (@Status = 'Approved' AND r.IsApproved = 1)
                             OR (@Status = 'Pending' AND r.IsApproved = 0))
                        AND (@EventID = 0 OR r.EventID = @EventID)
                    ORDER BY r.ReviewDate DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddReviewFilterParameters(
                        command, search, rating, status, eventID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder csv = new StringBuilder();

                        csv.AppendLine(
                            "Review ID,Customer,Email,Event,Rating,Comment,Date,Status");

                        while (reader.Read())
                        {
                            csv.Append(Csv(reader["ReviewID"])).Append(",");
                            csv.Append(Csv(reader["CustomerName"])).Append(",");
                            csv.Append(Csv(reader["Email"])).Append(",");
                            csv.Append(Csv(reader["EventName"])).Append(",");
                            csv.Append(Csv(reader["Rating"])).Append(",");
                            csv.Append(Csv(reader["Comment"])).Append(",");
                            csv.Append(Csv(Convert.ToDateTime(
                                reader["ReviewDate"]).ToString("dd MMM yyyy hh:mm tt")))
                                .Append(",");
                            csv.AppendLine(Csv(reader["Status"]));
                        }

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "text/csv";
                        Response.ContentEncoding = Encoding.UTF8;
                        Response.AddHeader(
                            "Content-Disposition",
                            "attachment;filename=RxEventsReviews.csv");

                        Response.Write("\uFEFF" + csv.ToString());
                        Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }

        private string Csv(object value)
        {
            string text = value == null || value == DBNull.Value
                ? ""
                : Convert.ToString(value);

            if (!string.IsNullOrEmpty(text) &&
                (text.StartsWith("=") || text.StartsWith("+") ||
                 text.StartsWith("-") || text.StartsWith("@")))
            {
                text = "'" + text;
            }

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}