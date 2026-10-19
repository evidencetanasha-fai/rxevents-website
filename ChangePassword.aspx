<%@ Page Title="Change Password"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs"
    Inherits="RxEvents.ChangePassword" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">

    Change Password

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-change-password-page">

        <!-- ================= BREADCRUMB ================= -->

        <div class="rx-change-breadcrumb">

            <a href="CustomerDashboard.aspx">
                Home
            </a>

            <span>›</span>

            <a href="Profile.aspx">
                Profile
            </a>

            <span>›</span>

            <span>Change Password</span>

        </div>


        <!-- ================= PAGE HEADER ================= -->

        <div class="rx-change-header">

            <div>

                <span class="rx-change-eyebrow">
                    ACCOUNT SECURITY
                </span>

                <h2>
                    Change Password
                </h2>

                <p>
                    Keep your account secure by updating your password regularly.
                </p>

            </div>

        </div>


        <!-- ================= MAIN GRID ================= -->

        <div class="rx-change-grid">


            <!-- ================= PASSWORD FORM ================= -->

            <div class="rx-change-card">

                <div class="rx-change-card-heading">

                    <div class="rx-change-lock">
                        🔒
                    </div>

                    <div>

                        <h3>
                            Update Your Password
                        </h3>

                        <p>
                            Enter your current password and choose a new one.
                        </p>

                    </div>

                </div>


                <div class="rx-change-form">


                    <!-- CURRENT PASSWORD -->

                    <div class="rx-change-field">

                        <label>
                            Current Password
                            <span>*</span>
                        </label>

                        <asp:TextBox
                            ID="CurrentPasswordTextBox"
                            runat="server"
                            TextMode="Password"
                            CssClass="rx-change-input"
                            placeholder="Enter your current password">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="CurrentPasswordRequired"
                            runat="server"
                            ControlToValidate="CurrentPasswordTextBox"
                            ErrorMessage="Current password is required."
                            Display="Dynamic"
                            CssClass="rx-change-validation"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>


                    <!-- NEW PASSWORD -->

                    <div class="rx-change-field">

                        <label>
                            New Password
                            <span>*</span>
                        </label>

                        <asp:TextBox
                            ID="NewPasswordTextBox"
                            runat="server"
                            TextMode="Password"
                            CssClass="rx-change-input"
                            placeholder="Enter your new password">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="NewPasswordRequired"
                            runat="server"
                            ControlToValidate="NewPasswordTextBox"
                            ErrorMessage="New password is required."
                            Display="Dynamic"
                            CssClass="rx-change-validation"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator
                            ID="NewPasswordLengthValidator"
                            runat="server"
                            ControlToValidate="NewPasswordTextBox"
                            ValidationExpression="^.{8,}$"
                            ErrorMessage="Password must be at least 8 characters."
                            Display="Dynamic"
                            CssClass="rx-change-validation"
                            EnableClientScript="false">
                        </asp:RegularExpressionValidator>

                    </div>


                    <!-- CONFIRM PASSWORD -->

                    <div class="rx-change-field">

                        <label>
                            Confirm New Password
                            <span>*</span>
                        </label>

                        <asp:TextBox
                            ID="ConfirmPasswordTextBox"
                            runat="server"
                            TextMode="Password"
                            CssClass="rx-change-input"
                            placeholder="Confirm your new password">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="ConfirmPasswordRequired"
                            runat="server"
                            ControlToValidate="ConfirmPasswordTextBox"
                            ErrorMessage="Please confirm your new password."
                            Display="Dynamic"
                            CssClass="rx-change-validation"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>


                    <!-- ================= PASSWORD REQUIREMENTS ================= -->

                    <div class="rx-password-requirements">

                        <div class="rx-requirements-icon">
                            i
                        </div>

                        <div>

                            <strong>
                                Password requirements
                            </strong>

                            <ul>

                                <li>
                                    At least 8 characters
                                </li>

                                <li>
                                    Use a password you do not use elsewhere
                                </li>

                                <li>
                                    Do not share your password with anyone
                                </li>

                            </ul>

                        </div>

                    </div>


                    <!-- ================= MESSAGE ================= -->

                    <asp:Label
                        ID="MessageLabel"
                        runat="server"
                        CssClass="rx-change-message">
                    </asp:Label>


                    <!-- ================= BUTTONS ================= -->

                    <div class="rx-change-actions">

                        <asp:Button
                            ID="CancelButton"
                            runat="server"
                            Text="Cancel"
                            CssClass="rx-change-btn rx-change-btn-secondary"
                            PostBackUrl="~/Profile.aspx" />

                        <asp:Button
                            ID="ChangePasswordButton"
                            runat="server"
                            Text="Change Password"
                            CssClass="rx-change-btn rx-change-btn-primary"
                            OnClick="ChangePasswordButton_Click" />

                    </div>

                </div>

            </div>


            <!-- ================= SECURITY CARD ================= -->

            <div class="rx-security-card">

                <div class="rx-security-top-icon">
                    🛡
                </div>

                <h3>
                    Your Security Matters
                </h3>

                <p class="rx-security-intro">
                    We take your security seriously. Follow these tips
                    to keep your account safe.
                </p>


                <!-- Security Item -->

                <div class="rx-security-item">

                    <div class="rx-security-icon">
                        🔒
                    </div>

                    <div>

                        <strong>
                            Secure Password Storage
                        </strong>

                        <p>
                            Your password is securely stored and never
                            saved as plain text.
                        </p>

                    </div>

                </div>


                <!-- Security Item -->

                <div class="rx-security-item">

                    <div class="rx-security-icon">
                        ✓
                    </div>

                    <div>

                        <strong>
                            Verification Required
                        </strong>

                        <p>
                            Your current password must be verified
                            before it can be changed.
                        </p>

                    </div>

                </div>


                <!-- Security Item -->

                <div class="rx-security-item">

                    <div class="rx-security-icon">
                        ♡
                    </div>

                    <div>

                        <strong>
                            Stay Protected
                        </strong>

                        <p>
                            Use a strong, unique password to keep
                            your account secure.
                        </p>

                    </div>

                </div>


                <!-- Quote -->

                <div class="rx-security-quote">

                    <span>
                        A safer you
                    </span>

                    <strong>
                        for more memorable moments.
                    </strong>

                    <div></div>

                </div>

            </div>

        </div>

    </div>

</asp:Content>