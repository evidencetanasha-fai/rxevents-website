<%@ Page Title="Edit Profile"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="EditProfile.aspx.cs"
    Inherits="RxEvents.EditProfile" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-edit-profile-page">

        <!-- Breadcrumb -->
        <!--<div class="rx-profile-breadcrumb">
            <span>Home</span>
            <strong>›</strong>
            <span>Profile</span>
            <strong>›</strong>
            <span>Edit Profile</span>
        </div>-->


        <!-- Page Heading -->
        <div class="rx-profile-heading">

            <div>
                <h2>Edit Profile</h2>

                <p>
                    Update your personal information and account details
                </p>
            </div>

        </div>


        <div class="rx-edit-profile-layout">

            <!-- ================= MAIN FORM ================= -->

            <div class="rx-edit-profile-main">

                <!-- Personal Information -->

                <div class="rx-edit-profile-card">

                    <div class="rx-edit-card-heading">

                        <span>♙</span>

                        <div>
                            <h3>Personal Information</h3>
                            <p>
                                Update your personal details
                            </p>
                        </div>

                    </div>


                    <!-- First Name / Last Name -->

                    <div class="rx-edit-row">

                        <div class="rx-edit-field">

                            <label>
                                First Name
                                <b>*</b>
                            </label>

                            <asp:TextBox
                                ID="FirstNameTextBox"
                                runat="server"
                                CssClass="rx-edit-input"
                                placeholder="Enter your first name">
                            </asp:TextBox>

                        </div>


                        <div class="rx-edit-field">

                            <label>
                                Last Name
                                <b>*</b>
                            </label>

                            <asp:TextBox
                                ID="LastNameTextBox"
                                runat="server"
                                CssClass="rx-edit-input"
                                placeholder="Enter your last name">
                            </asp:TextBox>

                        </div>

                    </div>


                    <!-- Email / Phone -->

                    <div class="rx-edit-row">

                        <div class="rx-edit-field">

                            <label>
                                Email Address
                                <b>*</b>
                            </label>

                            <asp:TextBox
                                ID="EmailTextBox"
                                runat="server"
                                CssClass="rx-edit-input"
                                placeholder="Enter your email">
                            </asp:TextBox>

                        </div>


                        <div class="rx-edit-field">

                            <label>
                                Phone Number
                                <b>*</b>
                            </label>

                            <asp:TextBox
                                ID="PhoneTextBox"
                                runat="server"
                                CssClass="rx-edit-input"
                                placeholder="Enter your phone number">
                            </asp:TextBox>

                        </div>

                    </div>


                    <!-- Validation Summary -->

                    <asp:ValidationSummary
                        ID="EditValidationSummary"
                        runat="server"
                        CssClass="rx-edit-validation-summary"
                        HeaderText="Please correct the following:"
                        EnableClientScript="false" />


                    <!-- Buttons -->

                    <div class="rx-edit-buttons">

                        <a href="Profile.aspx"
                           class="rx-edit-cancel-button">
                            Cancel
                        </a>

                        <asp:Button
                            ID="SaveProfileButton"
                            runat="server"
                            Text="Save Changes"
                            CssClass="rx-edit-save-button"
                            OnClick="SaveProfileButton_Click" />

                    </div>


                    <asp:Label
                        ID="MessageLabel"
                        runat="server"
                        CssClass="rx-edit-message">
                    </asp:Label>

                </div>


                <!-- Preferences visual section -->

                <div class="rx-edit-profile-card">

                    <div class="rx-edit-card-heading">

                        <span>⚙</span>

                        <div>
                            <h3>Preferences</h3>

                            <p>
                                Customize your event preferences
                            </p>
                        </div>

                    </div>


                    <div class="rx-edit-preferences">

                        <div class="rx-edit-preference-group">

                            <label>Preferred Event Types</label>

                            <div class="rx-edit-select">
                                Weddings
                                <span>⌄</span>
                            </div>

                        </div>


                        <div class="rx-edit-preference-group">

                            <label>Preferred Locations</label>

                            <div class="rx-edit-select">
                                Select locations
                                <span>⌄</span>
                            </div>

                        </div>


                        <div class="rx-edit-preference-group">

                            <label>Budget Range</label>

                            <div class="rx-edit-select">
                                Select budget
                                <span>⌄</span>
                            </div>

                        </div>


                        <div class="rx-edit-preference-group">

                            <label>Guest Range</label>

                            <div class="rx-edit-select">
                                Select guest range
                                <span>⌄</span>
                            </div>

                        </div>

                    </div>

                </div>

            </div>


            <!-- ================= RIGHT COLUMN ================= -->

            <aside class="rx-edit-profile-sidebar">


                <!-- Profile Photo -->

                <div class="rx-edit-photo-card">

                    <h2>Profile Photo</h2>

                    <div class="rx-edit-photo">

                        <asp:Image
                            ID="ProfileImage"
                            runat="server"
                            AlternateText="Profile Photo" />

                    </div>

                    <p>
                        JPG, PNG or GIF.<br />
                        Max size 2MB
                    </p>

                    <asp:FileUpload
                        ID="ProfilePhotoUpload"
                        runat="server"
                        CssClass="rx-profile-file-upload" />

                    <asp:Button
                        ID="ChangePhotoButton"
                        runat="server"
                        Text="Change Photo"
                        CssClass="rx-change-photo-button"
                        OnClick="ChangePhotoButton_Click" />

                </div>


                <!-- Account & Security -->

                <div class="rx-edit-security-card">

                    <div class="rx-edit-security-heading">

                        <span>♢</span>

                        <div>
                            <h3>Account &amp; Security</h3>

                            <p>
                                Manage your account security
                            </p>
                        </div>

                    </div>


                    <a href="#"
                       class="rx-edit-security-item">

                        <span>♢</span>

                        <div>
                            <strong>Change Password</strong>
                            <small>
                                Update your account password
                            </small>
                        </div>

                        <b>›</b>

                    </a>


                    <a href="#"
                       class="rx-edit-security-item">

                        <span>◇</span>

                        <div>
                            <strong>Two-Factor Authentication</strong>
                            <small>
                                Add extra security to your account
                            </small>
                        </div>

                        <b>›</b>

                    </a>


                    <a href="#"
                       class="rx-edit-security-item">

                        <span>✉</span>

                        <div>
                            <strong>Email Verification</strong>
                            <small>
                                Verify your email address
                            </small>
                        </div>

                        <b>›</b>

                    </a>


                    <a href="#"
                       class="rx-edit-security-item">

                        <span>♧</span>

                        <div>
                            <strong>Phone Verification</strong>
                            <small>
                                Verify your phone number
                            </small>
                        </div>

                        <b>›</b>

                    </a>


                    <a href="#"
                       class="rx-edit-security-item">

                        <span>⌖</span>

                        <div>
                            <strong>Manage Address</strong>
                            <small>
                                Manage your saved addresses
                            </small>
                        </div>

                        <b>›</b>

                    </a>


                    <a href="Notifications.aspx"
                       class="rx-edit-security-item">

                        <span>♧</span>

                        <div>
                            <strong>Notification Settings</strong>
                            <small>
                                Manage notification preferences
                            </small>
                        </div>

                        <b>›</b>

                    </a>


                    <a href="#"
                       class="rx-edit-security-item">

                        <span>◇</span>

                        <div>
                            <strong>Privacy &amp; Security</strong>
                            <small>
                                Manage privacy and security
                            </small>
                        </div>

                        <b>›</b>

                    </a>

                </div>

            </aside>

        </div>

    </section>

</asp:Content>