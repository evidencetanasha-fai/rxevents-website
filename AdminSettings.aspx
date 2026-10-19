<%@ Page Title="Settings" Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdminSettings.aspx.cs"
    Inherits="RxEvents.AdminSettings"
    EnableViewState="false" %>

<asp:Content ID="SettingsContent"
    ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-settings-page">

        <div class="settings-page-heading">
            <div>
                <h2>Settings</h2>
                <p>Manage your administrator account and preferences.</p>
            </div>
        </div>

        <asp:Label ID="SettingsMessage" runat="server"
            CssClass="settings-message" />

        <section class="settings-card">
            <div class="settings-card-heading">
                <h3>Account Information</h3>
                <p>Update the details associated with your administrator account.</p>
            </div>

            <div class="settings-form-grid">
                <div class="settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="FirstNameTextBox"
                        Text="First Name" />
                    <asp:TextBox ID="FirstNameTextBox" runat="server"
                        MaxLength="100" />
                </div>

                <div class="settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="LastNameTextBox"
                        Text="Last Name" />
                    <asp:TextBox ID="LastNameTextBox" runat="server"
                        MaxLength="100" />
                </div>

                <div class="settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="EmailTextBox"
                        Text="Email Address" />
                    <asp:TextBox ID="EmailTextBox" runat="server"
                        TextMode="Email" MaxLength="255" />
                </div>

                <div class="settings-field">
                    <asp:Label runat="server"
                        AssociatedControlID="PhoneTextBox"
                        Text="Phone Number" />
                    <asp:TextBox ID="PhoneTextBox" runat="server"
                        MaxLength="30" />
                </div>
            </div>

            <div class="settings-actions">
                <asp:Button ID="SaveAccountButton" runat="server"
                    Text="Save Changes"
                    CssClass="settings-save-button"
                    OnClick="SaveAccountButton_Click" />
            </div>
        </section>

        <section class="settings-card">
            <div class="settings-card-heading">
                <h3>Appearance</h3>
                <p>Choose how the administration interface is displayed.</p>
            </div>

            <div class="settings-field settings-display-field">
                <asp:Label runat="server"
                    AssociatedControlID="DisplayModeDropdown"
                    Text="Display Mode" />

                <asp:DropDownList ID="DisplayModeDropdown" runat="server">
                    <asp:ListItem Value="Light">Light Mode</asp:ListItem>
                    <asp:ListItem Value="Dark">Dark Mode</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="settings-actions">
                <asp:Button ID="SaveAppearanceButton" runat="server"
                    Text="Save Appearance"
                    CssClass="settings-save-button"
                    OnClick="SaveAppearanceButton_Click" />
            </div>
        </section>

        <section class="settings-card settings-security-card">
            <div>
                <h3>Security</h3>
                <p>Change your administrator password from your profile page.</p>
            </div>

            <asp:HyperLink ID="GoToProfileLink" runat="server"
                NavigateUrl="~/AdminProfile.aspx"
                CssClass="settings-secondary-button">
                Open My Profile
            </asp:HyperLink>
        </section>

    </div>

</asp:Content>
