using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using RxEvents.Data;
using RxEvents.Models;
using WishlistFolderModel = RxEvents.Models.WishlistFolder;

namespace RxEvents
{
    public partial class WishlistFolder : System.Web.UI.Page
    {
        private readonly WishlistRepository wishlistRepository =
            new WishlistRepository();
        private readonly BookingRepository bookingRepository =
    new BookingRepository();

        private int FolderID
        {
            get
            {
                int folderId;

                if (int.TryParse(
                    Request.QueryString["id"],
                    out folderId))
                {
                    return folderId;
                }

                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (FolderID <= 0)
            {
                Response.Redirect("Wishlist.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadFolder();
                LoadImages();
            }
        }

        // ==========================================
        // LOAD FOLDER
        // ==========================================

        private void LoadFolder()
        {
            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                WishlistFolderModel folder =
    wishlistRepository.GetFolderById(
        FolderID,
        userId);

                if (folder == null)
                {
                    Response.Redirect("Wishlist.aspx");
                    return;
                }

                FolderNameLabel.Text =
                    folder.FolderName;

                CategoryNameLabel.Text =
                    folder.CategoryName;

                BudgetLabel.Text =
                    GetBudget(folder.Budget);

                if (string.IsNullOrWhiteSpace(
                    folder.Notes))
                {
                    NotesLabel.Text =
                        "No planning notes added yet.";
                }
                else
                {
                    NotesLabel.Text =
                        folder.Notes;
                }
            }
            catch (Exception)
            {
                BoardMessageLabel.Text =
                    "Unable to load the inspiration board.";

                BoardMessageLabel.CssClass =
                    "rx-wishlist-message error";
            }
        }

        // ==========================================
        // LOAD IMAGES
        // ==========================================

        private void LoadImages()
        {
            try
            {
                List<WishlistImage> images =
                    wishlistRepository.GetImagesByFolder(
                        FolderID);

                WishlistImagesRepeater.DataSource =
                    images;

                WishlistImagesRepeater.DataBind();

                ImageCountLabel.Text =
                    images.Count == 1
                        ? "1 image"
                        : images.Count + " images";

                NoImagesPanel.Visible =
                    images.Count == 0;
            }
            catch (Exception)
            {
                BoardMessageLabel.Text =
                    "Unable to load your inspiration images.";

                BoardMessageLabel.CssClass =
                    "rx-wishlist-message error";

                NoImagesPanel.Visible = true;
            }
        }

        // ==========================================
        // ADD INSPIRATION BUTTON
        // ==========================================

        protected void AddInspirationButton_Click(
            object sender,
            EventArgs e)
        {
            InspirationMessageLabel.Text = "";

            AddInspirationPopup.Visible = true;
        }

        // ==========================================
        // CLOSE INSPIRATION POPUP
        // ==========================================

        protected void CloseInspirationButton_Click(
            object sender,
            EventArgs e)
        {
            AddInspirationPopup.Visible = false;

            InspirationMessageLabel.Text = "";

            InspirationCaptionTextBox.Text = "";
        }

        // ==========================================
        // SAVE INSPIRATION IMAGE
        // ==========================================

        protected void SaveInspirationButton_Click(
            object sender,
            EventArgs e)
        {
            AddInspirationPopup.Visible = true;

            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                // Confirm that this folder belongs
                // to the logged-in customer.
                WishlistFolderModel folder =
    wishlistRepository.GetFolderById(
        FolderID,
        userId);

                if (folder == null)
                {
                    Response.Redirect("Wishlist.aspx");
                    return;
                }

                if (!InspirationFileUpload.HasFile)
                {
                    InspirationMessageLabel.Text =
                        "Please select an image.";

                    return;
                }

                string extension =
                    Path.GetExtension(
                        InspirationFileUpload.FileName)
                    .ToLowerInvariant();

                string[] allowedExtensions =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

                bool validExtension = false;

                foreach (string allowedExtension
                    in allowedExtensions)
                {
                    if (extension ==
                        allowedExtension)
                    {
                        validExtension = true;
                        break;
                    }
                }

                if (!validExtension)
                {
                    InspirationMessageLabel.Text =
                        "Please upload a JPG, JPEG, PNG, or WEBP image.";

                    InspirationMessageLabel.CssClass =
                        "rx-wishlist-form-message error";

                    return;
                }

                if (InspirationFileUpload.PostedFile.ContentLength
                    > 5 * 1024 * 1024)
                {
                    InspirationMessageLabel.Text =
                        "The image must be smaller than 5 MB.";

                    InspirationMessageLabel.CssClass =
                        "rx-wishlist-form-message error";

                    return;
                }

                string fileName =
                    Guid.NewGuid().ToString("N") +
                    extension;

                string folderPath =
                    Server.MapPath(
                        "~/Content/images/wishlist/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(
                        folderPath);
                }

                string filePath =
                    Path.Combine(
                        folderPath,
                        fileName);

                InspirationFileUpload.SaveAs(
                    filePath);

                WishlistImage image =
                    new WishlistImage
                    {
                        WishlistFolderID =
                            FolderID,

                        ImagePath =
                            "wishlist/" +
                            fileName,

                        Caption =
                            InspirationCaptionTextBox.Text.Trim()
                    };

                bool added =
                    wishlistRepository.AddImage(
                        image);

                if (added)
                {
                    AddInspirationPopup.Visible =
                        false;

                    InspirationCaptionTextBox.Text =
                        "";

                    BoardMessageLabel.Text =
                        "Inspiration added to your board.";

                    BoardMessageLabel.CssClass =
                        "rx-wishlist-message success";

                    LoadImages();
                }
                else
                {
                    InspirationMessageLabel.Text =
                        "Unable to save the inspiration image.";

                    InspirationMessageLabel.CssClass =
                        "rx-wishlist-form-message error";
                }
            }
            catch (Exception)
            {
                InspirationMessageLabel.Text =
                    "Something went wrong while uploading the image.";

                InspirationMessageLabel.CssClass =
                    "rx-wishlist-form-message error";
            }
        }

        // ==========================================
        // IMAGE COMMANDS
        // ==========================================

        protected void Image_Command(
            object sender,
            CommandEventArgs e)
        {
            if (e.CommandName != "DeleteImage")
            {
                return;
            }

            try
            {
                int imageId =
                    Convert.ToInt32(
                        e.CommandArgument);

                int userId =
                    Convert.ToInt32(
                        Session["UserID"]);

                // Verify folder ownership first.
                WishlistFolderModel folder =
    wishlistRepository.GetFolderById(
        FolderID,
        userId);

                if (folder == null)
                {
                    Response.Redirect("Wishlist.aspx");
                    return;
                }

                bool deleted =
                    wishlistRepository.DeleteImage(
                        imageId,
                        FolderID);

                if (deleted)
                {
                    BoardMessageLabel.Text =
                        "The inspiration image was removed.";

                    BoardMessageLabel.CssClass =
                        "rx-wishlist-message success";

                    LoadImages();
                }
            }
            catch (Exception)
            {
                BoardMessageLabel.Text =
                    "Unable to remove the image.";

                BoardMessageLabel.CssClass =
                    "rx-wishlist-message error";
            }
        }

        // ==========================================
        // BACK TO WISHLIST
        // ==========================================

        protected void BackToWishlistButton_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect("Wishlist.aspx");
        }

        // ==========================================
        // IMAGE URL
        // ==========================================

        protected string GetImageUrl(
            object imagePath)
        {
            if (imagePath == null ||
                imagePath == DBNull.Value ||
                string.IsNullOrWhiteSpace(
                    imagePath.ToString()))
            {
                return ResolveUrl(
                    "~/Content/images/rx-events-logo.png");
            }

            return ResolveUrl(
                "~/Content/images/" +
                imagePath.ToString());
        }

        // ==========================================
        // BUDGET
        // ==========================================

        private string GetBudget(
            decimal? budget)
        {
            if (!budget.HasValue)
            {
                return "Budget not set";
            }

            return "UGX " +
                   budget.Value.ToString("N0");
        }

        // ==========================================
        // ATTACH TO BOOKING BUTTON
        // ==========================================

        protected void AttachToBookingButton_Click(
    object sender,
    EventArgs e)
        {
            AttachBookingMessageLabel.Text = "";

            LoadBookingsForAttachment();

            AttachBookingPopup.Visible = true;
        }

        private void LoadBookingsForAttachment()
        {
            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                List<Booking> bookings =
                    bookingRepository.GetBookingsForWishlistAttachment(
                        userId);

                BookingDropDown.Items.Clear();

                BookingDropDown.Items.Add(
                    new ListItem(
                        "Select a booking",
                        ""));

                foreach (Booking booking in bookings)
                {
                    string bookingText =
                        "Booking #" +
                        booking.BookingID +
                        " — " +
                        booking.EventName +
                        " (" +
                        booking.BookingStatus +
                        ")";

                    string bookingValue =
                        booking.BookingID.ToString();

                    BookingDropDown.Items.Add(
                        new ListItem(
                            bookingText,
                            bookingValue));
                }

                if (bookings.Count == 0)
                {
                    AttachBookingMessageLabel.Text =
                        "You do not have any bookings to attach this board to.";

                    AttachBookingMessageLabel.CssClass =
                        "rx-wishlist-form-message error";
                }
            }
            catch (Exception)
            {
                AttachBookingMessageLabel.Text =
                    "Unable to load your bookings.";

                AttachBookingMessageLabel.CssClass =
                    "rx-wishlist-form-message error";
            }
        }

        protected void CloseAttachBookingButton_Click(
    object sender,
    EventArgs e)
        {
            AttachBookingPopup.Visible = false;

            AttachBookingMessageLabel.Text = "";

            if (BookingDropDown.Items.Count > 0)
            {
                BookingDropDown.SelectedIndex = 0;
            }
        }

        // ==========================================
        // SAVE ATTACHMENT TO BOOKING
        // ==========================================

        protected void SaveAttachBookingButton_Click(
            object sender,
            EventArgs e)
        {
            AttachBookingPopup.Visible = true;

            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                int bookingId =
                    Convert.ToInt32(
                        BookingDropDown.SelectedValue);

                // ------------------------------------------
                // Verify that this inspiration folder
                // belongs to the logged-in customer.
                // ------------------------------------------

                WishlistFolderModel folder =
                    wishlistRepository.GetFolderById(
                        FolderID,
                        userId);

                if (folder == null)
                {
                    Response.Redirect("Wishlist.aspx");
                    return;
                }

                // ------------------------------------------
                // Attach the inspiration folder to the booking.
                // The repository also verifies that the
                // booking belongs to this customer.
                // ------------------------------------------

                bool attached =
                    bookingRepository.AttachWishlistFolderToBooking(
                        bookingId,
                        userId,
                        FolderID);

                if (attached)
                {
                    AttachBookingPopup.Visible = false;

                    BookingDropDown.SelectedIndex = 0;

                    BoardMessageLabel.Text =
                        "Your inspiration board has been attached to the booking.";

                    BoardMessageLabel.CssClass =
                        "rx-wishlist-message success";
                }
                else
                {
                    AttachBookingMessageLabel.Text =
                        "Unable to attach the inspiration board to that booking.";

                    AttachBookingMessageLabel.CssClass =
                        "rx-wishlist-form-message error";
                }
            }
            catch (Exception)
            {
                AttachBookingMessageLabel.Text =
                    "Something went wrong while attaching the inspiration board.";

                AttachBookingMessageLabel.CssClass =
                    "rx-wishlist-form-message error";
            }
        }
    }
}