using RxEvents.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RxEvents.Data
{
    public class BookingRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public BookingRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        // ==========================================
        // GET PREFERRED EVENTS
        // ==========================================

        public List<PreferredEvent> GetPreferredEvents(int userId)
        {
            List<PreferredEvent> preferredEvents =
                new List<PreferredEvent>();

            string query = @"
                SELECT TOP 3
                    ec.CategoryID,
                    ec.CategoryName,
                    COUNT(*) AS BookingCount
                FROM Bookings b
                INNER JOIN Events e
                    ON b.EventID = e.EventID
                INNER JOIN EventCategories ec
                    ON e.CategoryID = ec.CategoryID
                WHERE b.UserID = @UserID
                  AND b.BookingStatus IN ('Confirmed', 'Completed')
                GROUP BY
                    ec.CategoryID,
                    ec.CategoryName
                ORDER BY
                    BookingCount DESC,
                    ec.CategoryName ASC";

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
                        PreferredEvent preferredEvent =
                            new PreferredEvent();

                        preferredEvent.CategoryID =
                            Convert.ToInt32(
                                reader["CategoryID"]);

                        preferredEvent.CategoryName =
                            reader["CategoryName"].ToString();

                        preferredEvent.BookingCount =
                            Convert.ToInt32(
                                reader["BookingCount"]);

                        preferredEvents.Add(
                            preferredEvent);
                    }
                }
            }

            return preferredEvents;
        }
        public List<Booking> GetReviewableBookings(int userId)
        {
            List<Booking> bookings =
                new List<Booking>();

            string query = @"
        SELECT
            b.BookingID,
            b.UserID,
            b.EventID,
            b.BookingStatus,
            e.EventName
        FROM Bookings b
        INNER JOIN Events e
            ON b.EventID = e.EventID
        WHERE b.UserID = @UserID
          AND b.BookingStatus = 'Completed'
          AND NOT EXISTS
          (
              SELECT 1
              FROM Reviews r
              WHERE r.BookingID = b.BookingID
                AND r.UserID = b.UserID
          )
        ORDER BY b.BookingID DESC";

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
                        Booking booking =
                            new Booking();

                        booking.BookingID =
                            Convert.ToInt32(
                                reader["BookingID"]);

                        booking.UserID =
                            Convert.ToInt32(
                                reader["UserID"]);

                        booking.EventID =
                            Convert.ToInt32(
                                reader["EventID"]);

                        booking.BookingStatus =
                            reader["BookingStatus"].ToString();

                        booking.EventName =
                            reader["EventName"].ToString();

                        bookings.Add(booking);
                    }
                }
            }

            return bookings;
        }
        // ==========================================
        // GET CUSTOMER BOOKINGS FOR WISHLIST ATTACHMENT
        // ==========================================

        public List<Booking> GetBookingsForWishlistAttachment(int userId)
        {
            List<Booking> bookings =
                new List<Booking>();

            string query = @"
        SELECT
            b.BookingID,
            b.UserID,
            b.EventID,
            b.WishlistFolderID,
            b.BookingStatus,
            e.EventName
        FROM Bookings b
        INNER JOIN Events e
            ON b.EventID = e.EventID
        WHERE b.UserID = @UserID
        ORDER BY b.BookingID DESC";

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
                        Booking booking =
                            new Booking();

                        booking.BookingID =
                            Convert.ToInt32(
                                reader["BookingID"]);

                        booking.UserID =
                            Convert.ToInt32(
                                reader["UserID"]);

                        booking.EventID =
                            Convert.ToInt32(
                                reader["EventID"]);

                        booking.WishlistFolderID =
                            reader["WishlistFolderID"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(
                                    reader["WishlistFolderID"]);

                        booking.BookingStatus =
                            reader["BookingStatus"].ToString();

                        booking.EventName =
                            reader["EventName"].ToString();

                        bookings.Add(booking);
                    }
                }
            }

            return bookings;
        }
        // ==========================================
        // ATTACH WISHLIST FOLDER TO BOOKING
        // ==========================================

        public bool AttachWishlistFolderToBooking(
            int bookingId,
            int userId,
            int wishlistFolderId)
        {
            string query = @"
        UPDATE Bookings
        SET WishlistFolderID = @WishlistFolderID
        WHERE BookingID = @BookingID
          AND UserID = @UserID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@WishlistFolderID",
                    wishlistFolderId);

                command.Parameters.AddWithValue(
                    "@BookingID",
                    bookingId);

                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        public int CreateBooking(Booking booking)
        {
            string query = @"
        INSERT INTO Bookings
        (
            BookingReference,
            UserID,
            EventID,
            VenueID,
            PackageID,
            EventDate,
            EventTime,
            GuestCount,
            TotalAmount,
            BookingStatus,
            AdditionalNotes,
            WishlistFolderID,
            VenueAddress
        )
        VALUES
        (
            @BookingReference,
            @UserID,
            @EventID,
            @VenueID,
            @PackageID,
            @EventDate,
            @EventTime,
            @GuestCount,
            @TotalAmount,
            'Pending',
            @AdditionalNotes,
            @WishlistFolderID,
            @VenueAddress
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@BookingReference",
                    SqlDbType.VarChar, 30).Value =
                    booking.BookingReference;

                command.Parameters.Add(
                    "@UserID",
                    SqlDbType.Int).Value =
                    booking.UserID;

                command.Parameters.Add(
                    "@EventID",
                    SqlDbType.Int).Value =
                    booking.EventID;

                command.Parameters.Add("@VenueID", SqlDbType.Int).Value =
    booking.VenueID.HasValue
        ? (object)booking.VenueID.Value
        : DBNull.Value;

                command.Parameters.Add(
                    "@PackageID",
                    SqlDbType.Int).Value =
                    booking.PackageID;

                command.Parameters.Add(
                    "@EventDate",
                    SqlDbType.Date).Value =
                    booking.EventDate;

                command.Parameters.Add(
                    "@EventTime",
                    SqlDbType.Time).Value =
                    booking.EventTime;

                command.Parameters.Add(
                    "@GuestCount",
                    SqlDbType.Int).Value =
                    booking.GuestCount;

                command.Parameters.Add(
                    "@TotalAmount",
                    SqlDbType.Decimal).Value =
                    booking.TotalAmount;

                command.Parameters["@TotalAmount"].Precision = 12;
                command.Parameters["@TotalAmount"].Scale = 2;

                command.Parameters.Add(
                    "@AdditionalNotes",
                    SqlDbType.VarChar, 1000).Value =
                    string.IsNullOrWhiteSpace(booking.AdditionalNotes)
                        ? (object)DBNull.Value
                        : booking.AdditionalNotes;

                command.Parameters.Add(
                    "@WishlistFolderID",
                    SqlDbType.Int).Value =
                    booking.WishlistFolderID.HasValue
                        ? (object)booking.WishlistFolderID.Value
                        : DBNull.Value;

                command.Parameters.Add(
                    "@VenueAddress",
                    SqlDbType.VarChar, 500).Value =
                    string.IsNullOrWhiteSpace(booking.VenueAddress)
                        ? (object)DBNull.Value
                        : booking.VenueAddress;

                connection.Open();

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public DataTable GetCustomerBookingHistory(int userId)
        {
            DataTable table = new DataTable();

            string query = @"
        SELECT
            b.BookingID,
            b.BookingReference,
            b.EventDate,
            b.EventTime,
            b.VenueAddress,
            b.BookingStatus,
            b.TotalAmount,
            e.EventName,
            e.EventImage,
            p.PackageName
        FROM Bookings b
        INNER JOIN Events e
            ON b.EventID = e.EventID
        INNER JOIN Packages p
            ON b.PackageID = p.PackageID
        WHERE b.UserID = @UserID
        ORDER BY b.EventDate DESC, b.BookingID DESC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                command.Parameters.Add(
                    "@UserID", SqlDbType.Int).Value = userId;

                adapter.Fill(table);
            }

            return table;
        }

    }
}