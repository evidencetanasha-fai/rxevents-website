<%@ Page Title="Testimonials" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Testimonials.aspx.cs"
    Inherits="RxEvents.Testimonials" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-testimonials-page">

        <!-- Heading -->
        <div class="rx-testimonials-heading">

            <div class="rx-testimonials-label">
                <span></span>
                CLIENT TESTIMONIALS
                <span></span>
            </div>

            <h1>What Our Clients Say</h1>

            <p>
                Every celebration is special to us.
                Here's what our clients have to say about their experiences.
            </p>

        </div>

        <!-- Reviews -->
        <div class="rx-testimonials-grid">

            <asp:Repeater ID="TestimonialsRepeater" runat="server">

                <ItemTemplate>

                    <article class="rx-testimonial-card">

                        <div class="rx-testimonial-top">

                            <div class="rx-testimonial-avatar">
                                <%# Eval("CustomerName").ToString().Substring(0, 1).ToUpper() %>
                            </div>

                            <div class="rx-testimonial-person">

                                <h2>
                                    <%# Eval("CustomerName") %>
                                </h2>

                                <span>
                                    <%# Eval("EventName") %>
                                </span>

                            </div>

                        </div>

                        <!-- Rating -->
                        <div class="rx-testimonial-rating">

                            <%# GetStars(Convert.ToInt32(Eval("Rating"))) %>

                        </div>

                        <!-- Comment -->
                        <div class="rx-testimonial-quote">
                            “
                        </div>

                        <p class="rx-testimonial-comment">
                            <%# Eval("Comment") %>
                        </p>

                        <div class="rx-testimonial-footer">

                            <%# String.Format("{0:dd MMM yyyy}", Eval("ReviewDate")) %>

                        </div>

                    </article>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </section>

</asp:Content>