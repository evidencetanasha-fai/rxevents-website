
<%@ Page Title="Packages" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminPackages.aspx.cs" Inherits="RxEvents.AdminPackages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="packages-page">

        <div class="packages-heading">
            <div>
                <h2>All Packages</h2>
                <p>Manage and track event packages</p>
            </div>

            <div class="packages-heading-actions">
                <asp:Button ID="ExportPackagesButton" runat="server"
                    Text="↓ Export"
                    CssClass="packages-export-button"
                    CausesValidation="false"
                    OnClick="ExportPackagesButton_Click" />

                <asp:HyperLink ID="AddPackageLink" runat="server"
                    NavigateUrl="~/AdminAddPackage.aspx"
                    CssClass="packages-add-button">
                    + Add New Package
                </asp:HyperLink>
            </div>
        </div>

        <asp:Label ID="PackagesMessage" runat="server"
            CssClass="packages-message" />

        <div class="packages-statistics">

            <div class="packages-stat-card">
                <div class="packages-stat-icon purple">⬡</div>
                <div>
                    <span>Total Packages</span>
                    <strong><asp:Literal ID="TotalPackagesValue" runat="server" Text="0" /></strong>
                </div>
            </div>

            <div class="packages-stat-card">
                <div class="packages-stat-icon orange">◇</div>
                <div>
                    <span>Active Packages</span>
                    <strong><asp:Literal ID="ActivePackagesValue" runat="server" Text="0" /></strong>
                </div>
            </div>

            <div class="packages-stat-card">
                <div class="packages-stat-icon green">●</div>
                <div>
                    <span>Inactive Packages</span>
                    <strong><asp:Literal ID="InactivePackagesValue" runat="server" Text="0" /></strong>
                </div>
            </div>

            <div class="packages-stat-card">
                <div class="packages-stat-icon dark-purple">☆</div>
                <div>
                    <span>Most Popular Package</span>
                    <strong class="popular-package-name">
                        <asp:Literal ID="MostPopularPackageValue" runat="server" Text="None" />
                    </strong>
                </div>
            </div>

        </div>

        <div class="packages-content-layout">

            <div class="packages-list-panel">

                <div class="packages-filter-panel">
                    <div class="packages-filter-row">

                        <asp:TextBox ID="PackageSearchTextBox" runat="server"
                            CssClass="packages-search-input"
                            placeholder="Search by package name, type..." />

                        <div class="packages-filter-field">
                            <label for="<%= PackageStatusFilter.ClientID %>">Status</label>
                            <asp:DropDownList ID="PackageStatusFilter" runat="server"
                                CssClass="packages-filter-control">
                                <asp:ListItem Value="">All Status</asp:ListItem>
                                <asp:ListItem Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="packages-filter-field">
                            <label for="<%= PackageTypeFilter.ClientID %>">Package Type</label>
                            <asp:DropDownList ID="PackageTypeFilter" runat="server"
                                CssClass="packages-filter-control">
                                <asp:ListItem Value="">All Types</asp:ListItem>
                                <asp:ListItem Value="Premium++">Premium++</asp:ListItem>
                                <asp:ListItem Value="Premium">Premium</asp:ListItem>
                                <asp:ListItem Value="Standard">Standard</asp:ListItem>
                                <asp:ListItem Value="Basic">Basic</asp:ListItem>
                                <asp:ListItem Value="Custom">Custom</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <asp:Button ID="ApplyPackageFiltersButton" runat="server"
                            Text="Search"
                            CssClass="packages-filter-button"
                            OnClick="ApplyPackageFiltersButton_Click" />

                    </div>

                    <div class="packages-filter-bottom">
                        <asp:Button ID="ClearPackageFiltersButton" runat="server"
                            Text="↻ Clear All Filters"
                            CssClass="packages-clear-button"
                            CausesValidation="false"
                            OnClick="ClearPackageFiltersButton_Click" />
                    </div>
                </div>

                <div class="packages-table-wrapper">

                    <asp:GridView ID="PackagesGridView" runat="server"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        CssClass="packages-grid"
                        DataKeyNames="PackageID"
                        EmptyDataText="No packages found."
                        OnRowCommand="PackagesGridView_RowCommand">

                        <Columns>

                            <asp:TemplateField HeaderText="Packages">
                                <ItemTemplate>
                                    <div class="packages-table-item">

                                        <asp:Image ID="PackageThumbnail" runat="server"
                                            ImageUrl='<%# GetPackageImageUrl(Eval("ImagePath")) %>'
                                            AlternateText='<%# Eval("PackageName") %>'
                                            CssClass="packages-thumbnail" />

                                        <div class="packages-item-details">
                                            <strong><%# Eval("PackageName") %></strong>
                                            <span><%# Eval("Description") %></span>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="PackageType"
                                HeaderText="Type" />

                            <asp:BoundField DataField="FeatureCount"
                                HeaderText="Features" />

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="PackageStatusLabel" runat="server"
                                        Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>'
                                        CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "package-status-active" : "package-status-inactive" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <div class="packages-row-actions">

                                        <asp:LinkButton ID="ViewPackageButton" runat="server"
                                            CommandName="ViewPackage"
                                            CommandArgument='<%# Eval("PackageID") %>'
                                            CssClass="packages-row-action"
                                            CausesValidation="false"
                                            ToolTip="View package">◉</asp:LinkButton>

                                        <asp:LinkButton ID="EditPackageButton" runat="server"
                                            CommandName="EditPackage"
                                            CommandArgument='<%# Eval("PackageID") %>'
                                            CssClass="packages-row-action"
                                            CausesValidation="false"
                                            ToolTip="Edit package">✎</asp:LinkButton>

                                        <asp:LinkButton ID="TogglePackageButton" runat="server"
                                            CommandName="TogglePackage"
                                            CommandArgument='<%# Eval("PackageID") %>'
                                            CssClass="packages-row-action"
                                            CausesValidation="false"
                                            ToolTip="Activate or deactivate package"
                                            OnClientClick="return confirm('Change this package status?');">⋮</asp:LinkButton>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                </div>

                <div class="packages-grid-footer">
                    <asp:Label ID="PackagesCountLabel" runat="server"
                        Text="Showing 0 packages" />

                    <div class="packages-pagination">

                        <asp:LinkButton ID="PreviousPackagePageButton" runat="server"
                            CssClass="packages-page-arrow"
                            CausesValidation="false"
                            OnClick="PreviousPackagePageButton_Click">‹</asp:LinkButton>

                        <asp:Repeater ID="PackagePaginationRepeater" runat="server"
                            OnItemCommand="PackagePaginationRepeater_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="PackagePageButton" runat="server"
                                    CommandName="GoToPage"
                                    CommandArgument='<%# Eval("PageNumber") %>'
                                    Text='<%# Eval("DisplayText") %>'
                                    CssClass='<%# Convert.ToBoolean(Eval("IsCurrent")) ? "packages-page-number active" : "packages-page-number" %>'
                                    CausesValidation="false" />
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:LinkButton ID="NextPackagePageButton" runat="server"
                            CssClass="packages-page-arrow"
                            CausesValidation="false"
                            OnClick="NextPackagePageButton_Click">›</asp:LinkButton>

                    </div>
                </div>

            </div>

            <aside class="packages-categories-panel">
                <h3>Package Categories</h3>

                <asp:Repeater ID="PackageCategoriesRepeater" runat="server">
                    <ItemTemplate>
                        <div class="packages-category-item">
                            <div class="packages-category-icon">
                                <%# Eval("CategoryIcon") %>
                            </div>
                            <div class="packages-category-details">
                                <strong><%# Eval("CategoryName") %></strong>
                                <span><%# Eval("PackageCount") %> packages</span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:HyperLink ID="ManagePackageCategoriesLink" runat="server"
                    NavigateUrl="~/AdminPackageCategories.aspx"
                    CssClass="packages-manage-categories">
                    Manage Categories&nbsp; ›
                </asp:HyperLink>
            </aside>

        </div>

    </div>

</asp:Content>
