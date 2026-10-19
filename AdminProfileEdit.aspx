
<%@ Page Title="Edit Admin Profile" Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdminProfileEdit.aspx.cs"
    Inherits="RxEvents.AdminProfileEdit"
    UnobtrusiveValidationMode="None" %>

<asp:Content ID="EditProfileContent"
    ContentPlaceHolderID="MainContent" runat="server">

<div class="rx-admin-profile">

    <div class="profile-page-heading">
        <div>
            <div class="profile-breadcrumb">
                Home &nbsp; › &nbsp;
                <asp:HyperLink ID="ProfileBreadcrumbLink" runat="server"
                    NavigateUrl="~/AdminProfile.aspx">Profile</asp:HyperLink>
                &nbsp; › &nbsp; Edit Profile
            </div>
            <h1>Edit Admin Profile</h1>
            <p>Update your administrator account information.</p>
        </div>
    </div>

    <asp:Panel ID="ProfileFormPanel" runat="server"
        CssClass="admin-profile-card edit-profile-card">

        <h2>Personal Information</h2>
        <p class="edit-profile-intro">
            Keep your contact details up to date.
        </p>

        <asp:Label ID="MessageLabel" runat="server"
            CssClass="profile-message" EnableViewState="false" />

        <div class="edit-profile-form">

            <div class="edit-profile-field">
                <asp:Label ID="FirstNameLabel" runat="server"
                    AssociatedControlID="FirstNameTextBox"
                    Text="First Name" />
                <asp:TextBox ID="FirstNameTextBox" runat="server"
                    MaxLength="100" />
                <asp:RequiredFieldValidator ID="FirstNameValidator"
                    runat="server" ControlToValidate="FirstNameTextBox"
                    ErrorMessage="First name is required."
                    CssClass="profile-validation" Display="Dynamic" />
            </div>

            <div class="edit-profile-field">
                <asp:Label ID="LastNameLabel" runat="server"
                    AssociatedControlID="LastNameTextBox"
                    Text="Last Name" />
                <asp:TextBox ID="LastNameTextBox" runat="server"
                    MaxLength="100" />
                <asp:RequiredFieldValidator ID="LastNameValidator"
                    runat="server" ControlToValidate="LastNameTextBox"
                    ErrorMessage="Last name is required."
                    CssClass="profile-validation" Display="Dynamic" />
            </div>

            <div class="edit-profile-field">
                <asp:Label ID="EmailLabel" runat="server"
                    AssociatedControlID="EmailTextBox"
                    Text="Email Address" />
                <asp:TextBox ID="EmailTextBox" runat="server"
                    TextMode="Email" MaxLength="255" />
                <asp:RequiredFieldValidator ID="EmailRequiredValidator"
                    runat="server" ControlToValidate="EmailTextBox"
                    ErrorMessage="Email is required."
                    CssClass="profile-validation" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="EmailFormatValidator"
                    runat="server" ControlToValidate="EmailTextBox"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                    ErrorMessage="Enter a valid email address."
                    CssClass="profile-validation" Display="Dynamic" />
            </div>

            <div class="edit-profile-field">
                <asp:Label ID="PhoneLabel" runat="server"
                    AssociatedControlID="PhoneTextBox"
                    Text="Phone Number" />
                <asp:TextBox ID="PhoneTextBox" runat="server"
                    MaxLength="30" />
            </div>

            <div class="edit-profile-field edit-profile-photo-field">
                <asp:Label ID="PhotoLabel" runat="server"
                    AssociatedControlID="ProfilePhotoUpload"
                    Text="Profile Photo" />

                <asp:Image ID="CurrentProfilePhoto" runat="server"
                    CssClass="edit-profile-photo"
                    AlternateText="Current administrator profile photo"
                    Visible="false" />

                <asp:FileUpload ID="ProfilePhotoUpload" runat="server"
                    CssClass="profile-file-upload" />

                <span class="profile-field-help">
                    Optional. JPG, JPEG, PNG, or WEBP. Maximum 2 MB.
                </span>
            </div>

        </div>

        <div class="edit-profile-actions">
            <asp:HyperLink ID="CancelEditLink" runat="server"
                NavigateUrl="~/AdminProfile.aspx"
                CssClass="profile-cancel-button">
                Cancel
            </asp:HyperLink>

            <asp:Button ID="SaveProfileButton" runat="server"
                Text="Save Changes"
                CssClass="profile-save-button"
                OnClick="SaveProfileButton_Click" />
        </div>

    </asp:Panel>

</div>

</asp:Content>
