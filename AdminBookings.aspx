
<%@ Page Title="Bookings" Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdminBookings.aspx.cs"
    Inherits="RxEvents.AdminBookings" %>

<asp:Content ID="BookingsContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="bookings-page">

        <div class="bookings-page-heading">
            <div>
                <h2>All Bookings</h2>
                <p>Manage and track all event bookings</p>
            </div>

            <div class="bookings-heading-actions">
                

<asp:Button ID="ExportBookingsButton"
    runat="server"
    Text="↓ Export CSV"
    CssClass="booking-export-button"
    OnClick="ExportBookingsButton_Click"
    CausesValidation="false" />


                <asp:HyperLink ID="AddNewBookingLink" runat="server"
    NavigateUrl="~/AdminAddBooking.aspx"
    CssClass="add-booking-button">
    + Add New Booking
</asp:HyperLink>
            </div>
        </div>

        <asp:Label ID="BookingsMessage" runat="server"
            CssClass="bookings-message" />

        <!-- SUMMARY CARDS -->
        <div class="booking-summary-grid">

            <div class="booking-summary-card">
                <div class="booking-summary-icon total-icon">▦</div>
                <div>
                    <span>Total Bookings</span>
                    <strong>
                        <asp:Literal ID="TotalBookingsCount" runat="server"
                            Text="0" />
                    </strong>
                </div>
            </div>

            <div class="booking-summary-card">
                <div class="booking-summary-icon confirmed-icon">▦</div>
                <div>
                    <span>Confirmed</span>
                    <strong>
                        <asp:Literal ID="ConfirmedBookingsCount" runat="server"
                            Text="0" />
                    </strong>
                </div>
            </div>

            <div class="booking-summary-card">
                <div class="booking-summary-icon completed-icon">✓</div>
                <div>
                    <span>Completed</span>
                    <strong>
                        <asp:Literal ID="CompletedBookingsCount" runat="server"
                            Text="0" />
                    </strong>
                </div>
            </div>

            <div class="booking-summary-card">
                <div class="booking-summary-icon cancelled-icon">×</div>
                <div>
                    <span>Cancelled</span>
                    <strong>
                        <asp:Literal ID="CancelledBookingsCount" runat="server"
                            Text="0" />
                    </strong>
                </div>
            </div>

        </div>

        <!-- MAIN CONTENT -->
        <div class="bookings-main-grid">

            <div class="bookings-table-panel">

                <!-- FILTERS -->
                <div class="bookings-filter-panel">

                    <div class="booking-filter-row">

                        <asp:TextBox ID="SearchBookingsTextBox" runat="server"
                            CssClass="booking-search-input"
                            placeholder="Search by Booking ID, Customer..." />

                        <div class="booking-filter-field">
                            <label for="<%= StatusFilter.ClientID %>">
                                Status
                            </label>

                            <asp:DropDownList ID="StatusFilter" runat="server"
                                CssClass="booking-filter-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="BookingFilters_Changed">
                                <asp:ListItem Value="">All Status</asp:ListItem>
                                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                <asp:ListItem Value="Confirmed">Confirmed</asp:ListItem>
                                <asp:ListItem Value="Completed">Completed</asp:ListItem>
                                <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                                <asp:ListItem Value="Rejected">Rejected</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="booking-filter-field">
                            <label for="<%= EventFilter.ClientID %>">
                                Events
                            </label>

                            <asp:DropDownList ID="EventFilter" runat="server"
                                CssClass="booking-filter-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="BookingFilters_Changed">
                                <asp:ListItem Value="">All Events</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <asp:Button ID="SearchBookingsButton" runat="server"
                            Text="Search"
                            CssClass="booking-filter-action"
                            OnClick="SearchBookingsButton_Click" />

                    </div>

                    <div class="booking-filter-row booking-date-row">

                        <div class="booking-filter-field booking-date-field">
                            <label for="<%= StartDateTextBox.ClientID %>">
                                From Date
                            </label>
                            <asp:TextBox ID="StartDateTextBox" runat="server"
                                TextMode="Date"
                                CssClass="booking-filter-select" />
                        </div>

                        <div class="booking-filter-field booking-date-field">
                            <label for="<%= EndDateTextBox.ClientID %>">
                                To Date
                            </label>
                            <asp:TextBox ID="EndDateTextBox" runat="server"
                                TextMode="Date"
                                CssClass="booking-filter-select" />
                        </div>

                        <asp:Button ID="ApplyDateFilterButton" runat="server"
                            Text="Apply Filters"
                            CssClass="booking-filter-action"
                            OnClick="SearchBookingsButton_Click" />

                        <asp:Button ID="ClearBookingFiltersButton" runat="server"
                            Text="Clear All Filters"
                            CssClass="booking-clear-button"
                            CausesValidation="false"
                            OnClick="ClearBookingFiltersButton_Click" />

                    </div>
                </div>

                <!-- BOOKINGS TABLE -->
                <div class="booking-grid-scroll">

                    <asp:GridView ID="BookingsGrid" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="bookings-data-grid"
                        GridLines="None"
                        DataKeyNames="BookingID"
                        AllowPaging="True"
                        PageSize="3"
                        OnPageIndexChanging="BookingsGrid_PageIndexChanging"
                        OnSelectedIndexChanged="BookingsGrid_SelectedIndexChanged"
                        OnRowDataBound="BookingsGrid_RowDataBound"
                        EmptyDataText="No bookings found.">

                        <Columns>

                            <asp:BoundField DataField="BookingReference"
                                HeaderText="Booking ID" />

                            <asp:BoundField DataField="CustomerName"
                                HeaderText="Customer" />

                            <asp:BoundField DataField="EventName"
                                HeaderText="Event" />

                            <asp:BoundField DataField="EventDate"
                                HeaderText="Date &amp; Time"
                                DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:BoundField DataField="VenueName"
                                HeaderText="Venue" />

                            <asp:BoundField DataField="TotalAmount"
                                HeaderText="Total Amount"
                                DataFormatString="{0:N0}" />

                            <asp:BoundField DataField="BookingStatus"
                                HeaderText="Status" />

                            <asp:CommandField ShowSelectButton="True"
                                SelectText="View"
                                HeaderText="Action" />

                        </Columns>

                        <PagerStyle CssClass="booking-pager" />

                    </asp:GridView>

                </div>

                <div class="booking-table-footer">
                    <asp:Label ID="BookingResultsLabel" runat="server"
                        Text="Showing bookings" />
                </div>

            </div>

            <!-- BOOKING DETAILS -->
            <aside class="booking-details-panel">

                <h3>Booking Details</h3>

                <asp:Image ID="BookingEventImage" runat="server"
                    CssClass="booking-details-image"
                    AlternateText="Selected event image"
                    Visible="false" />

                <div class="booking-detail-reference">
                    <div>
                        <span>Booking ID</span>
                        <strong>
                            <asp:Literal ID="SelectedBookingReference"
                                runat="server" Text="Select a booking" />
                        </strong>
                    </div>

                    <asp:Label ID="SelectedBookingStatus" runat="server"
                        CssClass="booking-status-badge" />
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">♙</span>
                    <div>
                        <span>Customer</span>
                        <strong>
                            <asp:Literal ID="SelectedCustomerName"
                                runat="server" Text="—" />
                        </strong>
                        <span>
                            <asp:Literal ID="SelectedCustomerPhone"
                                runat="server" />
                        </span>
                        <span>
                            <asp:Literal ID="SelectedCustomerEmail"
                                runat="server" />
                        </span>
                    </div>
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">▦</span>
                    <div>
                        <span>Event</span>
                        <strong>
                            <asp:Literal ID="SelectedEventName"
                                runat="server" Text="—" />
                        </strong>
                    </div>
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">▦</span>
                    <div>
                        <span>Date &amp; Time</span>
                        <strong>
                            <asp:Literal ID="SelectedEventDate"
                                runat="server" Text="—" />
                        </strong>
                    </div>
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">●</span>
                    <div>
                        <span>Location</span>
                        <strong>
                            <asp:Literal ID="SelectedVenueName"
                                runat="server" Text="—" />
                        </strong>
                    </div>
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">◇</span>
                    <div>
                        <span>Package</span>
                        <strong>
                            <asp:Literal ID="SelectedPackageName"
                                runat="server" Text="—" />
                        </strong>
                    </div>
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">$</span>
                    <div>
                        <span>Total Amount</span>
                        <strong>
                            <asp:Literal ID="SelectedTotalAmount"
                                runat="server" Text="0" />
                        </strong>
                    </div>
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">$</span>
                    <div>
                        <span>Paid Amount</span>
                        <strong>
                            <asp:Literal ID="SelectedPaidAmount"
                                runat="server" Text="0" />
                        </strong>
                    </div>
                </div>

                <div class="booking-detail-item">
                    <span class="booking-detail-icon">✓</span>
                    <div>
                        <span>Payment Status</span>
                        <strong>
                            <asp:Literal ID="SelectedPaymentStatus"
                                runat="server" Text="—" />
                        </strong>
                    </div>
                </div>

                <asp:HyperLink ID="ViewFullBookingLink" runat="server"
                    NavigateUrl="~/AdminBookings.aspx"
                    CssClass="booking-view-details-button">
                    View Full Details →
                </asp:HyperLink>

            </aside>

        </div>

    </div>

</asp:Content>
