
<%@ Page Title="Web Settings" Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdminWebSettings.aspx.cs"
    Inherits="RxEvents.AdminWebSettings" %>

<asp:Content ID="WebSettingsContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="web-settings-page">

        <div class="web-settings-heading">
            <div>
                <h2>Web Settings</h2>
                <p>Manage website settings and general information.</p>
            </div>
        </div>

        <asp:Label ID="WebSettingsMessage" runat="server"
            CssClass="web-settings-message" />

        <div class="web-settings-grid">

            <!-- SITE IDENTITY -->
            <section class="web-settings-card">
                <div class="web-settings-card-heading">
                    <div class="web-settings-icon">▣</div>
                    <div>
                        <h3>Site Identity</h3>
                        <p>Update your website name, logo and favicon.</p>
                    </div>
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="WebsiteNameTextBox"
                        Text="Website Name" />
                    <asp:TextBox ID="WebsiteNameTextBox" runat="server"
                        MaxLength="150" />
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server" Text="Website Logo" />

                    <asp:Image ID="LogoPreview" runat="server"
                        CssClass="web-settings-logo-preview"
                        AlternateText="Website logo"
                        Visible="false" />

                    <asp:FileUpload ID="LogoUpload" runat="server"
                        CssClass="web-settings-file" />
                    <small>Recommended: transparent PNG.</small>
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server" Text="Website Favicon" />

                    <asp:Image ID="FaviconPreview" runat="server"
                        CssClass="web-settings-favicon-preview"
                        AlternateText="Website favicon"
                        Visible="false" />

                    <asp:FileUpload ID="FaviconUpload" runat="server"
                        CssClass="web-settings-file" />
                    <small>Recommended: square PNG or ICO.</small>
                </div>
            </section>

            <!-- CONTACT INFORMATION -->
            <section class="web-settings-card">
                <div class="web-settings-card-heading">
                    <div class="web-settings-icon">☎</div>
                    <div>
                        <h3>Contact Information</h3>
                        <p>Contact details displayed on your website.</p>
                    </div>
                </div>

                <div class="web-settings-form-grid">
                    <div class="web-settings-field">
                        <asp:Label runat="server"
                            AssociatedControlID="CompanyEmailTextBox"
                            Text="Email Address" />
                        <asp:TextBox ID="CompanyEmailTextBox" runat="server"
                            TextMode="Email" MaxLength="255" />
                    </div>

                    <div class="web-settings-field">
                        <asp:Label runat="server"
                            AssociatedControlID="CompanyPhoneTextBox"
                            Text="Phone Number" />
                        <asp:TextBox ID="CompanyPhoneTextBox" runat="server"
                            MaxLength="50" />
                    </div>

                    <div class="web-settings-field">
                        <asp:Label runat="server"
                            AssociatedControlID="CompanyAddressTextBox"
                            Text="Address" />
                        <asp:TextBox ID="CompanyAddressTextBox" runat="server"
                            TextMode="MultiLine" Rows="3"
                            MaxLength="500" />
                    </div>

                    <div class="web-settings-field">
                        <asp:Label runat="server"
                            AssociatedControlID="WorkingHoursTextBox"
                            Text="Working Hours" />
                        <asp:TextBox ID="WorkingHoursTextBox" runat="server"
                            TextMode="MultiLine" Rows="3"
                            MaxLength="250" />
                    </div>
                </div>

                <h4 class="web-settings-subheading">Social Media Links</h4>

                <div class="web-settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="FacebookTextBox"
                        Text="Facebook URL" />
                    <asp:TextBox ID="FacebookTextBox" runat="server"
                        MaxLength="500" />
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="InstagramTextBox"
                        Text="Instagram URL" />
                    <asp:TextBox ID="InstagramTextBox" runat="server"
                        MaxLength="500" />
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="XTextBox"
                        Text="X URL" />
                    <asp:TextBox ID="XTextBox" runat="server"
                        MaxLength="500" />
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="LinkedInTextBox"
                        Text="LinkedIn URL" />
                    <asp:TextBox ID="LinkedInTextBox" runat="server"
                        MaxLength="500" />
                </div>
            </section>

            <!-- ABOUT SECTION -->
            <section class="web-settings-card">
                <div class="web-settings-card-heading">
                    <div class="web-settings-icon">▤</div>
                    <div>
                        <h3>About Section</h3>
                        <p>Edit the description displayed on your website.</p>
                    </div>
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="AboutTextBox"
                        Text="About Us" />
                    <asp:TextBox ID="AboutTextBox" runat="server"
                        TextMode="MultiLine" Rows="5"
                        MaxLength="500" />
                </div>
            </section>

            <!-- FOOTER SETTINGS -->
            <section class="web-settings-card">
                <div class="web-settings-card-heading">
                    <div class="web-settings-icon">▤</div>
                    <div>
                        <h3>Footer Settings</h3>
                        <p>Manage the website footer and copyright text.</p>
                    </div>
                </div>

                <div class="web-settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="FooterTextBox"
                        Text="Footer Text" />
                    <asp:TextBox ID="FooterTextBox" runat="server"
                        TextMode="MultiLine" Rows="4"
                        MaxLength="500" />
                </div>

                <div class="web-settings-actions">
                    <asp:Button ID="SaveWebSettingsButton"
                        runat="server"
                        Text="Save Settings"
                        CssClass="web-settings-save-button"
                        OnClick="SaveWebSettingsButton_Click" />
                </div>
            </section>

        </div>
    </div>

</asp:Content>
