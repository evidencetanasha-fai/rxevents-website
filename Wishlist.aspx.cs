using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using RxEvents.Data;
using RxEvents.Models;
using WishlistFolderModel = RxEvents.Models.WishlistFolder;

namespace RxEvents
{
    public partial class Wishlist : System.Web.UI.Page
    {
        private readonly WishlistRepository wishlistRepository =
            new WishlistRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCategories();
                LoadFolders();
            }
        }

        // ==========================================
        // LOAD EVENT CATEGORIES
        // ==========================================

        private void LoadCategories()
        {
            try
            {
                string query = @"
                    SELECT CategoryID, CategoryName
                    FROM EventCategories
                    ORDER BY CategoryName ASC";

                using (SqlConnection connection =
                       new DatabaseHelper().GetConnection())
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        FolderCategoryDropDown.Items.Clear();

                        FolderCategoryDropDown.Items.Add(
                            new ListItem(
                                "Select an event category",
                                ""));

                        while (reader.Read())
                        {
                            FolderCategoryDropDown.Items.Add(
                                new ListItem(
                                    reader["CategoryName"].ToString(),
                                    reader["CategoryID"].ToString()));
                        }
                    }
                }
            }
            catch (Exception)
            {
                CreateFolderMessageLabel.Text =
                    "Unable to load event categories.";

                CreateFolderMessageLabel.CssClass =
                    "rx-wishlist-form-message error";
            }
        }

        // ==========================================
        // LOAD CUSTOMER FOLDERS
        // ==========================================

        private void LoadFolders()
        {
            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                List<WishlistFolderModel> folders =
    wishlistRepository.GetFoldersByUser(userId); ;

                WishlistFoldersRepeater.DataSource =
                    folders;

                WishlistFoldersRepeater.DataBind();

                FolderCountLabel.Text =
                    folders.Count == 1
                        ? "1 Folder"
                        : folders.Count + " Folders";

                NoFoldersPanel.Visible =
                    folders.Count == 0;
            }
            catch (Exception)
            {
                WishlistMessageLabel.Text =
                    "Unable to load your inspiration folders.";

                WishlistMessageLabel.CssClass =
                    "rx-wishlist-message error";

                NoFoldersPanel.Visible = true;
            }
        }

        // ==========================================
        // CREATE FOLDER BUTTON
        // ==========================================

        protected void CreateFolderButton_Click(
            object sender,
            EventArgs e)
        {
            LoadCategories();

            CreateFolderMessageLabel.Text = "";

            CreateFolderPopup.Visible = true;
        }

        // ==========================================
        // CLOSE CREATE FOLDER POPUP
        // ==========================================

        protected void CloseCreateFolderButton_Click(
            object sender,
            EventArgs e)
        {
            CreateFolderPopup.Visible = false;

            CreateFolderMessageLabel.Text = "";

            FolderNameTextBox.Text = "";
            FolderBudgetTextBox.Text = "";
            FolderNotesTextBox.Text = "";

            if (FolderCategoryDropDown.Items.Count > 0)
            {
                FolderCategoryDropDown.SelectedIndex = 0;
            }
        }

        // ==========================================
        // SAVE FOLDER
        // ==========================================

        protected void SaveFolderButton_Click(
            object sender,
            EventArgs e)
        {
            CreateFolderPopup.Visible = true;

            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                int userId =
                    Convert.ToInt32(Session["UserID"]);

                int categoryId =
                    Convert.ToInt32(
                        FolderCategoryDropDown.SelectedValue);

                decimal? budget = null;

                if (!string.IsNullOrWhiteSpace(
                    FolderBudgetTextBox.Text))
                {
                    decimal parsedBudget;

                    if (!decimal.TryParse(
                        FolderBudgetTextBox.Text,
                        out parsedBudget))
                    {
                        CreateFolderMessageLabel.Text =
                            "Please enter a valid budget.";

                        CreateFolderMessageLabel.CssClass =
                            "rx-wishlist-form-message error";

                        return;
                    }

                    if (parsedBudget < 0)
                    {
                        CreateFolderMessageLabel.Text =
                            "Budget cannot be negative.";

                        CreateFolderMessageLabel.CssClass =
                            "rx-wishlist-form-message error";

                        return;
                    }

                    budget = parsedBudget;
                }

                WishlistFolderModel folder =
    new WishlistFolderModel
    {
        UserID = userId,
        CategoryID = categoryId,
        FolderName =
            FolderNameTextBox.Text.Trim(),
        Budget = budget,
        Notes =
            FolderNotesTextBox.Text.Trim()
    };

                int folderId =
                    wishlistRepository.CreateFolder(folder);

                if (folderId > 0)
                {
                    CreateFolderPopup.Visible = false;

                    FolderNameTextBox.Text = "";
                    FolderBudgetTextBox.Text = "";
                    FolderNotesTextBox.Text = "";
                    FolderCategoryDropDown.SelectedIndex = 0;

                    WishlistMessageLabel.Text =
                        "Your inspiration folder was created successfully.";

                    WishlistMessageLabel.CssClass =
                        "rx-wishlist-message success";

                    LoadFolders();
                }
                else
                {
                    CreateFolderMessageLabel.Text =
                        "Unable to create the folder.";

                    CreateFolderMessageLabel.CssClass =
                        "rx-wishlist-form-message error";
                }
            }
            catch (Exception)
            {
                CreateFolderMessageLabel.Text =
                    "Something went wrong while creating the folder.";

                CreateFolderMessageLabel.CssClass =
                    "rx-wishlist-form-message error";
            }
        }

        // ==========================================
        // FOLDER COMMANDS
        // ==========================================

        protected void WishlistFolder_Command(
            object sender,
            CommandEventArgs e)
        {
            try
            {
                int folderId =
                    Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "OpenFolder")
                {
                    Response.Redirect(
                        "WishlistFolder.aspx?id=" +
                        folderId);

                    return;
                }

                if (e.CommandName == "DeleteFolder")
                {
                    int userId =
                        Convert.ToInt32(Session["UserID"]);

                    bool deleted =
                        wishlistRepository.DeleteFolder(
                            folderId,
                            userId);

                    if (deleted)
                    {
                        WishlistMessageLabel.Text =
                            "The inspiration folder was deleted.";

                        WishlistMessageLabel.CssClass =
                            "rx-wishlist-message success";

                        LoadFolders();
                    }
                }
            }
            catch (Exception)
            {
                WishlistMessageLabel.Text =
                    "Unable to complete that action.";

                WishlistMessageLabel.CssClass =
                    "rx-wishlist-message error";
            }
        }

        // ==========================================
        // COVER IMAGE
        // ==========================================

        protected string GetCoverImage(
            object coverImage)
        {
            if (coverImage == null ||
                coverImage == DBNull.Value ||
                string.IsNullOrWhiteSpace(
                    coverImage.ToString()))
            {
                return ResolveUrl(
                    "~/Content/images/rx-events-logo.png");
            }

            return ResolveUrl(
                "~/Content/images/" +
                coverImage.ToString());
        }

        // ==========================================
        // BUDGET DISPLAY
        // ==========================================

        protected string GetBudget(
            object budget)
        {
            if (budget == null ||
                budget == DBNull.Value)
            {
                return "Budget not set";
            }

            decimal amount =
                Convert.ToDecimal(budget);

            return "UGX " +
                   amount.ToString("N0");
        }
    }
}