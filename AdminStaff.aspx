<%@ Page Title="Staff" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminStaff.aspx.cs"
    Inherits="RxEvents.AdminStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="staff-page">

        <div class="staff-header">
            <div>
                <h1>Staff</h1>
                <p>Dashboard &gt; Staff</p>
            </div>

            <div class="staff-header-actions">
                <asp:Button ID="ExportStaffButton" runat="server"
                    Text="↓ Export"
                    CssClass="staff-export-button"
                    OnClick="ExportStaffButton_Click" />

                <asp:HyperLink ID="AddStaffLink" runat="server"
                    NavigateUrl="~/AdminAddStaff.aspx"
                    CssClass="staff-add-button">
                    + Add New Staff
                </asp:HyperLink>
            </div>
        </div>

        <asp:Label ID="StaffMessage" runat="server"
            CssClass="staff-message" EnableViewState="false" />

        <div class="staff-filter-card">

            <div class="staff-filter-field staff-search-field">
                <asp:Label ID="StaffSearchLabel" runat="server"
                    AssociatedControlID="StaffSearchTextBox"
                    Text="Search Staff" />

                <asp:TextBox ID="StaffSearchTextBox" runat="server"
                    CssClass="staff-input"
                    placeholder="Search staff name, email or phone..." />
            </div>

            <div class="staff-filter-field">
                <asp:Label ID="DepartmentLabel" runat="server"
                    AssociatedControlID="DepartmentFilter"
                    Text="Department" />

                <asp:DropDownList ID="DepartmentFilter" runat="server"
                    CssClass="staff-input">
                    <asp:ListItem Text="All Departments" Value="" />
                    <asp:ListItem Text="Management" Value="Management" />
                    <asp:ListItem Text="Operations" Value="Operations" />
                    <asp:ListItem Text="Planning" Value="Planning" />
                    <asp:ListItem Text="Finance" Value="Finance" />
                    <asp:ListItem Text="Marketing" Value="Marketing" />
                    <asp:ListItem Text="Logistics" Value="Logistics" />
                    <asp:ListItem Text="Customer Service" Value="Customer Service" />
                    <asp:ListItem Text="Technical" Value="Technical" />
                </asp:DropDownList>
            </div>

            <div class="staff-filter-field">
                <asp:Label ID="StaffRoleLabel" runat="server"
                    AssociatedControlID="StaffRoleFilter"
                    Text="Role" />

                <asp:DropDownList ID="StaffRoleFilter" runat="server"
                    CssClass="staff-input">
                    <asp:ListItem Text="All Roles" Value="" />
                    <asp:ListItem Text="Event Manager" Value="Event Manager" />
                    <asp:ListItem Text="Operations Head" Value="Operations Head" />
                    <asp:ListItem Text="Event Planner" Value="Event Planner" />
                    <asp:ListItem Text="Accountant" Value="Accountant" />
                    <asp:ListItem Text="Marketing Officer" Value="Marketing Officer" />
                    <asp:ListItem Text="Logistics Coordinator" Value="Logistics Coordinator" />
                    <asp:ListItem Text="Customer Support" Value="Customer Support" />
                    <asp:ListItem Text="Technical Lead" Value="Technical Lead" />
                </asp:DropDownList>
            </div>

            <div class="staff-filter-field">
                <asp:Label ID="StaffStatusLabel" runat="server"
                    AssociatedControlID="StaffStatusFilter"
                    Text="Status" />

                <asp:DropDownList ID="StaffStatusFilter" runat="server"
                    CssClass="staff-input">
                    <asp:ListItem Text="All Statuses" Value="" />
                    <asp:ListItem Text="Active" Value="1" />
                    <asp:ListItem Text="Inactive" Value="0" />
                </asp:DropDownList>
            </div>

            <div class="staff-filter-actions">
                <asp:Button ID="ApplyStaffFiltersButton" runat="server"
                    Text="Apply Filters"
                    CssClass="staff-filter-button"
                    OnClick="ApplyStaffFiltersButton_Click" />

                <asp:Button ID="ClearStaffFiltersButton" runat="server"
                    Text="↻ Clear Filters"
                    CssClass="staff-clear-button"
                    CausesValidation="false"
                    OnClick="ClearStaffFiltersButton_Click" />
            </div>

        </div>

        <div class="staff-table-card">

            <asp:GridView ID="StaffGridView" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="UserID"
                CssClass="staff-grid"
                GridLines="None"
                OnRowCommand="StaffGridView_RowCommand"
                EmptyDataText="No staff members found.">

                <Columns>

                    <asp:TemplateField HeaderText="Staff Member">
                        <ItemTemplate>
                            <div class="staff-member-cell">

                                <asp:Image ID="StaffImage" runat="server"
                                    CssClass="staff-avatar"
                                    ImageUrl='<%# GetStaffImageUrl(Eval("ProfilePhoto")) %>'
                                    AlternateText="Staff profile" />

                                <div>
                                    <strong>
                                        <%#: Eval("FirstName") %>
                                        <%#: Eval("LastName") %>
                                    </strong>

                                    <p><%#: Eval("Email") %></p>
                                </div>

                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Department"
                        HeaderText="Department" />

                    <asp:BoundField DataField="StaffRole"
                        HeaderText="Role" />

                    <asp:BoundField DataField="Phone"
                        HeaderText="Contact" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# Convert.ToBoolean(Eval("IsActive"))
                                ? "staff-status-active"
                                : "staff-status-inactive" %>'>
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
                            <div class="staff-row-actions">

                                <asp:LinkButton ID="ViewStaffButton" runat="server"
                                    Text="View"
                                    CommandName="ViewStaff"
                                    CommandArgument='<%# Eval("UserID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="EditStaffButton" runat="server"
                                    Text="Edit"
                                    CommandName="EditStaff"
                                    CommandArgument='<%# Eval("UserID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="ToggleStaffButton" runat="server"
                                    Text='<%# Convert.ToBoolean(Eval("IsActive"))
                                        ? "Deactivate" : "Activate" %>'
                                    CommandName="ToggleStaff"
                                    CommandArgument='<%# Eval("UserID") %>'
                                    CausesValidation="false" />

                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <div class="staff-pagination">

                <asp:Label ID="StaffPaginationSummary" runat="server"
                    Text="Showing 0 staff members" />

                <div class="staff-pagination-buttons">

                    <asp:Button ID="PreviousStaffPageButton" runat="server"
                        Text="‹"
                        CssClass="staff-page-button"
                        CausesValidation="false"
                        OnClick="PreviousStaffPageButton_Click" />

                    <asp:Repeater ID="StaffPaginationRepeater" runat="server"
                        OnItemCommand="StaffPaginationRepeater_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="StaffPageButton" runat="server"
                                Text='<%# Eval("PageNumber") %>'
                                CommandName="GoToPage"
                                CommandArgument='<%# Eval("PageNumber") %>'
                                CssClass='<%# Convert.ToInt32(Eval("PageNumber")) == CurrentStaffPage
                                    ? "staff-page-button staff-page-active"
                                    : "staff-page-button" %>'
                                CausesValidation="false" />
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Button ID="NextStaffPageButton" runat="server"
                        Text="›"
                        CssClass="staff-page-button"
                        CausesValidation="false"
                        OnClick="NextStaffPageButton_Click" />

                </div>

            </div>

        </div>

    </div>

</asp:Content>