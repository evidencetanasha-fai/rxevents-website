using System;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class UserRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public UserRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public bool EmailExists(string email)
        {
            string query = @"
                SELECT COUNT(*)
                FROM Users
                WHERE Email = @Email";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();

                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
        }
        public User GetUserByEmail(string email)
        {
            User user = null;

            string query = @"
        SELECT
            U.UserID,
            U.RoleID,
            U.FirstName,
            U.LastName,
            U.Email,
            U.Phone,
            U.PasswordHash,
            U.ProfilePhoto,
            U.CreatedDate,
            U.IsActive,
            R.RoleName
        FROM Users U
        INNER JOIN Roles R
            ON U.RoleID = R.RoleID
        WHERE U.Email = @Email";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User();

                        user.UserID =
                            Convert.ToInt32(reader["UserID"]);

                        user.RoleID =
                            Convert.ToInt32(reader["RoleID"]);

                        user.FirstName =
                            reader["FirstName"].ToString();

                        user.LastName =
                            reader["LastName"].ToString();

                        user.Email =
                            reader["Email"].ToString();

                        user.Phone =
                            reader["Phone"] == DBNull.Value
                                ? null
                                : reader["Phone"].ToString();

                        user.PasswordHash =
                            reader["PasswordHash"].ToString();

                        user.ProfilePhoto =
                            reader["ProfilePhoto"] == DBNull.Value
                                ? null
                                : reader["ProfilePhoto"].ToString();

                        user.CreatedDate =
                            Convert.ToDateTime(reader["CreatedDate"]);

                        user.IsActive =
                            Convert.ToBoolean(reader["IsActive"]);

                        user.RoleName =
                            reader["RoleName"].ToString();
                    }
                }
            }

            return user;
        }

        public User GetUserById(int userId)
        {
            User user = null;

            string query = @"
    SELECT
        U.UserID,
        U.RoleID,
        U.FirstName,
        U.LastName,
        U.Email,
        U.Phone,
        U.PasswordHash,
        U.ProfilePhoto,
        U.CreatedDate,
        U.IsActive,
        U.DisplayMode,
        R.RoleName
    FROM Users U
    INNER JOIN Roles R
        ON U.RoleID = R.RoleID
    WHERE U.UserID = @UserID";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User();

                        user.UserID =
                            Convert.ToInt32(reader["UserID"]);

                        user.RoleID =
                            Convert.ToInt32(reader["RoleID"]);

                        user.FirstName =
                            reader["FirstName"].ToString();

                        user.LastName =
                            reader["LastName"].ToString();

                        user.Email =
                            reader["Email"].ToString();

                        user.Phone =
                            reader["Phone"] == DBNull.Value
                                ? null
                                : reader["Phone"].ToString();

                        user.PasswordHash =
                            reader["PasswordHash"].ToString();

                        user.ProfilePhoto =
                            reader["ProfilePhoto"] == DBNull.Value
                                ? null
                                : reader["ProfilePhoto"].ToString();

                        user.CreatedDate =
                            Convert.ToDateTime(reader["CreatedDate"]);

                        user.IsActive =
     Convert.ToBoolean(reader["IsActive"]);

                        user.DisplayMode =
                            reader["DisplayMode"].ToString();

                        user.RoleName =
                            reader["RoleName"].ToString();
                    }
                }
            }

            return user;
        }

        public bool RegisterUser(User user)
        {
            string query = @"
                INSERT INTO Users
                (
                    RoleID,
                    FirstName,
                    LastName,
                    Email,
                    Phone,
                    PasswordHash,
                    IsActive
                )
                VALUES
                (
                    @RoleID,
                    @FirstName,
                    @LastName,
                    @Email,
                    @Phone,
                    @PasswordHash,
                    1
                )";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleID", user.RoleID);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Phone",
                    string.IsNullOrEmpty(user.Phone)
                        ? (object)DBNull.Value
                        : user.Phone);
                command.Parameters.AddWithValue("@PasswordHash",
                    user.PasswordHash);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateUserProfile(User user)
        {
            string query = @"
        UPDATE Users
        SET
            FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            Phone = @Phone
        WHERE UserID = @UserID";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", user.UserID);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue(
                    "@Phone",
                    string.IsNullOrEmpty(user.Phone)
                        ? (object)DBNull.Value
                        : user.Phone);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }


        public bool UpdateProfilePhoto(int userId, string fileName)
        {
            string query = @"
        UPDATE Users
        SET ProfilePhoto = @ProfilePhoto
        WHERE UserID = @UserID";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@ProfilePhoto", fileName);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
        public bool UpdatePassword(
    int userId,
    string passwordHash)
        {
            string query = @"
        UPDATE Users
        SET PasswordHash = @PasswordHash
        WHERE UserID = @UserID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                command.Parameters.AddWithValue(
                    "@PasswordHash",
                    passwordHash);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
        public bool UpdateDisplayMode(int userId, string displayMode)
        {
            string query = @"
        UPDATE Users
        SET DisplayMode = @DisplayMode
        WHERE UserID = @UserID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                command.Parameters.AddWithValue(
                    "@DisplayMode",
                    displayMode);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}