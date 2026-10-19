<%@ Page Title="Add Service" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminAddService.aspx.cs"
    Inherits="RxEvents.AdminAddService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="add-service-page">

        <div class="add-service-header">
            <div>
                <h1>
                    <asp:Literal ID="PageHeadingLiteral" runat="server"
                        Text="Add New Service" />
                </h1>
                <p>Create and manage the services offered to your customers.</p>
            </div>

            <asp:HyperLink ID="BackToServicesLink" runat="server"
                NavigateUrl="~/AdminServices.aspx"
                CssClass="add-service-back">
                Back to Services
            </asp:HyperLink>
        </div>

        <asp:Label ID="ServiceMessage" runat="server"
            CssClass="add-service-message" EnableViewState="false" />

        <div class="add-service-card">

            <div class="add-service-section-title">
                <h2>Service Information</h2>
                <p>Enter the details of the service below.</p>
            </div>

            <div class="add-service-form-grid">

                <div class="add-service-field">
                    <asp:Label ID="ServiceNameLabel" runat="server"
                        AssociatedControlID="ServiceNameInput"
                        Text="Service Name *" />

                    <asp:TextBox ID="ServiceNameInput" runat="server"
                        CssClass="add-service-input"
                        MaxLength="150"
                        placeholder="e.g. Catering Service" />

                    <asp:RequiredFieldValidator ID="ServiceNameValidator"
                        runat="server"
                        ControlToValidate="ServiceNameInput"
                        ErrorMessage="Service name is required."
                        CssClass="add-service-error"
                        Display="Dynamic" />
                </div>

                <div class="add-service-field">
                    <asp:Label ID="ServiceCategoryLabel" runat="server"
                        AssociatedControlID="ServiceCategoryDropdown"
                        Text="Service Category" />

                    <asp:DropDownList ID="ServiceCategoryDropdown" runat="server"
                        CssClass="add-service-input">
                        <asp:ListItem Text="Select a category" Value="" />
                        <asp:ListItem Text="Food &amp; Drinks" Value="Food & Drinks" />
                        <asp:ListItem Text="Decorations" Value="Decorations" />
                        <asp:ListItem Text="Photography" Value="Photography" />
                        <asp:ListItem Text="Entertainment" Value="Entertainment" />
                        <asp:ListItem Text="Event Planning" Value="Event Planning" />
                        <asp:ListItem Text="Event Coordination" Value="Event Coordination" />
                        <asp:ListItem Text="Others" Value="Others" />
                    </asp:DropDownList>
                </div>

                <div class="add-service-field add-service-full">
                    <asp:Label ID="ServiceDescriptionLabel" runat="server"
                        AssociatedControlID="ServiceDescriptionInput"
                        Text="Description" />

                    <asp:TextBox ID="ServiceDescriptionInput" runat="server"
                        CssClass="add-service-input"
                        TextMode="MultiLine"
                        Rows="5"
                        MaxLength="500"
                        placeholder="Describe the service and what customers can expect..." />
                </div>

                <div class="add-service-field">
                    <asp:Label ID="ServicePriceLabel" runat="server"
                        AssociatedControlID="ServicePriceInput"
                        Text="Service Price *" />

                    <asp:TextBox ID="ServicePriceInput" runat="server"
                        CssClass="add-service-input"
                        TextMode="Number"
                        placeholder="Enter service price" />

                    <asp:RequiredFieldValidator ID="ServicePriceRequiredValidator"
                        runat="server"
                        ControlToValidate="ServicePriceInput"
                        ErrorMessage="Service price is required."
                        CssClass="add-service-error"
                        Display="Dynamic" />

                    <asp:RangeValidator ID="ServicePriceRangeValidator"
                        runat="server"
                        ControlToValidate="ServicePriceInput"
                        Type="Double"
                        MinimumValue="0"
                        MaximumValue="9999999999"
                        ErrorMessage="Enter a valid non-negative price."
                        CssClass="add-service-error"
                        Display="Dynamic" />
                </div>

                <div class="add-service-field">
                    <asp:Label ID="ServiceStatusLabel" runat="server"
                        AssociatedControlID="ServiceStatusDropdown"
                        Text="Service Status" />

                    <asp:DropDownList ID="ServiceStatusDropdown" runat="server"
                        CssClass="add-service-input">
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>

                <div class="add-service-field add-service-full">
                    <asp:Label ID="ServiceImageLabel" runat="server"
                        AssociatedControlID="ServiceImageInput"
                        Text="Service Image" />

                    <asp:FileUpload ID="ServiceImageInput" runat="server"
                        CssClass="add-service-input" />

                    <p class="add-service-help">
                        Accepted formats: JPG, JPEG, PNG and WEBP. Maximum file size: 5 MB.
                        When editing, leave this field empty to retain the current image.
                    </p>
                </div>

            </div>

            <div class="add-service-actions">

                <asp:HyperLink ID="CancelServiceLink" runat="server"
                    NavigateUrl="~/AdminServices.aspx"
                    CssClass="add-service-cancel">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="SaveServiceButton" runat="server"
                    Text="Save Service"
                    CssClass="add-service-save"
                    OnClick="SaveServiceButton_Click" />

            </div>

        </div>

    </div>

</asp:Content>