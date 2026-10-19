<%@ Page Title="Support"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="Support.aspx.cs"
    Inherits="RxEvents.Support" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">
    Support
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-support-page">

        <!-- Breadcrumb -->
        <div class="rx-support-breadcrumb">
            <a href="CustomerDashboard.aspx">Home</a>
            <span>›</span>
            <span>Support</span>
        </div>

        <!-- Hero -->
        <section class="rx-support-hero">

            <div class="rx-support-hero-content">

                <span class="rx-support-eyebrow">
                    CUSTOMER SUPPORT
                </span>

                <h2>
                    We're Here to
                    <span>Help You</span>
                </h2>

                <p>
                    Have questions or need assistance?
                    Our support team is ready to help
                    with your event planning journey.
                </p>

            </div>

            <div class="rx-support-hero-decoration">
                <div class="rx-support-hero-card">
                    <span>How can we</span>
                    <strong>help you?</strong>
                    <small>✦</small>
                </div>
            </div>

        </section>

        <!-- Search -->
        <div class="rx-support-search">

            <div class="rx-support-search-icon">
                ⌕
            </div>

            <asp:TextBox
                ID="SupportSearchTextBox"
                runat="server"
                CssClass="rx-support-search-input"
                placeholder="Search for help, e.g. booking, payment, cancellation..." />

            <asp:Button
                ID="SupportSearchButton"
                runat="server"
                Text="Search"
                CssClass="rx-support-search-button"
                CausesValidation="false"
                OnClick="SupportSearchButton_Click" />

        </div>

        <!-- Search Message -->
        <asp:Label
            ID="SupportMessageLabel"
            runat="server"
            CssClass="rx-support-message">
        </asp:Label>

        <!-- Support Categories -->
        <div class="rx-support-categories">

            <!-- Bookings -->
            <div class="rx-support-category-card">

                <div class="rx-support-category-icon">
                    📅
                </div>

                <h3>Bookings</h3>

                <p>
                    Get help with booking, rescheduling
                    or managing your events.
                </p>

                <a href="MyBookings.aspx"
                   class="rx-support-category-link">
                    View Bookings
                    <span>→</span>
                </a>

            </div>

            <!-- Payments -->
            <div class="rx-support-category-card">

                <div class="rx-support-category-icon">
                    💳
                </div>

                <h3>Payments</h3>

                <p>
                    Questions about payment methods,
                    receipts or payment status.
                </p>

                <a href="MyPayments.aspx"
                   class="rx-support-category-link">
                    View Payments
                    <span>→</span>
                </a>

            </div>

            <!-- Account -->
            <div class="rx-support-category-card">

                <div class="rx-support-category-icon">
                    ⚙
                </div>

                <h3>Account Support</h3>

                <p>
                    Need help with your profile,
                    password or account information?
                </p>

                <a href="Profile.aspx"
                   class="rx-support-category-link">
                    Manage Account
                    <span>→</span>
                </a>

            </div>

            <!-- General -->
            <div class="rx-support-category-card">

                <div class="rx-support-category-icon">
                    💬
                </div>

                <h3>General Inquiries</h3>

                <p>
                    Have another question?
                    We're happy to assist you.
                </p>

                <a href="Contact.aspx"
                   class="rx-support-category-link">
                    Contact Us
                    <span>→</span>
                </a>

            </div>

        </div>

        <!-- FAQ -->
        <div class="rx-support-faq-list">

    <asp:Repeater ID="FAQsRepeater" runat="server">

        <ItemTemplate>

            <div class="rx-support-faq-item">

                <strong>
                    <%# Eval("Question") %>
                </strong>

                <span>
                    <%# Eval("Answer") %>
                </span>

            </div>

        </ItemTemplate>

    </asp:Repeater>

    <asp:Panel ID="NoFAQsPanel"
        runat="server"
        Visible="false"
        CssClass="rx-support-no-faqs">

        <strong>No FAQs available</strong>

        <span>
            Frequently asked questions will appear here when they are added.
        </span>

    </asp:Panel>

</div>

        <!-- Still Need Help -->
        <div class="rx-support-contact-banner">

            <div class="rx-support-contact-heading">

                <span class="rx-support-contact-icon">
                    ✦
                </span>

                <div>
                    <h3>Still Need Help?</h3>

                    <p>
                        You can also contact our support team directly.
                    </p>
                </div>

            </div>

            <div class="rx-support-contact-details">

                <div class="rx-support-contact-item">
                    <span>☎</span>

                    <div>
                        <small>Call Us</small>
                        <strong>+91 00000 00000</strong>
                    </div>
                </div>

                <div class="rx-support-contact-item">
                    <span>✉</span>

                    <div>
                        <small>Email Us</small>
                        <strong>support@rxevents.com</strong>
                    </div>
                </div>

                <div class="rx-support-contact-item">
                    <span>◷</span>

                    <div>
                        <small>Support Hours</small>
                        <strong>Mon - Sat: 8:00 AM - 6:00 PM</strong>
                    </div>
                </div>

            </div>

        </div>

    </div>

</asp:Content>