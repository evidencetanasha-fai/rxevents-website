using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class EventCategoryRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public EventCategoryRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public List<EventCategory> GetActiveEventCategories()
        {
            List<EventCategory> categories = new List<EventCategory>();

            string query = @"
                SELECT
                    CategoryID,
                    CategoryName,
                    Description,
                    ImagePath,
                    IsActive
                FROM EventCategories
                WHERE IsActive = 1
                ORDER BY CategoryID ASC";

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EventCategory category = new EventCategory();

                            category.CategoryID =
                                Convert.ToInt32(reader["CategoryID"]);

                            category.CategoryName =
                                reader["CategoryName"].ToString();

                            category.Description =
                                reader["Description"].ToString();

                            category.ImagePath =
                                reader["ImagePath"].ToString();

                            category.IsActive =
                                Convert.ToBoolean(reader["IsActive"]);

                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }
    }
}