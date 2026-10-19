<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Home.aspx.cs"
    Inherits="RxEvents.Home" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Home | Rx Events Solutions
</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <!-- =========================================
         HERO SECTION
         ========================================= -->

    <section class="rx-hero">

        <div class="rx-hero-background"></div>

        <div class="rx-hero-overlay"></div>

        <div class="rx-hero-content">

            <div class="rx-hero-eyebrow">

                <span class="rx-eyebrow-line"></span>

                PLAN YOUR DREAM EVENT

            </div>


            <h1>

                Professional Event
                <br />

                Planning for
                <span>Weddings,</span>

                <br />

                <span>Birthday Parties,</span>

                <br />

                Corporate Events &amp; More

            </h1>


            <p class="rx-hero-description">

                From concept to celebration, we handle every detail
                so you can enjoy every moment.

            </p>


            <!-- Hero Buttons -->

            <div class="rx-hero-buttons">

                <a href="#"
                   class="rx-hero-btn rx-hero-btn-primary">

                    Book Now

                    <span class="rx-button-arrow">
                        →
                    </span>

                </a>


                <a href="#"
                   class="rx-hero-btn rx-hero-btn-secondary">

                    View Packages

                    <span class="rx-button-arrow">
                        →
                    </span>

                </a>

            </div>


            <!-- Trust Features -->

            <div class="rx-hero-features">

                <div class="rx-hero-feature">

                    <div class="rx-feature-icon">
                        ♡
                    </div>

                    <div>
                        <strong>
                            Personalized
                        </strong>

                        <span>
                            Experiences
                        </span>
                    </div>

                </div>


                <div class="rx-hero-feature">

                    <div class="rx-feature-icon">
                        ☆
                    </div>

                    <div>
                        <strong>
                            Professional
                        </strong>

                        <span>
                            Team
                        </span>
                    </div>

                </div>


                <div class="rx-hero-feature">

                    <div class="rx-feature-icon">
                        ♢
                    </div>

                    <div>
                        <strong>
                            Reliable
                        </strong>

                        <span>
                            &amp; Hassle-Free
                        </span>
                    </div>

                </div>

            </div>

        </div>


        <!-- Quote Card -->

        <div class="rx-hero-quote">

            <div class="rx-quote-mark">
                “
            </div>

            <p>
                Beautiful events,
                <br />
                happier people.
            </p>

            <div class="rx-quote-line"></div>

        </div>


        <!-- Scroll Indicator -->

        <!--<div class="rx-scroll-indicator">

            <div class="rx-scroll-icon">
                ↓
            </div>

            <span>
                SCROLL DOWN
            </span>

            <div class="rx-scroll-line"></div>

        </div>-->

    </section>

</asp:Content>