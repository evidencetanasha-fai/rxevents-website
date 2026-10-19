using System;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Profile : System.Web.UI.Page
    {
        private readonly AddressRepository addressRepository =
    new AddressRepository();

        private int EditingAddressID
        {
            get
            {
                if (ViewState["EditingAddressID"] == null)
                {
                    return 0;
                }

                return Convert.ToInt32(
                    ViewState["EditingAddressID"]);
            }

            set
            {
                ViewState["EditingAddressID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
                LoadAddresses();
                LoadDisplayMode();
            }
        }

        private void LoadProfile()
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

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

                ProfileNameLabel.Text =
                    user.FirstName + " " + user.LastName;

                FullNameLabel.Text =
                    user.FirstName + " " + user.LastName;

                EmailLabel.Text =
                    user.Email;

                PhoneLabel.Text =
                    string.IsNullOrWhiteSpace(user.Phone)
                        ? "Not provided"
                        : user.Phone;

                MemberSinceLabel.Text =
                    user.CreatedDate.ToString("MMMM yyyy");

                CreatedDateLabel.Text =
                    user.CreatedDate.ToString("dd MMMM yyyy");


                // Profile photo
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
                Response.Redirect("Login.aspx");
            }
        }
        private void LoadAddresses()
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

                var addresses =
                    addressRepository.GetAddressesByUser(userId);

                AddressesRepeater.DataSource = addresses;
                AddressesRepeater.DataBind();

                if (addresses.Count == 0)
                {
                    NoAddressesPanel.Visible = true;
                }
                else
                {
                    NoAddressesPanel.Visible = false;
                }
            }
            catch (Exception)
            {
                AddressMessageLabel.Text =
                    "Unable to load your saved addresses.";

                AddressMessageLabel.CssClass =
                    "rx-address-message error";
            }
        }
        protected void ManageAddressButton_Click(
    object sender,
    EventArgs e)
        {
            AddressModal.Visible = true;

            SavedAddressesPanel.Visible = true;
            AddressFormPanel.Visible = false;

            LoadAddresses();
        }
        protected void CloseAddressButton_Click(
    object sender,
    EventArgs e)
        {
            AddressModal.Visible = false;

            ClearAddressForm();
        }
        protected void AddNewAddressButton_Click(
    object sender,
    EventArgs e)
        {
            EditingAddressID = 0;

            ClearAddressForm();

            AddressFormTitleLabel.Text =
                "Add Address";

            SavedAddressesPanel.Visible = false;
            AddressFormPanel.Visible = true;

            AddressModal.Visible = true;
        }
        protected void CancelAddressFormButton_Click(
    object sender,
    EventArgs e)
        {
            ClearAddressForm();

            SavedAddressesPanel.Visible = true;
            AddressFormPanel.Visible = false;

            AddressModal.Visible = true;

            LoadAddresses();
        }
        private void ClearAddressForm()
        {
            AddressLine1TextBox.Text = "";
            AddressLine2TextBox.Text = "";
            CityTextBox.Text = "";
            StateTextBox.Text = "";
            PostalCodeTextBox.Text = "";
            CountryTextBox.Text = "India";

            DefaultAddressCheckBox.Checked = false;

            AddressMessageLabel.Text = "";
            AddressMessageLabel.CssClass =
                "rx-address-message";

            EditingAddressID = 0;
        }
        protected void SaveAddressButton_Click(
    object sender,
    EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!Page.IsValid)
            {
                AddressModal.Visible = true;
                SavedAddressesPanel.Visible = false;
                AddressFormPanel.Visible = true;

                return;
            }

            try
            {
                int userId =
                    Convert.ToInt32(Session["UserID"]);


                Address address =
                    new Address();

                address.UserID = userId;

                address.AddressLine1 =
                    AddressLine1TextBox.Text.Trim();

                address.AddressLine2 =
                    AddressLine2TextBox.Text.Trim();

                address.City =
                    CityTextBox.Text.Trim();

                address.State =
                    StateTextBox.Text.Trim();

                address.PostalCode =
                    PostalCodeTextBox.Text.Trim();

                address.Country =
                    CountryTextBox.Text.Trim();

                address.IsDefault =
                    DefaultAddressCheckBox.Checked;


                bool saved;


                // EDIT

                if (EditingAddressID > 0)
                {
                    address.AddressID =
                        EditingAddressID;

                    saved =
                        addressRepository.UpdateAddress(
                            address);
                }

                // ADD

                else
                {
                    saved =
                        addressRepository.AddAddress(
                            address);
                }


                if (saved)
                {
                    string successMessage =
    EditingAddressID > 0
        ? "Address updated successfully."
        : "Address added successfully.";

                    ClearAddressForm();

                    AddressMessageLabel.Text =
                        successMessage;

                    AddressMessageLabel.CssClass =
                        "rx-address-message success";

                    SavedAddressesPanel.Visible = true;
                    AddressFormPanel.Visible = false;
                    AddressModal.Visible = true;

                    LoadAddresses();
                }
                else
                {
                    AddressMessageLabel.Text =
                        "Unable to save the address. Please try again.";

                    AddressMessageLabel.CssClass =
                        "rx-address-message error";

                    AddressModal.Visible = true;
                }
            }
            catch (Exception)
            {
                AddressMessageLabel.Text =
                    "An error occurred while saving the address.";

                AddressMessageLabel.CssClass =
                    "rx-address-message error";

                AddressModal.Visible = true;
            }
        }
        protected void AddressesRepeater_ItemCommand(
    object source,
    System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }


            int addressId =
                Convert.ToInt32(e.CommandArgument);


            int userId =
                Convert.ToInt32(Session["UserID"]);


            try
            {
                if (e.CommandName == "EditAddress")
                {
                    EditAddress(
                        addressId,
                        userId);
                }

                else if (e.CommandName == "DeleteAddress")
                {
                    DeleteAddress(
                        addressId,
                        userId);
                }

                else if (e.CommandName == "SetDefault")
                {
                    SetDefaultAddress(
                        addressId,
                        userId);
                }
            }
            catch (Exception)
            {
                AddressMessageLabel.Text =
                    "Unable to complete the requested action.";

                AddressMessageLabel.CssClass =
                    "rx-address-message error";

                AddressModal.Visible = true;

                SavedAddressesPanel.Visible = true;
                AddressFormPanel.Visible = false;

                LoadAddresses();
            }
        }
        private void EditAddress(
    int addressId,
    int userId)
        {
            Address address =
                addressRepository.GetAddressById(
                    addressId,
                    userId);


            if (address == null)
            {
                return;
            }


            EditingAddressID =
                address.AddressID;


            AddressLine1TextBox.Text =
                address.AddressLine1;

            AddressLine2TextBox.Text =
                address.AddressLine2;

            CityTextBox.Text =
                address.City;

            StateTextBox.Text =
                address.State;

            PostalCodeTextBox.Text =
                address.PostalCode;

            CountryTextBox.Text =
                address.Country;

            DefaultAddressCheckBox.Checked =
                address.IsDefault;


            AddressFormTitleLabel.Text =
                "Edit Address";


            SavedAddressesPanel.Visible = false;
            AddressFormPanel.Visible = true;

            AddressModal.Visible = true;
        }
        private void DeleteAddress(
    int addressId,
    int userId)
        {
            bool deleted =
                addressRepository.DeleteAddress(
                    addressId,
                    userId);


            if (deleted)
            {
                AddressMessageLabel.Text =
                    "Address deleted successfully.";

                AddressMessageLabel.CssClass =
                    "rx-address-message success";
            }
            else
            {
                AddressMessageLabel.Text =
                    "Unable to delete the address.";

                AddressMessageLabel.CssClass =
                    "rx-address-message error";
            }


            AddressModal.Visible = true;

            SavedAddressesPanel.Visible = true;
            AddressFormPanel.Visible = false;

            LoadAddresses();
        }
        private void SetDefaultAddress(
    int addressId,
    int userId)
        {
            bool updated =
                addressRepository.SetDefaultAddress(
                    addressId,
                    userId);


            if (updated)
            {
                AddressMessageLabel.Text =
                    "Default address updated successfully.";

                AddressMessageLabel.CssClass =
                    "rx-address-message success";
            }
            else
            {
                AddressMessageLabel.Text =
                    "Unable to update the default address.";

                AddressMessageLabel.CssClass =
                    "rx-address-message error";
            }


            AddressModal.Visible = true;

            SavedAddressesPanel.Visible = true;
            AddressFormPanel.Visible = false;

            LoadAddresses();
        }
        private void LoadDisplayMode()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                UserRepository repository = new UserRepository();
                User user = repository.GetUserById(userId);

                if (user != null &&
                    string.Equals(user.DisplayMode, "Dark",
                        StringComparison.OrdinalIgnoreCase))
                {
                    DarkModeCheckBox.Checked = true;
                }
                else
                {
                    DarkModeCheckBox.Checked = false;
                }
            }
            catch (Exception)
            {
                DarkModeCheckBox.Checked = false;
            }
        }
        protected void DarkModeCheckBox_CheckedChanged(
    object sender,
    EventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                int userId = Convert.ToInt32(Session["UserID"]);

                string displayMode =
                    DarkModeCheckBox.Checked ? "Dark" : "Light";

                UserRepository repository = new UserRepository();

                bool updated =
                    repository.UpdateDisplayMode(
                        userId,
                        displayMode);

                if (!updated)
                {
                    DarkModeCheckBox.Checked =
                        !DarkModeCheckBox.Checked;
                }
            }
            catch (Exception)
            {
                DarkModeCheckBox.Checked =
                    !DarkModeCheckBox.Checked;
            }
        }
    }
}