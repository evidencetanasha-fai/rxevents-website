using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;
using WishlistFolderModel = RxEvents.Models.WishlistFolder;

namespace RxEvents.Data
{
    public class WishlistRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public WishlistRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        // ==========================================
        // GET FOLDERS FOR A CUSTOMER
        // ==========================================

        public List<WishlistFolderModel> GetFoldersByUser(int userId)
        {
            List<WishlistFolderModel> folders =
    new List<WishlistFolderModel>();

            string query = @"
                SELECT
                    wf.WishlistFolderID,
                    wf.UserID,
                    wf.CategoryID,
                    wf.FolderName,
                    wf.Budget,
                    wf.Notes,
                    wf.CreatedDate,
                    wf.UpdatedDate,
                    ec.CategoryName,

                    COUNT(wi.WishlistImageID) AS ImageCount,

                    (
                        SELECT TOP 1 wi2.ImagePath
                        FROM WishlistImages wi2
                        WHERE wi2.WishlistFolderID =
                              wf.WishlistFolderID
                        ORDER BY wi2.DateAdded ASC
                    ) AS CoverImage

                FROM WishlistFolders wf

                INNER JOIN EventCategories ec
                    ON wf.CategoryID = ec.CategoryID

                LEFT JOIN WishlistImages wi
                    ON wf.WishlistFolderID =
                       wi.WishlistFolderID

                WHERE wf.UserID = @UserID

                GROUP BY
                    wf.WishlistFolderID,
                    wf.UserID,
                    wf.CategoryID,
                    wf.FolderName,
                    wf.Budget,
                    wf.Notes,
                    wf.CreatedDate,
                    wf.UpdatedDate,
                    ec.CategoryName

                ORDER BY wf.UpdatedDate DESC,
                         wf.CreatedDate DESC";

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
                        WishlistFolderModel folder =
    new WishlistFolderModel();

                        folder.WishlistFolderID =
                            Convert.ToInt32(
                                reader["WishlistFolderID"]);

                        folder.UserID =
                            Convert.ToInt32(
                                reader["UserID"]);

                        folder.CategoryID =
                            Convert.ToInt32(
                                reader["CategoryID"]);

                        folder.FolderName =
                            reader["FolderName"].ToString();

                        folder.Budget =
                            reader["Budget"] == DBNull.Value
                                ? (decimal?)null
                                : Convert.ToDecimal(
                                    reader["Budget"]);

                        folder.Notes =
                            reader["Notes"] == DBNull.Value
                                ? null
                                : reader["Notes"].ToString();

                        folder.CreatedDate =
                            Convert.ToDateTime(
                                reader["CreatedDate"]);

                        folder.UpdatedDate =
                            reader["UpdatedDate"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(
                                    reader["UpdatedDate"]);

                        folder.CategoryName =
                            reader["CategoryName"].ToString();

                        folder.ImageCount =
                            Convert.ToInt32(
                                reader["ImageCount"]);

                        folder.CoverImage =
                            reader["CoverImage"] == DBNull.Value
                                ? null
                                : reader["CoverImage"].ToString();

                        folders.Add(folder);
                    }
                }
            }

            return folders;
        }

        // ==========================================
        // CREATE FOLDER
        // ==========================================

        public int CreateFolder(WishlistFolderModel folder)
        {
            string query = @"
                INSERT INTO WishlistFolders
                (
                    UserID,
                    CategoryID,
                    FolderName,
                    Budget,
                    Notes
                )
                OUTPUT INSERTED.WishlistFolderID
                VALUES
                (
                    @UserID,
                    @CategoryID,
                    @FolderName,
                    @Budget,
                    @Notes
                )";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@UserID",
                    folder.UserID);

                command.Parameters.AddWithValue(
                    "@CategoryID",
                    folder.CategoryID);

                command.Parameters.AddWithValue(
                    "@FolderName",
                    folder.FolderName);

                command.Parameters.AddWithValue(
                    "@Budget",
                    folder.Budget.HasValue
                        ? (object)folder.Budget.Value
                        : DBNull.Value);

                command.Parameters.AddWithValue(
                    "@Notes",
                    string.IsNullOrWhiteSpace(folder.Notes)
                        ? (object)DBNull.Value
                        : folder.Notes);

                connection.Open();

                return Convert.ToInt32(
                    command.ExecuteScalar());
            }
        }

        // ==========================================
        // GET SINGLE FOLDER
        // ==========================================


        public WishlistFolderModel GetFolderById(int folderId, int userId)
        {
            WishlistFolderModel folder = null;

            string query = @"
        SELECT
            wf.WishlistFolderID,
            wf.UserID,
            wf.CategoryID,
            wf.FolderName,
            wf.Budget,
            wf.Notes,
            wf.CreatedDate,
            wf.UpdatedDate,
            ec.CategoryName,
            (
                SELECT COUNT(*)
                FROM WishlistImages wi
                WHERE wi.WishlistFolderID = wf.WishlistFolderID
            ) AS ImageCount,
            (
                SELECT TOP 1 wi.ImagePath
                FROM WishlistImages wi
                WHERE wi.WishlistFolderID = wf.WishlistFolderID
                ORDER BY wi.DateAdded DESC
            ) AS CoverImage
        FROM WishlistFolders wf
        INNER JOIN EventCategories ec
            ON wf.CategoryID = ec.CategoryID
        WHERE wf.WishlistFolderID = @FolderID
          AND wf.UserID = @UserID";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FolderID", folderId);
                command.Parameters.AddWithValue("@UserID", userId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        folder = new WishlistFolderModel
                        {
                            WishlistFolderID =
                                Convert.ToInt32(reader["WishlistFolderID"]),

                            UserID =
                                Convert.ToInt32(reader["UserID"]),

                            CategoryID =
                                Convert.ToInt32(reader["CategoryID"]),

                            FolderName =
                                reader["FolderName"].ToString(),

                            Budget = reader["Budget"] == DBNull.Value
                                ? (decimal?)null
                                : Convert.ToDecimal(reader["Budget"]),

                            Notes = reader["Notes"] == DBNull.Value
                                ? null
                                : reader["Notes"].ToString(),

                            CreatedDate =
                                Convert.ToDateTime(reader["CreatedDate"]),

                            UpdatedDate = reader["UpdatedDate"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(reader["UpdatedDate"]),

                            CategoryName =
                                reader["CategoryName"].ToString(),

                            ImageCount =
                                Convert.ToInt32(reader["ImageCount"]),

                            CoverImage = reader["CoverImage"] == DBNull.Value
                                ? null
                                : reader["CoverImage"].ToString()
                        };
                    }
                }
            }

            return folder;
        }


        // ==========================================
        // DELETE FOLDER
        // ==========================================

        public bool DeleteFolder(
            int folderId,
            int userId)
        {
            string query = @"
                DELETE FROM WishlistFolders
                WHERE WishlistFolderID = @FolderID
                  AND UserID = @UserID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@FolderID",
                    folderId);

                command.Parameters.AddWithValue(
                    "@UserID",
                    userId);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        // ==========================================
        // GET IMAGES IN FOLDER
        // ==========================================

        public List<WishlistImage> GetImagesByFolder(
            int folderId)
        {
            List<WishlistImage> images =
                new List<WishlistImage>();

            string query = @"
                SELECT
                    WishlistImageID,
                    WishlistFolderID,
                    ImagePath,
                    Caption,
                    DateAdded
                FROM WishlistImages
                WHERE WishlistFolderID = @FolderID
                ORDER BY DateAdded DESC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@FolderID",
                    folderId);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WishlistImage image =
                            new WishlistImage();

                        image.WishlistImageID =
                            Convert.ToInt32(
                                reader["WishlistImageID"]);

                        image.WishlistFolderID =
                            Convert.ToInt32(
                                reader["WishlistFolderID"]);

                        image.ImagePath =
                            reader["ImagePath"].ToString();

                        image.Caption =
                            reader["Caption"] == DBNull.Value
                                ? null
                                : reader["Caption"].ToString();

                        image.DateAdded =
                            Convert.ToDateTime(
                                reader["DateAdded"]);

                        images.Add(image);
                    }
                }
            }

            return images;
        }

        // ==========================================
        // ADD IMAGE
        // ==========================================

        public bool AddImage(
            WishlistImage image)
        {
            string query = @"
                INSERT INTO WishlistImages
                (
                    WishlistFolderID,
                    ImagePath,
                    Caption
                )
                VALUES
                (
                    @WishlistFolderID,
                    @ImagePath,
                    @Caption
                )";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@WishlistFolderID",
                    image.WishlistFolderID);

                command.Parameters.AddWithValue(
                    "@ImagePath",
                    image.ImagePath);

                command.Parameters.AddWithValue(
                    "@Caption",
                    string.IsNullOrWhiteSpace(image.Caption)
                        ? (object)DBNull.Value
                        : image.Caption);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        // ==========================================
        // DELETE IMAGE
        // ==========================================

        public bool DeleteImage(
            int imageId,
            int folderId)
        {
            string query = @"
                DELETE FROM WishlistImages
                WHERE WishlistImageID = @ImageID
                  AND WishlistFolderID = @FolderID";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@ImageID",
                    imageId);

                command.Parameters.AddWithValue(
                    "@FolderID",
                    folderId);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}