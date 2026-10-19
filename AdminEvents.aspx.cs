
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminEvents : System.Web.UI.Page
    {
        private readonly DatabaseHelper databaseHelper =
            new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 1)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCategoryFilter();
                LoadEventStatistics();
                LoadEventCategories();
                LoadEvents();
            }
        }

        // ------------------------------------------
        // LOAD CATEGORY FILTER
        // ------------------------------------------
        private void LoadCategoryFilter()
        {
            string query = @"
                SELECT CategoryID, CategoryName
                FROM EventCategories
                WHERE IsActive = 1
                ORDER BY CategoryName;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                DataTable categories = new DataTable();
                adapter.Fill(categories);

                CategoryFilter.DataSource = categories;
                CategoryFilter.DataTextField = "CategoryName";
                CategoryFilter.DataValueField = "CategoryID";
                CategoryFilter.DataBind();
            }

            CategoryFilter.Items.Insert(
                0, new ListItem("All Events", ""));
        }

        // ------------------------------------------
        // LOAD SUMMARY STATISTICS
        // ------------------------------------------
        private void LoadEventStatistics()
        {
            string query = @"
                SELECT
                    (SELECT COUNT(*)
                     FROM Events) AS TotalEvents,

                    (SELECT COUNT(*)
                     FROM Events
                     WHERE IsActive = 1) AS ActiveEvents,

                    (SELECT COUNT(*)
                     FROM Events
                     WHERE IsActive = 0) AS InactiveEvents;

                SELECT TOP 1
                    e.EventName,
                    COUNT(b.BookingID) AS BookingCount
                FROM Events e
                INNER JOIN Bookings b
                    ON e.EventID = b.EventID
                WHERE b.BookingStatus <> 'Cancelled'
                GROUP BY e.EventID, e.EventName
                ORDER BY COUNT(b.BookingID) DESC, e.EventName;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TotalEventsValue.Text =
                            Convert.ToInt32(reader["TotalEvents"])
                                .ToString("N0");

                        ActiveEventsValue.Text =
                            Convert.ToInt32(reader["ActiveEvents"])
                                .ToString("N0");

                        InactiveEventsValue.Text =
                            Convert.ToInt32(reader["InactiveEvents"])
                                .ToString("N0");
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        MostBookedEventValue.Text =
                            Convert.ToString(reader["EventName"]);
                    }
                    else
                    {
                        MostBookedEventValue.Text = "None";
                    }
                }
            }
        }

        // ------------------------------------------
        // LOAD EVENT CATEGORIES PANEL
        // ------------------------------------------
        private void LoadEventCategories()
        {
            string query = @"
                SELECT
                    c.CategoryID,
                    c.CategoryName,
                    COUNT(e.EventID) AS CategoryEventCount
                FROM EventCategories c
                LEFT JOIN Events e
                    ON c.CategoryID = e.CategoryID
                WHERE c.IsActive = 1
                GROUP BY c.CategoryID, c.CategoryName
                ORDER BY c.CategoryName;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                DataTable categories = new DataTable();
                adapter.Fill(categories);

                EventCategoriesRepeater.DataSource = categories;
                EventCategoriesRepeater.DataBind();
            }
        }

        // ------------------------------------------
        // LOAD EVENTS AND APPLY FILTERS
        // ------------------------------------------

        private void LoadEvents()
        {
            DateTime? startDate = ParseDate(StartDateTextBox.Text);
            DateTime? endDate = ParseDate(EndDateTextBox.Text);

            if (startDate.HasValue && endDate.HasValue &&
                startDate.Value.Date > endDate.Value.Date)
            {
                EventsMessage.Text =
                    "The start date cannot be later than the end date.";
                EventsGridView.DataSource = null;
                EventsGridView.DataBind();
                EventsCountLabel.Text = "Showing 0 events";
                PaginationRepeater.DataSource = null;
                PaginationRepeater.DataBind();
                return;
            }

            EventsMessage.Text = "";

            string whereClause = @"
        WHERE
            (@Search = ''
             OR e.EventName LIKE '%' + @Search + '%'
             OR ISNULL(e.Description, '') LIKE '%' + @Search + '%'
             OR ISNULL(c.CategoryName, '') LIKE '%' + @Search + '%')
            AND (@Status = ''
                 OR e.IsActive = CASE WHEN @Status = '1' THEN 1 ELSE 0 END)
            AND (@CategoryID = 0 OR e.CategoryID = @CategoryID)
            AND (@StartDate IS NULL OR e.DateAdded >= @StartDate)
            AND (@EndDate IS NULL OR e.DateAdded < DATEADD(day, 1, @EndDate))";

            string fromClause = @"
        FROM Events e
        LEFT JOIN EventCategories c ON e.CategoryID = c.CategoryID
        LEFT JOIN Bookings b ON e.EventID = b.EventID
            AND b.BookingStatus <> 'Cancelled' ";

            int totalRecords = 0;

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                connection.Open();

                string countQuery = @"
            SELECT COUNT(*)
            FROM Events e
            LEFT JOIN EventCategories c ON e.CategoryID = c.CategoryID "
                    + whereClause;

                using (SqlCommand countCommand =
                       new SqlCommand(countQuery, connection))
                {
                    AddEventFilterParameters(
                        countCommand, startDate, endDate);

                    totalRecords = Convert.ToInt32(
                        countCommand.ExecuteScalar());
                }

                int totalPages = Math.Max(
                    1, (int)Math.Ceiling(totalRecords / (double)PageSize));

                if (CurrentPage > totalPages)
                    CurrentPage = totalPages;

                if (CurrentPage < 1)
                    CurrentPage = 1;

                string query = @"
            SELECT
                e.EventID,
                e.CategoryID,
                e.EventName,
                e.Description,
                e.EventImage,
                e.BasePrice,
                e.IsActive,
                e.DateAdded,
                c.CategoryName,
                COUNT(b.BookingID) AS BookingCount "
                    + fromClause + whereClause + @"
            GROUP BY
                e.EventID, e.CategoryID, e.EventName,
                e.Description, e.EventImage, e.BasePrice,
                e.IsActive, e.DateAdded, c.CategoryName
            ORDER BY e.DateAdded DESC, e.EventID DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

                DataTable events = new DataTable();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddEventFilterParameters(command, startDate, endDate);

                    command.Parameters.Add("@Offset", SqlDbType.Int).Value =
                        (CurrentPage - 1) * PageSize;

                    command.Parameters.Add("@PageSize", SqlDbType.Int).Value =
                        PageSize;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(events);
                    }
                }

                EventsGridView.DataSource = events;
                EventsGridView.DataBind();
            }

            int firstRecord = totalRecords == 0
                ? 0
                : ((CurrentPage - 1) * PageSize) + 1;

            int lastRecord = Math.Min(
                CurrentPage * PageSize, totalRecords);

            EventsCountLabel.Text = "Showing " +
                firstRecord + " to " + lastRecord +
                " of " + totalRecords + " events";

            BindPagination(totalRecords);
        }

        private void AddEventFilterParameters(
            SqlCommand command,
            DateTime? startDate,
            DateTime? endDate)
        {
            command.Parameters.Add(
                "@Search", SqlDbType.VarChar, 200).Value =
                EventSearchTextBox.Text.Trim();

            command.Parameters.Add(
                "@Status", SqlDbType.VarChar, 10).Value =
                StatusFilter.SelectedValue;

            command.Parameters.Add(
                "@CategoryID", SqlDbType.Int).Value =
                string.IsNullOrEmpty(CategoryFilter.SelectedValue)
                    ? 0
                    : Convert.ToInt32(CategoryFilter.SelectedValue);

            command.Parameters.Add(
                "@StartDate", SqlDbType.Date).Value =
                startDate.HasValue
                    ? (object)startDate.Value.Date
                    : DBNull.Value;

            command.Parameters.Add(
                "@EndDate", SqlDbType.Date).Value =
                endDate.HasValue
                    ? (object)endDate.Value.Date
                    : DBNull.Value;
        }


        // ------------------------------------------
        // SEARCH / FILTER BUTTON
        // ------------------------------------------
        protected void ApplyEventFiltersButton_Click(
            object sender, EventArgs e)
        {
            LoadEvents();
        }

        // ------------------------------------------
        // CLEAR FILTERS
        // ------------------------------------------
        protected void ClearEventFiltersButton_Click(
            object sender, EventArgs e)
        {
            EventSearchTextBox.Text = "";
            StatusFilter.SelectedIndex = 0;
            CategoryFilter.SelectedIndex = 0;
            StartDateTextBox.Text = "";
            EndDateTextBox.Text = "";

            LoadEvents();
        }

        // ------------------------------------------
        // EVENT IMAGE URL
        // ------------------------------------------

        public string GetEventImageUrl(object imageValue)
        {
            if (imageValue == null || imageValue == DBNull.Value)
                return ResolveUrl("~/Content/images/event-placeholder.png");

            string imagePath = imageValue.ToString().Trim();

            if (string.IsNullOrWhiteSpace(imagePath))
                return ResolveUrl("~/Content/images/event-placeholder.png");

            // Allow complete external image URLs.
            if (imagePath.StartsWith("https://",
                    StringComparison.OrdinalIgnoreCase) ||
                imagePath.StartsWith("http://",
                    StringComparison.OrdinalIgnoreCase))
            {
                return imagePath;
            }

            // Normalize database paths.
            imagePath = imagePath.Replace("\\", "/");

            if (imagePath.StartsWith("~/"))
                return ResolveUrl(imagePath);

            if (imagePath.StartsWith("/"))
                return ResolveUrl("~" + imagePath);

            if (imagePath.StartsWith("Content/",
                    StringComparison.OrdinalIgnoreCase))
            {
                return ResolveUrl("~/" + imagePath);
            }

            // If the database contains only the filename.
            return ResolveUrl("~/Content/images/" +
                Path.GetFileName(imagePath));
        }

        // ------------------------------------------
        // EVENT ROW ACTIONS
        // ------------------------------------------
        protected void EventsGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out int eventID))
            {
                return;
            }

            if (e.CommandName == "ViewEvent")
            {
                Response.Redirect(
                    "~/AdminAddEvent.aspx?EventID=" + eventID);
                return;
            }

            if (e.CommandName == "EditEvent")
            {
                Response.Redirect(
                    "~/AdminAddEvent.aspx?EventID=" + eventID);
                return;
            }

            if (e.CommandName == "ToggleEvent")
            {
                ToggleEventStatus(eventID);

                LoadEventStatistics();
                LoadEventCategories();
                LoadEvents();
            }
        }

        // ------------------------------------------
        // ACTIVATE / DEACTIVATE EVENT
        // ------------------------------------------
        private void ToggleEventStatus(int eventID)
        {
            string query = @"
                UPDATE Events
                SET IsActive =
                    CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                WHERE EventID = @EventID;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@EventID", SqlDbType.Int).Value = eventID;

                connection.Open();

                int rows = command.ExecuteNonQuery();

                EventsMessage.Text = rows > 0
                    ? "Event status updated successfully."
                    : "The event could not be found.";
            }
        }

        // ------------------------------------------
        // CSV EXPORT
        // ------------------------------------------
        protected void ExportEventsButton_Click(
            object sender, EventArgs e)
        {
            DateTime? startDate =
                ParseDate(StartDateTextBox.Text);

            DateTime? endDate =
                ParseDate(EndDateTextBox.Text);

            if (startDate.HasValue && endDate.HasValue &&
                startDate.Value.Date > endDate.Value.Date)
            {
                EventsMessage.Text =
                    "The start date cannot be later than the end date.";
                return;
            }

            string query = @"
                SELECT
                    e.EventID,
                    e.EventName,
                    c.CategoryName,
                    e.Description,
                    e.BasePrice,
                    e.IsActive,
                    e.DateAdded,
                    COUNT(b.BookingID) AS BookingCount
                FROM Events e
                LEFT JOIN EventCategories c
                    ON e.CategoryID = c.CategoryID
                LEFT JOIN Bookings b
                    ON e.EventID = b.EventID
                    AND b.BookingStatus <> 'Cancelled'
                WHERE
                    (@Search = ''
                     OR e.EventName LIKE '%' + @Search + '%'
                     OR ISNULL(e.Description, '')
                        LIKE '%' + @Search + '%'
                     OR ISNULL(c.CategoryName, '')
                        LIKE '%' + @Search + '%')
                    AND
                    (@Status = ''
                     OR e.IsActive =
                        CASE WHEN @Status = '1'
                             THEN 1 ELSE 0 END)
                    AND
                    (@CategoryID = 0
                     OR e.CategoryID = @CategoryID)
                    AND
                    (@StartDate IS NULL
                     OR e.DateAdded >= @StartDate)
                    AND
                    (@EndDate IS NULL
                     OR e.DateAdded < DATEADD(day, 1, @EndDate))
                GROUP BY
                    e.EventID, e.EventName, c.CategoryName,
                    e.Description, e.BasePrice, e.IsActive,
                    e.DateAdded
                ORDER BY e.DateAdded DESC;";

            DataTable table = new DataTable();

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@Search", SqlDbType.VarChar, 200).Value =
                    EventSearchTextBox.Text.Trim();

                command.Parameters.Add(
                    "@Status", SqlDbType.VarChar, 10).Value =
                    StatusFilter.SelectedValue;

                command.Parameters.Add(
                    "@CategoryID", SqlDbType.Int).Value =
                    string.IsNullOrEmpty(CategoryFilter.SelectedValue)
                        ? 0
                        : Convert.ToInt32(
                            CategoryFilter.SelectedValue);

                command.Parameters.Add(
                    "@StartDate", SqlDbType.Date).Value =
                    startDate.HasValue
                        ? (object)startDate.Value.Date
                        : DBNull.Value;

                command.Parameters.Add(
                    "@EndDate", SqlDbType.Date).Value =
                    endDate.HasValue
                        ? (object)endDate.Value.Date
                        : DBNull.Value;

                using (SqlDataAdapter adapter =
                       new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }

            StringBuilder csv = new StringBuilder();

            csv.AppendLine(
                "Event ID,Event Name,Category,Description,Base Price,Status,Date Added,Bookings");

            foreach (DataRow row in table.Rows)
            {
                csv.AppendLine(
                    Csv(row["EventID"]) + "," +
                    Csv(row["EventName"]) + "," +
                    Csv(row["CategoryName"]) + "," +
                    Csv(row["Description"]) + "," +
                    Csv(row["BasePrice"]) + "," +
                    Csv(Convert.ToBoolean(row["IsActive"])
                        ? "Active" : "Inactive") + "," +
                    Csv(Convert.ToDateTime(row["DateAdded"])
                        .ToString("dd/MM/yyyy")) + "," +
                    Csv(row["BookingCount"]));
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.UTF8;

            Response.AddHeader(
                "Content-Disposition",
                "attachment;filename=RxEvents_Events_" +
                DateTime.Now.ToString("yyyyMMdd_HHmm") +
                ".csv");

            Response.BinaryWrite(Encoding.UTF8.GetPreamble());
            Response.Write(csv.ToString());
            Response.Flush();

            HttpContext.Current.ApplicationInstance
                .CompleteRequest();
        }

        private string Csv(object value)
        {
            string text =
                value == null || value == DBNull.Value
                    ? ""
                    : Convert.ToString(
                        value, CultureInfo.InvariantCulture);

            if (text.Length > 0 &&
                (text[0] == '=' || text[0] == '+' ||
                 text[0] == '-' || text[0] == '@'))
            {
                text = "'" + text;
            }

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        private DateTime? ParseDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            DateTime result;

            if (DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result))
            {
                return result.Date;
            }

            return null;
        }
        private const int PageSize = 4;
        private int CurrentPage
        {
            get
            {
                return ViewState["CurrentPage"] == null
                    ? 1
                    : (int)ViewState["CurrentPage"];
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        private void BindPagination(int totalRecords)
        {
            int totalPages = Math.Max(
                1, (int)Math.Ceiling(totalRecords / (double)PageSize));

            var pages = new System.Collections.Generic.List<object>();

            // Show the first page, nearby pages, and the final page.
            for (int page = 1; page <= totalPages; page++)
            {
                bool showPage =
                    page == 1 ||
                    page == totalPages ||
                    Math.Abs(page - CurrentPage) <= 1;

                if (showPage)
                {
                    pages.Add(new
                    {
                        PageNumber = page,
                        DisplayText = page.ToString(),
                        IsCurrent = page == CurrentPage
                    });
                }
                else if (page == 2 && CurrentPage > 3)
                {
                    pages.Add(new
                    {
                        PageNumber = 0,
                        DisplayText = "…",
                        IsCurrent = false
                    });
                }
                else if (page == totalPages - 1 &&
                         CurrentPage < totalPages - 2)
                {
                    pages.Add(new
                    {
                        PageNumber = 0,
                        DisplayText = "…",
                        IsCurrent = false
                    });
                }
            }

            PaginationRepeater.DataSource = pages;
            PaginationRepeater.DataBind();

            PreviousPageButton.Enabled = CurrentPage > 1;
            NextPageButton.Enabled = CurrentPage < totalPages;
        }

        protected void PaginationRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "GoToPage")
                return;

            int page;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument), out page))
                return;

            // Ellipsis entries are not navigable.
            if (page < 1)
                return;

            CurrentPage = page;
            LoadEvents();
        }

        protected void PreviousPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            LoadEvents();
        }

        protected void NextPageButton_Click(
            object sender, EventArgs e)
        {
            CurrentPage++;
            LoadEvents();
        }

    }
}
