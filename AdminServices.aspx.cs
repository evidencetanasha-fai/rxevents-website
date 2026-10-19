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
    public partial class AdminServices : Page
    {
        private const int PageSize = 3;

        protected int CurrentServicePage
        {
            get
            {
                return ViewState["CurrentServicePage"] == null
                    ? 1
                    : Convert.ToInt32(ViewState["CurrentServicePage"]);
            }
            set
            {
                ViewState["CurrentServicePage"] = value;
            }
        }

        private int TotalServicePages
        {
            get
            {
                return ViewState["TotalServicePages"] == null
                    ? 1
                    : Convert.ToInt32(ViewState["TotalServicePages"]);
            }
            set
            {
                ViewState["TotalServicePages"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int roleID;

            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                !int.TryParse(Convert.ToString(Session["RoleID"]), out roleID) ||
                roleID != 1)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadServiceStatistics();
                LoadServiceCategories();
                LoadServices();
            }
        }

        private void LoadServiceStatistics()
        {
            using (SqlConnection connection = new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT
                        COUNT(*) AS TotalServices,
                        ISNULL(SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END), 0)
                            AS ActiveServices,
                        ISNULL(SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END), 0)
                            AS InactiveServices
                    FROM dbo.Services;

                    SELECT TOP 1 s.ServiceName
                    FROM dbo.PackageServices ps
                    INNER JOIN dbo.Services s
                        ON s.ServiceID = ps.ServiceID
                    GROUP BY s.ServiceID, s.ServiceName
                    ORDER BY COUNT(*) DESC, s.ServiceName ASC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TotalServicesValue.Text =
                                Convert.ToString(reader["TotalServices"]);

                            ActiveServicesValue.Text =
                                Convert.ToString(reader["ActiveServices"]);

                            InactiveServicesValue.Text =
                                Convert.ToString(reader["InactiveServices"]);
                        }

                        if (reader.NextResult() && reader.Read())
                        {
                            MostPopularServiceValue.Text =
                                Server.HtmlEncode(
                                    Convert.ToString(reader["ServiceName"]));
                        }
                        else
                        {
                            MostPopularServiceValue.Text = "N/A";
                        }
                    }
                }
            }
        }

        private void LoadServiceCategories()
        {
            using (SqlConnection connection = new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT
                        ISNULL(NULLIF(LTRIM(RTRIM(Category)), ''), 'Others')
                            AS CategoryName,
                        COUNT(*) AS ServiceCount
                    FROM dbo.Services
                    GROUP BY ISNULL(NULLIF(LTRIM(RTRIM(Category)), ''), 'Others')
                    ORDER BY CategoryName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable categories = new DataTable();
                    adapter.Fill(categories);

                    if (!categories.Columns.Contains("CategoryIcon"))
                    {
                        categories.Columns.Add("CategoryIcon", typeof(string));
                    }

                    foreach (DataRow row in categories.Rows)
                    {
                        string category =
                            Convert.ToString(row["CategoryName"]);

                        switch (category.ToLowerInvariant())
                        {
                            case "food & drinks":
                            case "food and drinks":
                                row["CategoryIcon"] = "🍽";
                                break;

                            case "decorations":
                                row["CategoryIcon"] = "✦";
                                break;

                            case "photography":
                                row["CategoryIcon"] = "▣";
                                break;

                            case "entertainment":
                                row["CategoryIcon"] = "♫";
                                break;

                            default:
                                row["CategoryIcon"] = "◆";
                                break;
                        }
                    }

                    ServiceCategoriesRepeater.DataSource = categories;
                    ServiceCategoriesRepeater.DataBind();
                }
            }
        }

        private void LoadServices()
        {
            string search = ServiceSearchTextBox.Text.Trim();
            string status = ServiceStatusFilter.SelectedValue;
            string category = ServiceTypeFilter.SelectedValue;

            using (SqlConnection connection = new DatabaseHelper().GetConnection())
            {
                const string countQuery = @"
                    SELECT COUNT(*)
                    FROM dbo.Services
                    WHERE (@Search = ''
                           OR ServiceName LIKE @SearchPattern
                           OR ISNULL(Description, '') LIKE @SearchPattern)
                      AND (@Status = ''
                           OR IsActive = @IsActive)
                      AND (@Category = ''
                           OR ISNULL(NULLIF(LTRIM(RTRIM(Category)), ''), 'Others')
                                = @Category);";

                int totalRecords;

                using (SqlCommand countCommand =
                    new SqlCommand(countQuery, connection))
                {
                    AddFilterParameters(
                        countCommand, search, status, category);

                    connection.Open();

                    totalRecords =
                        Convert.ToInt32(countCommand.ExecuteScalar());
                }

                TotalServicePages = Math.Max(
                    1, (int)Math.Ceiling(totalRecords / (double)PageSize));

                if (CurrentServicePage > TotalServicePages)
                {
                    CurrentServicePage = TotalServicePages;
                }

                const string query = @"
                    SELECT
                        ServiceID,
                        ServiceName,
                        Category AS ServiceType,
                        Description,
                        Price,
                        ImagePath,
                        IsActive,
                        CAST(0 AS INT) AS BookingCount
                    FROM dbo.Services
                    WHERE (@Search = ''
                           OR ServiceName LIKE @SearchPattern
                           OR ISNULL(Description, '') LIKE @SearchPattern)
                      AND (@Status = ''
                           OR IsActive = @IsActive)
                      AND (@Category = ''
                           OR ISNULL(NULLIF(LTRIM(RTRIM(Category)), ''), 'Others')
                                = @Category)
                    ORDER BY ServiceID DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddFilterParameters(command, search, status, category);

                    command.Parameters.Add("@Offset", SqlDbType.Int).Value =
                        (CurrentServicePage - 1) * PageSize;

                    command.Parameters.Add("@PageSize", SqlDbType.Int).Value =
                        PageSize;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable services = new DataTable();
                        adapter.Fill(services);

                        ServicesGridView.DataSource = services;
                        ServicesGridView.DataBind();
                    }
                }

                int firstRecord = totalRecords == 0
                    ? 0
                    : (CurrentServicePage - 1) * PageSize + 1;

                int lastRecord = Math.Min(
                    CurrentServicePage * PageSize, totalRecords);

                ServicePaginationSummary.Text =
                    "Showing " + firstRecord + " to " + lastRecord +
                    " of " + totalRecords + " services";

                BindServicePagination();
            }
        }

        private void AddFilterParameters(
            SqlCommand command,
            string search,
            string status,
            string category)
        {
            command.Parameters.Add("@Search", SqlDbType.VarChar, 150)
                .Value = search;

            command.Parameters.Add("@SearchPattern", SqlDbType.VarChar, 302)
                .Value = "%" + search + "%";

            command.Parameters.Add("@Status", SqlDbType.VarChar, 1)
                .Value = status;

            command.Parameters.Add("@IsActive", SqlDbType.Bit)
                .Value = status == "1";

            command.Parameters.Add("@Category", SqlDbType.VarChar, 100)
                .Value = category;
        }

        private void BindServicePagination()
        {
            DataTable pages = new DataTable();
            pages.Columns.Add("PageNumber", typeof(int));

            for (int page = 1; page <= TotalServicePages; page++)
            {
                pages.Rows.Add(page);
            }

            ServicePaginationRepeater.DataSource = pages;
            ServicePaginationRepeater.DataBind();

            PreviousServicePageButton.Enabled = CurrentServicePage > 1;
            NextServicePageButton.Enabled = CurrentServicePage < TotalServicePages;
        }

        protected string GetServiceImageUrl(object imagePath)
        {
            string path = imagePath == null || imagePath == DBNull.Value
                ? ""
                : Convert.ToString(imagePath).Trim();

            if (string.IsNullOrWhiteSpace(path))
            {
                return ResolveUrl("~/Content/images/event-placeholder.png");
            }

            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            path = path.Replace("\\", "/").TrimStart('/');

            if (!path.Contains("/"))
            {
                path = "Content/images/" + path;
            }

            return ResolveUrl("~/" + path);
        }

        protected void ApplyServiceFiltersButton_Click(
            object sender, EventArgs e)
        {
            CurrentServicePage = 1;
            LoadServiceStatistics();
            LoadServiceCategories();
            LoadServices();
        }

        protected void ClearServiceFiltersButton_Click(
            object sender, EventArgs e)
        {
            ServiceSearchTextBox.Text = "";
            ServiceStatusFilter.SelectedIndex = 0;
            ServiceTypeFilter.SelectedIndex = 0;
            CurrentServicePage = 1;

            LoadServiceStatistics();
            LoadServiceCategories();
            LoadServices();
        }

        protected void PreviousServicePageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentServicePage > 1)
            {
                CurrentServicePage--;
            }

            LoadServices();
        }

        protected void NextServicePageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentServicePage < TotalServicePages)
            {
                CurrentServicePage++;
            }

            LoadServices();
        }

        protected void ServicePaginationRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "GoToPage")
            {
                int pageNumber;

                if (int.TryParse(
                    Convert.ToString(e.CommandArgument), out pageNumber) &&
                    pageNumber >= 1 &&
                    pageNumber <= TotalServicePages)
                {
                    CurrentServicePage = pageNumber;
                    LoadServices();
                }
            }
        }

        protected void ServicesGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            int serviceID;

            if (!int.TryParse(Convert.ToString(e.CommandArgument), out serviceID))
            {
                return;
            }

            if (e.CommandName == "ViewService")
            {
                Response.Redirect(
                    "~/AdminAddService.aspx?ServiceID=" + serviceID);
            }
            else if (e.CommandName == "EditService")
            {
                Response.Redirect(
                    "~/AdminAddService.aspx?ServiceID=" + serviceID);
            }
            else if (e.CommandName == "ToggleService")
            {
                using (SqlConnection connection =
                    new DatabaseHelper().GetConnection())
                {
                    const string query = @"
                        UPDATE dbo.Services
                        SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                        WHERE ServiceID = @ServiceID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@ServiceID", SqlDbType.Int)
                            .Value = serviceID;

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                LoadServiceStatistics();
                LoadServiceCategories();
                LoadServices();
            }
        }

        protected void ExportServicesButton_Click(
            object sender, EventArgs e)
        {
            string search = ServiceSearchTextBox.Text.Trim();
            string status = ServiceStatusFilter.SelectedValue;
            string category = ServiceTypeFilter.SelectedValue;

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT ServiceID, ServiceName, Category,
                           Description, Price, IsActive
                    FROM dbo.Services
                    WHERE (@Search = ''
                           OR ServiceName LIKE @SearchPattern
                           OR ISNULL(Description, '') LIKE @SearchPattern)
                      AND (@Status = ''
                           OR IsActive = @IsActive)
                      AND (@Category = ''
                           OR ISNULL(NULLIF(LTRIM(RTRIM(Category)), ''), 'Others')
                                = @Category)
                    ORDER BY ServiceID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddFilterParameters(command, search, status, category);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder csv = new StringBuilder();

                        csv.AppendLine(
                            "Service ID,Service Name,Category,Description,Price,Status");

                        while (reader.Read())
                        {
                            csv.Append(Csv(reader["ServiceID"])).Append(",");
                            csv.Append(Csv(reader["ServiceName"])).Append(",");
                            csv.Append(Csv(reader["Category"])).Append(",");
                            csv.Append(Csv(reader["Description"])).Append(",");
                            csv.Append(Csv(Convert.ToDecimal(reader["Price"])
                                .ToString("0.00", CultureInfo.InvariantCulture)))
                                .Append(",");
                            csv.AppendLine(Csv(
                                Convert.ToBoolean(reader["IsActive"])
                                    ? "Active" : "Inactive"));
                        }

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "text/csv";
                        Response.ContentEncoding = Encoding.UTF8;
                        Response.AddHeader(
                            "Content-Disposition",
                            "attachment;filename=RxEventsServices.csv");

                        Response.Write("\uFEFF" + csv.ToString());
                        Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }

        private string Csv(object value)
        {
            string text = value == null || value == DBNull.Value
                ? ""
                : Convert.ToString(value);

            if (!string.IsNullOrEmpty(text) &&
                (text.StartsWith("=") || text.StartsWith("+") ||
                 text.StartsWith("-") || text.StartsWith("@")))
            {
                text = "'" + text;
            }

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}