using System;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            // Make sure the customer is logged in
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Make sure the logged-in user is a customer
            if (Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                UserRepository repository =
                    new UserRepository();

                User user =
                    repository.GetUserById(userId);

                if (user == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                // Load user's information into the form

                FirstNameTextBox.Text =
                    user.FirstName;

                LastNameTextBox.Text =
                    user.LastName;

                EmailTextBox.Text =
                    user.Email;

                PhoneTextBox.Text =
                    user.Phone ?? "";

                // Load profile photo

                if (!string.IsNullOrWhiteSpace(user.ProfilePhoto))
                {
                    ProfileImage.ImageUrl =
                        ResolveUrl(
                            "~/Content/images/" +
                            user.ProfilePhoto);
                }
                else
                {
                    ProfileImage.ImageUrl =
                        ResolveUrl(
                            "~/Content/images/rx-events-logo.png");
                }
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to load your profile.";

                MessageLabel.CssClass =
                    "rx-edit-message error";
            }
        }


        protected void SaveProfileButton_Click(
            object sender,
            EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);

                User user = new User();

                user.UserID = userId;

                user.FirstName =
                    FirstNameTextBox.Text.Trim();

                user.LastName =
                    LastNameTextBox.Text.Trim();

                user.Email =
                    EmailTextBox.Text.Trim();

                user.Phone =
                    PhoneTextBox.Text.Trim();

                UserRepository repository =
                    new UserRepository();

                bool updated =
                    repository.UpdateUserProfile(user);

                if (updated)
                {
                    // Update session information

                    Session["FirstName"] =
                        user.FirstName;

                    Session["LastName"] =
                        user.LastName;

                    Session["Email"] =
                        user.Email;

                    MessageLabel.Text =
                        "Profile updated successfully.";

                    MessageLabel.CssClass =
                        "rx-edit-message success";
                }
                else
                {
                    MessageLabel.Text =
                        "No changes were made.";

                    MessageLabel.CssClass =
                        "rx-edit-message";
                }
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to update your profile.";

                MessageLabel.CssClass =
                    "rx-edit-message error";
            }
        }


        protected void ChangePhotoButton_Click(
            object sender,
            EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!ProfilePhotoUpload.HasFile)
            {
                MessageLabel.Text =
                    "Please select a profile photo.";

                MessageLabel.CssClass =
                    "rx-edit-message error";

                return;
            }

            try
            {
                string extension =
                    System.IO.Path
                    .GetExtension(
                        ProfilePhotoUpload.FileName)
                    .ToLower();

                if (extension != ".jpg" &&
                    extension != ".jpeg" &&
                    extension != ".png" &&
                    extension != ".gif")
                {
                    MessageLabel.Text =
                        "Only JPG, PNG or GIF images are allowed.";

                    MessageLabel.CssClass =
                        "rx-edit-message error";

                    return;
                }

                if (ProfilePhotoUpload.PostedFile.ContentLength
                    > 2 * 1024 * 1024)
                {
                    MessageLabel.Text =
                        "The image must be smaller than 2MB.";

                    MessageLabel.CssClass =
                        "rx-edit-message error";

                    return;
                }

                int userId =
                    Convert.ToInt32(Session["UserID"]);

                string fileName =
                    "profile_" +
                    userId +
                    extension;

                string folder =
                    Server.MapPath(
                        "~/Content/images/");

                string filePath =
                    System.IO.Path.Combine(
                        folder,
                        fileName);

                ProfilePhotoUpload.SaveAs(filePath);

                UserRepository repository =
                    new UserRepository();

                bool updated =
                    repository.UpdateProfilePhoto(
                        userId,
                        fileName);

                if (updated)
                {
                    ProfileImage.ImageUrl =
                        ResolveUrl(
                            "~/Content/images/" +
                            fileName);

                    MessageLabel.Text =
                        "Profile photo updated successfully.";

                    MessageLabel.CssClass =
                        "rx-edit-message success";
                }
            }
            catch (Exception)
            {
                MessageLabel.Text =
                    "Unable to update profile photo.";

                MessageLabel.CssClass =
                    "rx-edit-message error";
            }
        }
    }
}