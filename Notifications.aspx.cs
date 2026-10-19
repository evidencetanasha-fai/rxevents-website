using System;
using System.Collections.Generic;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Notifications : System.Web.UI.Page
    {
        private readonly NotificationRepository notificationRepository =
            new NotificationRepository();

        protected void Page_Load(object sender, EventArgs e)
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

            if (!IsPostBack)
            {
                LoadNotifications();
            }
        }

        private void LoadNotifications()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                List<Notification> notifications =
                    notificationRepository.GetNotificationsByUser(userId);

                NotificationsRepeater.DataSource = notifications;
                NotificationsRepeater.DataBind();

                int total = notifications.Count;
                int unread = notificationRepository.GetUnreadCount(userId);
                int read = total - unread;

                TotalNotificationsLabel.Text = total.ToString();
                UnreadNotificationsLabel.Text = unread.ToString();
                ReadNotificationsLabel.Text = read.ToString();

                EmptyNotificationsPanel.Visible = total == 0;
                MarkAllReadButton.Visible = unread > 0;
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to load your notifications. Please try again.";
                MessageLabel.CssClass =
                    "rx-notifications-message error";
            }
        }

        protected void NotificationsRepeater_ItemCommand(
            object source,
            System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                int notificationId =
                    Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "MarkRead")
                {
                    bool updated =
                        notificationRepository.MarkAsRead(
                            notificationId,
                            userId);

                    if (updated)
                    {
                        MessageLabel.Text =
                            "Notification marked as read.";
                        MessageLabel.CssClass =
                            "rx-notifications-message success";
                    }
                }
                else if (e.CommandName == "DeleteNotification")
                {
                    bool deleted =
                        notificationRepository.DeleteNotification(
                            notificationId,
                            userId);

                    if (deleted)
                    {
                        MessageLabel.Text =
                            "Notification deleted.";
                        MessageLabel.CssClass =
                            "rx-notifications-message success";
                    }
                }

                LoadNotifications();
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to complete the requested action.";
                MessageLabel.CssClass =
                    "rx-notifications-message error";

                LoadNotifications();
            }
        }

        protected void MarkAllReadButton_Click(
            object sender,
            EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                bool updated =
                    notificationRepository.MarkAllAsRead(userId);

                if (updated)
                {
                    MessageLabel.Text =
                        "All notifications marked as read.";
                    MessageLabel.CssClass =
                        "rx-notifications-message success";
                }
                else
                {
                    MessageLabel.Text =
                        "There are no unread notifications.";
                    MessageLabel.CssClass =
                        "rx-notifications-message";
                }

                LoadNotifications();
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to mark notifications as read.";
                MessageLabel.CssClass =
                    "rx-notifications-message error";
            }
        }

        protected void ClearReadButton_Click(
            object sender,
            EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                bool deleted =
                    notificationRepository.DeleteAllReadNotifications(
                        userId);

                if (deleted)
                {
                    MessageLabel.Text =
                        "Read notifications cleared successfully.";
                    MessageLabel.CssClass =
                        "rx-notifications-message success";
                }
                else
                {
                    MessageLabel.Text =
                        "There are no read notifications to clear.";
                    MessageLabel.CssClass =
                        "rx-notifications-message";
                }

                LoadNotifications();
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to clear read notifications.";
                MessageLabel.CssClass =
                    "rx-notifications-message error";
            }
        }

        public string GetNotificationIcon(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return "🔔";
            }

            switch (type.ToLower())
            {
                case "booking":
                    return "📅";

                case "payment":
                    return "💳";

                case "event":
                    return "🎉";

                case "review":
                    return "★";

                case "account":
                    return "👤";

                case "offer":
                    return "✦";

                default:
                    return "🔔";
            }
        }

        public string FormatNotificationDate(DateTime date)
        {
            TimeSpan difference = DateTime.Now - date;

            if (difference.TotalMinutes < 1)
            {
                return "Just now";
            }

            if (difference.TotalMinutes < 60)
            {
                return ((int)difference.TotalMinutes) + " minutes ago";
            }

            if (difference.TotalHours < 24)
            {
                return ((int)difference.TotalHours) + " hours ago";
            }

            if (difference.TotalDays < 2)
            {
                return "Yesterday";
            }

            if (difference.TotalDays < 7)
            {
                return ((int)difference.TotalDays) + " days ago";
            }

            return date.ToString("dd MMM yyyy, hh:mm tt");
        }
    }
}