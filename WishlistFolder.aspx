<%@ Page Title="Inspiration Board"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="WishlistFolder.aspx.cs"
    Inherits="RxEvents.WishlistFolder" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">
    Inspiration Board
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-wishlist-board-page">

        <!-- Breadcrumb -->
        <div class="rx-wishlist-breadcrumb">
            <a href="CustomerDashboard.aspx">Home</a>
            <span>›</span>
            <a href="Wishlist.aspx">Wishlist</a>
            <span>›</span>
            <span>Inspiration Board</span>
        </div>


        <!-- Board Header -->
        <div class="rx-wishlist-board-header">

            <div>
                <span class="rx-wishlist-eyebrow">
                    EVENT INSPIRATION
                </span>

                <h2>
                    <asp:Label
                        ID="FolderNameLabel"
                        runat="server"
                        Text="Inspiration Board">
                    </asp:Label>
                </h2>

                <div class="rx-wishlist-board-category">
                    <asp:Label
                        ID="CategoryNameLabel"
                        runat="server"
                        Text="Event Category">
                    </asp:Label>
                </div>
            </div>

            <div class="rx-wishlist-board-actions">

    <asp:Button
        ID="AddInspirationButton"
        runat="server"
        Text="+ Add Inspiration"
        CssClass="rx-wishlist-btn primary"
        OnClick="AddInspirationButton_Click" />

    <asp:Button
        ID="AttachToBookingButton"
        runat="server"
        Text="Attach to Booking"
        CssClass="rx-wishlist-btn attach"
        OnClick="AttachToBookingButton_Click" />

    <asp:Button
        ID="BackToWishlistButton"
        runat="server"
        Text="Back to Wishlist"
        CssClass="rx-wishlist-btn secondary"
        OnClick="BackToWishlistButton_Click" />

</div>

        </div>


        <!-- Board Information -->
        <div class="rx-wishlist-board-info">

            <div class="rx-wishlist-board-info-card">

                <span>ESTIMATED BUDGET</span>

                <strong>
                    <asp:Label
                        ID="BudgetLabel"
                        runat="server"
                        Text="Budget not set">
                    </asp:Label>
                </strong>

            </div>


            <div class="rx-wishlist-board-info-card">

                <span>INSPIRATION</span>

                <strong>
                    <asp:Label
                        ID="ImageCountLabel"
                        runat="server"
                        Text="0 images">
                    </asp:Label>
                </strong>

            </div>


            <div class="rx-wishlist-board-notes">

                <span>PLANNING NOTES</span>

                <p>
                    <asp:Label
                        ID="NotesLabel"
                        runat="server"
                        Text="No planning notes added yet.">
                    </asp:Label>
                </p>

            </div>

        </div>


        <!-- Message -->
        <asp:Label
            ID="BoardMessageLabel"
            runat="server"
            CssClass="rx-wishlist-message">
        </asp:Label>


        <!-- Pinterest Style Gallery -->
        <div class="rx-wishlist-board-section">

            <div class="rx-wishlist-section-heading">

                <div>
                    <span>YOUR IDEAS</span>

                    <h3>Inspiration Gallery</h3>

                    <p>
                        Collect images that show your event planner
                        exactly what you have in mind.
                    </p>
                </div>

            </div>


            <div class="rx-wishlist-masonry">

                <asp:Repeater
                    ID="WishlistImagesRepeater"
                    runat="server">

                    <ItemTemplate>

                        <div class="rx-wishlist-inspiration-card">

                            <asp:Image
                                ID="InspirationImage"
                                runat="server"
                                ImageUrl='<%# GetImageUrl(Eval("ImagePath")) %>'
                                AlternateText='<%# Eval("Caption") %>'
                                CssClass="rx-wishlist-inspiration-image" />

                            <div class="rx-wishlist-inspiration-info">

                                <p>
                                    <%# Eval("Caption") %>
                                </p>

                                <asp:Button
                                    ID="DeleteImageButton"
                                    runat="server"
                                    Text="Remove"
                                    CssClass="rx-wishlist-image-delete"
                                    CommandArgument='<%# Eval("WishlistImageID") %>'
                                    CommandName="DeleteImage"
                                    OnCommand="Image_Command"
                                    CausesValidation="false" />

                            </div>

                        </div>

                    </ItemTemplate>

                </asp:Repeater>

            </div>


            <!-- Empty Gallery -->
            <asp:Panel
                ID="NoImagesPanel"
                runat="server"
                Visible="false"
                CssClass="rx-wishlist-board-empty">

                <div class="rx-wishlist-empty-icon">
                    ♡
                </div>

                <h3>
                    Your Inspiration Gallery is Empty
                </h3>

                <p>
                    Start collecting images that represent
                    the style, colors, decorations, and atmosphere
                    you want for your event.
                </p>

                <asp:Button
                    ID="EmptyAddInspirationButton"
                    runat="server"
                    Text="Add Your First Inspiration"
                    CssClass="rx-wishlist-create-button"
                    CausesValidation="false"
                    OnClick="AddInspirationButton_Click" />

            </asp:Panel>

        </div>


        <!-- Add Inspiration Popup -->
        <asp:Panel
            ID="AddInspirationPopup"
            runat="server"
            Visible="false"
            CssClass="rx-wishlist-popup">

            <div class="rx-wishlist-popup-overlay"></div>

            <div class="rx-wishlist-popup-card">

                <div class="rx-wishlist-popup-header">

                    <div>

                        <span class="rx-wishlist-eyebrow">
                            ADD TO YOUR BOARD
                        </span>

                        <h3>
                            Add Inspiration
                        </h3>

                        <p>
                            Upload an image that represents your
                            event vision.
                        </p>

                    </div>

                    <asp:Button
                        ID="CloseInspirationButton"
                        runat="server"
                        Text="×"
                        CssClass="rx-wishlist-popup-close"
                        CausesValidation="false"
                        OnClick="CloseInspirationButton_Click" />

                </div>


                <div class="rx-wishlist-popup-body">

                    <!-- Image -->
                    <div class="rx-wishlist-form-group">

                        <asp:Label
                            ID="InspirationImageLabel"
                            runat="server"
                            Text="Inspiration Image"
                            CssClass="rx-wishlist-form-label">
                        </asp:Label>

                        <asp:FileUpload
                            ID="InspirationFileUpload"
                            runat="server"
                            CssClass="rx-wishlist-file-upload" />

                        <asp:RequiredFieldValidator
                            ID="InspirationImageValidator"
                            runat="server"
                            ControlToValidate="InspirationFileUpload"
                            ErrorMessage="Please select an image."
                            CssClass="rx-wishlist-validation"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>


                    <!-- Caption -->
                    <div class="rx-wishlist-form-group">

                        <asp:Label
                            ID="InspirationCaptionLabel"
                            runat="server"
                            Text="Caption"
                            CssClass="rx-wishlist-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="InspirationCaptionTextBox"
                            runat="server"
                            CssClass="rx-wishlist-form-control"
                            MaxLength="255"
                            placeholder="e.g. Pastel floral decoration inspiration">
                        </asp:TextBox>

                    </div>


                    <!-- Message -->
                    <asp:Label
                        ID="InspirationMessageLabel"
                        runat="server"
                        CssClass="rx-wishlist-form-message">
                    </asp:Label>


                    <!-- Actions -->
                    <div class="rx-wishlist-popup-actions">

                        <asp:Button
                            ID="CancelInspirationButton"
                            runat="server"
                            Text="Cancel"
                            CssClass="rx-wishlist-cancel-button"
                            CausesValidation="false"
                            OnClick="CloseInspirationButton_Click" />

                        <asp:Button
                            ID="SaveInspirationButton"
                            runat="server"
                            Text="Add to Board"
                            CssClass="rx-wishlist-create-button"
                            OnClick="SaveInspirationButton_Click" />

                    </div>

                </div>

            </div>

        </asp:Panel>

        <!-- ATTACH TO BOOKING POPUP -->
<asp:Panel
    ID="AttachBookingPopup"
    runat="server"
    CssClass="rx-wishlist-popup"
    Visible="false">

    <div class="rx-wishlist-popup-card">

        <div class="rx-wishlist-popup-header">
            <div>
                <span class="rx-wishlist-popup-eyebrow">
                    CONNECT YOUR PLAN
                </span>

                <h2>Attach to Booking</h2>

                <p>
                    Choose a booking to share this inspiration board
                    with your event planner.
                </p>
            </div>

            <asp:Button
                ID="CloseAttachBookingButton"
                runat="server"
                Text="×"
                CssClass="rx-wishlist-popup-close"
                OnClick="CloseAttachBookingButton_Click" />
        </div>

        <div class="rx-wishlist-form-group">

            <asp:Label
                ID="AttachBookingLabel"
                runat="server"
                Text="Select Booking"
                CssClass="rx-wishlist-form-label" />

            <asp:DropDownList
                ID="BookingDropDown"
                runat="server"
                CssClass="rx-wishlist-form-control">
            </asp:DropDownList>

            <asp:RequiredFieldValidator
                ID="BookingRequiredValidator"
                runat="server"
                ControlToValidate="BookingDropDown"
                InitialValue=""
                ErrorMessage="Please select a booking."
                CssClass="rx-wishlist-validation"
                EnableClientScript="false"
                Display="Dynamic" />

        </div>

        <asp:Label
            ID="AttachBookingMessageLabel"
            runat="server"
            CssClass="rx-wishlist-form-message"
            EnableViewState="false">
        </asp:Label>

        <div class="rx-wishlist-popup-actions">

            <asp:Button
                ID="CancelAttachBookingButton"
                runat="server"
                Text="Cancel"
                CssClass="rx-wishlist-btn secondary"
                CausesValidation="false"
                OnClick="CloseAttachBookingButton_Click" />

            <asp:Button
                ID="SaveAttachBookingButton"
                runat="server"
                Text="Attach Board"
                CssClass="rx-wishlist-btn primary"
                OnClick="SaveAttachBookingButton_Click" />

        </div>

    </div>

</asp:Panel>

    </div>

</asp:Content>