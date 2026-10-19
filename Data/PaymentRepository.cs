
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class PaymentRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public PaymentRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public List<RxEvents.Models.Payment> GetPaymentsByUser(int userId)
        {
            List<RxEvents.Models.Payment> payments =
    new List<RxEvents.Models.Payment>();

            string query = @"
                SELECT
                    p.PaymentID,
                    p.BookingID,
                    p.PaymentReference,
                    p.Amount,
                    p.PaymentMethod,
                    p.PaymentDate,
                    p.PaymentStatus,
                    p.TransactionReference
                FROM Payments p
                INNER JOIN Bookings b
                    ON p.BookingID = b.BookingID
                WHERE b.UserID = @UserID
                ORDER BY p.PaymentDate DESC, p.PaymentID DESC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@UserID", SqlDbType.Int).Value = userId;

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(new RxEvents.Models.Payment
                        {
                            PaymentID =
                                Convert.ToInt32(reader["PaymentID"]),

                            BookingID =
                                Convert.ToInt32(reader["BookingID"]),

                            PaymentReference =
                                reader["PaymentReference"].ToString(),

                            Amount =
                                Convert.ToDecimal(reader["Amount"]),

                            PaymentMethod =
                                reader["PaymentMethod"].ToString(),

                            PaymentDate =
                                Convert.ToDateTime(reader["PaymentDate"]),

                            PaymentStatus =
                                reader["PaymentStatus"].ToString(),

                            TransactionReference =
                                reader["TransactionReference"] == DBNull.Value
                                    ? null
                                    : reader["TransactionReference"].ToString()
                        });
                    }
                }
            }

            return payments;
        }

        public bool ProcessSimulatedPayment(
            int bookingId,
            int userId,
            string paymentMethod,
            string paymentReference)
        {
            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Verify ownership and get the amount due.
                        string bookingQuery = @"
                    SELECT TotalAmount, BookingStatus
                    FROM Bookings
                    WHERE BookingID = @BookingID
                      AND UserID = @UserID";

                        decimal amount;
                        string bookingStatus;

                        using (SqlCommand command = new SqlCommand(
                            bookingQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BookingID", SqlDbType.Int)
                                .Value = bookingId;
                            command.Parameters.Add("@UserID", SqlDbType.Int)
                                .Value = userId;

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                    throw new InvalidOperationException(
                                        "Booking not found.");

                                amount = Convert.ToDecimal(reader["TotalAmount"]);
                                bookingStatus = reader["BookingStatus"].ToString();
                            }
                        }

                        if (amount <= 0)
                            throw new InvalidOperationException(
                                "The booking amount must be greater than zero.");

                        if (bookingStatus == "Cancelled")
                            throw new InvalidOperationException(
                                "Cancelled bookings cannot be paid.");

                        // Prevent a second successful payment.
                        string existingQuery = @"
                    SELECT COUNT(*)
                    FROM Payments
                    WHERE BookingID = @BookingID
                      AND PaymentStatus = 'Paid'";

                        using (SqlCommand command = new SqlCommand(
                            existingQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BookingID", SqlDbType.Int)
                                .Value = bookingId;

                            if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                                throw new InvalidOperationException(
                                    "This booking has already been paid.");
                        }

                        // The simulated action has reached its success point.
                        // Record the successful demo transaction.
                        string insertQuery = @"
                    INSERT INTO Payments
                    (
                        BookingID,
                        PaymentReference,
                        Amount,
                        PaymentMethod,
                        PaymentStatus,
                        TransactionReference
                    )
                    VALUES
                    (
                        @BookingID,
                        @PaymentReference,
                        @Amount,
                        @PaymentMethod,
                        'Paid',
                        @TransactionReference
                    )";

                        using (SqlCommand command = new SqlCommand(
                            insertQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BookingID", SqlDbType.Int)
                                .Value = bookingId;

                            command.Parameters.Add("@PaymentReference",
                                SqlDbType.VarChar, 50).Value = paymentReference;

                            SqlParameter amountParameter =
                                command.Parameters.Add("@Amount", SqlDbType.Decimal);
                            amountParameter.Precision = 12;
                            amountParameter.Scale = 2;
                            amountParameter.Value = amount;

                            command.Parameters.Add("@PaymentMethod",
                                SqlDbType.VarChar, 50).Value = paymentMethod;

                            command.Parameters.Add("@TransactionReference",
                                SqlDbType.VarChar, 100).Value =
                                "DEMO-" + Guid.NewGuid().ToString("N")
                                    .Substring(0, 16).ToUpper();

                            command.ExecuteNonQuery();
                        }

                        // Update the booking only after inserting the payment.
                        using (SqlCommand command = new SqlCommand(@"
                    UPDATE Bookings
                    SET BookingStatus = 'Confirmed'
                    WHERE BookingID = @BookingID
                      AND UserID = @UserID
                      AND BookingStatus <> 'Cancelled'",
                            connection, transaction))
                        {
                            command.Parameters.Add("@BookingID", SqlDbType.Int)
                                .Value = bookingId;
                            command.Parameters.Add("@UserID", SqlDbType.Int)
                                .Value = userId;

                            if (command.ExecuteNonQuery() != 1)
                                throw new InvalidOperationException(
                                    "The booking could not be confirmed.");
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
