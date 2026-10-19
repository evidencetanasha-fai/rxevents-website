<%@ Page Title="Add Staff" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminAddStaff.aspx.cs"
    Inherits="RxEvents.AdminAddStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="add-staff-page">

        <div class="add-staff-header">
            <div>
                <h1>
                    <asp:Literal ID="PageHeadingLiteral" runat="server"
                        Text="Add New Staff" />
                </h1>
                <p>Manage staff members and their employment details.</p>
            </div>

            <asp:HyperLink ID="BackToStaffLink" runat="server"
                NavigateUrl="~/AdminStaff.aspx"
                CssClass="add-staff-back">
                Back to Staff
            </asp:HyperLink>
        </div>

        <asp:Label ID="StaffMessage" runat="server"
            CssClass="add-staff-message" EnableViewState="false" />

        <div class="add-staff-card">

            <div class="add-staff-section-title">
                <h2>Staff Information</h2>
                <p>Enter the staff member's personal and employment details.</p>
            </div>

            <div class="add-staff-form-grid">

                <div class="add-staff-field">
                    <asp:Label ID="FirstNameLabel" runat="server"
                        AssociatedControlID="FirstNameInput"
                        Text="First Name *" />

                    <asp:TextBox ID="FirstNameInput" runat="server"
                        CssClass="add-staff-input"
                        MaxLength="50"
                        placeholder="Enter first name" />

                    <asp:RequiredFieldValidator ID="FirstNameValidator"
                        runat="server"
                        ControlToValidate="FirstNameInput"
                        ErrorMessage="First name is required."
                        CssClass="add-staff-error"
                        Display="Dynamic" />
                </div>

                <div class="add-staff-field">
                    <asp:Label ID="LastNameLabel" runat="server"
                        AssociatedControlID="LastNameInput"
                        Text="Last Name *" />

                    <asp:TextBox ID="LastNameInput" runat="server"
                        CssClass="add-staff-input"
                        MaxLength="50"
                        placeholder="Enter last name" />

                    <asp:RequiredFieldValidator ID="LastNameValidator"
                        runat="server"
                        ControlToValidate="LastNameInput"
                        ErrorMessage="Last name is required."
                        CssClass="add-staff-error"
                        Display="Dynamic" />
                </div>

                <div class="add-staff-field">
                    <asp:Label ID="StaffEmailLabel" runat="server"
                        AssociatedControlID="StaffEmailInput"
                        Text="Email Address" />

                    <asp:TextBox ID="StaffEmailInput" runat="server"
                        CssClass="add-staff-input"
                        TextMode="Email"
                        MaxLength="100"
                        placeholder="staff@example.com" />

                    <asp:RegularExpressionValidator ID="StaffEmailValidator"
                        runat="server"
                        ControlToValidate="StaffEmailInput"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Enter a valid email address."
                        CssClass="add-staff-error"
                        Display="Dynamic" />
                </div>

                <div class="add-staff-field">
                    <asp:Label ID="StaffPhoneLabel" runat="server"
                        AssociatedControlID="StaffPhoneInput"
                        Text="Phone Number" />

                    <asp:TextBox ID="StaffPhoneInput" runat="server"
                        CssClass="add-staff-input"
                        MaxLength="20"
                        placeholder="Enter phone number" />
                </div>

                <div class="add-staff-field">
                    <asp:Label ID="StaffPositionLabel" runat="server"
                        AssociatedControlID="StaffPositionDropdown"
                        Text="Position / Role" />

                    <asp:DropDownList ID="StaffPositionDropdown" runat="server"
                        CssClass="add-staff-input">
                        <asp:ListItem Text="Select Position" Value="" />
                        <asp:ListItem Text="Event Manager" Value="Event Manager" />
                        <asp:ListItem Text="Operations Head" Value="Operations Head" />
                        <asp:ListItem Text="Event Planner" Value="Event Planner" />
                        <asp:ListItem Text="Accountant" Value="Accountant" />
                        <asp:ListItem Text="Marketing Officer" Value="Marketing Officer" />
                        <asp:ListItem Text="Logistics Coordinator" Value="Logistics Coordinator" />
                        <asp:ListItem Text="Customer Support" Value="Customer Support" />
                        <asp:ListItem Text="Technical Lead" Value="Technical Lead" />
                        <asp:ListItem Text="Other" Value="Other" />
                    </asp:DropDownList>
                </div>

                <div class="add-staff-field">
                    <asp:Label ID="HireDateLabel" runat="server"
                        AssociatedControlID="HireDateInput"
                        Text="Hire Date" />

                    <asp:TextBox ID="HireDateInput" runat="server"
                        CssClass="add-staff-input"
                        TextMode="Date" />
                </div>

                <div class="add-staff-field">
                    <asp:Label ID="StaffStatusLabel" runat="server"
                        AssociatedControlID="StaffStatusDropdown"
                        Text="Employment Status" />

                    <asp:DropDownList ID="StaffStatusDropdown" runat="server"
                        CssClass="add-staff-input">
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>

            </div>

            <div class="add-staff-actions">

                <asp:HyperLink ID="CancelStaffLink" runat="server"
                    NavigateUrl="~/AdminStaff.aspx"
                    CssClass="add-staff-cancel">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="SaveStaffButton" runat="server"
                    Text="Save Staff"
                    CssClass="add-staff-save"
                    OnClick="SaveStaffButton_Click" />

            </div>

        </div>

    </div>

</asp:Content>