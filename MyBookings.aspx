<%@ Page Title="My Bookings"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="MyBookings.aspx.cs"
    Inherits="RxEvents.MyBookings" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="head"
    runat="server">

    <style>
        .my-bookings-page {
            padding: 25px;
            color: #171717;
            font-family: Arial, sans-serif;
        }

        .booking-breadcrumb {
            color: #64748b;
            margin-bottom: 25px;
            font-size: 15px;
        }

        .my-bookings-page h1 {
            font-size: 28px;
            margin-bottom: 8px;
        }

        .booking-subtitle {
            color: #475569;
            margin-bottom: 25px;
        }

        .booking-toolbar {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 10px;
            border-bottom: 1px solid #d1d5db;
            padding-bottom: 12px;
            margin-bottom: 20px;
        }

        .booking-tab {
            color: #171717;
            text-decoration: none;
            padding: 10px 12px;
            font-weight: 600;
        }

        .booking-tab-active {
            color: #7024e8;
            border-bottom: 3px solid #7024e8;
        }

        .booking-search {
            width: 200px;
            max-width: 100%;
            padding: 10px 12px;
            border: 1px solid #d1d5db;
            border-radius: 10px;
        }

        .booking-filter,
        .booking-button {
            padding: 10px 14px;
            border: 1px solid #7c3aed;
            border-radius: 10px;
            cursor: pointer;
        }

        .booking-filter {
            background: white;
            color: #7024e8;
        }

        .booking-button {
            background: #7024e8;
            color: white;
        }

        .booking-message {
            display: block;
            margin: 12px 0;
            color: #475569;
        }

        .booking-list-item {
            display: grid;
            grid-template-columns: 190px minmax(180px, 1fr)
                                   170px auto;
            gap: 18px;
            align-items: center;
            padding: 20px 0;
            border-bottom: 1px solid #e5e7eb;
        }

        .booking-image {
            width: 100%;
            height: 135px;
            object-fit: cover;
            border-radius: 5px;
        }

        .booking-event-name {
            color: #7024e8;
            font-size: 20px;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .booking-detail {
            margin: 7px 0;
            overflow-wrap: anywhere;
        }

        .booking-status {
            display: inline-block;
            padding: 10px 13px;
            border-radius: 12px;
            margin-bottom: 14px;
            font-weight: bold;
        }

        .status-upcoming {
            background: #e8edb5;
            color: #946200;
        }

        .status-completed {
            background: #e5e7eb;
            color: #16803d;
        }

        .status-cancelled {
            background: #e5e7eb;
            color: #dc2626;
        }

        .status-pending {
            background: #fef3c7;
            color: #92400e;
        }

        .booking-package {
            color: #7024e8;
            font-weight: 600;
            margin-bottom: 12px;
        }

        .booking-amount {
            font-weight: bold;
        }

        @media (max-width: 900px) {
            .booking-list-item {
                grid-template-columns: 140px minmax(150px, 1fr);
            }
        }

        @media (max-width: 600px) {
            .my-bookings-page {
                padding: 15px;
            }

            .booking-list-item {
                grid-template-columns: 1fr;
            }

            .booking-image {
                height: 190px;
            }
        }
    </style>

</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="my-bookings-page">

        <div class="booking-breadcrumb">
            Home &nbsp; › &nbsp; My Bookings
            &nbsp; › &nbsp; Booking History
        </div>

        <h1>Booking History</h1>

        <p class="booking-subtitle">
            View and manage all your event bookings.
        </p>

        <asp:Label ID="MessageLabel"
            runat="server"
            CssClass="booking-message"
            EnableViewState="false" />

        <div class="booking-toolbar">

            <asp:LinkButton ID="AllTab"
                runat="server"
                CssClass="booking-tab booking-tab-active"
                CommandArgument="All"
                OnCommand="StatusTab_Command">
                All (<asp:Label ID="AllCountLabel" runat="server" />)
            </asp:LinkButton>

            <asp:LinkButton ID="UpcomingTab"
                runat="server"
                CssClass="booking-tab"
                CommandArgument="Upcoming"
                OnCommand="StatusTab_Command">
                Upcoming (<asp:Label ID="UpcomingCountLabel" runat="server" />)
            </asp:LinkButton>

            <asp:LinkButton ID="CompletedTab"
                runat="server"
                CssClass="booking-tab"
                CommandArgument="Completed"
                OnCommand="StatusTab_Command">
                Completed (<asp:Label ID="CompletedCountLabel" runat="server" />)
            </asp:LinkButton>

            <asp:LinkButton ID="CancelledTab"
                runat="server"
                CssClass="booking-tab"
                CommandArgument="Cancelled"
                OnCommand="StatusTab_Command">
                Cancelled (<asp:Label ID="CancelledCountLabel" runat="server" />)
            </asp:LinkButton>

            <asp:TextBox ID="SearchTextBox"
                runat="server"
                CssClass="booking-search"
                MaxLength="100"
                placeholder="Search bookings..." />

            <asp:DropDownList ID="StatusFilter"
                runat="server"
                CssClass="booking-filter">
                <asp:ListItem Text="All statuses" Value="All" />
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Confirmed" Value="Confirmed" />
                <asp:ListItem Text="Completed" Value="Completed" />
                <asp:ListItem Text="Cancelled" Value="Cancelled" />
            </asp:DropDownList>

            <asp:Button ID="SearchButton"
                runat="server"
                Text="Search"
                CssClass="booking-button"
                OnClick="SearchButton_Click" />

        </div>

        <asp:Repeater ID="BookingsRepeater"
            runat="server">

            <ItemTemplate>

                <div class="booking-list-item">

                    <asp:Image ID="EventImage"
                        runat="server"
                        CssClass="booking-image"
                        ImageUrl='<%# Eval("EventImage") %>'
                        AlternateText="Event image" />

                    <div>
                        <div class="booking-event-name">
                            <%#: Eval("EventName") %>
                        </div>

                        <div class="booking-detail">
                            <%#: Eval("EventDate", "{0:dd MMM yyyy}") %>,
                            <%#: Eval("EventTime", "{0:hh\\:mm}") %>
                        </div>

                        <div class="booking-detail">
                            <%#: Eval("VenueAddress") %>
                        </div>

                        <div class="booking-detail">
                            Booking ID: <%#: Eval("BookingReference") %>
                        </div>
                    </div>

                    <div>
                        <span class='<%# GetStatusCss(
                            Eval("BookingStatus"),
                            Eval("EventDate")) %>'>
                            <%#: Eval("BookingStatus") %>
                        </span>

                        <div class="booking-package">
                            <%#: Eval("PackageName") %>
                        </div>

                        <div class="booking-amount">
                            UGX <%#: Eval("TotalAmount", "{0:N0}") %>
                        </div>
                    </div>

                    <div>
                        <asp:Button ID="ViewDetailsButton"
                            runat="server"
                            Text="View Details"
                            CssClass="booking-button"
                            CausesValidation="false"
                            CommandArgument='<%# Eval("BookingID") %>'
                            OnCommand="ViewDetails_Command" />
                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>

    </div>

</asp:Content>