using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class NotificationRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public NotificationRepository()
        {
            databaseHelper = new DatabaseHelper();
        }


        // ==========================================
        // GET ALL NOTIFICATIONS
        // ==========================================

        public List<Notification> GetNotificationsByUser(
            int userId)
        {
            List<Notification> notifications =
                new List<Notification>();

            string query = @"
                SELECT
                    NotificationID,
                    UserID,
                    Title,
                    Message,
                    NotificationType,
                    IsRead,
                    CreatedDate
                FROM Notifications
                WHERE UserID = @UserID
                ORDER BY CreatedDate DESC";


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
                        Notification notification =
                            new Notification();

                        notification.NotificationID =
                            Convert.ToInt32(
                                reader["NotificationID"]);

                        notification.UserID =
                            Convert.ToInt32(
                                reader["UserID"]);

                        notification.Title =
                            reader["Title"].ToString();

                        notification.Message =
                            reader["Message"].ToString();

                        notification.NotificationType =
                            reader["NotificationType"] == DBNull.Value
                                ? ""
                                : reader["NotificationType"].ToString();

                        notification.IsRead =
                            Convert.ToBoolean(
                                reader["IsRead"]);

                        notification.CreatedDate =
                            Convert.ToDateTime(
                                reader["CreatedDate"]);


                        notifications.Add(notification);
                    }
                }
            }


            return notifications;
        }


        // ==========================================
        // GET UNREAD COUNT
        // ==========================================

        public int GetUnreadCount(int userId)
        {
            string query = @"
                SELECT COUNT(*)
                FROM Notifications
                WHERE UserID = @UserID
                  AND IsRead = 0";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                return Convert.ToInt32(
                    command.ExecuteScalar());
            }
        }


        // ==========================================
        // MARK ONE AS READ
        // ==========================================

        public bool MarkAsRead(
            int notificationId,
            int userId)
        {
            string query = @"
                UPDATE Notifications
                SET IsRead = 1
                WHERE NotificationID = @NotificationID
                  AND UserID = @UserID";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@NotificationID",
                    notificationId);

                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // MARK ALL AS READ
        // ==========================================

        public bool MarkAllAsRead(int userId)
        {
            string query = @"
                UPDATE Notifications
                SET IsRead = 1
                WHERE UserID = @UserID
                  AND IsRead = 0";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // DELETE ONE NOTIFICATION
        // ==========================================

        public bool DeleteNotification(
            int notificationId,
            int userId)
        {
            string query = @"
                DELETE FROM Notifications
                WHERE NotificationID = @NotificationID
                  AND UserID = @UserID";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@NotificationID",
                    notificationId);

                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // DELETE ALL READ NOTIFICATIONS
        // ==========================================

        public bool DeleteAllReadNotifications(
            int userId)
        {
            string query = @"
                DELETE FROM Notifications
                WHERE UserID = @UserID
                  AND IsRead = 1";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // CREATE NOTIFICATION
        // ==========================================

        public bool CreateNotification(
            Notification notification)
        {
            string query = @"
                INSERT INTO Notifications
                (
                    UserID,
                    Title,
                    Message,
                    NotificationType,
                    IsRead
                )
                VALUES
                (
                    @UserID,
                    @Title,
                    @Message,
                    @NotificationType,
                    0
                )";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    notification.UserID);

                command.Parameters.AddWithValue(
                    "@Title",
                    notification.Title);

                command.Parameters.AddWithValue(
                    "@Message",
                    notification.Message);

                command.Parameters.AddWithValue(
                    "@NotificationType",
                    string.IsNullOrWhiteSpace(
                        notification.NotificationType)
                        ? (object)DBNull.Value
                        : notification.NotificationType);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}