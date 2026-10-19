<%@ Page Title="Services" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminServices.aspx.cs"
    Inherits="RxEvents.AdminServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="services-page">

        <div class="services-header">
            <div>
                <h1>Services</h1>
                <p>Manage the services offered by Rx Events Solutions.</p>
            </div>

            <div class="services-header-actions">
                <asp:Button ID="ExportServicesButton" runat="server"
                    Text="Export"
                    CssClass="services-export-button"
                    OnClick="ExportServicesButton_Click" />

                <asp:HyperLink ID="AddServiceLink" runat="server"
                    NavigateUrl="~/AdminAddService.aspx"
                    CssClass="services-add-button">
                    + Add New Service
                </asp:HyperLink>
            </div>
        </div>

        <div class="services-statistics">

            <div class="services-stat-card">
                <span class="services-stat-icon purple-icon">▣</span>
                <div>
                    <p>Total Services</p>
                    <h2><asp:Literal ID="TotalServicesValue" runat="server" Text="0" /></h2>
                </div>
            </div>

            <div class="services-stat-card">
                <span class="services-stat-icon gold-icon">◆</span>
                <div>
                    <p>Active Services</p>
                    <h2><asp:Literal ID="ActiveServicesValue" runat="server" Text="0" /></h2>
                </div>
            </div>

            <div class="services-stat-card">
                <span class="services-stat-icon green-icon">●</span>
                <div>
                    <p>Inactive Services</p>
                    <h2><asp:Literal ID="InactiveServicesValue" runat="server" Text="0" /></h2>
                </div>
            </div>

            <div class="services-stat-card">
                <span class="services-stat-icon purple-icon">★</span>
                <div>
                    <p>Most Popular Service</p>
                    <h2><asp:Literal ID="MostPopularServiceValue"
                        runat="server" Text="N/A" /></h2>
                </div>
            </div>

        </div>

        <asp:Label ID="ServicesMessage" runat="server"
            CssClass="services-message" EnableViewState="false" />

        <div class="services-content-layout">

            <div class="services-main-column">

                <div class="services-filter-card">

                    <div class="services-filter-search">
                        <asp:TextBox ID="ServiceSearchTextBox" runat="server"
                            CssClass="services-input"
                            placeholder="Search by service name..." />
                    </div>

                    <div class="services-filter-field">
                        <asp:Label ID="ServiceStatusLabel" runat="server"
                            AssociatedControlID="ServiceStatusFilter"
                            Text="Status" />

                        <asp:DropDownList ID="ServiceStatusFilter" runat="server"
                            CssClass="services-input">
                            <asp:ListItem Text="All Statuses" Value="" />
                            <asp:ListItem Text="Active" Value="1" />
                            <asp:ListItem Text="Inactive" Value="0" />
                        </asp:DropDownList>
                    </div>

                    <div class="services-filter-field">
                        <asp:Label ID="ServiceTypeLabel" runat="server"
                            AssociatedControlID="ServiceTypeFilter"
                            Text="Service Type" />

                        <asp:DropDownList ID="ServiceTypeFilter" runat="server"
                            CssClass="services-input">
                            <asp:ListItem Text="All Types" Value="" />
                            <asp:ListItem Text="Food & Drinks" Value="Food & Drinks" />
                            <asp:ListItem Text="Decorations" Value="Decorations" />
                            <asp:ListItem Text="Photography" Value="Photography" />
                            <asp:ListItem Text="Entertainment" Value="Entertainment" />
                            <asp:ListItem Text="Others" Value="Others" />
                        </asp:DropDownList>
                    </div>

                    <div class="services-filter-actions">
                        <asp:Button ID="ApplyServiceFiltersButton" runat="server"
                            Text="Apply Filters"
                            CssClass="services-filter-button"
                            OnClick="ApplyServiceFiltersButton_Click" />

                        <asp:Button ID="ClearServiceFiltersButton" runat="server"
                            Text="Clear All Filters"
                            CssClass="services-clear-button"
                            CausesValidation="false"
                            OnClick="ClearServiceFiltersButton_Click" />
                    </div>

                </div>

                <div class="services-table-card">

                    <div class="services-table-title">
                        <h2>Services</h2>
                    </div>

                    <asp:GridView ID="ServicesGridView" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="ServiceID"
                        CssClass="services-grid"
                        GridLines="None"
                        OnRowCommand="ServicesGridView_RowCommand"
                        EmptyDataText="No services found.">

                        <Columns>

                            <asp:TemplateField HeaderText="Service">
                                <ItemTemplate>
                                    <div class="services-name-cell">

                                        <asp:Image ID="ServiceImage" runat="server"
                                            CssClass="services-thumbnail"
                                            ImageUrl='<%# GetServiceImageUrl(Eval("ImagePath")) %>'
                                            AlternateText="Service image" />

                                        <div>
                                            <strong><%#: Eval("ServiceName") %></strong>
                                            <p><%#: Eval("Description") %></p>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ServiceType"
                                HeaderText="Category" />

                            <asp:BoundField DataField="BookingCount"
                                HeaderText="Bookings" />

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("IsActive"))
                                        ? "services-status-active"
                                        : "services-status-inactive" %>'>
                                        <%# Convert.ToBoolean(Eval("IsActive"))
                                            ? "Active" : "Inactive" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>

                                    <div class="services-row-actions">

                                        <asp:LinkButton ID="ViewServiceButton" runat="server"
                                            Text="View"
                                            ToolTip="View service"
                                            CommandName="ViewService"
                                            CommandArgument='<%# Eval("ServiceID") %>'
                                            CausesValidation="false" />

                                        <asp:LinkButton ID="EditServiceButton" runat="server"
                                            Text="Edit"
                                            ToolTip="Edit service"
                                            CommandName="EditService"
                                            CommandArgument='<%# Eval("ServiceID") %>'
                                            CausesValidation="false" />

                                        <asp:LinkButton ID="ToggleServiceButton" runat="server"
                                            Text='<%# Convert.ToBoolean(Eval("IsActive"))
                                                ? "Deactivate" : "Activate" %>'
                                            ToolTip="Change service status"
                                            CommandName="ToggleService"
                                            CommandArgument='<%# Eval("ServiceID") %>'
                                            CausesValidation="false"
                                            OnClientClick="return confirm('Change this service status?');" />

                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                    <div class="services-pagination">

                        <asp:Label ID="ServicePaginationSummary" runat="server"
                            Text="Showing 0 services" />

                        <div class="services-pagination-buttons">

                            <asp:Button ID="PreviousServicePageButton" runat="server"
                                Text="‹"
                                CssClass="services-page-button"
                                CausesValidation="false"
                                OnClick="PreviousServicePageButton_Click" />

                            <asp:Repeater ID="ServicePaginationRepeater" runat="server"
                                OnItemCommand="ServicePaginationRepeater_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="ServicePageButton" runat="server"
                                        Text='<%# Eval("PageNumber") %>'
                                        CommandName="GoToPage"
                                        CommandArgument='<%# Eval("PageNumber") %>'
                                        CssClass='<%# Convert.ToInt32(Eval("PageNumber")) == CurrentServicePage
                                            ? "services-page-button services-page-active"
                                            : "services-page-button" %>'
                                        CausesValidation="false" />
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Button ID="NextServicePageButton" runat="server"
                                Text="›"
                                CssClass="services-page-button"
                                CausesValidation="false"
                                OnClick="NextServicePageButton_Click" />

                        </div>

                    </div>

                </div>

            </div>

            <div class="services-category-panel">

                <h2>Service Categories</h2>

                <asp:Repeater ID="ServiceCategoriesRepeater" runat="server">
                    <ItemTemplate>
                        <div class="services-category-item">
                            <span class="services-category-icon">
                                <%#: Eval("CategoryIcon") %>
                            </span>

                            <div>
                                <strong><%#: Eval("CategoryName") %></strong>
                                <p><%#: Eval("ServiceCount") %> services</p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:HyperLink ID="ManageServiceCategoriesLink" runat="server"
                    NavigateUrl="~/AdminServiceCategories.aspx"
                    CssClass="services-manage-categories">
                    Manage Categories &rsaquo;
                </asp:HyperLink>

            </div>

        </div>

    </div>

</asp:Content>