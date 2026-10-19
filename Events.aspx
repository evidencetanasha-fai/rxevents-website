<%@ Page Title="Events" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Events.aspx.cs"
    Inherits="RxEvents.Events" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-events-page">

        <!-- Page Heading -->
        <div class="rx-events-heading">

            <div class="rx-events-label">
                <span></span>
                WHAT WE OFFER
                <span></span>
            </div>

            <h1>Our Events</h1>

        </div>


        <!-- Event Categories -->
        <div class="rx-events-grid">

            <asp:Repeater ID="EventsRepeater"
                runat="server">

                <ItemTemplate>

                    <article class="rx-event-card">

                        <div class="rx-event-circle">
    <img src='<%# ResolveUrl("~/Content/images/" + Eval("ImagePath")) %>'
         alt='<%# Eval("CategoryName") %>' />
</div>

                        <h2>
                            <%# Eval("CategoryName") %>
                        </h2>

                        <p>
                            <%# Eval("Description") %>
                        </p>

                        <a href='<%# "EventTypes.aspx?categoryId=" + Eval("CategoryID") %>' 
   class="rx-event-details">
    View Details
</a>

                    </article>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </section>

</asp:Content>