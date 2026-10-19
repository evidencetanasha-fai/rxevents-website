using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminGallery : Page
    {
        private const int PageSize = 8;

        protected int CurrentGalleryPage
        {
            get
            {
                return ViewState["CurrentGalleryPage"] == null
                    ? 1 : Convert.ToInt32(ViewState["CurrentGalleryPage"]);
            }
            set { ViewState["CurrentGalleryPage"] = value; }
        }

        private int TotalGalleryPages
        {
            get
            {
                return ViewState["TotalGalleryPages"] == null
                    ? 1 : Convert.ToInt32(ViewState["TotalGalleryPages"]);
            }
            set { ViewState["TotalGalleryPages"] = value; }
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
                LoadEventFilter();
                LoadGallery();
            }
        }

        private void LoadEventFilter()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT EventID, EventName
                    FROM dbo.Events
                    ORDER BY EventName";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        GalleryEventFilter.DataSource = reader;
                        GalleryEventFilter.DataTextField = "EventName";
                        GalleryEventFilter.DataValueField = "EventID";
                        GalleryEventFilter.DataBind();
                    }
                }
            }

            GalleryEventFilter.Items.Insert(
                0, new ListItem("All Events", ""));
        }

        private string BuildGalleryFilter()
        {
            return @"
                (@Search = ''
                    OR ISNULL(g.Caption, '') LIKE @SearchPattern
                    OR e.EventName LIKE @SearchPattern
                    OR g.ImagePath LIKE @SearchPattern)
                AND (@EventID = 0 OR g.EventID = @EventID)
                AND (@Status = ''
                    OR (@Status = '1' AND g.IsActive = 1)
                    OR (@Status = '0' AND g.IsActive = 0))";
        }

        private void AddGalleryFilterParameters(SqlCommand command)
        {
            int eventID;
            int.TryParse(GalleryEventFilter.SelectedValue, out eventID);

            command.Parameters.Add("@Search", SqlDbType.VarChar, 255)
                .Value = GallerySearchInput.Text.Trim();

            command.Parameters.Add("@SearchPattern", SqlDbType.VarChar, 257)
                .Value = "%" + GallerySearchInput.Text.Trim() + "%";

            command.Parameters.Add("@EventID", SqlDbType.Int)
                .Value = eventID;

            command.Parameters.Add("@Status", SqlDbType.VarChar, 1)
                .Value = GalleryStatusFilter.SelectedValue;
        }

        private void LoadGallery()
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                connection.Open();

                string filter = BuildGalleryFilter();

                const string countQuery = @"
                    SELECT COUNT(*)
                    FROM dbo.Gallery g
                    INNER JOIN dbo.Events e ON g.EventID = e.EventID
                    WHERE ";

                int totalRecords;

                using (SqlCommand command = new SqlCommand(
                    countQuery + filter, connection))
                {
                    AddGalleryFilterParameters(command);
                    totalRecords = Convert.ToInt32(
                        command.ExecuteScalar());
                }

                TotalGalleryPages = Math.Max(
                    1, (int)Math.Ceiling(totalRecords / (double)PageSize));

                if (CurrentGalleryPage > TotalGalleryPages)
                    CurrentGalleryPage = TotalGalleryPages;

                string query = @"
                    SELECT
                        g.GalleryID,
                        g.EventID,
                        g.ImagePath,
                        g.Caption,
                        g.DateAdded,
                        g.IsActive,
                        e.EventName,
                        COALESCE(
                            NULLIF(g.Caption, ''),
                            e.EventName) AS Title,
                        COALESCE(g.Caption, '') AS Description
                    FROM dbo.Gallery g
                    INNER JOIN dbo.Events e ON g.EventID = e.EventID
                    WHERE " + filter + @"
                    ORDER BY g.DateAdded DESC, g.GalleryID DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;";

                DataTable gallery = new DataTable();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddGalleryFilterParameters(command);

                    command.Parameters.Add("@Offset", SqlDbType.Int)
                        .Value = (CurrentGalleryPage - 1) * PageSize;

                    command.Parameters.Add("@PageSize", SqlDbType.Int)
                        .Value = PageSize;

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(gallery);
                    }
                }

                GalleryRepeater.DataSource = gallery;
                GalleryRepeater.DataBind();

                GalleryEmptyLabel.Visible = gallery.Rows.Count == 0;

                int firstRecord = totalRecords == 0
                    ? 0 : (CurrentGalleryPage - 1) * PageSize + 1;

                int lastRecord = Math.Min(
                    CurrentGalleryPage * PageSize, totalRecords);

                GalleryPaginationSummary.Text =
                    "Showing " + firstRecord + " to " + lastRecord +
                    " of " + totalRecords + " images";

                BindGalleryPagination();
            }
        }

        protected string GetGalleryImageUrl(object imagePathValue)
        {
            string imagePath = Convert.ToString(imagePathValue);

            if (string.IsNullOrWhiteSpace(imagePath))
                return ResolveUrl("~/Content/images/event-placeholder.png");

            imagePath = imagePath.Trim().Replace("\\", "/");

            if (imagePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                imagePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return imagePath;
            }

            imagePath = imagePath.TrimStart('~', '/');

            if (!imagePath.Contains("/"))
                imagePath = "Content/images/" + imagePath;

            return ResolveUrl("~/" + imagePath);
        }

        private void BindGalleryPagination()
        {
            DataTable pages = new DataTable();
            pages.Columns.Add("PageNumber", typeof(int));

            for (int page = 1; page <= TotalGalleryPages; page++)
                pages.Rows.Add(page);

            GalleryPaginationRepeater.DataSource = pages;
            GalleryPaginationRepeater.DataBind();

            PreviousGalleryPageButton.Enabled = CurrentGalleryPage > 1;
            NextGalleryPageButton.Enabled =
                CurrentGalleryPage < TotalGalleryPages;
        }

        protected void ApplyGalleryFiltersButton_Click(
            object sender, EventArgs e)
        {
            CurrentGalleryPage = 1;
            LoadGallery();
        }

        protected void ClearGalleryFiltersButton_Click(
            object sender, EventArgs e)
        {
            GallerySearchInput.Text = "";
            GalleryEventFilter.SelectedIndex = 0;
            GalleryStatusFilter.SelectedIndex = 0;

            CurrentGalleryPage = 1;
            LoadGallery();
        }

        protected void PreviousGalleryPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentGalleryPage > 1)
                CurrentGalleryPage--;

            LoadGallery();
        }

        protected void NextGalleryPageButton_Click(
            object sender, EventArgs e)
        {
            if (CurrentGalleryPage < TotalGalleryPages)
                CurrentGalleryPage++;

            LoadGallery();
        }

        protected void GalleryPaginationRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            int pageNumber;

            if (e.CommandName == "GoToPage" &&
                int.TryParse(Convert.ToString(e.CommandArgument), out pageNumber) &&
                pageNumber >= 1 && pageNumber <= TotalGalleryPages)
            {
                CurrentGalleryPage = pageNumber;
                LoadGallery();
            }
        }

        protected void GalleryRepeater_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            int galleryID;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument), out galleryID))
                return;

            if (e.CommandName == "EditImage")
            {
                Response.Redirect(
                    "~/AdminAddGallery.aspx?GalleryID=" + galleryID);
                return;
            }

            if (e.CommandName == "ViewImage")
            {
                ShowGalleryDetails(galleryID);
                return;
            }

            if (e.CommandName == "ToggleImage")
            {
                ToggleGalleryImage(galleryID);
            }
        }

        private void ShowGalleryDetails(int galleryID)
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    SELECT g.Caption, g.ImagePath, g.DateAdded,
                           g.IsActive, e.EventName
                    FROM dbo.Gallery g
                    INNER JOIN dbo.Events e ON g.EventID = e.EventID
                    WHERE g.GalleryID = @GalleryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@GalleryID", SqlDbType.Int)
                        .Value = galleryID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string caption = reader["Caption"] == DBNull.Value
                                ? "(No caption)"
                                : Convert.ToString(reader["Caption"]);

                            string details =
                                "Event: " + Convert.ToString(reader["EventName"]) +
                                " | Caption: " + caption +
                                " | Image: " + Convert.ToString(reader["ImagePath"]) +
                                " | Added: " +
                                Convert.ToDateTime(reader["DateAdded"])
                                    .ToString("dd MMM yyyy") +
                                " | Status: " +
                                (Convert.ToBoolean(reader["IsActive"])
                                    ? "Active" : "Inactive");

                            GalleryMessage.Text = Server.HtmlEncode(details);
                            GalleryMessage.ForeColor =
                                System.Drawing.Color.DarkGreen;
                        }
                        else
                        {
                            GalleryMessage.Text = "Gallery image not found.";
                        }
                    }
                }
            }
        }

        private void ToggleGalleryImage(int galleryID)
        {
            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                const string query = @"
                    UPDATE dbo.Gallery
                    SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                    WHERE GalleryID = @GalleryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@GalleryID", SqlDbType.Int)
                        .Value = galleryID;

                    connection.Open();

                    if (command.ExecuteNonQuery() == 0)
                    {
                        GalleryMessage.Text = "Gallery image not found.";
                        GalleryMessage.ForeColor =
                            System.Drawing.Color.Firebrick;
                        return;
                    }
                }
            }

            GalleryMessage.Text = "Gallery image status updated.";
            GalleryMessage.ForeColor = System.Drawing.Color.DarkGreen;

            LoadGallery();
        }

        protected void ExportGalleryButton_Click(
            object sender, EventArgs e)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection =
                new DatabaseHelper().GetConnection())
            {
                string query = @"
                    SELECT g.GalleryID, e.EventName, g.ImagePath,
                           g.Caption, g.DateAdded,
                           CASE WHEN g.IsActive = 1
                                THEN 'Active' ELSE 'Inactive' END AS Status
                    FROM dbo.Gallery g
                    INNER JOIN dbo.Events e ON g.EventID = e.EventID
                    WHERE " + BuildGalleryFilter() + @"
                    ORDER BY g.DateAdded DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddGalleryFilterParameters(command);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            StringBuilder csv = new StringBuilder();

            for (int column = 0; column < data.Columns.Count; column++)
            {
                if (column > 0) csv.Append(",");
                csv.Append(Csv(data.Columns[column].ColumnName));
            }

            csv.AppendLine();

            foreach (DataRow row in data.Rows)
            {
                for (int column = 0; column < data.Columns.Count; column++)
                {
                    if (column > 0) csv.Append(",");
                    csv.Append(Csv(row[column]));
                }

                csv.AppendLine();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AddHeader(
                "Content-Disposition",
                "attachment;filename=RxEventsGallery.csv");

            Response.Write("\uFEFF" + csv.ToString());
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
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