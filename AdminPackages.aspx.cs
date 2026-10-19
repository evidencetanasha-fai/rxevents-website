
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminPackages : System.Web.UI.Page
    {
        private readonly DatabaseHelper databaseHelper =
            new DatabaseHelper();

        private const int PageSize = 4;

        private int CurrentPage
        {
            get
            {
                return ViewState["PackagesCurrentPage"] == null
                    ? 1
                    : (int)ViewState["PackagesCurrentPage"];
            }
            set
            {
                ViewState["PackagesCurrentPage"] = value;
            }
        }

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
                LoadPackageStatistics();
                LoadPackageCategories();
                LoadPackages();
            }
        }

        // ------------------------------------------
        // PACKAGE STATISTICS
        // ------------------------------------------
        private void LoadPackageStatistics()
        {
            string query = @"
                SELECT
                    COUNT(*) AS TotalPackages,
                    COALESCE(SUM(CASE WHEN IsActive = 1
                        THEN 1 ELSE 0 END), 0) AS ActivePackages,
                    COALESCE(SUM(CASE WHEN IsActive = 0
                        THEN 1 ELSE 0 END), 0) AS InactivePackages
                FROM Packages;

                SELECT TOP 1
                    p.PackageName,
                    COUNT(b.BookingID) AS BookingCount
                FROM Packages p
                INNER JOIN Bookings b ON p.PackageID = b.PackageID
                WHERE b.BookingStatus <> 'Cancelled'
                GROUP BY p.PackageID, p.PackageName
                ORDER BY COUNT(b.BookingID) DESC, p.PackageName;";

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TotalPackagesValue.Text =
                            Convert.ToString(reader["TotalPackages"]);

                        ActivePackagesValue.Text =
                            Convert.ToString(reader["ActivePackages"]);

                        InactivePackagesValue.Text =
                            Convert.ToString(reader["InactivePackages"]);
                    }

                    MostPopularPackageValue.Text = "None";

                    if (reader.NextResult() && reader.Read())
                    {
                        MostPopularPackageValue.Text =
                            Convert.ToString(reader["PackageName"]);
                    }
                }
            }
        }

        // ------------------------------------------
        // PACKAGE CATEGORY SUMMARY
        // Uses PackageType because there is no
        // separate package-category table confirmed.
        // ------------------------------------------
        private void LoadPackageCategories()
        {
            string query = @"
                SELECT
                    COALESCE(NULLIF(LTRIM(RTRIM(PackageType)), ''),
                        'Unclassified') AS CategoryName,
                    COUNT(*) AS PackageCount
                FROM Packages
                GROUP BY COALESCE(NULLIF(LTRIM(RTRIM(PackageType)), ''),
                    'Unclassified')
                ORDER BY CategoryName;";

            DataTable categories = new DataTable();

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                adapter.Fill(categories);
            }

            if (!categories.Columns.Contains("CategoryIcon"))
                categories.Columns.Add("CategoryIcon", typeof(string));

            foreach (DataRow row in categories.Rows)
            {
                string type = Convert.ToString(row["CategoryName"]);

                if (type.IndexOf("premium",
                    StringComparison.OrdinalIgnoreCase) >= 0)
                    row["CategoryIcon"] = "✦";
                else if (type.IndexOf("standard",
                    StringComparison.OrdinalIgnoreCase) >= 0)
                    row["CategoryIcon"] = "◇";
                else if (type.IndexOf("basic",
                    StringComparison.OrdinalIgnoreCase) >= 0)
                    row["CategoryIcon"] = "○";
                else
                    row["CategoryIcon"] = "☆";
            }

            PackageCategoriesRepeater.DataSource = categories;
            PackageCategoriesRepeater.DataBind();
        }

        // ------------------------------------------
        // LOAD FILTERED PACKAGES WITH PAGINATION
        // ------------------------------------------
        private void LoadPackages()
        {
            string whereClause = @"
                WHERE
                    (@Search = ''
                     OR p.PackageName LIKE '%' + @Search + '%'
                     OR ISNULL(p.PackageType, '') LIKE '%' + @Search + '%'
                     OR ISNULL(p.Description, '') LIKE '%' + @Search + '%')
                    AND
                    (@Status = ''
                     OR p.IsActive =
                        CASE WHEN @Status = '1' THEN 1 ELSE 0 END)
                    AND
                    (@PackageType = ''
                     OR p.PackageType = @PackageType) ";

            int totalRecords;

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            {
                connection.Open();

                string countQuery =
                    "SELECT COUNT(*) FROM Packages p " + whereClause;

                using (SqlCommand countCommand =
                       new SqlCommand(countQuery, connection))
                {
                    AddPackageFilterParameters(countCommand);
                    totalRecords = Convert.ToInt32(
                        countCommand.ExecuteScalar());
                }

                int totalPages = Math.Max(
                    1, (int)Math.Ceiling(
                        totalRecords / (double)PageSize));

                if (CurrentPage > totalPages)
                    CurrentPage = totalPages;

                if (CurrentPage < 1)
                    CurrentPage = 1;

                string query = @"
                    SELECT
                        p.PackageID,
                        p.PackageName,
                        p.PackageType,
                        p.Description,
                        p.Price,
                        p.ImagePath,
                        p.IsActive,
                        COUNT(ps.ServiceID) AS FeatureCount
                    FROM Packages p
                    LEFT JOIN PackageServices ps
                        ON p.PackageID = ps.PackageID "
                    + whereClause + @"
                    GROUP BY
                        p.PackageID, p.PackageName, p.PackageType,
                        p.Description, p.Price, p.ImagePath, p.IsActive
                    ORDER BY p.PackageID DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;";

                DataTable packages = new DataTable();

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    AddPackageFilterParameters(command);

                    command.Parameters.Add(
                        "@Offset", SqlDbType.Int).Value =
                        (CurrentPage - 1) * PageSize;

                    command.Parameters.Add(
                        "@PageSize", SqlDbType.Int).Value = PageSize;

                    using (SqlDataAdapter adapter =
                           new SqlDataAdapter(command))
                    {
                        adapter.Fill(packages);
                    }
                }

                PackagesGridView.DataSource = packages;
                PackagesGridView.DataBind();
            }

            int firstRecord = totalRecords == 0
                ? 0
                : (CurrentPage - 1) * PageSize + 1;

            int lastRecord = Math.Min(
                CurrentPage * PageSize, totalRecords);

            PackagesCountLabel.Text =
                "Showing " + firstRecord + " to " +
                lastRecord + " of " + totalRecords + " packages";

            BindPackagePagination(totalRecords);
        }

        private void AddPackageFilterParameters(SqlCommand command)
        {
            command.Parameters.Add(
                "@Search", SqlDbType.VarChar, 200).Value =
                PackageSearchTextBox.Text.Trim();

            command.Parameters.Add(
                "@Status", SqlDbType.VarChar, 10).Value =
                PackageStatusFilter.SelectedValue;

            command.Parameters.Add(
                "@PackageType", SqlDbType.VarChar, 50).Value =
                PackageTypeFilter.SelectedValue;
        }

        // ------------------------------------------
        // PACKAGE IMAGE URL
        // ------------------------------------------
        public string GetPackageImageUrl(object imageValue)
        {
            string fallback =
                ResolveUrl("~/Content/images/event-placeholder.png");

            if (imageValue == null || imageValue == DBNull.Value)
                return fallback;

            string path = Convert.ToString(imageValue).Trim();

            if (string.IsNullOrWhiteSpace(path))
                return fallback;

            path = path.Replace("\\", "/");

            if (path.StartsWith("https://",
                    StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("http://",
                    StringComparison.OrdinalIgnoreCase))
                return path;

            if (path.StartsWith("~/"))
                return ResolveUrl(path);

            if (path.StartsWith("/"))
                return ResolveUrl("~" + path);

            if (path.StartsWith("Content/",
                    StringComparison.OrdinalIgnoreCase))
                return ResolveUrl("~/" + path);

            return ResolveUrl("~/Content/images/" +
                System.IO.Path.GetFileName(path));
        }

        // ------------------------------------------
        // FILTER BUTTONS
        // ------------------------------------------
        protected void ApplyPackageFiltersButton_Click(
            object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadPackages();
        }

        protected void ClearPackageFiltersButton_Click(
            object sender, EventArgs e)
        {
            CurrentPage = 1;

            PackageSearchTextBox.Text = "";
            PackageStatusFilter.SelectedIndex = 0;
            PackageTypeFilter.SelectedIndex = 0;

            LoadPackages();
        }

        // ------------------------------------------
        // PAGINATION
        // ------------------------------------------
        private void BindPackagePagination(int totalRecords)
        {
            int totalPages = Math.Max(
                1, (int)Math.Ceiling(
                    totalRecords / (double)PageSize));

            var pages = new System.Collections.Generic.List<object>();

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
                else if ((page == 2 && CurrentPage > 3) ||
                         (page == totalPages - 1 &&
                          CurrentPage < totalPages - 2))
                {
                    pages.Add(new
                    {
                        PageNumber = 0,
                        DisplayText = "…",
                        IsCurrent = false
                    });
                }
            }

            PackagePaginationRepeater.DataSource = pages;
            PackagePaginationRepeater.DataBind();

            PreviousPackagePageButton.Enabled = CurrentPage > 1;
            NextPackagePageButton.Enabled = CurrentPage < totalPages;
        }

        protected void PackagePaginationRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "GoToPage")
                return;

            int page;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument), out page))
                return;

            if (page < 1)
                return;

            CurrentPage = page;
            LoadPackages();
        }

        protected void PreviousPackagePageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            LoadPackages();
        }

        protected void NextPackagePageButton_Click(
            object sender, EventArgs e)
        {
            CurrentPage++;
            LoadPackages();
        }

        // ------------------------------------------
        // ACTIVATE / DEACTIVATE PACKAGE
        // ------------------------------------------
        protected void PackagesGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            int packageID;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument), out packageID))
                return;

            if (e.CommandName == "ViewPackage" ||
                e.CommandName == "EditPackage")
            {
                Response.Redirect(
                    "~/AdminAddPackage.aspx?PackageID=" + packageID);
                return;
            }

            if (e.CommandName == "TogglePackage")
            {
                string query = @"
                    UPDATE Packages
                    SET IsActive =
                        CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                    WHERE PackageID = @PackageID;";

                using (SqlConnection connection =
                       databaseHelper.GetConnection())
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@PackageID", SqlDbType.Int).Value = packageID;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                PackagesMessage.Text =
                    "Package status updated successfully.";

                LoadPackageStatistics();
                LoadPackageCategories();
                LoadPackages();
            }
        }

        // ------------------------------------------
        // CSV EXPORT
        // ------------------------------------------
        protected void ExportPackagesButton_Click(
            object sender, EventArgs e)
        {
            string query = @"
                SELECT
                    p.PackageID,
                    p.PackageName,
                    p.PackageType,
                    p.Description,
                    p.Price,
                    p.IsActive,
                    COUNT(ps.ServiceID) AS FeatureCount
                FROM Packages p
                LEFT JOIN PackageServices ps
                    ON p.PackageID = ps.PackageID "
                + @"
                WHERE
                    (@Search = ''
                     OR p.PackageName LIKE '%' + @Search + '%'
                     OR ISNULL(p.PackageType, '') LIKE '%' + @Search + '%'
                     OR ISNULL(p.Description, '') LIKE '%' + @Search + '%')
                    AND
                    (@Status = ''
                     OR p.IsActive =
                        CASE WHEN @Status = '1' THEN 1 ELSE 0 END)
                    AND
                    (@PackageType = ''
                     OR p.PackageType = @PackageType)
                GROUP BY
                    p.PackageID, p.PackageName, p.PackageType,
                    p.Description, p.Price, p.IsActive
                ORDER BY p.PackageID DESC;";

            DataTable table = new DataTable();

            using (SqlConnection connection =
                   databaseHelper.GetConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                AddPackageFilterParameters(command);

                using (SqlDataAdapter adapter =
                       new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }

            StringBuilder csv = new StringBuilder();

            csv.AppendLine(
                "Package ID,Package Name,Package Type,Description,Price,Status,Features");

            foreach (DataRow row in table.Rows)
            {
                csv.AppendLine(
                    Csv(row["PackageID"]) + "," +
                    Csv(row["PackageName"]) + "," +
                    Csv(row["PackageType"]) + "," +
                    Csv(row["Description"]) + "," +
                    Csv(row["Price"]) + "," +
                    Csv(Convert.ToBoolean(row["IsActive"])
                        ? "Active" : "Inactive") + "," +
                    Csv(row["FeatureCount"]));
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.UTF8;

            Response.AddHeader(
                "Content-Disposition",
                "attachment;filename=RxEvents_Packages_" +
                DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv");

            Response.BinaryWrite(Encoding.UTF8.GetPreamble());
            Response.Write(csv.ToString());
            Response.Flush();

            HttpContext.Current.ApplicationInstance
                .CompleteRequest();
        }

        private string Csv(object value)
        {
            string text = value == null || value == DBNull.Value
                ? ""
                : Convert.ToString(
                    value, CultureInfo.InvariantCulture);

            if (text.Length > 0 &&
                (text[0] == '=' || text[0] == '+' ||
                 text[0] == '-' || text[0] == '@'))
                text = "'" + text;

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}
