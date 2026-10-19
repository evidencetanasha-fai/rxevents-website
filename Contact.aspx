<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Contact.aspx.cs"
    Inherits="RxEvents.Contact" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <!-- Contact Hero -->
    <section class="rx-contact-hero">

        <div class="rx-contact-hero-content">

            <div class="rx-contact-label">
                <span></span>
                CONTACT US
            </div>

            <h1>We'd Love to Hear<br />From You!</h1>

            <p>
                Have a question, need a quote, or ready to plan your
                event? Get in touch with us today.
            </p>

        </div>

        <div class="rx-contact-hero-image">
            <img src='<%= ResolveUrl("~/Content/images/hero-event.png") %>'
                 alt="Rx Events celebration" />
        </div>

    </section>


    <!-- Contact Content -->
    <section class="rx-contact-content">

        <div class="rx-contact-info">

            <div class="rx-contact-detail">
                <div class="rx-contact-icon">☎</div>

                <div>
                    <span>Phone</span>
                    <strong>
                        <asp:Label ID="PhoneLabel" runat="server"></asp:Label>
                    </strong>
                </div>
            </div>


            <div class="rx-contact-detail">
                <div class="rx-contact-icon">✉</div>

                <div>
                    <span>Email</span>
                    <strong>
                        <asp:Label ID="EmailLabel" runat="server"></asp:Label>
                    </strong>
                </div>
            </div>


            <div class="rx-contact-detail">
                <div class="rx-contact-icon">⌖</div>

                <div>
                    <span>Address</span>
                    <strong>
                        <asp:Label ID="LocationLabel" runat="server"></asp:Label>
                    </strong>
                </div>
            </div>


            <div class="rx-contact-detail">
                <div class="rx-contact-icon">◷</div>

                <div>
                    <span>Business Hours</span>
                    <strong>
                        <asp:Label ID="BusinessHoursLabel" runat="server"></asp:Label>
                    </strong>
                </div>
            </div>

        </div>


        <!-- Contact Form -->
        <div class="rx-contact-form-card">

            <div class="rx-contact-row">

                <div class="rx-contact-field">
                    <asp:TextBox
                        ID="NameTextBox"
                        runat="server"
                        CssClass="rx-contact-input"
                        placeholder="Your Name">
                    </asp:TextBox>
                </div>

                <div class="rx-contact-field">
                    <asp:TextBox
                        ID="EmailTextBox"
                        runat="server"
                        CssClass="rx-contact-input"
                        placeholder="Your Email">
                    </asp:TextBox>
                </div>

            </div>


            <div class="rx-contact-row">

                <div class="rx-contact-field">
                    <asp:TextBox
                        ID="PhoneTextBox"
                        runat="server"
                        CssClass="rx-contact-input"
                        placeholder="Your Phone">
                    </asp:TextBox>
                </div>

                <div class="rx-contact-field">
                    <asp:TextBox
                        ID="SubjectTextBox"
                        runat="server"
                        CssClass="rx-contact-input"
                        placeholder="Subject">
                    </asp:TextBox>
                </div>

            </div>


            <div class="rx-contact-field">

                <asp:TextBox
                    ID="MessageTextBox"
                    runat="server"
                    CssClass="rx-contact-input rx-contact-message"
                    TextMode="MultiLine"
                    Rows="5"
                    placeholder="Your Message">
                </asp:TextBox>

            </div>


            <asp:Button
                ID="SendMessageButton"
                runat="server"
                Text="Send Message"
                CssClass="rx-contact-button"
                OnClick="SendMessageButton_Click" />

            <asp:Label
                ID="MessageLabel"
                runat="server"
                CssClass="rx-contact-message-label">
            </asp:Label>

        </div>

    </section>

</asp:Content>