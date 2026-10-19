<%@ Page Title="Gallery" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminGallery.aspx.cs"
    Inherits="RxEvents.AdminGallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="gallery-page">

        <div class="gallery-header">
            <div>
                <h1>Gallery</h1>
                <p>Dashboard &gt; Gallery</p>
            </div>

            <div class="gallery-header-actions">
                <asp:Button ID="ExportGalleryButton" runat="server"
                    Text="↓ Export"
                    CssClass="gallery-export-button"
                    OnClick="ExportGalleryButton_Click" />

                <asp:HyperLink ID="AddGalleryLink" runat="server"
                    NavigateUrl="~/AdminAddGallery.aspx"
                    CssClass="gallery-add-button">
                    + Add New Image
                </asp:HyperLink>
            </div>
        </div>

        <asp:Label ID="GalleryMessage" runat="server"
            CssClass="gallery-message" EnableViewState="false" />

        <div class="gallery-filter-card">

            <div class="gallery-filter-field gallery-search-field">
                <asp:Label ID="GallerySearchLabel" runat="server"
                    AssociatedControlID="GallerySearchInput"
                    Text="Search Gallery" />

                <asp:TextBox ID="GallerySearchInput" runat="server"
                    CssClass="gallery-input"
                    placeholder="Search images, event names or descriptions..." />
            </div>

            <div class="gallery-filter-field">
                <asp:Label ID="GalleryEventLabel" runat="server"
                    AssociatedControlID="GalleryEventFilter"
                    Text="Event / Service" />

                <asp:DropDownList ID="GalleryEventFilter" runat="server"
                    CssClass="gallery-input">
                    <asp:ListItem Text="All Events" Value="" />
                </asp:DropDownList>
            </div>

            <div class="gallery-filter-field">
                <asp:Label ID="GalleryCategoryLabel" runat="server"
                    AssociatedControlID="GalleryCategoryFilter"
                    Text="Category" />

                <asp:DropDownList ID="GalleryCategoryFilter" runat="server"
                    CssClass="gallery-input">
                    <asp:ListItem Text="All Categories" Value="" />
                </asp:DropDownList>
            </div>

            <div class="gallery-filter-field">
                <asp:Label ID="GalleryStatusLabel" runat="server"
                    AssociatedControlID="GalleryStatusFilter"
                    Text="Status" />

                <asp:DropDownList ID="GalleryStatusFilter" runat="server"
                    CssClass="gallery-input">
                    <asp:ListItem Text="All Statuses" Value="" />
                    <asp:ListItem Text="Active" Value="1" />
                    <asp:ListItem Text="Inactive" Value="0" />
                </asp:DropDownList>
            </div>

            <div class="gallery-filter-actions">
                <asp:Button ID="ApplyGalleryFiltersButton" runat="server"
                    Text="Apply Filters"
                    CssClass="gallery-filter-button"
                    OnClick="ApplyGalleryFiltersButton_Click" />

                <asp:Button ID="ClearGalleryFiltersButton" runat="server"
                    Text="↻ Clear Filters"
                    CssClass="gallery-clear-button"
                    CausesValidation="false"
                    OnClick="ClearGalleryFiltersButton_Click" />
            </div>

        </div>

        <div class="gallery-grid">

            <asp:Repeater ID="GalleryRepeater" runat="server"
                OnItemCommand="GalleryRepeater_ItemCommand">

                <ItemTemplate>
                    <div class="gallery-card">

                        <div class="gallery-image-container">
                            <asp:Image ID="GalleryImage" runat="server"
                                ImageUrl='<%# GetGalleryImageUrl(Eval("ImagePath")) %>'
                                AlternateText='<%# Eval("Title") %>'
                                CssClass="gallery-image" />

                            <span class='<%# Convert.ToBoolean(Eval("IsActive"))
                                ? "gallery-status gallery-active"
                                : "gallery-status gallery-inactive" %>'>
                                <%# Convert.ToBoolean(Eval("IsActive"))
                                    ? "Active" : "Inactive" %>
                            </span>
                        </div>

                        <div class="gallery-card-body">
                            <h3><%#: Eval("Title") %></h3>

                            <p class="gallery-event-name">
                                <%#: Eval("EventName") %>
                            </p>

                            <p class="gallery-description">
                                <%#: Eval("Description") %>
                            </p>

                            <div class="gallery-card-actions">

                                <asp:LinkButton ID="ViewGalleryButton" runat="server"
                                    Text="View"
                                    CssClass="gallery-action gallery-view"
                                    CommandName="ViewImage"
                                    CommandArgument='<%# Eval("GalleryID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="EditGalleryButton" runat="server"
                                    Text="Edit"
                                    CssClass="gallery-action gallery-edit"
                                    CommandName="EditImage"
                                    CommandArgument='<%# Eval("GalleryID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="ToggleGalleryButton" runat="server"
                                    Text='<%# Convert.ToBoolean(Eval("IsActive"))
                                        ? "Deactivate" : "Activate" %>'
                                    CssClass="gallery-action gallery-toggle"
                                    CommandName="ToggleImage"
                                    CommandArgument='<%# Eval("GalleryID") %>'
                                    CausesValidation="false" />

                            </div>
                        </div>

                    </div>
                </ItemTemplate>

            </asp:Repeater>

        </div>

        <asp:Label ID="GalleryEmptyLabel" runat="server"
            CssClass="gallery-empty-message"
            Text="No gallery images match your filters."
            Visible="false" />

        <div class="gallery-pagination">

            <asp:Label ID="GalleryPaginationSummary" runat="server"
                Text="Showing 0 images" />

            <div class="gallery-pagination-buttons">

                <asp:Button ID="PreviousGalleryPageButton" runat="server"
                    Text="‹"
                    CssClass="gallery-page-button"
                    CausesValidation="false"
                    OnClick="PreviousGalleryPageButton_Click" />

                <asp:Repeater ID="GalleryPaginationRepeater" runat="server"
                    OnItemCommand="GalleryPaginationRepeater_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="GalleryPageButton" runat="server"
                            Text='<%# Eval("PageNumber") %>'
                            CommandName="GoToPage"
                            CommandArgument='<%# Eval("PageNumber") %>'
                            CssClass='<%# Convert.ToInt32(Eval("PageNumber")) == CurrentGalleryPage
                                ? "gallery-page-button gallery-page-active"
                                : "gallery-page-button" %>'
                            CausesValidation="false" />
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Button ID="NextGalleryPageButton" runat="server"
                    Text="›"
                    CssClass="gallery-page-button"
                    CausesValidation="false"
                    OnClick="NextGalleryPageButton_Click" />

            </div>
        </div>

    </div>

</asp:Content>