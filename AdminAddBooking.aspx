
<%@ Page Title="Add New Booking"
    Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdminAddBooking.aspx.cs"
    Inherits="RxEvents.AdminAddBooking" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="add-booking-page">
        <div class="add-booking-heading">
            <div>
                <h2>Add New Booking</h2>
                <p>Create a booking for a customer at Rx Events Solutions.</p>
            </div>

            <asp:HyperLink ID="BackToBookingsLink" runat="server"
                NavigateUrl="~/AdminBookings.aspx"
                CssClass="booking-back-button">
                Back to Bookings
            </asp:HyperLink>
        </div>

        <asp:Label ID="BookingMessage" runat="server"
            CssClass="booking-form-message" />

        <div class="add-booking-card">
            <h3>Customer &amp; Event Details</h3>

            <div class="add-booking-grid">
                
<div class="booking-form-field">
    <asp:Label ID="CustomerLabel" runat="server"
        AssociatedControlID="CustomerNameInput"
        Text="Customer Name *" />

    <asp:TextBox ID="CustomerNameInput" runat="server"
        CssClass="booking-form-control"
        MaxLength="200"
        placeholder="Enter customer name" />
</div>


                <div class="booking-form-field">
                    <asp:Label ID="EventLabel" runat="server"
                        AssociatedControlID="EventDropdown"
                        Text="Event *" />

                    <asp:DropDownList ID="EventDropdown" runat="server"
                        CssClass="booking-form-control"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="EventDropdown_SelectedIndexChanged" />
                </div>

                <div class="booking-form-field">
                    <asp:Label ID="VenueLabel" runat="server"
                        AssociatedControlID="VenueDropdown"
                        Text="Venue" />

                    <asp:DropDownList ID="VenueDropdown" runat="server"
                        CssClass="booking-form-control" />
                </div>

                <div class="booking-form-field">
                    <asp:Label ID="PackageLabel" runat="server"
                        AssociatedControlID="PackageDropdown"
                        Text="Package *" />

                    <asp:DropDownList ID="PackageDropdown" runat="server"
                        CssClass="booking-form-control"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="PackageDropdown_SelectedIndexChanged" />
                </div>

                <div class="booking-form-field">
                    <asp:Label ID="EventDateLabel" runat="server"
                        AssociatedControlID="EventDateInput"
                        Text="Event Date *" />

                    <asp:TextBox ID="EventDateInput" runat="server"
                        TextMode="Date"
                        CssClass="booking-form-control" />
                </div>

                <div class="booking-form-field">
                    <asp:Label ID="EventTimeLabel" runat="server"
                        AssociatedControlID="EventTimeInput"
                        Text="Event Time *" />

                    <asp:TextBox ID="EventTimeInput" runat="server"
                        TextMode="Time"
                        CssClass="booking-form-control" />
                </div>

                <div class="booking-form-field">
                    <asp:Label ID="GuestCountLabel" runat="server"
                        AssociatedControlID="GuestCountInput"
                        Text="Number of Guests *" />

                    <asp:TextBox ID="GuestCountInput" runat="server"
                        TextMode="Number"
                        Text="1"
                        CssClass="booking-form-control" />
                </div>

                <div class="booking-form-field">
                    <asp:Label ID="TotalAmountLabel" runat="server"
                        AssociatedControlID="TotalAmountInput"
                        Text="Total Amount *" />

                    <asp:TextBox ID="TotalAmountInput" runat="server"
                        TextMode="Number"
                        CssClass="booking-form-control" />
                </div>

                <div class="booking-form-field">
                    <asp:Label ID="BookingStatusLabel" runat="server"
                        AssociatedControlID="BookingStatusDropdown"
                        Text="Booking Status *" />

                    <asp:DropDownList ID="BookingStatusDropdown"
                        runat="server"
                        CssClass="booking-form-control">
                        <asp:ListItem Value="Pending">Pending</asp:ListItem>
                        <asp:ListItem Value="Confirmed">Confirmed</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="booking-form-field full-width">
                    <asp:Label ID="VenueAddressLabel" runat="server"
                        AssociatedControlID="VenueAddressInput"
                        Text="Venue Address" />

                    <asp:TextBox ID="VenueAddressInput" runat="server"
                        CssClass="booking-form-control"
                        MaxLength="500" />
                </div>

                <div class="booking-form-field full-width">
                    <asp:Label ID="BookingNotesLabel" runat="server"
                        AssociatedControlID="BookingNotesInput"
                        Text="Additional Notes" />

                    <asp:TextBox ID="BookingNotesInput" runat="server"
                        TextMode="MultiLine"
                        Rows="4"
                        CssClass="booking-form-control"
                        MaxLength="1000" />
                </div>
            </div>

            <div class="add-booking-actions">
                <asp:Button ID="CancelBookingButton" runat="server"
                    Text="Cancel"
                    CssClass="booking-cancel-button"
                    CausesValidation="false"
                    PostBackUrl="~/AdminBookings.aspx" />

                <asp:Button ID="SaveBookingButton" runat="server"
                    Text="Create Booking"
                    CssClass="booking-save-button"
                    OnClick="SaveBookingButton_Click" />
            </div>
        </div>
    </div>

</asp:Content>
