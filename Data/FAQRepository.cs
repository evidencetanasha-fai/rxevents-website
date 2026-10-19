using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Models;

namespace RxEvents.Data
{
    public class FAQRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public FAQRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        // ==========================================
        // GET ACTIVE FAQS
        // ==========================================

        public List<FAQ> GetActiveFAQs()
        {
            List<FAQ> faqs =
                new List<FAQ>();

            string query = @"
                SELECT
                    FAQID,
                    Question,
                    Answer,
                    Category,
                    DisplayOrder,
                    IsActive,
                    CreatedDate
                FROM FAQs
                WHERE IsActive = 1
                ORDER BY
                    DisplayOrder ASC,
                    CreatedDate DESC";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FAQ faq = new FAQ();

                        faq.FAQID =
                            Convert.ToInt32(
                                reader["FAQID"]);

                        faq.Question =
                            reader["Question"].ToString();

                        faq.Answer =
                            reader["Answer"].ToString();

                        faq.Category =
                            reader["Category"] == DBNull.Value
                                ? ""
                                : reader["Category"].ToString();

                        faq.DisplayOrder =
                            Convert.ToInt32(
                                reader["DisplayOrder"]);

                        faq.IsActive =
                            Convert.ToBoolean(
                                reader["IsActive"]);

                        faq.CreatedDate =
                            Convert.ToDateTime(
                                reader["CreatedDate"]);

                        faqs.Add(faq);
                    }
                }
            }

            return faqs;
        }
    }
}