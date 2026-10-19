using System;
using System.Configuration;
using System.Data.SqlClient;

namespace RxEvents.Data
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["RxEventsDB"]
                .ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}