<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Register.aspx.cs"
    Inherits="RxEvents.Register" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-auth-page">

        <div class="rx-auth-card">

            <!-- Header -->
            <div class="rx-auth-header">

                <div class="rx-auth-label">
                    <span></span>
                    CREATE ACCOUNT
                    <span></span>
                </div>

                <h1>Register</h1>

                <p>
                    Create your account to get started with
                    managing your events and bookings.
                </p>

            </div>


            <!-- Validation Summary -->
            <asp:ValidationSummary
                ID="RegisterValidationSummary"
                runat="server"
                CssClass="rx-validation-summary"
                HeaderText="Please correct the following:"
                EnableClientScript="false" />


            <!-- Registration Form -->
            <div class="rx-auth-form">

                <!-- First Name + Last Name -->
                <div class="rx-auth-row">

                    <div class="rx-auth-field">

                        <label>First Name</label>

                        <asp:TextBox
                            ID="FirstNameTextBox"
                            runat="server"
                            CssClass="rx-auth-input"
                            placeholder="Enter your first name">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="FirstNameValidator"
                            runat="server"
                            ControlToValidate="FirstNameTextBox"
                            ErrorMessage="First name is required."
                            Display="Dynamic"
                            CssClass="rx-validation-error"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>


                    <div class="rx-auth-field">

                        <label>Last Name</label>

                        <asp:TextBox
                            ID="LastNameTextBox"
                            runat="server"
                            CssClass="rx-auth-input"
                            placeholder="Enter your last name">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="LastNameValidator"
                            runat="server"
                            ControlToValidate="LastNameTextBox"
                            ErrorMessage="Last name is required."
                            Display="Dynamic"
                            CssClass="rx-validation-error"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>

                </div>


                <!-- Email + Phone -->
                <div class="rx-auth-row">

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


                    <div class="rx-auth-field">

                        <label>Phone Number</label>

                        <asp:TextBox
                            ID="PhoneTextBox"
                            runat="server"
                            CssClass="rx-auth-input"
                            placeholder="Enter your phone number">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="PhoneRequiredValidator"
                            runat="server"
                            ControlToValidate="PhoneTextBox"
                            ErrorMessage="Phone number is required."
                            Display="Dynamic"
                            CssClass="rx-validation-error"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator
                            ID="PhoneFormatValidator"
                            runat="server"
                            ControlToValidate="PhoneTextBox"
                            ValidationExpression="^[0-9+\-\s]{7,20}$"
                            ErrorMessage="Please enter a valid phone number."
                            Display="Dynamic"
                            CssClass="rx-validation-error"
                            EnableClientScript="false">
                        </asp:RegularExpressionValidator>

                    </div>

                </div>


                <!-- Password + Confirm Password -->
                <div class="rx-auth-row">

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

                        <asp:RegularExpressionValidator
                            ID="PasswordFormatValidator"
                            runat="server"
                            ControlToValidate="PasswordTextBox"
                            ValidationExpression="^.{8,}$"
                            ErrorMessage="Password must contain at least 8 characters."
                            Display="Dynamic"
                            CssClass="rx-validation-error"
                            EnableClientScript="false">
                        </asp:RegularExpressionValidator>

                    </div>


                    <div class="rx-auth-field">

                        <label>Confirm Password</label>

                        <asp:TextBox
                            ID="ConfirmPasswordTextBox"
                            runat="server"
                            CssClass="rx-auth-input"
                            placeholder="Confirm your password"
                            TextMode="Password">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="ConfirmPasswordRequiredValidator"
                            runat="server"
                            ControlToValidate="ConfirmPasswordTextBox"
                            ErrorMessage="Please confirm your password."
                            Display="Dynamic"
                            CssClass="rx-validation-error"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CompareValidator
                            ID="PasswordCompareValidator"
                            runat="server"
                            ControlToValidate="ConfirmPasswordTextBox"
                            ControlToCompare="PasswordTextBox"
                            Operator="Equal"
                            Type="String"
                            ErrorMessage="Passwords do not match."
                            Display="Dynamic"
                            CssClass="rx-validation-error"
                            EnableClientScript="false">
                        </asp:CompareValidator>

                    </div>

                </div>


                <!-- Register Button -->
                <asp:Button
                    ID="RegisterButton"
                    runat="server"
                    Text="Register"
                    CssClass="rx-auth-button"
                    OnClick="RegisterButton_Click" />


                <!-- Server Message -->
                <asp:Label
                    ID="MessageLabel"
                    runat="server"
                    CssClass="rx-auth-message">
                </asp:Label>


                <!-- Login Link -->
                <div class="rx-auth-footer">

                    Already have an account?

                    <a href="Login.aspx">
                        Login here
                    </a>

                </div>

            </div>

        </div>

    </section>

</asp:Content>