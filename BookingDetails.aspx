
<%@ Page Title="Booking Details"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="BookingDetails.aspx.cs"
    Inherits="RxEvents.BookingDetails" %>



<asp:Content ID="HeadContent"
    ContentPlaceHolderID="head"
    runat="server">

    <style>
        .booking-details-page {
            padding: 25px;
            color: #20202a;
            font-family: Arial, sans-serif;
        }

        .booking-details-card {
            max-width: 850px;
            margin: 25px auto;
            padding: 28px;
            background: #fff;
            border-radius: 16px;
            box-shadow: 0 4px 18px rgba(0,0,0,.08);
        }

        .booking-details-page h1 {
            color: #7024e8;
            margin-bottom: 10px;
        }

        .booking-details-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 20px;
            margin-top: 25px;
        }

        .booking-detail-item {
            padding: 14px;
            background: #f8f7fc;
            border-radius: 10px;
            overflow-wrap: anywhere;
        }

        .booking-detail-label {
            display: block;
            color: #64748b;
            font-size: 13px;
            margin-bottom: 7px;
        }

        .booking-detail-value {
            font-weight: 600;
        }

        .booking-back-button {
            display: inline-block;
            margin-top: 25px;
            padding: 12px 18px;
            border: 0;
            border-radius: 9px;
            background: #7024e8;
            color: white;
            cursor: pointer;
        }

        @media (max-width: 600px) {
            .booking-details-grid {
                grid-template-columns: 1fr;
            }

            .booking-details-card {
                padding: 18px;
            }
        }
    </style>

</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="booking-details-page">

        <div>Home &nbsp; › &nbsp; My Bookings &nbsp; › &nbsp; Booking Details</div>

        <div class="booking-details-card">

            <h1>Booking Details</h1>

            <p>View the complete information for your event booking.</p>

            <asp:Label ID="MessageLabel"
                runat="server"
                EnableViewState="false" />

            <asp:Panel ID="BookingPanel"
                runat="server"
                Visible="false">

                <div class="booking-details-grid">

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Booking Reference</span>
                        <asp:Label ID="BookingReferenceLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Event</span>
                        <asp:Label ID="EventNameLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Event Date</span>
                        <asp:Label ID="EventDateLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Event Time</span>
                        <asp:Label ID="EventTimeLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Venue</span>
                        <asp:Label ID="VenueLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Package</span>
                        <asp:Label ID="PackageLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Number of Guests</span>
                        <asp:Label ID="GuestCountLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Total Amount</span>
                        <asp:Label ID="AmountLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Booking Status</span>
                        <asp:Label ID="BookingStatusLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Booking Date</span>
                        <asp:Label ID="BookingDateLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                    <div class="booking-detail-item">
                        <span class="booking-detail-label">Additional Notes</span>
                        <asp:Label ID="NotesLabel"
                            runat="server"
                            CssClass="booking-detail-value" />
                    </div>

                </div>

            </asp:Panel>

            <asp:Button ID="BackButton"
                runat="server"
                Text="Back to My Bookings"
                CssClass="booking-back-button"
                CausesValidation="false"
                OnClick="BackButton_Click" />

        </div>
    </div>

</asp:Content>
