<%@ Page Title="Customer Dashboard"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="CustomerDashboard.aspx.cs"
    Inherits="RxEvents.CustomerDashboard" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-customer-dashboard">


        <!-- ================= WELCOME BANNER ================= -->

        <div class="rx-dashboard-welcome">

            <div class="rx-dashboard-welcome-content">

                <div class="rx-dashboard-welcome-small">
                    YOUR NEXT BEAUTIFUL MOMENT
                </div>

                <h2>
                    Welcome back,
                    <asp:Label
                        ID="CustomerNameLabel"
                        runat="server">
                    </asp:Label>!
                </h2>

                <p>
                    Let's create more beautiful memories.
                </p>

                <a href="BookEvent.aspx"
                   class="rx-dashboard-book-button">

                    Book a New Event

                    <span>→</span>

                </a>

            </div>


            <div class="rx-dashboard-welcome-image">

                <img src="Content/images/hero-event.png"
                     alt="Beautiful Event" />

                <div class="rx-dashboard-welcome-overlay">

                    <span>Moments</span>
                    <span>That Last</span>
                    <span>Forever</span>

                </div>

            </div>

        </div>


        <!-- ================= STATISTICS ================= -->

        <div class="rx-dashboard-stats">


            <div class="rx-dashboard-stat-card">

                <div class="rx-dashboard-stat-icon">
                    ♧
                </div>

                <div class="rx-dashboard-stat-content">

                    <span>Total Bookings</span>

                    <strong>
                        <asp:Label
                            ID="TotalBookingsLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>

                    <small>
                        All Time Bookings
                    </small>

                </div>

                <div class="rx-stat-arrow">
                    →
                </div>

            </div>


            <div class="rx-dashboard-stat-card">

                <div class="rx-dashboard-stat-icon gold">
                    ◉
                </div>

                <div class="rx-dashboard-stat-content">

                    <span>Total Spent</span>

                    <strong>
                        ₹
                        <asp:Label
                            ID="TotalSpentLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>

                    <small>
                        All Time Payments
                    </small>

                </div>

            </div>


            <div class="rx-dashboard-stat-card">

                <div class="rx-dashboard-stat-icon purple">
                    ☆
                </div>

                <div class="rx-dashboard-stat-content">

                    <span>Reviews Given</span>

                    <strong>
                        <asp:Label
                            ID="ReviewsGivenLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>

                    <small>
                        Thank you for your feedback!
                    </small>

                </div>

                <div class="rx-stat-arrow">
                    →
                </div>

            </div>

        </div>


        <!-- ================= DASHBOARD GRID ================= -->

        <div class="rx-dashboard-grid">


            <!-- Recent Bookings -->

            <div class="rx-dashboard-bookings-card">

                <div class="rx-dashboard-card-header">

                    <div>

                        <h2>Recent Bookings</h2>

                        <span></span>

                    </div>

                    <a href="MyBookings.aspx">
                        View All →
                    </a>

                </div>


                <asp:GridView
                    ID="RecentBookingsGridView"
                    runat="server"
                    AutoGenerateColumns="false"
                    CssClass="rx-dashboard-table"
                    GridLines="None">

                    <Columns>

                        <asp:BoundField
                            DataField="EventName"
                            HeaderText="Event" />

                        <asp:BoundField
                            DataField="EventDate"
                            HeaderText="Date"
                            DataFormatString="{0:dd MMM yyyy}" />

                        <asp:BoundField
                            DataField="VenueName"
                            HeaderText="Venue" />

                        <asp:BoundField
                            DataField="Status"
                            HeaderText="Status" />

                        <asp:BoundField
                            DataField="TotalAmount"
                            HeaderText="Total"
                            DataFormatString="₹ {0:N0}" />

                        <asp:TemplateField
                            HeaderText="Action">

                            <ItemTemplate>

                                <a href="#"
                                   class="rx-table-view">
                                    View
                                </a>

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>


            <!-- Upcoming Events -->

            <div class="rx-dashboard-upcoming-card">

                <div class="rx-dashboard-card-header">

                    <div>

                        <h2>Upcoming Events</h2>

                    </div>

                    <a href="MyBookings.aspx">
                        View All →
                    </a>

                </div>


                <asp:Repeater
                    ID="UpcomingEventsRepeater"
                    runat="server">

                    <ItemTemplate>

                        <div class="rx-upcoming-event">

                            <div class="rx-upcoming-image">

                                <img src='<%# ResolveUrl("~/Content/images/" + Eval("EventImage")) %>'
                                     alt='<%# Eval("EventName") %>' />

                            </div>


                            <div class="rx-upcoming-details">

                                <strong>
                                    <%# Eval("EventName") %>
                                </strong>

                                <span>
                                    ◷
                                    <%# String.Format("{0:dd MMM yyyy}", Eval("EventDate")) %>
                                </span>

                                <span>
                                    ⌖
                                    <%# Eval("VenueName") %>
                                </span>

                            </div>


                            <div class="rx-upcoming-arrow">
                                →
                            </div>

                        </div>

                    </ItemTemplate>

                </asp:Repeater>

            </div>


            <!-- Special Offer -->

            <div class="rx-dashboard-offer-card">

                <div class="rx-offer-content">

                    <span>Special Offer</span>

                    <strong>20%</strong>

                    <b>OFF</b>

                    <small>
                        ON SELECTED PACKAGES
                    </small>

                    <a href="Packages.aspx">
                        Explore Packages →
                    </a>

                </div>

            </div>


            <!-- Quote -->

            <div class="rx-dashboard-quote-card">

                <span class="rx-quote-mark">“</span>

                <p>
                    Because every moment
                    matters.
                </p>

                <div></div>

            </div>

        </div>

    </section>

</asp:Content>