
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using RxEvents.Data;

namespace RxEvents
{
    public partial class Payment : System.Web.UI.Page
    {
        private readonly DatabaseHelper databaseHelper =
            new DatabaseHelper();

        private readonly PaymentRepository paymentRepository =
            new PaymentRepository();

        private int BookingId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["bookingId"], out id)
                    ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (BookingId <= 0)
            {
                ShowUnavailable("Invalid booking reference.");
                return;
            }

            if (!IsPostBack)
                LoadBooking();
        }

        private void LoadBooking()
        {
            string query = @"
                SELECT
                    BookingReference,
                    TotalAmount,
                    BookingStatus
                FROM Bookings
                WHERE BookingID = @BookingID
                  AND UserID = @UserID";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@BookingID", SqlDbType.Int)
                    .Value = BookingId;
                command.Parameters.Add("@UserID", SqlDbType.Int)
                    .Value = Convert.ToInt32(Session["UserID"]);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        ShowUnavailable(
                            "This booking could not be found for your account.");
                        return;
                    }

                    BookingReferenceLabel.Text =
                        Server.HtmlEncode(reader["BookingReference"].ToString());

                    AmountDueLabel.Text =
                        "UGX " + Convert.ToDecimal(reader["TotalAmount"])
                            .ToString("N0", CultureInfo.InvariantCulture);

                    string status = reader["BookingStatus"].ToString();
                    BookingStatusLabel.Text = Server.HtmlEncode(status);

                    if (status == "Cancelled")
                    {
                        ShowUnavailable(
                            "This booking has been cancelled and cannot be paid.");
                    }
                }
            }
        }

        protected void PayNowButton_Click(object sender, EventArgs e)
        {
            PaymentMessageLabel.Text = "";

            if (Session["UserID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            Page.Validate();

            if (!Page.IsValid)
                return;

            string method = PaymentMethodList.SelectedValue;

            if (method != "Card" &&
                method != "UPI" &&
                method != "Wallet" &&
                method != "Cash")
            {
                PaymentMessageLabel.Text =
                    "Please select a valid payment method.";
                return;
            }

            try
            {
                string paymentReference = "RXP-" +
                    Guid.NewGuid().ToString("N")
                        .Substring(0, 16).ToUpper();

                bool succeeded = paymentRepository.ProcessSimulatedPayment(
                    BookingId,
                    Convert.ToInt32(Session["UserID"]),
                    method,
                    paymentReference);

                if (succeeded)
                {
                    Response.Redirect(
                        "MyPayments.aspx?payment=success",
                        false);

                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (InvalidOperationException ex)
            {
                PaymentMessageLabel.Text =
                    Server.HtmlEncode(ex.Message);
            }
            catch (Exception)
            {
                PaymentMessageLabel.Text =
                    "The simulated payment could not be completed. " +
                    "No successful payment was recorded. Please try again.";
            }
        }

        private void ShowUnavailable(string message)
        {
            PaymentMessageLabel.Text = Server.HtmlEncode(message);
            PayNowButton.Visible = false;
            PaymentMethodList.Visible = false;
            PaymentMethodValidator.Enabled = false;
        }

        protected void PaymentMethodList_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            string method = PaymentMethodList.SelectedValue;

            CardPaymentPanel.Visible = method == "Card";
            UpiPaymentPanel.Visible = method == "UPI";
            WalletPaymentPanel.Visible = method == "Wallet";
            CashPaymentPanel.Visible = method == "Cash";

            PayNowButton.Text = method == "Cash"
                ? "Confirm Cash Payment Method"
                : "Simulate Payment";

            PaymentMessageLabel.Text = "";
        }

    }
}
