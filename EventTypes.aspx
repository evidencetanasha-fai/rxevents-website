<%@ Page Title="Event Details" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="EventTypes.aspx.cs"
    Inherits="RxEvents.EventTypes" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-event-types-page">

        <div class="rx-event-types-back">
    <a href="Events.aspx">
        <span>←</span>
        Back to Events
    </a>
</div>

        <!-- Page Header -->
        <div class="rx-event-types-header">

            <span>EXPLORE OUR EVENTS</span>

            <h1>
                <asp:Label ID="CategoryTitle"
                    runat="server">
                </asp:Label>
            </h1>

            <p>
                Explore our event options and choose the perfect
                celebration for your special occasion.
            </p>

        </div>


        <!-- Event Types -->
        <div class="rx-event-types-grid">

            <asp:Repeater ID="EventTypesRepeater"
                runat="server">

                <ItemTemplate>

                    <article class="rx-event-type-card">

                        <div class="rx-event-type-image">

                            <img src='<%# ResolveUrl("~/Content/images/" + Eval("EventImage")) %>'
                                 alt='<%# Eval("EventName") %>' />

                        </div>

                        <div class="rx-event-type-content">

                            <h2>
                                <%# Eval("EventName") %>
                            </h2>

                            <p>
                                <%# Eval("Description") %>
                            </p>

                            <div class="rx-event-type-bottom">

                                <div class="rx-event-price">
                                    <small>Starting from</small>
                                    <strong>
                                        ₹<%# String.Format("{0:N0}", Eval("BasePrice")) %>
                                    </strong>
                                </div>

                                <!--<a href='<%# "EventDetails.aspx?eventId=" + Eval("EventID") %>'
                                   class="rx-event-details">
                                    View Details
                                    <span>→</span>
                                </a>-->

                            </div>

                        </div>

                    </article>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </section>

</asp:Content>