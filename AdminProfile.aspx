
<%@ Page Title="My Admin Profile" Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdminProfile.aspx.cs"
    Inherits="RxEvents.AdminProfile" %>

<asp:Content ID="AdminProfileContent"
    ContentPlaceHolderID="MainContent" runat="server">

<div class="rx-admin-profile">

    <!-- PAGE HEADING -->
    <div class="profile-page-heading">
        <div>
            <div class="profile-breadcrumb">Home &nbsp; › &nbsp; Profile</div>
            <h1>My Admin Profile</h1>
            <p>View and manage your personal information and account details.</p>
        </div>

        <asp:HyperLink ID="EditProfileButton" runat="server"
            NavigateUrl="~/AdminProfileEdit.aspx"
            CssClass="profile-edit-button">
            ✎ &nbsp; Edit Profile
        </asp:HyperLink>
    </div>

    <!-- PROFILE SUMMARY -->
    <section class="admin-profile-summary">

        <div class="admin-profile-avatar">
    <asp:Image ID="ProfileAvatarImage" runat="server"
        CssClass="admin-profile-avatar-image"
        AlternateText="Administrator profile photo"
        Visible="false" />

    <asp:Literal ID="ProfileInitials" runat="server" Text="AD" />
</div>

        <div class="admin-profile-identity">
            <h2><asp:Literal ID="ProfileFullName" runat="server" /></h2>
            <span class="admin-verified-badge">✓ Verified Administrator</span>
            <strong>Administrator Account</strong>
            <p>System Administrator at Rx Events Solutions</p>
        </div>

        <div class="admin-profile-stat">
            <span class="profile-stat-icon">▦</span>
            <strong><asp:Literal ID="ProfileBookingsCount" runat="server" Text="0" /></strong>
            <span>Bookings Managed</span>
        </div>

        <div class="admin-profile-stat">
            <span class="profile-stat-icon">◇</span>
            <strong><asp:Literal ID="ProfileEventsCount" runat="server" Text="0" /></strong>
            <span>Events Managed</span>
        </div>

        <div class="admin-profile-stat">
            <span class="profile-stat-icon">▥</span>
            <strong><asp:Literal ID="ProfileReportsCount" runat="server" Text="0" /></strong>
            <span>Reports Available</span>
        </div>

    </section>

    <!-- INFORMATION CARDS -->
    <div class="admin-profile-information">

        <section class="admin-profile-card">
            <h2><span>♙</span> Personal Information</h2>

            <div class="profile-detail-row">
                <span>Full Name</span>
                <strong><asp:Literal ID="DetailFullName" runat="server" /></strong>
            </div>

            <div class="profile-detail-row">
                <span>Email Address</span>
                <strong><asp:Literal ID="DetailEmail" runat="server" /></strong>
            </div>

            <div class="profile-detail-row">
                <span>Phone Number</span>
                <strong><asp:Literal ID="DetailPhone" runat="server" /></strong>
            </div>

            <div class="profile-detail-row">
                <span>Account Role</span>
                <strong>System Administrator</strong>
            </div>

            <div class="profile-detail-row">
                <span>Member Since</span>
                <strong><asp:Literal ID="DetailMemberSince" runat="server" /></strong>
            </div>
        </section>

        <section class="admin-profile-card">
            <h2><span>♢</span> Contact &amp; Security</h2>

            
<div class="profile-detail-row">
    <span>Password</span>
    <strong>••••••••••</strong>

    <asp:LinkButton ID="OpenChangePasswordPopup"
        runat="server"
        CssClass="change-password-link"
        CausesValidation="false"
        OnClick="OpenChangePasswordPopup_Click">
        Change Password
    </asp:LinkButton>
</div>


            <div class="profile-detail-row">
                <span>Email Address</span>
                <strong class="profile-available">● Account email</strong>
            </div>

            <div class="profile-detail-row">
                <span>Phone Number</span>
                <strong class="profile-available">● Account phone</strong>
            </div>

            <div class="profile-detail-row">
                <span>Account Status</span>
                <strong class="profile-available">
                    <asp:Literal ID="DetailAccountStatus" runat="server" />
                </strong>
            </div>
        </section>

        <!-- QUICK ACTIONS -->
        <aside class="admin-profile-card profile-quick-actions">
            <h2>⚡ Quick Actions</h2>

            <asp:HyperLink ID="QuickEditProfile" runat="server"
                NavigateUrl="~/AdminProfileEdit.aspx"
                CssClass="profile-action-link">
                <span class="profile-action-icon">✎</span>
                <span><strong>Edit Profile</strong><small>Update personal information</small></span>
                <span>›</span>
            </asp:HyperLink>

            
<asp:LinkButton ID="QuickChangePassword"
    runat="server"
    CssClass="profile-action-link"
    CausesValidation="false"
    OnClick="OpenChangePasswordPopup_Click">

    <span class="profile-action-icon">♙</span>

    <span>
        <strong>Change Password</strong>
        <small>Update account password</small>
    </span>

    <span>›</span>

</asp:LinkButton>


            <asp:HyperLink ID="QuickNotifications" runat="server"
                NavigateUrl="~/AdminSettings.aspx"
                CssClass="profile-action-link">
                <span class="profile-action-icon">♧</span>
                <span><strong>Settings</strong><small>Manage admin settings</small></span>
                <span>›</span>
            </asp:HyperLink>

            <asp:HyperLink ID="QuickWebSettings" runat="server"
                NavigateUrl="~/AdminWebSettings.aspx"
                CssClass="profile-action-link">
                <span class="profile-action-icon">⚙</span>
                <span><strong>Web Settings</strong><small>Manage website settings</small></span>
                <span>›</span>
            </asp:HyperLink>
        </aside>

    </div>

    <!-- ADMIN PREFERENCES -->
    <section class="admin-profile-card admin-preferences">
        <h2>⚙ Admin Preferences</h2>

        <div class="admin-preferences-grid">

            <div class="preference-group">
                <h3>Dashboard Theme</h3>
                <p>Choose your preferred dashboard appearance.</p>

                <asp:DropDownList ID="DisplayModeDropdown" runat="server"
                    CssClass="profile-preference-select">
                    <asp:ListItem Value="Light">Light</asp:ListItem>
                    <asp:ListItem Value="Dark">Dark</asp:ListItem>
                    <asp:ListItem Value="System">System default</asp:ListItem>
                </asp:DropDownList>

                <asp:Button ID="SavePreferencesButton" runat="server"
                    Text="Save Preference"
                    CssClass="profile-save-button"
                    OnClick="SavePreferencesButton_Click" />

                <asp:Label ID="PreferencesMessage" runat="server"
                    CssClass="profile-message" />
            </div>

            <div class="preference-group">
                <h3>Administrator Access</h3>
                <p>Your account is used to manage bookings, events, customers, and website content.</p>
                <span class="admin-verified-badge">✓ Administrator role</span>
            </div>

            <div class="preference-group">
                <h3>Account Information</h3>
                <p>Keep your contact details up to date so your account information remains accurate.</p>
                <asp:HyperLink ID="PreferenceEditLink" runat="server"
                    NavigateUrl="~/AdminProfileEdit.aspx"
                    CssClass="profile-text-link">
                    Review profile details →
                </asp:HyperLink>
            </div>

        </div>
    </section>

</div>
    
<!-- Change Password Popup -->
<asp:Panel ID="ChangePasswordOverlay" runat="server"
    CssClass="password-modal-overlay" Visible="false">

    <asp:Panel ID="ChangePasswordModal" runat="server"
        CssClass="password-modal">

        <div class="password-modal-header">
            <div>
                <h2>Change Password</h2>
                <p>Update your administrator account password.</p>
            </div>

            <asp:LinkButton ID="ClosePasswordPopup"
                runat="server"
                CssClass="password-modal-close"
                CausesValidation="false"
                OnClick="ClosePasswordPopup_Click">
                ×
            </asp:LinkButton>
        </div>

        <asp:Label ID="PasswordMessage" runat="server"
            CssClass="password-modal-message" />

        <div class="password-modal-field">
            <asp:Label runat="server"
                AssociatedControlID="CurrentPasswordTextBox"
                Text="Current Password" />

            <asp:TextBox ID="CurrentPasswordTextBox" runat="server"
                TextMode="Password" MaxLength="128" />
        </div>

        <div class="password-modal-field">
            <asp:Label runat="server"
                AssociatedControlID="NewPasswordTextBox"
                Text="New Password" />

            <asp:TextBox ID="NewPasswordTextBox" runat="server"
                TextMode="Password" MaxLength="128" />
        </div>

        <div class="password-modal-field">
            <asp:Label runat="server"
                AssociatedControlID="ConfirmPasswordTextBox"
                Text="Confirm New Password" />

            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server"
                TextMode="Password" MaxLength="128" />
        </div>

        <div class="password-modal-actions">
            <asp:LinkButton ID="CancelPasswordChange"
                runat="server"
                CssClass="password-cancel-button"
                CausesValidation="false"
                OnClick="ClosePasswordPopup_Click">
                Cancel
            </asp:LinkButton>

            <asp:Button ID="SavePasswordButton" runat="server"
                Text="Update Password"
                CssClass="password-save-button"
                OnClick="SavePasswordButton_Click" />
        </div>

    </asp:Panel>
</asp:Panel>

</asp:Content>
