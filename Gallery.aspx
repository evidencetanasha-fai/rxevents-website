<%@ Page Title="Gallery" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Gallery.aspx.cs"
    Inherits="RxEvents.Gallery" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-gallery-page">

        <!-- Gallery Heading -->
        <div class="rx-gallery-heading">

            <div class="rx-gallery-label">
                <span></span>
                OUR EVENT PORTFOLIO
                <span></span>
            </div>

            <h1>Moments We've Created</h1>

            <p>
                Take a look at some of the beautiful celebrations
                and memorable moments created by Rx Events Solutions.
            </p>

        </div>

        <!-- Gallery Grid -->
        <div class="rx-gallery-grid">

            <asp:Repeater ID="GalleryRepeater" runat="server">

                <ItemTemplate>

                    <article class="rx-gallery-card">

                        <div class="rx-gallery-image">

                            <img src='<%# ResolveUrl("~/Content/images/" + Eval("ImagePath")) %>'
                                 alt='<%# Eval("EventName") %>' />

                            <div class="rx-gallery-overlay">
                                <span>View Event</span>
                            </div>

                        </div>

                        <div class="rx-gallery-content">

                            <span class="rx-gallery-event">
                                <%# Eval("EventName") %>
                            </span>

                            <h2>
                                <%# Eval("Caption") %>
                            </h2>

                            <div class="rx-gallery-date">
                                <%# String.Format("{0:dd MMM yyyy}", Eval("DateAdded")) %>
                            </div>

                        </div>

                    </article>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </section>

</asp:Content>