
<%@ Page Title="Add New Event" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminAddEvent.aspx.cs" Inherits="RxEvents.AdminAddEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="add-event-page">

        <div class="add-event-heading">
            <div>
                <h2><asp:Literal ID="PageHeadingLiteral" runat="server" Text="Add New Event" /></h2>
                <p>Create and manage an event for Rx Events Solutions.</p>
            </div>

            <asp:HyperLink ID="BackToEventsLink" runat="server"
                NavigateUrl="~/AdminEvents.aspx"
                CssClass="add-event-back-button">
                ← Back to Events
            </asp:HyperLink>
        </div>

        <asp:Label ID="EventMessage" runat="server"
            CssClass="add-event-message" />

        <div class="add-event-layout">

            <div class="add-event-form-card">

                <h3>Event Information</h3>
                <p class="add-event-section-description">
                    Enter the event details below.
                </p>

                <div class="add-event-form-grid">

                    <div class="add-event-field">
                        <asp:Label runat="server"
                            AssociatedControlID="EventNameInput"
                            Text="Event Name *" />

                        <asp:TextBox ID="EventNameInput" runat="server"
                            CssClass="add-event-control"
                            MaxLength="200"
                            placeholder="e.g. Wedding Celebration" />

                        <asp:RequiredFieldValidator ID="EventNameValidator"
                            runat="server"
                            ControlToValidate="EventNameInput"
                            ErrorMessage="Enter an event name."
                            CssClass="add-event-validation"
                            Display="Dynamic" />
                    </div>

                    <div class="add-event-field">
                        <asp:Label runat="server"
                            AssociatedControlID="CategoryDropdown"
                            Text="Event Category *" />

                        <asp:DropDownList ID="CategoryDropdown" runat="server"
                            CssClass="add-event-control" />

                        <asp:RequiredFieldValidator ID="CategoryValidator"
                            runat="server"
                            ControlToValidate="CategoryDropdown"
                            InitialValue=""
                            ErrorMessage="Select an event category."
                            CssClass="add-event-validation"
                            Display="Dynamic" />
                    </div>

                    <div class="add-event-field add-event-full-width">
                        <asp:Label runat="server"
                            AssociatedControlID="DescriptionInput"
                            Text="Event Description" />

                        <asp:TextBox ID="DescriptionInput" runat="server"
                            CssClass="add-event-control"
                            TextMode="MultiLine"
                            Rows="5"
                            MaxLength="2000"
                            placeholder="Describe the event and what makes it special." />
                    </div>

                    <div class="add-event-field">
                        <asp:Label runat="server"
                            AssociatedControlID="BasePriceInput"
                            Text="Base Price *" />

                        <asp:TextBox ID="BasePriceInput" runat="server"
                            CssClass="add-event-control"
                            TextMode="Number"
                            placeholder="Enter base price" />

                        <asp:RequiredFieldValidator ID="PriceRequiredValidator"
                            runat="server"
                            ControlToValidate="BasePriceInput"
                            ErrorMessage="Enter a base price."
                            CssClass="add-event-validation"
                            Display="Dynamic" />

                        <asp:RangeValidator ID="PriceRangeValidator"
                            runat="server"
                            ControlToValidate="BasePriceInput"
                            MinimumValue="0"
                            MaximumValue="999999999"
                            Type="Double"
                            ErrorMessage="Enter a valid non-negative price."
                            CssClass="add-event-validation"
                            Display="Dynamic" />
                    </div>

                    <div class="add-event-field">
                        <asp:Label runat="server"
                            AssociatedControlID="EventStatusDropdown"
                            Text="Event Status *" />

                        <asp:DropDownList ID="EventStatusDropdown" runat="server"
                            CssClass="add-event-control">
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="add-event-field add-event-full-width">
                        <asp:Label runat="server"
                            AssociatedControlID="EventImageInput"
                            Text="Event Image" />

                        <asp:FileUpload ID="EventImageInput" runat="server"
                            CssClass="add-event-control" />

                        <small class="add-event-help">
                            Upload a JPG, JPEG, PNG, or WEBP image.
                        </small>
                    </div>

                </div>

                <div class="add-event-actions">
                    <asp:Button ID="CancelEventButton" runat="server"
                        Text="Cancel"
                        CssClass="add-event-cancel-button"
                        CausesValidation="false"
                        PostBackUrl="~/AdminEvents.aspx" />

                    <asp:Button ID="SaveEventButton" runat="server"
                        Text="Create Event"
                        CssClass="add-event-save-button"
                        OnClick="SaveEventButton_Click" />
                </div>

            </div>

            <aside class="add-event-preview-card">
                <h3>Before You Save</h3>

                <div class="add-event-preview-icon">✦</div>

                <p>
                    Give your event a clear name, select the correct category,
                    and provide a description that helps customers understand it.
                </p>

                <ul>
                    <li>Choose an appropriate category.</li>
                    <li>Enter a valid non-negative base price.</li>
                    <li>Use a suitable event image.</li>
                    <li>Confirm whether the event should be active.</li>
                </ul>
            </aside>

        </div>

    </div>

</asp:Content>
