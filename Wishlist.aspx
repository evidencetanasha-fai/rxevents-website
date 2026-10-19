<%@ Page Title="Wishlist"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="Wishlist.aspx.cs"
    Inherits="RxEvents.Wishlist" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">
    Wishlist
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-wishlist-page">

        <!-- Breadcrumb -->
        <div class="rx-wishlist-breadcrumb">
            <a href="CustomerDashboard.aspx">Home</a>
            <span>›</span>
            <span>Wishlist</span>
        </div>


        <!-- Page Header -->
        <div class="rx-wishlist-heading">

            <div>
                <span class="rx-wishlist-eyebrow">
                    YOUR EVENT INSPIRATION
                </span>

                <h2>My Wishlist</h2>

                <p>
                    Create inspiration boards, collect ideas,
                    and share your vision with your event planner.
                </p>
            </div>

            <asp:Button
                ID="CreateFolderButton"
                runat="server"
                Text="+ Create Folder"
                CssClass="rx-wishlist-create-button"
                CausesValidation="false"
                OnClick="CreateFolderButton_Click" />

        </div>


        <!-- Message -->
        <asp:Label
            ID="WishlistMessageLabel"
            runat="server"
            CssClass="rx-wishlist-message">
        </asp:Label>


        <!-- Folders -->
        <div class="rx-wishlist-section">

            <div class="rx-wishlist-section-heading">

                <div>
                    <span>YOUR COLLECTIONS</span>

                    <h3>Inspiration Folders</h3>

                    <p>
                        Organize your ideas by event.
                    </p>
                </div>

                <asp:Label
                    ID="FolderCountLabel"
                    runat="server"
                    CssClass="rx-wishlist-folder-count"
                    Text="0 Folders">
                </asp:Label>

            </div>


            <asp:Repeater
                ID="WishlistFoldersRepeater"
                runat="server">

                <ItemTemplate>

                    <div class="rx-wishlist-folder-card">

                        <!-- Cover -->
                        <div class="rx-wishlist-folder-cover">

                            <asp:Image
                                ID="FolderCoverImage"
                                runat="server"
                                ImageUrl='<%# GetCoverImage(Eval("CoverImage")) %>'
                                AlternateText='<%# Eval("FolderName") %>'
                                CssClass="rx-wishlist-folder-image" />

                            <div class="rx-wishlist-folder-overlay">

                                <span>
                                    <%# Eval("ImageCount") %> images
                                </span>

                            </div>

                        </div>


                        <!-- Folder Info -->
                        <div class="rx-wishlist-folder-content">

                            <span class="rx-wishlist-category">
                                <%# Eval("CategoryName") %>
                            </span>

                            <h4>
                                <%# Eval("FolderName") %>
                            </h4>

                            <div class="rx-wishlist-folder-meta">

                                <span>
                                    Created
                                    <%# Eval(
                                        "CreatedDate",
                                        "{0:dd MMM yyyy}") %>
                                </span>

                                <span>
                                    <%# GetBudget(Eval("Budget")) %>
                                </span>

                            </div>


                            <div class="rx-wishlist-folder-actions">

                                <asp:Button
                                    ID="OpenFolderButton"
                                    runat="server"
                                    Text="Open Folder"
                                    CssClass="rx-wishlist-open-button"
                                    CommandArgument='<%# Eval("WishlistFolderID") %>'
                                    CommandName="OpenFolder"
                                    OnCommand="WishlistFolder_Command"
                                    CausesValidation="false" />

                                <asp:Button
                                    ID="DeleteFolderButton"
                                    runat="server"
                                    Text="Delete"
                                    CssClass="rx-wishlist-delete-button"
                                    CommandArgument='<%# Eval("WishlistFolderID") %>'
                                    CommandName="DeleteFolder"
                                    OnCommand="WishlistFolder_Command"
                                    CausesValidation="false" />

                            </div>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>


            <!-- Empty State -->
            <asp:Panel
                ID="NoFoldersPanel"
                runat="server"
                Visible="false"
                CssClass="rx-wishlist-empty">

                <div class="rx-wishlist-empty-icon">
                    ♡
                </div>

                <h3>
                    Your Inspiration Board is Empty
                </h3>

                <p>
                    Start collecting ideas for your next event.
                    Create a folder and build your own visual
                    inspiration board.
                </p>

                <asp:Button
                    ID="EmptyCreateFolderButton"
                    runat="server"
                    Text="Create Your First Folder"
                    CssClass="rx-wishlist-create-button"
                    CausesValidation="false"
                    OnClick="CreateFolderButton_Click" />

            </asp:Panel>

        </div>


        <!-- Create Folder Popup -->
        <asp:Panel
            ID="CreateFolderPopup"
            runat="server"
            Visible="false"
            CssClass="rx-wishlist-popup">

            <div class="rx-wishlist-popup-overlay"></div>

            <div class="rx-wishlist-popup-card">

                <div class="rx-wishlist-popup-header">

                    <div>

                        <span class="rx-wishlist-eyebrow">
                            START A NEW BOARD
                        </span>

                        <h3>
                            Create Inspiration Folder
                        </h3>

                        <p>
                            Organize ideas for your upcoming event.
                        </p>

                    </div>

                    <asp:Button
                        ID="CloseCreateFolderButton"
                        runat="server"
                        Text="×"
                        CssClass="rx-wishlist-popup-close"
                        CausesValidation="false"
                        OnClick="CloseCreateFolderButton_Click" />

                </div>


                <div class="rx-wishlist-popup-body">

                    <!-- Folder Name -->
                    <div class="rx-wishlist-form-group">

                        <asp:Label
                            ID="FolderNameLabel"
                            runat="server"
                            Text="Folder Name"
                            CssClass="rx-wishlist-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="FolderNameTextBox"
                            runat="server"
                            CssClass="rx-wishlist-form-control"
                            MaxLength="150"
                            placeholder="e.g. My Dream Wedding">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="FolderNameValidator"
                            runat="server"
                            ControlToValidate="FolderNameTextBox"
                            ErrorMessage="Please enter a folder name."
                            CssClass="rx-wishlist-validation"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>


                    <!-- Category -->
                    <div class="rx-wishlist-form-group">

                        <asp:Label
                            ID="FolderCategoryLabel"
                            runat="server"
                            Text="Event Category"
                            CssClass="rx-wishlist-form-label">
                        </asp:Label>

                        <asp:DropDownList
                            ID="FolderCategoryDropDown"
                            runat="server"
                            CssClass="rx-wishlist-form-control">

                            <asp:ListItem
                                Text="Select an event category"
                                Value="">
                            </asp:ListItem>

                        </asp:DropDownList>

                        <asp:RequiredFieldValidator
                            ID="FolderCategoryValidator"
                            runat="server"
                            ControlToValidate="FolderCategoryDropDown"
                            InitialValue=""
                            ErrorMessage="Please select an event category."
                            CssClass="rx-wishlist-validation"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>


                    <!-- Budget -->
                    <div class="rx-wishlist-form-group">

                        <asp:Label
                            ID="FolderBudgetLabel"
                            runat="server"
                            Text="Estimated Budget (UGX)"
                            CssClass="rx-wishlist-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="FolderBudgetTextBox"
                            runat="server"
                            CssClass="rx-wishlist-form-control"
                            TextMode="Number"
                            placeholder="e.g. 5000000">
                        </asp:TextBox>

                    </div>


                    <!-- Notes -->
                    <div class="rx-wishlist-form-group">

                        <asp:Label
                            ID="FolderNotesLabel"
                            runat="server"
                            Text="Planning Notes"
                            CssClass="rx-wishlist-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="FolderNotesTextBox"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="4"
                            MaxLength="2000"
                            CssClass="rx-wishlist-form-control"
                            placeholder="Describe the style, colors, decorations, or ideas you have in mind...">
                        </asp:TextBox>

                    </div>


                    <!-- Message -->
                    <asp:Label
                        ID="CreateFolderMessageLabel"
                        runat="server"
                        CssClass="rx-wishlist-form-message">
                    </asp:Label>


                    <!-- Actions -->
                    <div class="rx-wishlist-popup-actions">

                        <asp:Button
                            ID="CancelCreateFolderButton"
                            runat="server"
                            Text="Cancel"
                            CssClass="rx-wishlist-cancel-button"
                            CausesValidation="false"
                            OnClick="CloseCreateFolderButton_Click" />

                        <asp:Button
                            ID="SaveFolderButton"
                            runat="server"
                            Text="Create Folder"
                            CssClass="rx-wishlist-create-button"
                            OnClick="SaveFolderButton_Click" />

                    </div>

                </div>

            </div>

        </asp:Panel>

    </div>

</asp:Content>