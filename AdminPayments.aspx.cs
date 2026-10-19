using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminPayments : Page
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToString(Session["RoleID"]) != "1")
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadPaymentMethods();
                LoadStatistics();
                LoadPayments();
            }
        }

        // Load the available payment methods from the database.
        private void LoadPaymentMethods()
        {
            MethodDropDown.Items.Clear();
            MethodDropDown.Items.Add(
                new ListItem("All Methods", ""));

            const string query = @"
                SELECT DISTINCT PaymentMethod
                FROM dbo.Payments
                WHERE PaymentMethod IS NOT NULL
                  AND LTRIM(RTRIM(PaymentMethod)) <> ''
                ORDER BY PaymentMethod;";

            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string method = reader["PaymentMethod"].ToString();

                        MethodDropDown.Items.Add(
                            new ListItem(method, method));
                    }
                }
            }
        }

        // Load the four summary cards.
        private void LoadStatistics()
        {
            const string query = @"
                SELECT
                    COUNT(*) AS TotalPayments,

                    ISNULL(
                        SUM(CASE
                            WHEN PaymentStatus = 'Paid'
                            THEN Amount
                            ELSE 0
                        END), 0
                    ) AS TotalRevenue,

                    SUM(CASE
                        WHEN PaymentStatus = 'Pending'
                        THEN 1
                        ELSE 0
                    END) AS PendingPayments,

                    SUM(CASE
                        WHEN PaymentStatus = 'Paid'
                        THEN 1
                        ELSE 0
                    END) AS SuccessfulPayments

                FROM dbo.Payments;";

            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TotalPaymentsLabel.Text =
                            Convert.ToInt32(reader["TotalPayments"])
                                .ToString("N0");

                        TotalRevenueLabel.Text =
                            "₹" + Convert.ToDecimal(
                                reader["TotalRevenue"]).ToString("N2");

                        PendingPaymentsLabel.Text =
                            Convert.ToInt32(reader["PendingPayments"])
                                .ToString("N0");

                        SuccessfulPaymentsLabel.Text =
                            Convert.ToInt32(reader["SuccessfulPayments"])
                                .ToString("N0");
                    }
                }
            }
        }

        // Build the filtered payment query.
        private string GetPaymentsQuery()
        {
            return @"
                SELECT
                    p.PaymentID,
                    p.PaymentReference,
                    b.BookingReference,

                    COALESCE(
                        NULLIF(
                            LTRIM(RTRIM(
                                COALESCE(u.FirstName, '') + ' ' +
                                COALESCE(u.LastName, '')
                            )),
                            ''
                        ),
                        NULLIF(b.GuestCustomerName, ''),
                        'Guest'
                    ) AS CustomerName,

                    e.EventName,
                    p.Amount,
                    p.PaymentMethod,
                    p.PaymentDate,
                    p.PaymentStatus

                FROM dbo.Payments p

                INNER JOIN dbo.Bookings b
                    ON p.BookingID = b.BookingID

                LEFT JOIN dbo.Users u
                    ON b.UserID = u.UserID

                INNER JOIN dbo.Events e
                    ON b.EventID = e.EventID

                WHERE
                    (@Search = ''
                     OR p.PaymentReference LIKE @SearchPattern
                     OR b.BookingReference LIKE @SearchPattern
                     OR COALESCE(u.FirstName, '') LIKE @SearchPattern
                     OR COALESCE(u.LastName, '') LIKE @SearchPattern
                     OR COALESCE(b.GuestCustomerName, '') LIKE @SearchPattern
                     OR e.EventName LIKE @SearchPattern)

                    AND (@Status = ''
                         OR p.PaymentStatus = @Status)

                    AND (@Method = ''
                         OR p.PaymentMethod = @Method)

                    AND (@StartDate IS NULL
                         OR p.PaymentDate >= @StartDate)

                    AND (@EndDate IS NULL
                         OR p.PaymentDate < DATEADD(DAY, 1, @EndDate))

                ORDER BY p.PaymentDate DESC, p.PaymentID DESC";
        }

        // Add filter values safely to the SQL command.
        private void AddPaymentParameters(SqlCommand cmd)
        {
            string search = SearchTextBox.Text.Trim();

            cmd.Parameters.Add("@Search", SqlDbType.VarChar, 200)
                .Value = search;

            cmd.Parameters.Add("@SearchPattern", SqlDbType.VarChar, 402)
                .Value = "%" + search + "%";

            cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30)
                .Value = StatusDropDown.SelectedValue;

            cmd.Parameters.Add("@Method", SqlDbType.VarChar, 50)
                .Value = MethodDropDown.SelectedValue;

            DateTime startDate;
            DateTime endDate;

            object startValue = DBNull.Value;
            object endValue = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(StartDateTextBox.Text))
            {
                if (DateTime.TryParseExact(
                    StartDateTextBox.Text,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out startDate))
                {
                    startValue = startDate.Date;
                }
                else
                {
                    throw new ArgumentException(
                        "Please enter a valid start date.");
                }
            }

            if (!string.IsNullOrWhiteSpace(EndDateTextBox.Text))
            {
                if (DateTime.TryParseExact(
                    EndDateTextBox.Text,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out endDate))
                {
                    endValue = endDate.Date;
                }
                else
                {
                    throw new ArgumentException(
                        "Please enter a valid end date.");
                }
            }

            if (startValue != DBNull.Value &&
                endValue != DBNull.Value &&
                (DateTime)startValue > (DateTime)endValue)
            {
                throw new ArgumentException(
                    "The start date cannot be after the end date.");
            }

            cmd.Parameters.Add("@StartDate", SqlDbType.Date)
                .Value = startValue;

            cmd.Parameters.Add("@EndDate", SqlDbType.Date)
                .Value = endValue;
        }

        // Load the payment table.
        private void LoadPayments()
        {
            try
            {
                string query = GetPaymentsQuery();

                using (SqlConnection con = db.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddPaymentParameters(cmd);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        PaymentsGridView.DataSource = table;
                        PaymentsGridView.DataBind();

                        RecordCountLabel.Text =
                            table.Rows.Count.ToString("N0") +
                            (table.Rows.Count == 1
                                ? " record"
                                : " records");
                    }
                }

                MessageLabel.Text = "";
            }
            catch (ArgumentException ex)
            {
                PaymentsGridView.DataSource = null;
                PaymentsGridView.DataBind();
                RecordCountLabel.Text = "0 records";
                MessageLabel.Text = ex.Message;
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to load payment records. Please check the database connection.";
            }
        }

        // Return the CSS class for the payment status.
        protected string GetPaymentStatusClass(object statusValue)
        {
            string status = Convert.ToString(statusValue);

            switch (status.ToLowerInvariant())
            {
                case "paid":
                    return "payment-status payment-status-paid";

                case "pending":
                    return "payment-status payment-status-pending";

                case "failed":
                    return "payment-status payment-status-failed";

                case "refunded":
                    return "payment-status payment-status-refunded";

                default:
                    return "payment-status";
            }
        }

        // Apply filters.
        protected void ApplyFilterButton_Click(
            object sender, EventArgs e)
        {
            PaymentsGridView.PageIndex = 0;
            LoadPayments();
        }

        // Reset all filters.
        protected void ResetFilterButton_Click(
            object sender, EventArgs e)
        {
            SearchTextBox.Text = "";
            StatusDropDown.SelectedIndex = 0;
            MethodDropDown.SelectedIndex = 0;
            StartDateTextBox.Text = "";
            EndDateTextBox.Text = "";

            PaymentsGridView.PageIndex = 0;
            LoadPayments();
        }

        // Handle pagination.
        protected void PaymentsGridView_PageIndexChanging(
            object sender, GridViewPageEventArgs e)
        {
            PaymentsGridView.PageIndex = e.NewPageIndex;
            LoadPayments();
        }

        // View the selected payment's details.
        protected void PaymentsGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "ViewPayment")
                return;

            int paymentId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out paymentId))
            {
                MessageLabel.Text = "Invalid payment record.";
                return;
            }

            const string query = @"
                SELECT
                    p.PaymentReference,
                    b.BookingReference,
                    p.Amount,
                    p.PaymentMethod,
                    p.PaymentDate,
                    p.PaymentStatus,
                    p.TransactionReference,
                    e.EventName,
                    COALESCE(
                        NULLIF(
                            LTRIM(RTRIM(
                                COALESCE(u.FirstName, '') + ' ' +
                                COALESCE(u.LastName, '')
                            )),
                            ''
                        ),
                        NULLIF(b.GuestCustomerName, ''),
                        'Guest'
                    ) AS CustomerName

                FROM dbo.Payments p

                INNER JOIN dbo.Bookings b
                    ON p.BookingID = b.BookingID

                LEFT JOIN dbo.Users u
                    ON b.UserID = u.UserID

                INNER JOIN dbo.Events e
                    ON b.EventID = e.EventID

                WHERE p.PaymentID = @PaymentID;";

            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@PaymentID", SqlDbType.Int)
                    .Value = paymentId;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string details =
                            "Payment: " +
                            reader["PaymentReference"] +
                            " | Booking: " +
                            reader["BookingReference"] +
                            " | Customer: " +
                            reader["CustomerName"] +
                            " | Event: " +
                            reader["EventName"] +
                            " | Amount: ₹" +
                            Convert.ToDecimal(reader["Amount"])
                                .ToString("N2") +
                            " | Method: " +
                            reader["PaymentMethod"] +
                            " | Date: " +
                            Convert.ToDateTime(reader["PaymentDate"])
                                .ToString("dd MMM yyyy HH:mm") +
                            " | Status: " +
                            reader["PaymentStatus"] +
                            " | Transaction: " +
                            (reader["TransactionReference"] == DBNull.Value
                                ? "Not provided"
                                : reader["TransactionReference"].ToString());

                        MessageLabel.Text = details;
                    }
                    else
                    {
                        MessageLabel.Text = "Payment record not found.";
                    }
                }
            }
        }

        // Export the filtered results to CSV.
        protected void ExportPaymentsButton_Click(
            object sender, EventArgs e)
        {
            try
            {
                string query = GetPaymentsQuery();

                using (SqlConnection con = db.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddPaymentParameters(cmd);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        StringBuilder csv = new StringBuilder();

                        csv.AppendLine(
                            "Payment Reference,Booking Reference,Customer,Event,Amount,Payment Method,Payment Date,Payment Status");

                        foreach (DataRow row in table.Rows)
                        {
                            csv.AppendLine(
                                CsvValue(row["PaymentReference"]) + "," +
                                CsvValue(row["BookingReference"]) + "," +
                                CsvValue(row["CustomerName"]) + "," +
                                CsvValue(row["EventName"]) + "," +
                                CsvValue(row["Amount"]) + "," +
                                CsvValue(row["PaymentMethod"]) + "," +
                                CsvValue(
                                    Convert.ToDateTime(row["PaymentDate"])
                                        .ToString("yyyy-MM-dd HH:mm:ss")) + "," +
                                CsvValue(row["PaymentStatus"]));
                        }

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "text/csv";
                        Response.ContentEncoding = Encoding.UTF8;
                        Response.AddHeader(
                            "Content-Disposition",
                            "attachment;filename=RxEventsPayments.csv");

                        Response.BinaryWrite(
                            Encoding.UTF8.GetPreamble());

                        Response.Write(csv.ToString());
                        Response.Flush();

                        HttpContext.Current.ApplicationInstance
                            .CompleteRequest();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageLabel.Text = ex.Message;
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to export payments. Please try again.";
            }
        }

        // Escape CSV values.
        private string CsvValue(object value)
        {
            string text = value == null || value == DBNull.Value
                ? ""
                : Convert.ToString(
                    value, CultureInfo.InvariantCulture);

            // Prevent spreadsheet formula execution from exported values.
            if (text.Length > 0 &&
                (text[0] == '=' || text[0] == '+' ||
                 text[0] == '-' || text[0] == '@' ||
                 text[0] == '\t' || text[0] == '\r'))
            {
                text = "'" + text;
            }

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}