<%@ Page Title="Reviews" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminReviews.aspx.cs"
    Inherits="RxEvents.AdminReviews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="reviews-page">

        <div class="reviews-header">
            <div>
                <h1>Reviews</h1>
                <p>Dashboard &gt; Reviews</p>
            </div>

            <div class="reviews-header-actions">
                <asp:Button ID="ExportReviewsButton" runat="server"
                    Text="↓ Export"
                    CssClass="reviews-export-button"
                    OnClick="ExportReviewsButton_Click" />
            </div>
        </div>

        <asp:Label ID="ReviewsMessage" runat="server"
            CssClass="reviews-message" EnableViewState="false" />

        <div class="reviews-filter-card">

            <div class="reviews-filter-field reviews-search-field">
                <asp:Label ID="ReviewSearchLabel" runat="server"
                    AssociatedControlID="ReviewSearchTextBox"
                    Text="Search Reviews" />

                <asp:TextBox ID="ReviewSearchTextBox" runat="server"
                    CssClass="reviews-input"
                    placeholder="Search by customer, event or review..." />
            </div>

            <div class="reviews-filter-field">
                <asp:Label ID="RatingFilterLabel" runat="server"
                    AssociatedControlID="RatingFilter"
                    Text="Rating" />

                <asp:DropDownList ID="RatingFilter" runat="server"
                    CssClass="reviews-input">
                    <asp:ListItem Text="All Ratings" Value="" />
                    <asp:ListItem Text="5 Stars" Value="5" />
                    <asp:ListItem Text="4 Stars" Value="4" />
                    <asp:ListItem Text="3 Stars" Value="3" />
                    <asp:ListItem Text="2 Stars" Value="2" />
                    <asp:ListItem Text="1 Star" Value="1" />
                </asp:DropDownList>
            </div>

            <div class="reviews-filter-field">
                <asp:Label ID="ReviewStatusLabel" runat="server"
                    AssociatedControlID="ReviewStatusFilter"
                    Text="Status" />

                <asp:DropDownList ID="ReviewStatusFilter" runat="server"
                    CssClass="reviews-input">
                    <asp:ListItem Text="All Statuses" Value="" />
                    <asp:ListItem Text="Pending" Value="Pending" />
                    <asp:ListItem Text="Approved" Value="Approved" />
                    <asp:ListItem Text="Rejected" Value="Rejected" />
                </asp:DropDownList>
            </div>

            <div class="reviews-filter-field">
                <asp:Label ID="ReviewEventLabel" runat="server"
                    AssociatedControlID="ReviewEventFilter"
                    Text="Event / Service" />

                <asp:DropDownList ID="ReviewEventFilter" runat="server"
                    CssClass="reviews-input">
                    <asp:ListItem Text="All Events & Services" Value="" />
                </asp:DropDownList>
            </div>

            <div class="reviews-filter-actions">
                <asp:Button ID="ApplyReviewFiltersButton" runat="server"
                    Text="Apply Filters"
                    CssClass="reviews-filter-button"
                    OnClick="ApplyReviewFiltersButton_Click" />

                <asp:Button ID="ClearReviewFiltersButton" runat="server"
                    Text="↻ Clear Filters"
                    CssClass="reviews-clear-button"
                    CausesValidation="false"
                    OnClick="ClearReviewFiltersButton_Click" />
            </div>

        </div>

        <div class="reviews-table-card">

            <asp:GridView ID="ReviewsGridView" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="ReviewID"
                CssClass="reviews-grid"
                GridLines="None"
                OnRowCommand="ReviewsGridView_RowCommand"
                EmptyDataText="No reviews found.">

                <Columns>

                    <asp:TemplateField HeaderText="Customer">
                        <ItemTemplate>
                            <div class="reviews-customer-cell">
                                <div class="reviews-avatar">
                                    <%#: Eval("CustomerInitials") %>
                                </div>
                                <div>
                                    <strong><%#: Eval("CustomerName") %></strong>
                                    <p><%#: Eval("CustomerEmail") %></p>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Event / Service">
                        <ItemTemplate>
                            <div class="reviews-event-cell">
                                <strong><%#: Eval("EventName") %></strong>
                                <p><%# Eval("ReviewDate", "{0:dd MMM yyyy}") %></p>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Rating">
                        <ItemTemplate>
                            <span class="reviews-stars">
                                <%# GetStars(Eval("Rating")) %>
                            </span>
                            <span class="reviews-rating-number">
                                <%#: Eval("Rating") %>/5
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Review">
                        <ItemTemplate>
                            <div class="reviews-text-cell">
                                <%#: Eval("ReviewText") %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="ReviewDate"
                        HeaderText="Date"
                        DataFormatString="{0:dd MMM yyyy hh:mm tt}" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# GetReviewStatusCss(Eval("Status")) %>'>
                                <%#: Eval("Status") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div class="reviews-row-actions">

                                <asp:LinkButton ID="ViewReviewButton" runat="server"
                                    Text="View"
                                    ToolTip="View review"
                                    CommandName="ViewReview"
                                    CommandArgument='<%# Eval("ReviewID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="ApproveReviewButton" runat="server"
                                    Text="Approve"
                                    ToolTip="Approve review"
                                    Visible='<%# Convert.ToString(Eval("Status")) != "Approved" %>'
                                    CommandName="ApproveReview"
                                    CommandArgument='<%# Eval("ReviewID") %>'
                                    CausesValidation="false" />

                                <asp:LinkButton ID="RejectReviewButton" runat="server"
                                    Text="Reject"
                                    ToolTip="Reject review"
                                    Visible='<%# Convert.ToString(Eval("Status")) != "Rejected" %>'
                                    CommandName="RejectReview"
                                    CommandArgument='<%# Eval("ReviewID") %>'
                                    CausesValidation="false" />

                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <div class="reviews-pagination">

                <asp:Label ID="ReviewPaginationSummary" runat="server"
                    Text="Showing 0 reviews" />

                <div class="reviews-pagination-buttons">

                    <asp:Button ID="PreviousReviewPageButton" runat="server"
                        Text="‹"
                        CssClass="reviews-page-button"
                        CausesValidation="false"
                        OnClick="PreviousReviewPageButton_Click" />

                    <asp:Repeater ID="ReviewPaginationRepeater" runat="server"
                        OnItemCommand="ReviewPaginationRepeater_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="ReviewPageButton" runat="server"
                                Text='<%# Eval("PageNumber") %>'
                                CommandName="GoToPage"
                                CommandArgument='<%# Eval("PageNumber") %>'
                                CssClass='<%# Convert.ToInt32(Eval("PageNumber")) == CurrentReviewPage
                                    ? "reviews-page-button reviews-page-active"
                                    : "reviews-page-button" %>'
                                CausesValidation="false" />
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Button ID="NextReviewPageButton" runat="server"
                        Text="›"
                        CssClass="reviews-page-button"
                        CausesValidation="false"
                        OnClick="NextReviewPageButton_Click" />

                </div>

            </div>

        </div>

    </div>

</asp:Content>