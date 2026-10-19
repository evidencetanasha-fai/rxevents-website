<%@ Page Title="Privacy & Security"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="PrivacySecurity.aspx.cs"
    Inherits="RxEvents.PrivacySecurity" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">
    Privacy & Security
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-security-page">

        <!-- Breadcrumb -->
        <div class="rx-security-breadcrumb">
            <a href="CustomerDashboard.aspx">Home</a>
            <span>›</span>
            <span>Privacy &amp; Security</span>
        </div>

        <!-- Page Header -->
        <div class="rx-security-header">

            <div>
                <span class="rx-security-eyebrow">
                    ACCOUNT PROTECTION
                </span>

                <h2>Privacy &amp; Security</h2>

                <p>
                    Manage your account security and control your
                    personal information.
                </p>
            </div>

            <div class="rx-security-status">
                <span class="rx-security-status-icon">✓</span>

                <div>
                    <strong>Account Protected</strong>
                    <small>Your account is active and secure</small>
                </div>
            </div>

        </div>

        <!-- Security Overview -->
        <div class="rx-security-overview">

            <div class="rx-security-overview-card">

                <div class="rx-security-overview-icon">
                    🔐
                </div>

                <div>
                    <span>Password</span>
                    <strong>Protected</strong>
                    <small>Keep your password private</small>
                </div>

            </div>

            <div class="rx-security-overview-card">

                <div class="rx-security-overview-icon">
                    👤
                </div>

                <div>
                    <span>Profile</span>
                    <strong>Private</strong>
                    <small>Your information is account-based</small>
                </div>

            </div>

            <div class="rx-security-overview-card">

                <div class="rx-security-overview-icon">
                    🔔
                </div>

                <div>
                    <span>Notifications</span>
                    <strong>Managed</strong>
                    <small>Control your account updates</small>
                </div>

            </div>

        </div>

        <!-- Main Security Grid -->
        <div class="rx-security-grid">

            <!-- Account Security -->
            <div class="rx-security-card">

                <div class="rx-security-card-header">

                    <div class="rx-security-card-heading">
                        <span class="rx-security-card-icon">
                            🔐
                        </span>

                        <div>
                            <h3>Account Security</h3>
                            <p>Protect your account credentials.</p>
                        </div>
                    </div>

                </div>

                <div class="rx-security-option">

                    <div class="rx-security-option-icon">
                        🔑
                    </div>

                    <div class="rx-security-option-content">
                        <strong>Password</strong>

                        <small>
                            Change your account password regularly
                            to help keep your account protected.
                        </small>
                    </div>

                    <a href="ChangePassword.aspx"
                       class="rx-security-option-button">
                        Change
                    </a>

                </div>

                <div class="rx-security-option">

                    <div class="rx-security-option-icon">
                        ✓
                    </div>

                    <div class="rx-security-option-content">
                        <strong>Password Protection</strong>

                        <small>
                            Your password is stored securely and is
                            never displayed in your profile.
                        </small>
                    </div>

                    <span class="rx-security-protected">
                        Protected
                    </span>

                </div>

            </div>


            <!-- Privacy Controls -->
            <div class="rx-security-card">

                <div class="rx-security-card-header">

                    <div class="rx-security-card-heading">
                        <span class="rx-security-card-icon">
                            🛡
                        </span>

                        <div>
                            <h3>Privacy Controls</h3>
                            <p>Manage the information connected to your account.</p>
                        </div>
                    </div>

                </div>

                <div class="rx-security-option">

                    <div class="rx-security-option-icon">
                        👤
                    </div>

                    <div class="rx-security-option-content">
                        <strong>Profile Information</strong>

                        <small>
                            Review and update your personal details.
                        </small>
                    </div>

                    <a href="EditProfile.aspx"
                       class="rx-security-option-button">
                        Manage
                    </a>

                </div>

                <div class="rx-security-option">

                    <div class="rx-security-option-icon">
                        📍
                    </div>

                    <div class="rx-security-option-content">
                        <strong>Saved Address</strong>

                        <small>
                            Review or update addresses saved to your account.
                        </small>
                    </div>

                    <a href="Profile.aspx"
                       class="rx-security-option-button">
                        Manage
                    </a>

                </div>

                <div class="rx-security-option">

                    <div class="rx-security-option-icon">
                        🔔
                    </div>

                    <div class="rx-security-option-content">
                        <strong>Notifications</strong>

                        <small>
                            Review your account notifications and updates.
                        </small>
                    </div>

                    <a href="Notifications.aspx"
                       class="rx-security-option-button">
                        View
                    </a>

                </div>

            </div>


            <!-- Account Access -->
            <div class="rx-security-card">

                <div class="rx-security-card-header">

                    <div class="rx-security-card-heading">
                        <span class="rx-security-card-icon">
                            🔑
                        </span>

                        <div>
                            <h3>Account Access</h3>
                            <p>Information about your current account.</p>
                        </div>
                    </div>

                </div>

                <div class="rx-security-access-row">

                    <span>Account Status</span>

                    <strong class="rx-security-active">
                        ● Active
                    </strong>

                </div>

                <div class="rx-security-access-row">

                    <span>Account Type</span>

                    <strong>Customer</strong>

                </div>

                <div class="rx-security-access-row">

                    <span>Email Address</span>

                    <strong>
                        <asp:Label
                            ID="SecurityEmailLabel"
                            runat="server">
                        </asp:Label>
                    </strong>

                </div>

                <div class="rx-security-access-row">

                    <span>Member Since</span>

                    <strong>
                        <asp:Label
                            ID="SecurityMemberSinceLabel"
                            runat="server">
                        </asp:Label>
                    </strong>

                </div>

            </div>


            <!-- Security Information -->
            <div class="rx-security-card rx-security-information">

                <div class="rx-security-card-header">

                    <div class="rx-security-card-heading">
                        <span class="rx-security-card-icon">
                            ✦
                        </span>

                        <div>
                            <h3>Security Information</h3>
                            <p>Simple steps to keep your account safe.</p>
                        </div>
                    </div>

                </div>

                <div class="rx-security-tips">

                    <div class="rx-security-tip">

                        <span>01</span>

                        <div>
                            <strong>Use a strong password</strong>

                            <p>
                                Use a password that is difficult for
                                others to guess.
                            </p>
                        </div>

                    </div>

                    <div class="rx-security-tip">

                        <span>02</span>

                        <div>
                            <strong>Keep your credentials private</strong>

                            <p>
                                Never share your account password with
                                another person.
                            </p>
                        </div>

                    </div>

                    <div class="rx-security-tip">

                        <span>03</span>

                        <div>
                            <strong>Review your profile regularly</strong>

                            <p>
                                Make sure your personal information
                                remains accurate.
                            </p>
                        </div>

                    </div>

                </div>

            </div>

        </div>


        <!-- Danger Zone -->
        <div class="rx-security-danger-zone">

            <div class="rx-security-danger-content">

                <div class="rx-security-danger-icon">
                    !
                </div>

                <div>
                    <h3>Account Security</h3>

                    <p>
                        If you believe someone else has accessed your
                        account, change your password immediately.
                    </p>
                </div>

            </div>

            <a href="ChangePassword.aspx"
               class="rx-security-danger-button">
                Change Password
            </a>

        </div>

    </div>

</asp:Content>