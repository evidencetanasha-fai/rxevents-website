using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class SettingsRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public SettingsRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public List<Setting> GetAllSettings()
        {
            List<Setting> settings = new List<Setting>();

            string query = @"
                SELECT
                    SettingID,
                    SettingName,
                    SettingValue,
                    Description,
                    UpdatedDate
                FROM Settings
                ORDER BY SettingID ASC";

            using (SqlConnection connection = databaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Setting setting = new Setting();

                        setting.SettingID =
                            Convert.ToInt32(reader["SettingID"]);

                        setting.SettingName =
                            reader["SettingName"].ToString();

                        setting.SettingValue =
                            reader["SettingValue"].ToString();

                        setting.Description =
                            reader["Description"].ToString();

                        setting.UpdatedDate =
                            Convert.ToDateTime(reader["UpdatedDate"]);

                        settings.Add(setting);
                    }
                }
            }

            return settings;
        }
    }
}