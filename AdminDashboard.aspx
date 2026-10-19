<%@ Page Title="Admin Dashboard" Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdminDashboard.aspx.cs"
    Inherits="RxEvents.AdminDashboard" %>

<asp:Content ID="DashboardContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="rx-figma-dashboard" id="DashboardPage">

        <!-- STATISTICS -->
        <section class="dashboard-stats">

            <div class="figma-stat-card">
                <div class="stat-icon purple-icon">▦</div>
                <div class="stat-info">
                    <span>Total Bookings</span>
                    <strong><asp:Literal ID="TotalBookingsValue" runat="server" Text="309" /></strong>
                </div>
            </div>

            <div class="figma-stat-card">
                <div class="stat-icon gold-icon">▦</div>
                <div class="stat-info">
                    <span>Upcoming Events</span>
                    <strong><asp:Literal ID="TotalEventsValue" runat="server" Text="23" /></strong>
                </div>
            </div>

            <div class="figma-stat-card">
                <div class="stat-icon purple-icon">♙</div>
                <div class="stat-info">
                    <span>Total Customers</span>
                    <strong><asp:Literal ID="TotalCustomersValue" runat="server" Text="259" /></strong>
                </div>
            </div>

            <div class="figma-stat-card">
                <div class="stat-icon gold-icon">$</div>
                <div class="stat-info">
                    <span>Total Revenue</span>
                    <strong><asp:Literal ID="TotalRevenueValue" runat="server" Text="$102,000" /></strong>
                </div>
            </div>

        </section>

        
<!-- CHARTS -->
<section class="figma-charts-grid">

    <!-- BOOKINGS OVERVIEW -->
    <div class="figma-panel chart-panel">
        <div class="panel-heading">
            <h2>Bookings Overview</h2>

            <asp:DropDownList ID="BookingsPeriod" runat="server"
                CssClass="period-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="BookingsPeriod_SelectedIndexChanged">
                <asp:ListItem>This week</asp:ListItem>
                <asp:ListItem>This month</asp:ListItem>
                <asp:ListItem>This year</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="bar-chart">
            <div class="chart-y-label">Number of bookings</div>

            <div class="chart-plot">
                <div class="y-axis-labels">
                    <span>40</span><span>30</span><span>20</span>
                    <span>10</span><span>0</span>
                </div>

                <div class="chart-bars">
                    <div class="bar-column">
                        <div id="BookingBar1" runat="server" class="bar"></div>
                        <asp:Label ID="BookingLabel1" runat="server" Text="Mon" />
                    </div>
                    <div class="bar-column">
                        <div id="BookingBar2" runat="server" class="bar"></div>
                        <asp:Label ID="BookingLabel2" runat="server" Text="Tue" />
                    </div>
                    <div class="bar-column">
                        <div id="BookingBar3" runat="server" class="bar"></div>
                        <asp:Label ID="BookingLabel3" runat="server" Text="Wed" />
                    </div>
                    <div class="bar-column">
                        <div id="BookingBar4" runat="server" class="bar"></div>
                        <asp:Label ID="BookingLabel4" runat="server" Text="Thu" />
                    </div>
                    <div class="bar-column">
                        <div id="BookingBar5" runat="server" class="bar"></div>
                        <asp:Label ID="BookingLabel5" runat="server" Text="Fri" />
                    </div>
                    <div class="bar-column">
                        <div id="BookingBar6" runat="server" class="bar"></div>
                        <asp:Label ID="BookingLabel6" runat="server" Text="Sat" />
                    </div>
                    <div class="bar-column">
                        <div id="BookingBar7" runat="server" class="bar"></div>
                        <asp:Label ID="BookingLabel7" runat="server" Text="Sun" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- REVENUE OVERVIEW -->
    <div class="figma-panel chart-panel">
        <div class="panel-heading">
            <h2>Revenue Overview</h2>

            <asp:DropDownList ID="RevenuePeriod" runat="server"
                CssClass="period-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="RevenuePeriod_SelectedIndexChanged">
                <asp:ListItem>This week</asp:ListItem>
                <asp:ListItem>This month</asp:ListItem>
                <asp:ListItem>This year</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="bar-chart">
            <div class="chart-y-label">Revenue</div>

            <div class="chart-plot">
                <div class="y-axis-labels">
                    <span>2000</span><span>1500</span><span>1000</span>
                    <span>500</span><span>0</span>
                </div>

                <div class="chart-bars">
                    <div class="bar-column">
                        <div id="RevenueBar1" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel1" runat="server" Text="Jan" />
                    </div>
                    <div class="bar-column">
                        <div id="RevenueBar2" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel2" runat="server" Text="Feb" />
                    </div>
                    <div class="bar-column">
                        <div id="RevenueBar3" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel3" runat="server" Text="Mar" />
                    </div>
                    <div class="bar-column">
                        <div id="RevenueBar4" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel4" runat="server" Text="Apr" />
                    </div>
                    <div class="bar-column">
                        <div id="RevenueBar5" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel5" runat="server" Text="May" />
                    </div>
                    <div class="bar-column">
                        <div id="RevenueBar6" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel6" runat="server" Text="Jun" />
                    </div>
                    <div class="bar-column">
                        <div id="RevenueBar7" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel7" runat="server" Text="Jul" />
                    </div>
                    <div class="bar-column">
                        <div id="RevenueBar8" runat="server" class="bar"></div>
                        <asp:Label ID="RevenueLabel8" runat="server" Text="Aug" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</section>


        <!-- QUICK ACTIONS AND UPCOMING EVENTS -->
        <section class="figma-lower-grid">

            <div class="figma-panel quick-actions-panel">
                <h2>Quick Action</h2>

                <div class="quick-actions-grid">
                    <asp:HyperLink ID="AddEventAction" runat="server"
                        NavigateUrl="~/AdminEvents.aspx" CssClass="quick-action">
                        <span class="quick-action-icon">⊕</span>
                        <span>Add New Event</span>
                    </asp:HyperLink>

                    <asp:HyperLink ID="AddBookingAction" runat="server"
                        NavigateUrl="~/AdminBookings.aspx" CssClass="quick-action">
                        <span class="quick-action-icon">▦</span>
                        <span>Add New Booking</span>
                    </asp:HyperLink>

                    <asp:HyperLink ID="AddPackageAction" runat="server"
                        NavigateUrl="~/AdminPackages.aspx" CssClass="quick-action">
                        <span class="quick-action-icon">◇</span>
                        <span>Add New Package</span>
                    </asp:HyperLink>

                    <asp:HyperLink ID="AddCustomerAction" runat="server"
                        NavigateUrl="~/AdminCustomers.aspx" CssClass="quick-action">
                        <span class="quick-action-icon">♙</span>
                        <span>Add New Customer</span>
                    </asp:HyperLink>

                    <asp:HyperLink ID="RecordPaymentAction" runat="server"
                        NavigateUrl="~/AdminPayments.aspx" CssClass="quick-action">
                        <span class="quick-action-icon">$</span>
                        <span>Record Payment</span>
                    </asp:HyperLink>

                    <asp:HyperLink ID="GenerateReportAction" runat="server"
                        NavigateUrl="~/AdminReports.aspx" CssClass="quick-action">
                        <span class="quick-action-icon">▥</span>
                        <span>Generate Report</span>
                    </asp:HyperLink>
                </div>
            </div>

            
<div class="figma-panel upcoming-panel">
    <div class="panel-heading">
        <h2>Upcoming Events</h2>

        <asp:HyperLink ID="ViewAllEvents" runat="server"
            NavigateUrl="~/AdminEvents.aspx"
            CssClass="view-all-link">
            View All
        </asp:HyperLink>
    </div>

    <div class="upcoming-event">
        <asp:Image ID="UpcomingEventImage" runat="server"
            AlternateText="Upcoming event"
            CssClass="upcoming-event-image" />

        <div class="upcoming-event-details">
            <strong>
                <asp:Literal ID="UpcomingEventName" runat="server"
                    Text="Loading event..." />
            </strong>

            <span>
                <asp:Literal ID="UpcomingEventDate" runat="server" />
            </span>

            <span>
                <asp:Literal ID="UpcomingEventVenue" runat="server" />
            </span>
        </div>

        <asp:Label ID="UpcomingEventStatus" runat="server"
            Text=""
            CssClass="event-status-pending" />
    </div>
</div>


        </section>

        <!-- RECENT BOOKINGS -->
        <section class="figma-panel recent-bookings-panel">

            <div class="panel-heading">
                <h2>Recent Bookings</h2>
                <asp:HyperLink ID="ViewAllBookings" runat="server"
                    NavigateUrl="~/AdminBookings.aspx" CssClass="view-all-link">
                    View All
                </asp:HyperLink>
            </div>

            <asp:Label ID="BookingsEmptyMessage" runat="server"
                Text="No bookings found." CssClass="empty-message" Visible="false" />

            <div class="table-scroll">
                <asp:GridView ID="RecentBookingsGrid" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="figma-data-grid"
                    GridLines="None"
                    EmptyDataText="No bookings available."
                    OnRowDataBound="RecentBookingsGrid_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="BookingID" HeaderText="Booking ID" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                        <asp:BoundField DataField="EventName" HeaderText="Event" />
                        <asp:BoundField DataField="BookingDate" HeaderText="Date"
                            DataFormatString="{0:dd/MM/yyyy h:mm tt}" />
                        <asp:BoundField DataField="Venue" HeaderText="Venue" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount"
                            DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:HyperLink ID="BookingDetailsLink" runat="server"
                                    NavigateUrl='<%# Eval("BookingID", "~/AdminBookings.aspx?id={0}") %>'
                                    CssClass="booking-action-link" ToolTip="View booking">
                                    ⋮
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>

        </section>

    </div>

</asp:Content>