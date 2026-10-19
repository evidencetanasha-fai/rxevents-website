using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class PackageRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public PackageRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public List<Package> GetActivePackages()
        {
            List<Package> packages = new List<Package>();

            string query = @"
                SELECT
                    PackageID,
                    PackageName,
                    PackageType,
                    Description,
                    Price,
                    ImagePath,
                    IsActive
                FROM Packages
                WHERE IsActive = 1
                ORDER BY Price DESC";

            using (SqlConnection connection = databaseHelper.GetConnection())
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
                            Package packageItem = new Package();

                            packageItem.PackageID =
                                Convert.ToInt32(reader["PackageID"]);

                            packageItem.PackageName =
                                reader["PackageName"].ToString();

                            packageItem.PackageType =
                                reader["PackageType"].ToString();

                            packageItem.Description =
                                reader["Description"].ToString();

                            packageItem.Price =
                                Convert.ToDecimal(reader["Price"]);

                            packageItem.ImagePath =
                                reader["ImagePath"].ToString();

                            packageItem.IsActive =
                                Convert.ToBoolean(reader["IsActive"]);

                            packages.Add(packageItem);
                        }
                    }
                }
            }

            return packages;
        }

        public List<Service> GetPackageServices(int packageId)
        {
            List<Service> services = new List<Service>();

            string query = @"
                SELECT
                    S.ServiceID,
                    S.ServiceName,
                    S.Category,
                    S.Description,
                    S.Price,
                    S.ImagePath,
                    S.IsActive
                FROM PackageServices PS
                INNER JOIN Services S
                    ON PS.ServiceID = S.ServiceID
                WHERE PS.PackageID = @PackageID
                  AND S.IsActive = 1
                ORDER BY S.ServiceID ASC";

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@PackageID",
                        packageId);

                    connection.Open();

                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Service service = new Service();

                            service.ServiceID =
                                Convert.ToInt32(reader["ServiceID"]);

                            service.ServiceName =
                                reader["ServiceName"].ToString();

                            service.Category =
                                reader["Category"].ToString();

                            service.Description =
                                reader["Description"].ToString();

                            service.Price =
                                Convert.ToDecimal(reader["Price"]);

                            service.ImagePath =
                                reader["ImagePath"].ToString();

                            service.IsActive =
                                Convert.ToBoolean(reader["IsActive"]);

                            services.Add(service);
                        }
                    }
                }
            }

            return services;
        }
    }
}