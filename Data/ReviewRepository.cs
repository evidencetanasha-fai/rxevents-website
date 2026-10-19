using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class ReviewRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public ReviewRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public List<Review> GetReviewsByUser(int userId)
        {
            List<Review> reviews = new List<Review>();

            string query = @"
                SELECT
                    r.ReviewID,
                    r.UserID,
                    r.EventID,
                    r.BookingID,
                    r.Rating,
                    r.Comment,
                    r.ReviewDate,
                    r.IsApproved,
                    e.EventName,
                    e.EventImage,
                    u.FirstName + ' ' + u.LastName AS CustomerName
                FROM Reviews r
                INNER JOIN Events e
                    ON r.EventID = e.EventID
                INNER JOIN Users u
                    ON r.UserID = u.UserID
                WHERE r.UserID = @UserID
                ORDER BY r.ReviewDate DESC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Review review = new Review();

                        review.ReviewID =
                            Convert.ToInt32(reader["ReviewID"]);

                        review.UserID =
                            Convert.ToInt32(reader["UserID"]);

                        review.EventID =
                            Convert.ToInt32(reader["EventID"]);

                        review.BookingID =
                            Convert.ToInt32(reader["BookingID"]);

                        review.Rating =
                            Convert.ToInt32(reader["Rating"]);

                        review.Comment =
                            reader["Comment"] == DBNull.Value
                                ? ""
                                : reader["Comment"].ToString();

                        review.ReviewDate =
                            Convert.ToDateTime(
                                reader["ReviewDate"]);

                        review.IsApproved =
                            Convert.ToBoolean(
                                reader["IsApproved"]);

                        review.EventName =
                            reader["EventName"].ToString();

                        review.EventImage =
                            reader["EventImage"] == DBNull.Value
                                ? ""
                                : reader["EventImage"].ToString();

                        review.CustomerName =
                            reader["CustomerName"].ToString();

                        reviews.Add(review);
                    }
                }
            }

            return reviews;
        }

        public Dictionary<string, object> GetReviewStatistics(
            int userId)
        {
            Dictionary<string, object> statistics =
                new Dictionary<string, object>();

            string query = @"
                SELECT
                    COUNT(*) AS TotalReviews,

                    ISNULL(
                        AVG(CAST(Rating AS DECIMAL(10,2))),
                        0
                    ) AS AverageRating,

                    SUM(
                        CASE
                            WHEN Rating = 5 THEN 1
                            ELSE 0
                        END
                    ) AS FiveStar,

                    SUM(
                        CASE
                            WHEN Rating = 4 THEN 1
                            ELSE 0
                        END
                    ) AS FourStar,

                    SUM(
                        CASE
                            WHEN Rating = 3 THEN 1
                            ELSE 0
                        END
                    ) AS ThreeStar,

                    SUM(
                        CASE
                            WHEN Rating = 2 THEN 1
                            ELSE 0
                        END
                    ) AS TwoStar,

                    SUM(
                        CASE
                            WHEN Rating = 1 THEN 1
                            ELSE 0
                        END
                    ) AS OneStar

                FROM Reviews

                WHERE UserID = @UserID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        statistics["TotalReviews"] =
                            Convert.ToInt32(
                                reader["TotalReviews"]);

                        statistics["AverageRating"] =
                            Convert.ToDecimal(
                                reader["AverageRating"]);

                        statistics["FiveStar"] =
                            Convert.ToInt32(
                                reader["FiveStar"]);

                        statistics["FourStar"] =
                            Convert.ToInt32(
                                reader["FourStar"]);

                        statistics["ThreeStar"] =
                            Convert.ToInt32(
                                reader["ThreeStar"]);

                        statistics["TwoStar"] =
                            Convert.ToInt32(
                                reader["TwoStar"]);

                        statistics["OneStar"] =
                            Convert.ToInt32(
                                reader["OneStar"]);
                    }
                }
            }

            return statistics;
        }
        public List<Review> GetApprovedReviews()
        {
            List<Review> reviews = new List<Review>();

            string query = @"
        SELECT
            r.ReviewID,
            r.UserID,
            r.EventID,
            r.BookingID,
            r.Rating,
            r.Comment,
            r.ReviewDate,
            r.IsApproved,
            e.EventName,
            e.EventImage,
            u.FirstName + ' ' + u.LastName AS CustomerName
        FROM Reviews r
        INNER JOIN Events e
            ON r.EventID = e.EventID
        INNER JOIN Users u
            ON r.UserID = u.UserID
        WHERE r.IsApproved = 1
        ORDER BY r.ReviewDate DESC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Review review = new Review();

                        review.ReviewID =
                            Convert.ToInt32(reader["ReviewID"]);

                        review.UserID =
                            Convert.ToInt32(reader["UserID"]);

                        review.EventID =
                            Convert.ToInt32(reader["EventID"]);

                        review.BookingID =
                            Convert.ToInt32(reader["BookingID"]);

                        review.Rating =
                            Convert.ToInt32(reader["Rating"]);

                        review.Comment =
                            reader["Comment"] == DBNull.Value
                                ? ""
                                : reader["Comment"].ToString();

                        review.ReviewDate =
                            Convert.ToDateTime(reader["ReviewDate"]);

                        review.IsApproved =
                            Convert.ToBoolean(reader["IsApproved"]);

                        review.EventName =
                            reader["EventName"].ToString();

                        review.EventImage =
                            reader["EventImage"] == DBNull.Value
                                ? ""
                                : reader["EventImage"].ToString();

                        review.CustomerName =
                            reader["CustomerName"].ToString();

                        reviews.Add(review);
                    }
                }
            }

            return reviews;
        }
        public bool HasReviewedBooking(int userId, int bookingId)
        {
            string query = @"
        SELECT COUNT(*)
        FROM Reviews
        WHERE UserID = @UserID
          AND BookingID = @BookingID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                command.Parameters.AddWithValue(
                    "@BookingID",
                    bookingId);

                connection.Open();

                int count =
                    Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
        }
        public bool AddReview(Review review)
        {
            string query = @"
        INSERT INTO Reviews
        (
            UserID,
            EventID,
            BookingID,
            Rating,
            Comment,
            IsApproved
        )
        VALUES
        (
            @UserID,
            @EventID,
            @BookingID,
            @Rating,
            @Comment,
            0
        )";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    review.UserID);

                command.Parameters.AddWithValue(
                    "@EventID",
                    review.EventID);

                command.Parameters.AddWithValue(
                    "@BookingID",
                    review.BookingID);

                command.Parameters.AddWithValue(
                    "@Rating",
                    review.Rating);

                command.Parameters.AddWithValue(
                    "@Comment",
                    string.IsNullOrWhiteSpace(review.Comment)
                        ? (object)DBNull.Value
                        : review.Comment);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}