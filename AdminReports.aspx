<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminReports.aspx.cs"
    Inherits="RxEvents.AdminReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="report-page">

        <div class="report-header">
            <div>
                <h1>Reports</h1>
                <p>Dashboard &gt; Reports</p>
            </div>

            <asp:Button ID="ExportReportButton" runat="server"
                Text="↓ Export Report"
                CssClass="report-export-button"
                OnClick="ExportReportButton_Click" />
        </div>

        <asp:Label ID="ReportMessage" runat="server"
            CssClass="report-message" EnableViewState="false" />

        <!-- Report Filters -->
        <div class="report-filter-card">

            <div class="report-filter-field">
                <asp:Label ID="StartDateLabel" runat="server"
                    AssociatedControlID="StartDateInput"
                    Text="Start Date" />

                <asp:TextBox ID="StartDateInput" runat="server"
                    TextMode="Date" CssClass="report-input" />
            </div>

            <div class="report-filter-field">
                <asp:Label ID="EndDateLabel" runat="server"
                    AssociatedControlID="EndDateInput"
                    Text="End Date" />

                <asp:TextBox ID="EndDateInput" runat="server"
                    TextMode="Date" CssClass="report-input" />
            </div>

            <div class="report-filter-field">
                <asp:Label ID="ReportTypeLabel" runat="server"
                    AssociatedControlID="ReportTypeDropdown"
                    Text="Report Type" />

                <asp:DropDownList ID="ReportTypeDropdown" runat="server"
                    CssClass="report-input">
                    <asp:ListItem Text="All Reports" Value="All" />
                    <asp:ListItem Text="Bookings" Value="Bookings" />
                    <asp:ListItem Text="Revenue" Value="Revenue" />
                    <asp:ListItem Text="Customers" Value="Customers" />
                    <asp:ListItem Text="Reviews" Value="Reviews" />
                </asp:DropDownList>
            </div>

            <div class="report-filter-field">
                <asp:Label ID="EventFilterLabel" runat="server"
                    AssociatedControlID="EventFilterDropdown"
                    Text="Event" />

                <asp:DropDownList ID="EventFilterDropdown" runat="server"
                    CssClass="report-input">
                    <asp:ListItem Text="All Events" Value="" />
                </asp:DropDownList>
            </div>

            <div class="report-filter-actions">
                <asp:Button ID="GenerateReportButton" runat="server"
                    Text="Generate Report"
                    CssClass="report-generate-button"
                    OnClick="GenerateReportButton_Click" />

                <asp:Button ID="ClearReportFiltersButton" runat="server"
                    Text="Clear"
                    CssClass="report-clear-button"
                    CausesValidation="false"
                    OnClick="ClearReportFiltersButton_Click" />
            </div>

        </div>

        <!-- KPI Cards -->
        <div class="report-kpi-grid">

            <div class="report-kpi-card kpi-purple">
                <div class="report-kpi-icon">▦</div>
                <div>
                    <p>Total Bookings</p>
                    <h2><asp:Literal ID="TotalBookingsValue" runat="server" Text="0" /></h2>
                    <span>Bookings in selected period</span>
                </div>
            </div>

            <div class="report-kpi-card kpi-green">
                <div class="report-kpi-icon">₹</div>
                <div>
                    <p>Total Revenue</p>
                    <h2><asp:Literal ID="TotalRevenueValue" runat="server" Text="UGX 0" /></h2>
                    <span>Payments marked as paid</span>
                </div>
            </div>

            <div class="report-kpi-card kpi-blue">
                <div class="report-kpi-icon">♙</div>
                <div>
                    <p>Customers</p>
                    <h2><asp:Literal ID="TotalCustomersValue" runat="server" Text="0" /></h2>
                    <span>Customers with bookings</span>
                </div>
            </div>

            <div class="report-kpi-card kpi-gold">
                <div class="report-kpi-icon">★</div>
                <div>
                    <p>Average Rating</p>
                    <h2><asp:Literal ID="AverageRatingValue" runat="server" Text="0.0/5" /></h2>
                    <span>Reviews in selected period</span>
                </div>
            </div>

        </div>

        <!-- Charts and Summary -->
        <div class="report-analysis-grid">

            <div class="report-panel">
                <div class="report-panel-heading">
                    <h2>Monthly Revenue</h2>
                    <span>UGX</span>
                </div>

                <div class="report-chart-legend">
                    <span><i class="legend-purple"></i> Revenue</span>
                </div>

                <asp:Repeater ID="MonthlyRevenueRepeater" runat="server">
                    <HeaderTemplate>
                        <div class="report-bar-chart">
                    </HeaderTemplate>

                    <ItemTemplate>
                        <div class="report-bar-column">
                            <div class="report-bar-value">
                                <%#: Eval("DisplayValue") %>
                            </div>

                            <div class="report-bar-track">
                                <div class="report-bar"
                                    style='height:<%#: Eval("BarHeight") %>%;'>
                                </div>
                            </div>

                            <span><%#: Eval("MonthLabel") %></span>
                        </div>
                    </ItemTemplate>

                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <div class="report-panel">
                <div class="report-panel-heading">
                    <h2>Top Events</h2>
                    <span>By bookings</span>
                </div>

                <asp:Repeater ID="TopEventsRepeater" runat="server">
                    <ItemTemplate>
                        <div class="report-top-event">
                            <div class="report-top-event-name">
                                <span><%#: Eval("EventName") %></span>
                                <strong><%#: Eval("BookingCount") %></strong>
                            </div>

                            <div class="report-progress-track">
                                <div class="report-progress-bar"
                                    style='width:<%#: Eval("BarWidth") %>%;'>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Label ID="TopEventsEmptyLabel" runat="server"
                    CssClass="report-empty-message"
                    Text="No event booking data for this period."
                    Visible="false" />
            </div>

        </div>

        <!-- Booking Status and Recent Bookings -->
        <div class="report-bottom-grid">

            <div class="report-panel report-status-panel">
                <div class="report-panel-heading">
                    <h2>Booking Status</h2>
                </div>

                <asp:Repeater ID="BookingStatusRepeater" runat="server">
                    <ItemTemplate>
                        <div class="report-status-row">
                            <span class="report-status-dot"
                                style='background:<%#: Eval("StatusColor") %>;'>
                            </span>

                            <span class="report-status-name">
                                <%#: Eval("BookingStatus") %>
                            </span>

                            <strong><%#: Eval("BookingCount") %></strong>

                            <span class="report-status-percent">
                                <%#: Eval("Percentage") %>%
                            </span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Label ID="BookingStatusEmptyLabel" runat="server"
                    CssClass="report-empty-message"
                    Text="No bookings found."
                    Visible="false" />
            </div>

            <div class="report-panel report-recent-panel">
                <div class="report-panel-heading">
                    <h2>Recent Bookings</h2>
                    <asp:HyperLink ID="ViewBookingsLink" runat="server"
                        NavigateUrl="~/AdminBookings.aspx">
                        View All
                    </asp:HyperLink>
                </div>

                <asp:GridView ID="RecentBookingsGridView" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="report-bookings-grid"
                    GridLines="None"
                    EmptyDataText="No bookings found for this period.">

                    <Columns>
                        <asp:BoundField DataField="BookingReference"
                            HeaderText="Booking Ref" />

                        <asp:BoundField DataField="CustomerName"
                            HeaderText="Customer" />

                        <asp:BoundField DataField="EventName"
                            HeaderText="Event" />

                        <asp:BoundField DataField="EventDate"
                            HeaderText="Event Date"
                            DataFormatString="{0:dd MMM yyyy}" />

                        <asp:BoundField DataField="TotalAmount"
                            HeaderText="Amount"
                            DataFormatString="{0:N0}" />

                        <asp:BoundField DataField="BookingStatus"
                            HeaderText="Status" />
                    </Columns>

                </asp:GridView>
            </div>

        </div>

        <div class="report-footer-note">
            Reports are calculated from the current database records and selected date range.
        </div>

    </div>

</asp:Content>