<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminCustomers.aspx.cs"
    Inherits="RxEvents.AdminCustomers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="customers-page">

        <div class="customers-header">
            <div>
                <h1>Customers</h1>
                <p>Dashboard &gt; Customers</p>
            </div>

            <div class="customers-header-actions">
                <asp:Button ID="ExportCustomersButton" runat="server"
                    Text="↓ Export"
                    CssClass="customers-export-button"
                    OnClick="ExportCustomersButton_Click" />

                <asp:HyperLink ID="AddCustomerLink" runat="server"
                    NavigateUrl="~/AdminAddCustomer.aspx"
                    CssClass="customers-add-button">
                    + Add New Customer
                </asp:HyperLink>
            </div>
        </div>

        <div class="customers-statistics">

            <div class="customers-stat-card">
                <span class="customers-stat-icon customer-purple">♟</span>
                <div>
                    <p>Total Customers</p>
                    <h2><asp:Literal ID="TotalCustomersValue" runat="server" Text="0" /></h2>
                </div>
            </div>

            <div class="customers-stat-card">
                <span class="customers-stat-icon customer-gold">♙</span>
                <div>
                    <p>New Customers</p>
                    <h2><asp:Literal ID="NewCustomersValue" runat="server" Text="0" /></h2>
                    <small>Joined this month</small>
                </div>
            </div>

            <div class="customers-stat-card">
                <span class="customers-stat-icon customer-green">✓</span>
                <div>
                    <p>Active Customers</p>
                    <h2><asp:Literal ID="ActiveCustomersValue" runat="server" Text="0" /></h2>
                </div>
            </div>

            <div class="customers-stat-card">
                <span class="customers-stat-icon customer-red">×</span>
                <div>
                    <p>Inactive Customers</p>
                    <h2><asp:Literal ID="InactiveCustomersValue" runat="server" Text="0" /></h2>
                </div>
            </div>

        </div>

        <asp:Label ID="CustomersMessage" runat="server"
            CssClass="customers-message" EnableViewState="false" />

        <div class="customers-filter-card">

            <div class="customers-search-field">
                <asp:TextBox ID="CustomerSearchTextBox" runat="server"
                    CssClass="customers-input"
                    placeholder="Search customer name, email or phone..." />
            </div>

            <div class="customers-filter-field">
                <asp:Label ID="CustomerStatusLabel" runat="server"
                    AssociatedControlID="CustomerStatusFilter"
                    Text="Status" />

                <asp:DropDownList ID="CustomerStatusFilter" runat="server"
                    CssClass="customers-input">
                    <asp:ListItem Text="All Statuses" Value="" />
                    <asp:ListItem Text="Active" Value="1" />
                    <asp:ListItem Text="Inactive" Value="0" />
                </asp:DropDownList>
            </div>

            <div class="customers-filter-field">
                <asp:Label ID="CustomerJoinDateLabel" runat="server"
                    AssociatedControlID="CustomerJoinDateFilter"
                    Text="Join Date" />

                <asp:DropDownList ID="CustomerJoinDateFilter" runat="server"
                    CssClass="customers-input">
                    <asp:ListItem Text="Any Time" Value="" />
                    <asp:ListItem Text="This Month" Value="month" />
                    <asp:ListItem Text="Last 30 Days" Value="30days" />
                    <asp:ListItem Text="This Year" Value="year" />
                </asp:DropDownList>
            </div>

            <div class="customers-filter-field">
                <asp:Label ID="CustomerSortLabel" runat="server"
                    AssociatedControlID="CustomerSortDropdown"
                    Text="Sort By" />

                <asp:DropDownList ID="CustomerSortDropdown" runat="server"
                    CssClass="customers-input">
                    <asp:ListItem Text="Recently Added" Value="recent" />
                    <asp:ListItem Text="Oldest First" Value="oldest" />
                    <asp:ListItem Text="Name A-Z" Value="name" />
                    <asp:ListItem Text="Most Bookings" Value="bookings" />
                    <asp:ListItem Text="Highest Spending" Value="spending" />
                </asp:DropDownList>
            </div>

            <div class="customers-filter-actions">
                <asp:Button ID="ApplyCustomerFiltersButton" runat="server"
                    Text="Apply Filters"
                    CssClass="customers-filter-button"
                    OnClick="ApplyCustomerFiltersButton_Click" />

                <asp:Button ID="ClearCustomerFiltersButton" runat="server"
                    Text="↻ Clear Filters"
                    CssClass="customers-clear-button"
                    CausesValidation="false"
                    OnClick="ClearCustomerFiltersButton_Click" />
            </div>

        </div>

        <div class="customers-table-card">

            <asp:GridView ID="CustomersGridView" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="UserID"
                CssClass="customers-grid"
                GridLines="None"
                OnRowCommand="CustomersGridView_RowCommand"
                EmptyDataText="No customers found.">

                <Columns>

                    <asp:TemplateField HeaderText="Customer">
                        <ItemTemplate>
                            <div class="customers-name-cell">

                                <asp:Image ID="CustomerImage" runat="server"
                                    CssClass="customers-avatar"
                                    ImageUrl='<%# GetCustomerImageUrl(Eval("ProfilePhoto")) %>'
                                    AlternateText="Customer profile" />

                                <div>
                                    <strong>
                                        <%#: Eval("FirstName") %>
                                        <%#: Eval("LastName") %>
                                    </strong>
                                    <p><%#: Eval("Email") %></p>
                                    <p><%#: Eval("Phone") %></p>
                                </div>

                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="TotalBookings"
                        HeaderText="Total Bookings" />

                    <asp:TemplateField HeaderText="Total Spent (UGX)">
                        <ItemTemplate>
                            UGX <%# Convert.ToDecimal(Eval("TotalSpent"))
                                .ToString("N0") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# Convert.ToBoolean(Eval("IsActive"))
                                ? "customers-status-active"
                                : "customers-status-inactive" %>'>
                                <%# Convert.ToBoolean(Eval("IsActive"))
                                    ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="CreatedDate"
                        HeaderText="Join Date"
                        DataFormatString="{0:dd MMM yyyy}" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>

                            <div class="customers-row-actions">

                                <asp:LinkButton ID="ViewCustomerButton" runat="server"
                                    Text="View"
                                    ToolTip="View customer"
                                    CommandName="ViewCustomer"
                                    CommandArgument='<%# Eval("UserID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="EditCustomerButton" runat="server"
                                    Text="Edit"
                                    ToolTip="Edit customer"
                                    CommandName="EditCustomer"
                                    CommandArgument='<%# Eval("UserID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="ToggleCustomerButton" runat="server"
                                    Text='<%# Convert.ToBoolean(Eval("IsActive"))
                                        ? "Deactivate" : "Activate" %>'
                                    ToolTip="Change customer status"
                                    CommandName="ToggleCustomer"
                                    CommandArgument='<%# Eval("UserID") %>'
                                    CausesValidation="false" />

                            </div>

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <div class="customers-pagination">

                <asp:Label ID="CustomerPaginationSummary" runat="server"
                    Text="Showing 0 customers" />

                <div class="customers-pagination-buttons">

                    <asp:Button ID="PreviousCustomerPageButton" runat="server"
                        Text="‹"
                        CssClass="customers-page-button"
                        CausesValidation="false"
                        OnClick="PreviousCustomerPageButton_Click" />

                    <asp:Repeater ID="CustomerPaginationRepeater" runat="server"
                        OnItemCommand="CustomerPaginationRepeater_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="CustomerPageButton" runat="server"
                                Text='<%# Eval("PageNumber") %>'
                                CommandName="GoToPage"
                                CommandArgument='<%# Eval("PageNumber") %>'
                                CssClass='<%# Convert.ToInt32(Eval("PageNumber")) == CurrentCustomerPage
                                    ? "customers-page-button customers-page-active"
                                    : "customers-page-button" %>'
                                CausesValidation="false" />
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Button ID="NextCustomerPageButton" runat="server"
                        Text="›"
                        CssClass="customers-page-button"
                        CausesValidation="false"
                        OnClick="NextCustomerPageButton_Click" />

                </div>

            </div>

        </div>

    </div>

</asp:Content>