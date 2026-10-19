using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class GalleryRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public GalleryRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public List<RxEvents.Models.Gallery> GetActiveGallery()
        {
            List<RxEvents.Models.Gallery> galleryItems =
                new List<RxEvents.Models.Gallery>();

            string query = @"
                SELECT
                    G.GalleryID,
                    G.EventID,
                    G.ImagePath,
                    G.Caption,
                    G.DateAdded,
                    G.IsActive,
                    E.EventName
                FROM Gallery G
                INNER JOIN Events E
                    ON G.EventID = E.EventID
                WHERE G.IsActive = 1
                  AND E.IsActive = 1
                ORDER BY G.DateAdded DESC, G.GalleryID DESC";

            using (SqlConnection connection =
                databaseHelper.GetConnection())
            {
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RxEvents.Models.Gallery gallery =
                                new RxEvents.Models.Gallery();

                            gallery.GalleryID =
                                Convert.ToInt32(reader["GalleryID"]);

                            gallery.EventID =
                                Convert.ToInt32(reader["EventID"]);

                            gallery.ImagePath =
                                reader["ImagePath"].ToString();

                            gallery.Caption =
                                reader["Caption"].ToString();

                            gallery.DateAdded =
                                Convert.ToDateTime(reader["DateAdded"]);

                            gallery.IsActive =
                                Convert.ToBoolean(reader["IsActive"]);

                            gallery.EventName =
                                reader["EventName"].ToString();

                            galleryItems.Add(gallery);
                        }
                    }
                }
            }

            return galleryItems;
        }
    }
}