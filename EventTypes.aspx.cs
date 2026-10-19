using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class EventTypes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEventTypes();
            }
        }

        private void LoadEventTypes()
        {
            try
            {
                int categoryId;

                if (!int.TryParse(Request.QueryString["categoryId"], out categoryId))
                {
                    Response.Redirect("Events.aspx");
                    return;
                }

                DatabaseHelper databaseHelper = new DatabaseHelper();

                string query = @"
                    SELECT
                        E.EventID,
                        E.CategoryID,
                        E.EventName,
                        E.Description,
                        E.EventImage,
                        E.BasePrice,
                        E.IsActive,
                        C.CategoryName
                    FROM Events E
                    INNER JOIN EventCategories C
                        ON E.CategoryID = C.CategoryID
                    WHERE E.CategoryID = @CategoryID
                      AND E.IsActive = 1
                      AND C.IsActive = 1
                    ORDER BY E.EventID ASC";

                List<Event> events = new List<Event>();

                using (SqlConnection connection = databaseHelper.GetConnection())
                {
                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@CategoryID",
                            categoryId);

                        connection.Open();

                        using (SqlDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Event eventItem = new Event();

                                eventItem.EventID =
                                    Convert.ToInt32(reader["EventID"]);

                                eventItem.CategoryID =
                                    Convert.ToInt32(reader["CategoryID"]);

                                eventItem.EventName =
                                    reader["EventName"].ToString();

                                eventItem.Description =
                                    reader["Description"].ToString();

                                eventItem.EventImage =
                                    reader["EventImage"].ToString();

                                eventItem.BasePrice =
                                    Convert.ToDecimal(reader["BasePrice"]);

                                eventItem.IsActive =
                                    Convert.ToBoolean(reader["IsActive"]);

                                eventItem.CategoryName =
                                    reader["CategoryName"].ToString();

                                events.Add(eventItem);
                            }
                        }
                    }
                }

                if (events.Count == 0)
                {
                    Response.Redirect("Events.aspx");
                    return;
                }

                CategoryTitle.Text =
                    events[0].CategoryName + " Events";

                EventTypesRepeater.DataSource = events;
                EventTypesRepeater.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<p style='color:red;'>Unable to load event details: "
                    + ex.Message +
                    "</p>"
                );
            }
        }
    }
}