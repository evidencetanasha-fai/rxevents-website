<%@ Page Title="Add Gallery Image" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminAddGallery.aspx.cs"
    Inherits="RxEvents.AdminAddGallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="add-gallery-page">

        <div class="add-gallery-header">
            <div>
                <h1>
                    <asp:Literal ID="PageHeadingLiteral" runat="server"
                        Text="Add Gallery Image" />
                </h1>
                <p>Upload and manage images for your events.</p>
            </div>

            <asp:HyperLink ID="BackToGalleryLink" runat="server"
                NavigateUrl="~/AdminGallery.aspx"
                CssClass="add-gallery-back">
                Back to Gallery
            </asp:HyperLink>
        </div>

        <asp:Label ID="GalleryFormMessage" runat="server"
            CssClass="add-gallery-message"
            EnableViewState="false" />

        <div class="add-gallery-card">

            <div class="add-gallery-section-title">
                <h2>Image Information</h2>
                <p>Choose an event and provide the image details.</p>
            </div>

            <div class="add-gallery-form-grid">

                <div class="add-gallery-field">
                    <asp:Label ID="GalleryEventLabel" runat="server"
                        AssociatedControlID="GalleryEventDropdown"
                        Text="Event *" />

                    <asp:DropDownList ID="GalleryEventDropdown" runat="server"
                        CssClass="add-gallery-input">
                        <asp:ListItem Text="Select an event" Value="" />
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="GalleryEventValidator"
                        runat="server"
                        ControlToValidate="GalleryEventDropdown"
                        InitialValue=""
                        ErrorMessage="Please select an event."
                        CssClass="add-gallery-error"
                        Display="Dynamic" />
                </div>

                <div class="add-gallery-field">
                    <asp:Label ID="GalleryCaptionLabel" runat="server"
                        AssociatedControlID="GalleryCaptionInput"
                        Text="Image Caption" />

                    <asp:TextBox ID="GalleryCaptionInput" runat="server"
                        CssClass="add-gallery-input"
                        MaxLength="255"
                        placeholder="Enter a short caption" />
                </div>

                <div class="add-gallery-field add-gallery-full-width">
                    <asp:Label ID="GalleryImageLabel" runat="server"
                        AssociatedControlID="GalleryImageUpload"
                        Text="Choose Image *" />

                    <asp:FileUpload ID="GalleryImageUpload" runat="server"
                        CssClass="add-gallery-file" />

                    <asp:Label ID="GalleryImageHelp" runat="server"
                        CssClass="add-gallery-help"
                        Text="Allowed formats: JPG, JPEG, PNG, and WEBP. Maximum size: 5 MB." />

                    <asp:CustomValidator ID="GalleryImageValidator" runat="server"
                        ErrorMessage="Please select a valid image."
                        CssClass="add-gallery-error"
                        Display="Dynamic"
                        OnServerValidate="GalleryImageValidator_ServerValidate"
                        ValidationGroup="GalleryForm" />
                </div>

                <div class="add-gallery-field">
                    <asp:Label ID="GalleryStatusLabel" runat="server"
                        AssociatedControlID="GalleryStatusDropdown"
                        Text="Image Status" />

                    <asp:DropDownList ID="GalleryStatusDropdown" runat="server"
                        CssClass="add-gallery-input">
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>

            </div>

            <div class="add-gallery-actions">

                <asp:HyperLink ID="CancelGalleryLink" runat="server"
                    NavigateUrl="~/AdminGallery.aspx"
                    CssClass="add-gallery-cancel">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="SaveGalleryButton" runat="server"
                    Text="Save Image"
                    CssClass="add-gallery-save"
                    ValidationGroup="GalleryForm"
                    OnClick="SaveGalleryButton_Click" />

            </div>

        </div>

    </div>

</asp:Content>