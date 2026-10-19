<%@ Page Title="My Profile"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="Profile.aspx.cs"
    Inherits="RxEvents.Profile" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-profile-page">

        <!-- ================= BREADCRUMB ================= -->

        <!--<div class="rx-profile-breadcrumb">
            <span>Home</span>
            <strong>›</strong>
            <span>Profile</span>
        </div>-->


        <!-- ================= PAGE HEADER ================= -->

        <div class="rx-profile-heading">

    <div>
        <h2>My Profile</h2>

        <p>
            View your personal information and account details
        </p>
    </div>

    <div class="rx-profile-heading-actions">

        <a href="EditProfile.aspx"
           class="rx-profile-edit-button">
            Edit Profile
        </a>

        <div class="rx-profile-theme-toggle">

            <div class="rx-profile-theme-label">
                <span class="rx-profile-theme-icon">☾</span>

                <div>
                    <strong>Dark Mode</strong>
                    <small>Use a darker appearance</small>
                </div>
            </div>

            <asp:CheckBox ID="DarkModeCheckBox"
    runat="server"
    CssClass="rx-profile-dark-checkbox"
    AutoPostBack="true"
    OnCheckedChanged="DarkModeCheckBox_CheckedChanged" />

        </div>

    </div>

</div>


        <!-- ================= PROFILE SUMMARY ================= -->

        <div class="rx-profile-summary">

            <div class="rx-profile-summary-left">

                <div class="rx-profile-avatar">

                    <asp:Image
                        ID="ProfileImage"
                        runat="server"
                        AlternateText="Profile Photo" />

                </div>


                <div class="rx-profile-identity">

                    <h2>
                        <asp:Label
                            ID="ProfileNameLabel"
                            runat="server">
                        </asp:Label>
                    </h2>

                    <span class="rx-profile-badge">
                        Verified Customer
                    </span>

                    <p>
                        Member Since
                        <asp:Label
                            ID="MemberSinceLabel"
                            runat="server">
                        </asp:Label>
                    </p>

                    <strong>
                        Loyal Customer
                    </strong>

                </div>

            </div>


            <!-- Profile statistics -->

            <div class="rx-profile-stat">

                <div class="rx-profile-stat-icon">
                    ◫
                </div>

                <strong>
                    <asp:Label
                        ID="TotalBookingsLabel"
                        runat="server"
                        Text="0">
                    </asp:Label>
                </strong>

                <span>Total Bookings</span>

            </div>


            <div class="rx-profile-stat">

                <div class="rx-profile-stat-icon">
                    ☆
                </div>

                <strong>
                    <asp:Label
                        ID="ReviewsGivenLabel"
                        runat="server"
                        Text="0">
                    </asp:Label>
                </strong>

                <span>Reviews Given</span>

            </div>


            <div class="rx-profile-stat">

                <div class="rx-profile-stat-icon">
                    ♡
                </div>

                <strong>
                    <asp:Label
                        ID="WishlistCountLabel"
                        runat="server"
                        Text="0">
                    </asp:Label>
                </strong>

                <span>Wishlist Items</span>

            </div>

        </div>


        <!-- ================= PROFILE GRID ================= -->

        <div class="rx-profile-grid">


            <!-- Personal Information -->

            <div class="rx-profile-card">

                <div class="rx-profile-card-heading">

                    <span class="rx-profile-card-icon">
                        ♙
                    </span>

                    <h3>Personal Information</h3>

                </div>


                <div class="rx-profile-information">

                    <div class="rx-profile-information-row">

                        <span>Full Name</span>

                        <strong>
                            <asp:Label
                                ID="FullNameLabel"
                                runat="server">
                            </asp:Label>
                        </strong>

                    </div>


                    <div class="rx-profile-information-row">

                        <span>Email Address</span>

                        <strong>
                            <asp:Label
                                ID="EmailLabel"
                                runat="server">
                            </asp:Label>
                        </strong>

                    </div>


                    <div class="rx-profile-information-row">

                        <span>Phone Number</span>

                        <strong>
                            <asp:Label
                                ID="PhoneLabel"
                                runat="server">
                            </asp:Label>
                        </strong>

                    </div>


                    <div class="rx-profile-information-row">

                        <span>Member Since</span>

                        <strong>
                            <asp:Label
                                ID="CreatedDateLabel"
                                runat="server">
                            </asp:Label>
                        </strong>

                    </div>


                    <div class="rx-profile-information-row">

                        <span>Account Type</span>

                        <strong>
                            Customer
                        </strong>

                    </div>

                </div>

            </div>


            <!-- Contact & Security -->

            <div class="rx-profile-card">

                <div class="rx-profile-card-heading">

                    <span class="rx-profile-card-icon">
                        ♢
                    </span>

                    <h3>Contact &amp; Security</h3>

                </div>


                <div class="rx-profile-information">

                    <div class="rx-profile-information-row">

                        <span>Email</span>

                        <strong class="rx-profile-verified">
                            ● Verified
                        </strong>

                    </div>


                    <div class="rx-profile-information-row">

                        <span>Phone</span>

                        <strong class="rx-profile-verified">
                            ● Verified
                        </strong>

                    </div>


                    <div class="rx-profile-information-row">

                        <span>Password</span>

                        <strong>
                            ********
                        </strong>

                        <a href="ChangePassword.aspx">
                            Change
                        </a>

                    </div>


                    <div class="rx-profile-information-row">

                        <span>Two-Factor Authentication</span>

                        <strong>
                            Not Enabled
                        </strong>

                    </div>

                </div>

            </div>


            <!-- Event Preferences -->

            <div class="rx-profile-card rx-profile-preferences">

                <div class="rx-profile-card-heading">

                    <span class="rx-profile-card-icon">
                        ⚙
                    </span>

                    <h3>Event Preferences</h3>

                </div>


                <div class="rx-profile-preference-section">

    <span>Preferred Event Types</span>

    <div class="rx-profile-tags">

        <asp:Repeater
            ID="PreferredEventsRepeater"
            runat="server">

            <ItemTemplate>

                <span>
                    <%# Eval("CategoryName") %>
                </span>

            </ItemTemplate>

        </asp:Repeater>

        <asp:Panel
            ID="NoPreferredEventsPanel"
            runat="server"
            Visible="false"
            CssClass="rx-profile-preference-empty">

            No preferences yet. Book an event to build your
            preferences.

        </asp:Panel>

    </div>

</div>


                <div class="rx-profile-preference-section">

                    <span>Preferred Locations</span>

                    <div class="rx-profile-tags">

                        <span>Kampala</span>
                        <span>Jinja</span>
                        <span>Entebbe</span>

                    </div>

                </div>


                <!--<div class="rx-profile-preference-section">

                    <span>Budget Range</span>

                    <div class="rx-profile-tags">

                        <span>UGX 500,000 - UGX 5,000,000</span>

                    </div>

                </div>


                <div class="rx-profile-preference-section">

                    <span>Guest Range</span>

                    <div class="rx-profile-tags">

                        <span>50 - 200 Guests</span>

                    </div>

                </div>-->

            </div>


            <!-- ================= QUICK ACTIONS ================= -->

<div class="rx-profile-quick-actions">

    <div class="rx-profile-card">

        <div class="rx-profile-card-title">

            <span class="rx-profile-card-icon">
                ✦
            </span>

            <h3>
                Quick Actions
            </h3>

        </div>


        <div class="rx-profile-actions">


            <!-- EDIT PROFILE -->

            <a href="EditProfile.aspx"
               class="rx-profile-action">

                <span class="rx-profile-action-icon">
                    ✎
                </span>

                <span class="rx-profile-action-content">

                    <strong>
                        Edit Profile
                    </strong>

                    <small>
                        Update your personal information
                    </small>

                </span>

                <span class="rx-profile-action-arrow">
                    ›
                </span>

            </a>


            <!-- CHANGE PASSWORD -->

            <a href="ChangePassword.aspx"
               class="rx-profile-action">

                <span class="rx-profile-action-icon">
                    🔒
                </span>

                <span class="rx-profile-action-content">

                    <strong>
                        Change Password
                    </strong>

                    <small>
                        Update your account password
                    </small>

                </span>

                <span class="rx-profile-action-arrow">
                    ›
                </span>

            </a>


            <!-- MANAGE ADDRESS -->

            <asp:LinkButton
                ID="ManageAddressButton"
                runat="server"
                CssClass="rx-profile-action"
                OnClick="ManageAddressButton_Click">

                <span class="rx-profile-action-icon">
                    📍
                </span>

                <span class="rx-profile-action-content">

                    <strong>
                        Manage Address
                    </strong>

                    <small>
                        Add or update your saved address
                    </small>

                </span>

                <span class="rx-profile-action-arrow">
                    ›
                </span>

            </asp:LinkButton>


            <!-- NOTIFICATION SETTINGS -->

            <a href="Notifications.aspx"
               class="rx-profile-action">

                <span class="rx-profile-action-icon">
                    ♧
                </span>

                <span class="rx-profile-action-content">

                    <strong>
                        Notification Settings
                    </strong>

                    <small>
                        Manage your notification preferences
                    </small>

                </span>

                <span class="rx-profile-action-arrow">
                    ›
                </span>

            </a>


            <!-- PRIVACY & SECURITY -->

            <a href="PrivacySecurity.aspx"
               class="rx-profile-action">

                <span class="rx-profile-action-icon">
                    ◇
                </span>

                <span class="rx-profile-action-content">

                    <strong>
                        Privacy &amp; Security
                    </strong>

                    <small>
                        Manage your privacy and security
                    </small>

                </span>

                <span class="rx-profile-action-arrow">
                    ›
                </span>

            </a>


        </div>

    </div>

</div>


            <!-- Promotional Card -->

            <div class="rx-profile-promo">

                <div>

                    <span>PLAN YOUR</span>

                    <strong>NEXT EVENT</strong>

                    <p>
                        Let's create more
                        beautiful memories.
                    </p>

                    <a href="BookEvent.aspx">
                        Book a New Event →
                    </a>

                </div>

            </div>

        </div>

    </section>

    <!-- =========================================================
     MANAGE ADDRESS POPUP
     ========================================================= -->

<asp:Panel
    ID="AddressModal"
    runat="server"
    CssClass="rx-address-modal-overlay"
    Visible="false">

    <div class="rx-address-modal">


        <!-- ================= HEADER ================= -->

        <div class="rx-address-modal-header">

            <div>

                <span class="rx-address-modal-eyebrow">
                    PROFILE
                </span>

                <h3>
                    Manage Address
                </h3>

                <p>
                    Add or update your saved address.
                </p>

            </div>

            <asp:Button
                ID="CloseAddressButton"
                runat="server"
                Text="×"
                CssClass="rx-address-close"
                CausesValidation="false"
                OnClick="CloseAddressButton_Click" />

        </div>


        <!-- ================= SAVED ADDRESSES ================= -->

        <asp:Panel
            ID="SavedAddressesPanel"
            runat="server"
            CssClass="rx-address-saved-section">

            <div class="rx-address-section-title">

                <h4>
                    Saved Addresses
                </h4>

                <asp:Button
                    ID="AddNewAddressButton"
                    runat="server"
                    Text="+ Add Address"
                    CssClass="rx-address-add-button"
                    CausesValidation="false"
                    OnClick="AddNewAddressButton_Click" />

            </div>


            <asp:Repeater
                ID="AddressesRepeater"
                runat="server"
                OnItemCommand="AddressesRepeater_ItemCommand">

                <ItemTemplate>

                    <div class="rx-address-item">

                        <div class="rx-address-item-top">

                            <div class="rx-address-location-icon">
                                📍
                            </div>

                            <div class="rx-address-details">

                                <div class="rx-address-title-row">

                                    <strong>
                                        Address
                                    </strong>

                                    <%# Convert.ToBoolean(
                                        Eval("IsDefault"))
                                        ? "<span class='rx-address-default'>Default</span>"
                                        : "" %>

                                </div>

                                <p>
                                    <%# Eval("AddressLine1") %>
                                    <br />

                                    <%# string.IsNullOrWhiteSpace(
                                        Eval("AddressLine2").ToString())
                                        ? ""
                                        : Eval("AddressLine2") + "<br />" %>

                                    <%# Eval("City") %>,
                                    <%# Eval("State") %>
                                    <%# Eval("PostalCode") %>
                                    <br />

                                    <%# Eval("Country") %>
                                </p>

                            </div>

                        </div>


                        <div class="rx-address-item-actions">

                            <asp:LinkButton
                                ID="EditAddressButton"
                                runat="server"
                                Text="Edit"
                                CssClass="rx-address-action edit"
                                CommandName="EditAddress"
                                CommandArgument='<%# Eval("AddressID") %>'
                                CausesValidation="false">
                            </asp:LinkButton>


                            <asp:LinkButton
                                ID="DefaultAddressButton"
                                runat="server"
                                Text="Set as Default"
                                CssClass="rx-address-action default"
                                CommandName="SetDefault"
                                CommandArgument='<%# Eval("AddressID") %>'
                                CausesValidation="false"
                                Visible='<%# !Convert.ToBoolean(Eval("IsDefault")) %>'>
                            </asp:LinkButton>


                            <asp:LinkButton
                                ID="DeleteAddressButton"
                                runat="server"
                                Text="Delete"
                                CssClass="rx-address-action delete"
                                CommandName="DeleteAddress"
                                CommandArgument='<%# Eval("AddressID") %>'
                                CausesValidation="false">
                            </asp:LinkButton>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>


            <asp:Panel
                ID="NoAddressesPanel"
                runat="server"
                CssClass="rx-address-empty"
                Visible="false">

                <div>
                    📍
                </div>

                <strong>
                    No saved address
                </strong>

                <p>
                    Add an address to make your event bookings easier.
                </p>

            </asp:Panel>

        </asp:Panel>


        <!-- ================= ADDRESS FORM ================= -->

        <asp:Panel
            ID="AddressFormPanel"
            runat="server"
            CssClass="rx-address-form-panel"
            Visible="false">

            <div class="rx-address-form-heading">

                <h4>
                    <asp:Label
                        ID="AddressFormTitleLabel"
                        runat="server"
                        Text="Add Address">
                    </asp:Label>
                </h4>

                <p>
                    Enter your address details below.
                </p>

            </div>


            <div class="rx-address-form-grid">


                <!-- Address Line 1 -->

                <div class="rx-address-field full">

                    <label>
                        Address Line 1
                        <span>*</span>
                    </label>

                    <asp:TextBox
                        ID="AddressLine1TextBox"
                        runat="server"
                        CssClass="rx-address-input"
                        placeholder="House number, street name">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="AddressLine1Required"
                        runat="server"
                        ControlToValidate="AddressLine1TextBox"
                        ErrorMessage="Address line 1 is required."
                        Display="Dynamic"
                        CssClass="rx-address-validation"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- Address Line 2 -->

                <div class="rx-address-field full">

                    <label>
                        Address Line 2
                        <small>(Optional)</small>
                    </label>

                    <asp:TextBox
                        ID="AddressLine2TextBox"
                        runat="server"
                        CssClass="rx-address-input"
                        placeholder="Apartment, landmark, area">
                    </asp:TextBox>

                </div>


                <!-- City -->

                <div class="rx-address-field">

                    <label>
                        City
                        <span>*</span>
                    </label>

                    <asp:TextBox
                        ID="CityTextBox"
                        runat="server"
                        CssClass="rx-address-input"
                        placeholder="City">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="CityRequired"
                        runat="server"
                        ControlToValidate="CityTextBox"
                        ErrorMessage="City is required."
                        Display="Dynamic"
                        CssClass="rx-address-validation"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- State -->

                <div class="rx-address-field">

                    <label>
                        State
                        <span>*</span>
                    </label>

                    <asp:TextBox
                        ID="StateTextBox"
                        runat="server"
                        CssClass="rx-address-input"
                        placeholder="State">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="StateRequired"
                        runat="server"
                        ControlToValidate="StateTextBox"
                        ErrorMessage="State is required."
                        Display="Dynamic"
                        CssClass="rx-address-validation"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- Postal Code -->

                <div class="rx-address-field">

                    <label>
                        Postal Code
                        <span>*</span>
                    </label>

                    <asp:TextBox
                        ID="PostalCodeTextBox"
                        runat="server"
                        CssClass="rx-address-input"
                        placeholder="Postal code">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="PostalCodeRequired"
                        runat="server"
                        ControlToValidate="PostalCodeTextBox"
                        ErrorMessage="Postal code is required."
                        Display="Dynamic"
                        CssClass="rx-address-validation"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- Country -->

                <div class="rx-address-field">

                    <label>
                        Country
                        <span>*</span>
                    </label>

                    <asp:TextBox
                        ID="CountryTextBox"
                        runat="server"
                        CssClass="rx-address-input"
                        Text="India">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="CountryRequired"
                        runat="server"
                        ControlToValidate="CountryTextBox"
                        ErrorMessage="Country is required."
                        Display="Dynamic"
                        CssClass="rx-address-validation"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                </div>

            </div>


            <!-- Default Address -->

            <div class="rx-address-default-option">

                <asp:CheckBox
                    ID="DefaultAddressCheckBox"
                    runat="server"
                    Text="Make this my default address" />

            </div>


            <!-- Form Message -->

            <asp:Label
                ID="AddressMessageLabel"
                runat="server"
                CssClass="rx-address-message">
            </asp:Label>


            <!-- Form Buttons -->

            <div class="rx-address-form-actions">

                <asp:Button
                    ID="CancelAddressFormButton"
                    runat="server"
                    Text="Cancel"
                    CssClass="rx-address-button secondary"
                    CausesValidation="false"
                    OnClick="CancelAddressFormButton_Click" />

                <asp:Button
                    ID="SaveAddressButton"
                    runat="server"
                    Text="Save Address"
                    CssClass="rx-address-button primary"
                    OnClick="SaveAddressButton_Click" />

            </div>

        </asp:Panel>

    </div>

</asp:Panel>

</asp:Content>