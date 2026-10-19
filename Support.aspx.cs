using System;
using System.Collections.Generic;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Support : System.Web.UI.Page
    {
        private readonly FAQRepository faqRepository =
           new FAQRepository();
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
                LoadFAQs();
            }
        }
        private void LoadFAQs()
        {
            try
            {
                List<FAQ> faqs =
                    faqRepository.GetActiveFAQs();

                FAQsRepeater.DataSource = faqs;
                FAQsRepeater.DataBind();

                NoFAQsPanel.Visible = faqs.Count == 0;
            }
            catch (Exception)
            {
                SupportMessageLabel.Text =
                    "Unable to load frequently asked questions.";

                SupportMessageLabel.CssClass =
                    "rx-support-message error";
            }
        }

        protected void SupportSearchButton_Click(
            object sender,
            EventArgs e)
        {
            string searchText =
                SupportSearchTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                SupportMessageLabel.Text =
                    "Please enter something you need help with.";
                SupportMessageLabel.CssClass =
                    "rx-support-message error";

                return;
            }

            string search = searchText.ToLower();

            if (search.Contains("booking") ||
                search.Contains("book") ||
                search.Contains("reschedule") ||
                search.Contains("cancel"))
            {
                SupportMessageLabel.Text =
                    "For booking assistance, please visit My Bookings or contact our support team.";
            }
            else if (search.Contains("payment") ||
                     search.Contains("pay") ||
                     search.Contains("receipt") ||
                     search.Contains("refund"))
            {
                SupportMessageLabel.Text =
                    "For payment assistance, please visit My Payments or contact our support team.";
            }
            else if (search.Contains("password") ||
                     search.Contains("login") ||
                     search.Contains("account") ||
                     search.Contains("profile"))
            {
                SupportMessageLabel.Text =
                    "For account assistance, visit your Profile or Privacy & Security page.";
            }
            else
            {
                SupportMessageLabel.Text =
                    "We couldn't find a direct answer. Please contact our support team for assistance.";
            }

            SupportMessageLabel.CssClass =
                "rx-support-message success";
        }
    }
}