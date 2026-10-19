<%@ Page Title="Add Customer" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminAddCustomer.aspx.cs"
    Inherits="RxEvents.AdminAddCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="add-customer-page">

        <div class="add-customer-header">
            <div>
                <h1>
                    <asp:Literal ID="PageHeadingLiteral" runat="server"
                        Text="Add New Customer" />
                </h1>
                <p>Create and manage customer accounts.</p>
            </div>

            <asp:HyperLink ID="BackToCustomersLink" runat="server"
                NavigateUrl="~/AdminCustomers.aspx"
                CssClass="add-customer-back">
                Back to Customers
            </asp:HyperLink>
        </div>

        <asp:Label ID="CustomerMessage" runat="server"
            CssClass="add-customer-message" EnableViewState="false" />

        <div class="add-customer-card">

            <div class="add-customer-section-title">
                <h2>Customer Information</h2>
                <p>Enter the customer's personal and account details.</p>
            </div>

            <div class="add-customer-form-grid">

                <div class="add-customer-field">
                    <asp:Label ID="FirstNameLabel" runat="server"
                        AssociatedControlID="FirstNameInput"
                        Text="First Name *" />

                    <asp:TextBox ID="FirstNameInput" runat="server"
                        CssClass="add-customer-input"
                        MaxLength="50"
                        placeholder="Enter first name" />

                    <asp:RequiredFieldValidator ID="FirstNameValidator"
                        runat="server"
                        ControlToValidate="FirstNameInput"
                        ErrorMessage="First name is required."
                        CssClass="add-customer-error"
                        Display="Dynamic" />
                </div>

                <div class="add-customer-field">
                    <asp:Label ID="LastNameLabel" runat="server"
                        AssociatedControlID="LastNameInput"
                        Text="Last Name *" />

                    <asp:TextBox ID="LastNameInput" runat="server"
                        CssClass="add-customer-input"
                        MaxLength="50"
                        placeholder="Enter last name" />

                    <asp:RequiredFieldValidator ID="LastNameValidator"
                        runat="server"
                        ControlToValidate="LastNameInput"
                        ErrorMessage="Last name is required."
                        CssClass="add-customer-error"
                        Display="Dynamic" />
                </div>

                <div class="add-customer-field">
                    <asp:Label ID="EmailLabel" runat="server"
                        AssociatedControlID="EmailInput"
                        Text="Email Address *" />

                    <asp:TextBox ID="EmailInput" runat="server"
                        CssClass="add-customer-input"
                        TextMode="Email"
                        MaxLength="100"
                        placeholder="customer@example.com" />

                    <asp:RequiredFieldValidator ID="EmailRequiredValidator"
                        runat="server"
                        ControlToValidate="EmailInput"
                        ErrorMessage="Email address is required."
                        CssClass="add-customer-error"
                        Display="Dynamic" />

                    <asp:RegularExpressionValidator ID="EmailFormatValidator"
                        runat="server"
                        ControlToValidate="EmailInput"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Enter a valid email address."
                        CssClass="add-customer-error"
                        Display="Dynamic" />
                </div>

                <div class="add-customer-field">
                    <asp:Label ID="PhoneLabel" runat="server"
                        AssociatedControlID="PhoneInput"
                        Text="Phone Number" />

                    <asp:TextBox ID="PhoneInput" runat="server"
                        CssClass="add-customer-input"
                        MaxLength="20"
                        placeholder="Enter phone number" />
                </div>

                <asp:Panel ID="PasswordPanel" runat="server"
                    CssClass="add-customer-field">

                    <asp:Label ID="PasswordLabel" runat="server"
                        AssociatedControlID="PasswordInput"
                        Text="Temporary Password *" />

                    <asp:TextBox ID="PasswordInput" runat="server"
                        CssClass="add-customer-input"
                        TextMode="Password"
                        MaxLength="100"
                        autocomplete="new-password"
                        placeholder="Enter a temporary password" />

                    <asp:RequiredFieldValidator ID="PasswordRequiredValidator"
                        runat="server"
                        ControlToValidate="PasswordInput"
                        ErrorMessage="A temporary password is required."
                        CssClass="add-customer-error"
                        Display="Dynamic" />

                    <asp:RegularExpressionValidator ID="PasswordStrengthValidator"
                        runat="server"
                        ControlToValidate="PasswordInput"
                        ValidationExpression="^(?=.*[A-Za-z])(?=.*\d).{8,100}$"
                        ErrorMessage="Use at least 8 characters with a letter and a number."
                        CssClass="add-customer-error"
                        Display="Dynamic" />

                </asp:Panel>

                <div class="add-customer-field">
                    <asp:Label ID="StatusLabel" runat="server"
                        AssociatedControlID="CustomerStatusDropdown"
                        Text="Account Status" />

                    <asp:DropDownList ID="CustomerStatusDropdown" runat="server"
                        CssClass="add-customer-input">
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>

                <div class="add-customer-field add-customer-full">
                    <asp:Label ID="ProfilePhotoLabel" runat="server"
                        AssociatedControlID="ProfilePhotoInput"
                        Text="Profile Photo" />

                    <asp:FileUpload ID="ProfilePhotoInput" runat="server"
                        CssClass="add-customer-input" />

                    <p class="add-customer-help">
                        Accepted formats: JPG, JPEG, PNG and WEBP.
                        Maximum file size: 5 MB. Leave empty when editing to keep the existing photo.
                    </p>
                </div>

            </div>

            <div class="add-customer-actions">

                <asp:HyperLink ID="CancelCustomerLink" runat="server"
                    NavigateUrl="~/AdminCustomers.aspx"
                    CssClass="add-customer-cancel">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="SaveCustomerButton" runat="server"
                    Text="Save Customer"
                    CssClass="add-customer-save"
                    OnClick="SaveCustomerButton_Click" />

            </div>

        </div>

    </div>

</asp:Content>