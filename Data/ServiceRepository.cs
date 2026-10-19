using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class ServiceRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public ServiceRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public List<Service> GetActiveServices()
        {
            List<Service> services = new List<Service>();

            string query = @"
                SELECT
                    ServiceID,
                    ServiceName,
                    Category,
                    Description,
                    Price,
                    ImagePath,
                    IsActive
                FROM Services
                WHERE IsActive = 1
                ORDER BY ServiceID ASC";

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Service service = new Service();

                            service.ServiceID = Convert.ToInt32(reader["ServiceID"]);
                            service.ServiceName = reader["ServiceName"].ToString();
                            service.Category = reader["Category"].ToString();
                            service.Description = reader["Description"].ToString();
                            service.Price = Convert.ToDecimal(reader["Price"]);
                            service.ImagePath = reader["ImagePath"].ToString();
                            service.IsActive = Convert.ToBoolean(reader["IsActive"]);

                            services.Add(service);
                        }
                    }
                }
            }

            return services;
        }
    }
}