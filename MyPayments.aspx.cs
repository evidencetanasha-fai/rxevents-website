
using System;
using System.Linq;
using RxEvents.Data;
using System.Web.UI;

namespace RxEvents
{
    public partial class MyPayments : Page
    {
        private readonly PaymentRepository paymentRepository =
            new PaymentRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadPayments();
            }
        }

        private void LoadPayments()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            var payments =
                paymentRepository.GetPaymentsByUser(userId);

            PaymentsGridView.DataSource = payments;
            PaymentsGridView.DataBind();

            TotalPaymentsLabel.Text =
                payments.Count.ToString();

            decimal totalPaid = payments
                .Where(p => string.Equals(
                    p.PaymentStatus,
                    "Paid",
                    StringComparison.OrdinalIgnoreCase))
                .Sum(p => p.Amount);

            TotalPaidLabel.Text =
                "UGX " + totalPaid.ToString("N0");

            PendingPaymentsLabel.Text =
                payments.Count(p => string.Equals(
                    p.PaymentStatus,
                    "Pending",
                    StringComparison.OrdinalIgnoreCase))
                .ToString();

            PaymentMessageLabel.Visible =
                payments.Count == 0;

            if (payments.Count == 0)
            {
                PaymentMessageLabel.Text =
                    "You haven't made any payments yet. " +
                    "Your payment history will appear here.";
            }
        }

        protected string GetPaymentStatusClass(object status)
        {
            string value = status == null
                ? ""
                : status.ToString().Trim().ToLowerInvariant();

            switch (value)
            {
                case "paid":
                    return "rx-payment-status rx-payment-status-paid";

                case "pending":
                    return "rx-payment-status rx-payment-status-pending";

                case "failed":
                    return "rx-payment-status rx-payment-status-failed";

                case "refunded":
                    return "rx-payment-status rx-payment-status-refunded";

                default:
                    return "rx-payment-status";
            }
        }
    }
}
