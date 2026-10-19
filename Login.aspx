<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="RxEvents.Login" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-auth-page">

        <div class="rx-login-welcome">
            Welcome back!
        </div>

        <div class="rx-auth-card rx-login-card">

            <!-- Header -->
            <div class="rx-auth-header">

                <div class="rx-auth-label">
                    <span></span>
                    ACCOUNT LOGIN
                    <span></span>
                </div>

                <h1>Login</h1>

                <p>
                    Login to your account to manage
                    your events and bookings.
                </p>

            </div>


            <!-- Login Form -->
            <div class="rx-auth-form">

                <!-- Email -->
                <div class="rx-auth-field">

                    <label>Email Address</label>

                    <asp:TextBox
                        ID="EmailTextBox"
                        runat="server"
                        CssClass="rx-auth-input"
                        placeholder="Enter your email">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="EmailRequiredValidator"
                        runat="server"
                        ControlToValidate="EmailTextBox"
                        ErrorMessage="Email address is required."
                        Display="Dynamic"
                        CssClass="rx-validation-error"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator
                        ID="EmailFormatValidator"
                        runat="server"
                        ControlToValidate="EmailTextBox"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Please enter a valid email address."
                        Display="Dynamic"
                        CssClass="rx-validation-error"
                        EnableClientScript="false">
                    </asp:RegularExpressionValidator>

                </div>


                <!-- Password -->
                <div class="rx-auth-field">

                    <label>Password</label>

                    <asp:TextBox
                        ID="PasswordTextBox"
                        runat="server"
                        CssClass="rx-auth-input"
                        placeholder="Enter your password"
                        TextMode="Password">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="PasswordRequiredValidator"
                        runat="server"
                        ControlToValidate="PasswordTextBox"
                        ErrorMessage="Password is required."
                        Display="Dynamic"
                        CssClass="rx-validation-error"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- Remember Me / Forgot Password -->
                <div class="rx-login-options">

                    <asp:CheckBox
                        ID="RememberMeCheckBox"
                        runat="server"
                        Text="Remember me" />

                    <a href="#">
                        Forgot Password?
                    </a>

                </div>


                <!-- Login Button -->
                <asp:Button
                    ID="LoginButton"
                    runat="server"
                    Text="Login"
                    CssClass="rx-auth-button"
                    OnClick="LoginButton_Click" />


                <!-- Message -->
                <asp:Label
                    ID="MessageLabel"
                    runat="server"
                    CssClass="rx-auth-message">
                </asp:Label>


                <!-- Register -->
                <div class="rx-auth-divider">
                    <span></span>
                    <strong>OR</strong>
                    <span></span>
                </div>


                <div class="rx-auth-footer">

                    Don't have an account?

                    <a href="Register.aspx">
                        Register here
                    </a>

                </div>

            </div>

        </div>

    </section>

</asp:Content>