using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class AddressRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public AddressRepository()
        {
            databaseHelper = new DatabaseHelper();
        }


        // ==========================================
        // GET ALL ADDRESSES FOR A USER
        // ==========================================

        public List<Address> GetAddressesByUser(int userId)
        {
            List<Address> addresses =
                new List<Address>();

            string query = @"
                SELECT
                    AddressID,
                    UserID,
                    AddressLine1,
                    AddressLine2,
                    City,
                    State,
                    PostalCode,
                    Country,
                    IsDefault,
                    CreatedDate,
                    UpdatedDate
                FROM Addresses
                WHERE UserID = @UserID
                ORDER BY IsDefault DESC, AddressID DESC";


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
                        Address address =
                            new Address();

                        address.AddressID =
                            Convert.ToInt32(
                                reader["AddressID"]);

                        address.UserID =
                            Convert.ToInt32(
                                reader["UserID"]);

                        address.AddressLine1 =
                            reader["AddressLine1"].ToString();

                        address.AddressLine2 =
                            reader["AddressLine2"] == DBNull.Value
                                ? ""
                                : reader["AddressLine2"].ToString();

                        address.City =
                            reader["City"].ToString();

                        address.State =
                            reader["State"].ToString();

                        address.PostalCode =
                            reader["PostalCode"].ToString();

                        address.Country =
                            reader["Country"].ToString();

                        address.IsDefault =
                            Convert.ToBoolean(
                                reader["IsDefault"]);

                        address.CreatedDate =
                            Convert.ToDateTime(
                                reader["CreatedDate"]);

                        address.UpdatedDate =
                            Convert.ToDateTime(
                                reader["UpdatedDate"]);


                        addresses.Add(address);
                    }
                }
            }


            return addresses;
        }


        // ==========================================
        // GET DEFAULT ADDRESS
        // ==========================================

        public Address GetDefaultAddress(int userId)
        {
            Address address = null;

            string query = @"
                SELECT TOP 1
                    AddressID,
                    UserID,
                    AddressLine1,
                    AddressLine2,
                    City,
                    State,
                    PostalCode,
                    Country,
                    IsDefault,
                    CreatedDate,
                    UpdatedDate
                FROM Addresses
                WHERE UserID = @UserID
                  AND IsDefault = 1
                ORDER BY AddressID DESC";


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
                        address =
                            MapAddress(reader);
                    }
                }
            }


            return address;
        }


        // ==========================================
        // GET ADDRESS BY ID
        // ==========================================

        public Address GetAddressById(
            int addressId,
            int userId)
        {
            Address address = null;

            string query = @"
                SELECT
                    AddressID,
                    UserID,
                    AddressLine1,
                    AddressLine2,
                    City,
                    State,
                    PostalCode,
                    Country,
                    IsDefault,
                    CreatedDate,
                    UpdatedDate
                FROM Addresses
                WHERE AddressID = @AddressID
                  AND UserID = @UserID";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@AddressID",
                    addressId);

                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();


                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        address =
                            MapAddress(reader);
                    }
                }
            }


            return address;
        }


        // ==========================================
        // ADD ADDRESS
        // ==========================================

        public bool AddAddress(Address address)
        {
            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            {
                connection.Open();


                // If this is the first address,
                // automatically make it default.

                string countQuery = @"
                    SELECT COUNT(*)
                    FROM Addresses
                    WHERE UserID = @UserID";


                int addressCount;


                using (SqlCommand countCommand =
                       new SqlCommand(
                           countQuery,
                           connection))
                {
                    countCommand.Parameters.AddWithValue(
                        "@UserID",
                        address.UserID);

                    addressCount =
                        Convert.ToInt32(
                            countCommand.ExecuteScalar());
                }


                if (addressCount == 0)
                {
                    address.IsDefault = true;
                }


                // If this address is default,
                // remove default status from
                // the user's other addresses.

                if (address.IsDefault)
                {
                    ClearDefaultAddress(
                        connection,
                        address.UserID);
                }


                string query = @"
                    INSERT INTO Addresses
                    (
                        UserID,
                        AddressLine1,
                        AddressLine2,
                        City,
                        State,
                        PostalCode,
                        Country,
                        IsDefault
                    )
                    VALUES
                    (
                        @UserID,
                        @AddressLine1,
                        @AddressLine2,
                        @City,
                        @State,
                        @PostalCode,
                        @Country,
                        @IsDefault
                    )";


                using (SqlCommand command =
                       new SqlCommand(
                           query,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@UserID",
                        address.UserID);

                    command.Parameters.AddWithValue(
                        "@AddressLine1",
                        address.AddressLine1);

                    command.Parameters.AddWithValue(
                        "@AddressLine2",
                        string.IsNullOrWhiteSpace(
                            address.AddressLine2)
                            ? (object)DBNull.Value
                            : address.AddressLine2);

                    command.Parameters.AddWithValue(
                        "@City",
                        address.City);

                    command.Parameters.AddWithValue(
                        "@State",
                        address.State);

                    command.Parameters.AddWithValue(
                        "@PostalCode",
                        address.PostalCode);

                    command.Parameters.AddWithValue(
                        "@Country",
                        address.Country);

                    command.Parameters.AddWithValue(
                        "@IsDefault",
                        address.IsDefault);


                    return command.ExecuteNonQuery() > 0;
                }
            }
        }


        // ==========================================
        // UPDATE ADDRESS
        // ==========================================

        public bool UpdateAddress(Address address)
        {
            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            {
                connection.Open();


                if (address.IsDefault)
                {
                    ClearDefaultAddress(
                        connection,
                        address.UserID);
                }


                string query = @"
                    UPDATE Addresses
                    SET
                        AddressLine1 = @AddressLine1,
                        AddressLine2 = @AddressLine2,
                        City = @City,
                        State = @State,
                        PostalCode = @PostalCode,
                        Country = @Country,
                        IsDefault = @IsDefault,
                        UpdatedDate = GETDATE()
                    WHERE AddressID = @AddressID
                      AND UserID = @UserID";


                using (SqlCommand command =
                       new SqlCommand(
                           query,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@AddressID",
                        address.AddressID);

                    command.Parameters.AddWithValue(
                        "@UserID",
                        address.UserID);

                    command.Parameters.AddWithValue(
                        "@AddressLine1",
                        address.AddressLine1);

                    command.Parameters.AddWithValue(
                        "@AddressLine2",
                        string.IsNullOrWhiteSpace(
                            address.AddressLine2)
                            ? (object)DBNull.Value
                            : address.AddressLine2);

                    command.Parameters.AddWithValue(
                        "@City",
                        address.City);

                    command.Parameters.AddWithValue(
                        "@State",
                        address.State);

                    command.Parameters.AddWithValue(
                        "@PostalCode",
                        address.PostalCode);

                    command.Parameters.AddWithValue(
                        "@Country",
                        address.Country);

                    command.Parameters.AddWithValue(
                        "@IsDefault",
                        address.IsDefault);


                    return command.ExecuteNonQuery() > 0;
                }
            }
        }


        // ==========================================
        // DELETE ADDRESS
        // ==========================================

        public bool DeleteAddress(
            int addressId,
            int userId)
        {
            string query = @"
                DELETE FROM Addresses
                WHERE AddressID = @AddressID
                  AND UserID = @UserID";


            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(
                       query,
                       connection))
            {
                command.Parameters.AddWithValue(
                    "@AddressID",
                    addressId);

                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();


                return command.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // SET DEFAULT ADDRESS
        // ==========================================

        public bool SetDefaultAddress(
            int addressId,
            int userId)
        {
            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            {
                connection.Open();


                // Remove default from all
                // addresses belonging to user.

                ClearDefaultAddress(
                    connection,
                    userId);


                string query = @"
                    UPDATE Addresses
                    SET
                        IsDefault = 1,
                        UpdatedDate = GETDATE()
                    WHERE AddressID = @AddressID
                      AND UserID = @UserID";


                using (SqlCommand command =
                       new SqlCommand(
                           query,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@AddressID",
                        addressId);

                    command.Parameters.AddWithValue(
                        "@UserID",
                        userId);


                    return command.ExecuteNonQuery() > 0;
                }
            }
        }


        // ==========================================
        // CLEAR DEFAULT ADDRESS
        // ==========================================

        private void ClearDefaultAddress(
            SqlConnection connection,
            int userId)
        {
            string query = @"
                UPDATE Addresses
                SET
                    IsDefault = 0
                WHERE UserID = @UserID";


            using (SqlCommand command =
                   new SqlCommand(
                       query,
                       connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                command.ExecuteNonQuery();
            }
        }


        // ==========================================
        // MAP DATABASE RECORD TO MODEL
        // ==========================================

        private Address MapAddress(
            SqlDataReader reader)
        {
            Address address =
                new Address();


            address.AddressID =
                Convert.ToInt32(
                    reader["AddressID"]);

            address.UserID =
                Convert.ToInt32(
                    reader["UserID"]);

            address.AddressLine1 =
                reader["AddressLine1"].ToString();

            address.AddressLine2 =
                reader["AddressLine2"] == DBNull.Value
                    ? ""
                    : reader["AddressLine2"].ToString();

            address.City =
                reader["City"].ToString();

            address.State =
                reader["State"].ToString();

            address.PostalCode =
                reader["PostalCode"].ToString();

            address.Country =
                reader["Country"].ToString();

            address.IsDefault =
                Convert.ToBoolean(
                    reader["IsDefault"]);

            address.CreatedDate =
                Convert.ToDateTime(
                    reader["CreatedDate"]);

            address.UpdatedDate =
                Convert.ToDateTime(
                    reader["UpdatedDate"]);


            return address;
        }
    }
}