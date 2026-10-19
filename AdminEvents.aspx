
<%@ Page Title="Events Management" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminEvents.aspx.cs" Inherits="RxEvents.AdminEvents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="events-page">

        <!-- Page heading -->
        <div class="events-page-heading">
            <div>
                <h2>All Events</h2>
                <p>Manage and track all events</p>
            </div>

            <div class="events-heading-actions">
                <asp:Button ID="ExportEventsButton" runat="server"
                    Text="↓ Export"
                    CssClass="events-export-button"
                    CausesValidation="false"
                    OnClick="ExportEventsButton_Click" />

                <asp:HyperLink ID="AddNewEventLink" runat="server"
                    NavigateUrl="~/AdminAddEvent.aspx"
                    CssClass="events-add-button">
                    + Add New Event
                </asp:HyperLink>
            </div>
        </div>

        <asp:Label ID="EventsMessage" runat="server"
            CssClass="events-message" />

        <!-- Statistics -->
        <div class="events-statistics">

            <div class="events-stat-card">
                <div class="events-stat-icon purple">▣</div>
                <div>
                    <span>Total Events</span>
                    <strong><asp:Literal ID="TotalEventsValue" runat="server" Text="0" /></strong>
                </div>
            </div>

            <div class="events-stat-card">
                <div class="events-stat-icon orange">▣</div>
                <div>
                    <span>Active Events</span>
                    <strong><asp:Literal ID="ActiveEventsValue" runat="server" Text="0" /></strong>
                </div>
            </div>

            <div class="events-stat-card">
                <div class="events-stat-icon green">✓</div>
                <div>
                    <span>Inactive Events</span>
                    <strong><asp:Literal ID="InactiveEventsValue" runat="server" Text="0" /></strong>
                </div>
            </div>

            <div class="events-stat-card">
                <div class="events-stat-icon dark-purple">☆</div>
                <div>
                    <span>Most Booked Event</span>
                    <strong class="most-booked-name">
                        <asp:Literal ID="MostBookedEventValue" runat="server" Text="None" />
                    </strong>
                </div>
            </div>

        </div>

        <!-- Main content and categories -->
        <div class="events-content-layout">

            <div class="events-list-panel">

                <!-- Filters -->
                <div class="events-filter-panel">

                    <div class="events-filter-row">

                        <asp:TextBox ID="EventSearchTextBox" runat="server"
                            CssClass="events-search-input"
                            placeholder="Search by event name, type..." />

                        <div class="events-filter-field">
                            <label for="<%= StatusFilter.ClientID %>">Status</label>
                            <asp:DropDownList ID="StatusFilter" runat="server"
                                CssClass="events-filter-select">
                                <asp:ListItem Value="">All Status</asp:ListItem>
                                <asp:ListItem Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="events-filter-field">
                            <label for="<%= CategoryFilter.ClientID %>">Event Type</label>
                            <asp:DropDownList ID="CategoryFilter" runat="server"
                                CssClass="events-filter-select" />
                        </div>

                        <asp:Button ID="ApplyEventFiltersButton" runat="server"
                            Text="Search"
                            CssClass="events-filter-button"
                            OnClick="ApplyEventFiltersButton_Click" />

                    </div>

                    <div class="events-filter-row events-date-row">
                        <div class="events-filter-field">
                            <label for="<%= StartDateTextBox.ClientID %>">Date Added From</label>
                            <asp:TextBox ID="StartDateTextBox" runat="server"
                                TextMode="Date"
                                CssClass="events-filter-select" />
                        </div>

                        <div class="events-filter-field">
                            <label for="<%= EndDateTextBox.ClientID %>">Date Added To</label>
                            <asp:TextBox ID="EndDateTextBox" runat="server"
                                TextMode="Date"
                                CssClass="events-filter-select" />
                        </div>

                        <asp:Button ID="ClearEventFiltersButton" runat="server"
                            Text="↻ Clear All Filters"
                            CssClass="events-clear-button"
                            CausesValidation="false"
                            OnClick="ClearEventFiltersButton_Click" />
                    </div>

                </div>

                <!-- Events table -->
                <div class="events-table-wrapper">

                    <asp:GridView ID="EventsGridView" runat="server"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        CssClass="events-grid"
                        EmptyDataText="No events found."
                        DataKeyNames="EventID"
                        OnRowCommand="EventsGridView_RowCommand">

                        <Columns>

                            <asp:TemplateField HeaderText="Events">
                                <ItemTemplate>
                                    <div class="events-table-event">
                                        <asp:Image ID="EventThumbnail" runat="server"
                                            ImageUrl='<%# GetEventImageUrl(Eval("EventImage")) %>'
                                            AlternateText='<%# Eval("EventName") %>'
                                            CssClass="events-thumbnail" />

                                        <div class="events-table-event-info">
                                            <strong><%# Eval("EventName") %></strong>
                                            <span><%# Eval("Description") %></span>
                                            <small><%# Eval("CategoryName") %></small>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="BookingCount"
                                HeaderText="Bookings" />

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="EventStatusLabel" runat="server"
                                        Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>'
                                        CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "event-status-active" : "event-status-inactive" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="DateAdded"
                                HeaderText="Date Added"
                                DataFormatString="{0:dd MMM yyyy}" />

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <div class="events-row-actions">

                                        <asp:LinkButton ID="ViewEventButton" runat="server"
                                            CommandName="ViewEvent"
                                            CommandArgument='<%# Eval("EventID") %>'
                                            CssClass="events-row-action"
                                            CausesValidation="false"
                                            ToolTip="View event">
                                            ◉
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="EditEventButton" runat="server"
                                            CommandName="EditEvent"
                                            CommandArgument='<%# Eval("EventID") %>'
                                            CssClass="events-row-action"
                                            CausesValidation="false"
                                            ToolTip="Edit event">
                                            ✎
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="ToggleEventButton" runat="server"
                                            CommandName="ToggleEvent"
                                            CommandArgument='<%# Eval("EventID") %>'
                                            CssClass="events-row-action"
                                            CausesValidation="false"
                                            ToolTip="Activate or deactivate event"
                                            OnClientClick="return confirm('Change this event status?');">
                                            ⋮
                                        </asp:LinkButton>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                </div>

                
<div class="events-grid-footer">
    <asp:Label ID="EventsCountLabel" runat="server"
        Text="Showing 0 events" />

    <div class="events-pagination">
        <asp:LinkButton ID="PreviousPageButton" runat="server"
            CssClass="pagination-arrow"
            CausesValidation="false"
            OnClick="PreviousPageButton_Click"
            ToolTip="Previous page">
            ‹
        </asp:LinkButton>

        <asp:Repeater ID="PaginationRepeater" runat="server"
            OnItemCommand="PaginationRepeater_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton ID="PageNumberButton" runat="server"
                    CommandName="GoToPage"
                    CommandArgument='<%# Eval("PageNumber") %>'
                    CssClass='<%# Convert.ToBoolean(Eval("IsCurrent")) ? "pagination-number active" : "pagination-number" %>'
                    CausesValidation="false"
                    Text='<%# Eval("DisplayText") %>' />
            </ItemTemplate>
        </asp:Repeater>

        <asp:LinkButton ID="NextPageButton" runat="server"
            CssClass="pagination-arrow"
            CausesValidation="false"
            OnClick="NextPageButton_Click"
            ToolTip="Next page">
            ›
        </asp:LinkButton>
    </div>
</div>

            </div>

            <!-- Event categories -->
            <aside class="events-categories-panel">

                <h3>Event Categories</h3>

                <asp:Repeater ID="EventCategoriesRepeater" runat="server">
                    <ItemTemplate>
                        <div class="events-category-item">
                            <div class="events-category-icon">◇</div>
                            <div class="events-category-details">
                                <strong><%# Eval("CategoryName") %></strong>
                                <span><%# Eval("CategoryEventCount") %> events</span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:HyperLink ID="ManageCategoriesLink" runat="server"
                    NavigateUrl="~/AdminEventCategories.aspx"
                    CssClass="events-manage-categories">
                    Manage Categories&nbsp; ›
                </asp:HyperLink>

            </aside>

        </div>

    </div>

</asp:Content>
