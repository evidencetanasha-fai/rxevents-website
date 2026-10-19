<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Services.aspx.cs"
    Inherits="RxEvents.Services" %>

<asp:Content ID="TitleContent"
    ContentPlaceHolderID="TitleContent"
    runat="server">

    Services | Rx Events Solutions

</asp:Content>


<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <!-- =========================================
         SERVICES PAGE HERO
         ========================================= -->

    <section class="rx-page-hero">

        <div class="rx-page-hero-overlay"></div>

        <div class="rx-page-hero-content">

            <span>
                WHAT WE DO
            </span>

            <h1>
                Our Services
            </h1>

            <p>
                From planning to the final celebration,
                we take care of every detail to create
                an event you'll never forget.
            </p>

        </div>

    </section>


    <!-- =========================================
         SERVICES INTRODUCTION
         ========================================= -->

    <section class="rx-services-intro">

        <span class="rx-section-label">
            MAKING EVERY MOMENT SPECIAL
        </span>

        <h2>
            Everything You Need for Your Perfect Event
        </h2>

        <p>
            Our team provides professional event services
            tailored to your occasion, style and vision.
            Whether you're celebrating a wedding, birthday,
            corporate event or special occasion, we're here
            to bring your ideas to life.
        </p>

    </section>


    <!-- =========================================
         SERVICES GRID
         ========================================= -->

    <section class="rx-services-section">

        <div class="rx-services-grid">

    <asp:Repeater ID="ServicesRepeater"
        runat="server">

        <ItemTemplate>

            <article class="rx-service-card">

                <div class="rx-service-image"
                     style='<%# "background-image: url(\"" + ResolveUrl("~/Content/images/" + Eval("ImagePath")) + "\");" %>'>
                </div>

                <div class="rx-service-content">

                    <span class="rx-service-number">
                        <%# "0" + Eval("ServiceID") %>
                    </span>

                    <h3>
                        <%# Eval("ServiceName") %>
                    </h3>

                    <p>
                        <%# Eval("Description") %>
                    </p>

                    <a href="#">
                        Learn More
                        <span>→</span>
                    </a>

                </div>

            </article>

        </ItemTemplate>

    </asp:Repeater>

</div>

    </section>


    <!-- =========================================
         SERVICES CTA
         ========================================= -->

    <section class="rx-services-cta">

        <div>

            <span>
                HAVE AN EVENT IN MIND?
            </span>

            <h2>
                Let's Bring Your Vision to Life
            </h2>

            <p>
                Tell us what you're planning and let our team
                help you create an unforgettable experience.
            </p>

        </div>

        <a href="#">
            Start Planning
            <span>→</span>
        </a>

    </section>

</asp:Content>