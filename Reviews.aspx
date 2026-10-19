<%@ Page Title="Reviews"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="Reviews.aspx.cs"
    Inherits="RxEvents.Reviews" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">
    Reviews
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-reviews-page">

        <!-- Breadcrumb -->
        <div class="rx-reviews-breadcrumb">
            <a href="CustomerDashboard.aspx">Home</a>
            <span>›</span>
            <span>Reviews</span>
        </div>


        <!-- Page Header -->
        <div class="rx-reviews-heading">

            <div>
                <span class="rx-reviews-eyebrow">
                    CUSTOMER FEEDBACK
                </span>

                <h2>My Reviews</h2>

                <p>
                    Share your experience and help us create
                    even better moments.
                </p>
            </div>

            <asp:Button
    ID="WriteReviewButton"
    runat="server"
    Text="Write a Review"
    CssClass="rx-reviews-write-button"
    CausesValidation="false"
    OnClick="WriteReviewButton_Click" />

        </div>


        <!-- Review Summary -->
        <div class="rx-reviews-summary">

            <!-- Average -->
            <div class="rx-reviews-average">

                <span class="rx-reviews-average-number">
                    <asp:Label
                        ID="AverageRatingLabel"
                        runat="server"
                        Text="0.0">
                    </asp:Label>
                </span>

                <div class="rx-reviews-stars">
                    ★★★★★
                </div>

                <span class="rx-reviews-average-text">
                    Average Rating
                </span>

                <strong>
                    <asp:Label
                        ID="TotalReviewsLabel"
                        runat="server"
                        Text="0">
                    </asp:Label>
                    Reviews
                </strong>

            </div>


            <!-- Rating Breakdown -->
            <div class="rx-reviews-breakdown">

                <div class="rx-rating-row">
                    <span>5</span>
                    <span class="rx-rating-star">★</span>
                    <div class="rx-rating-bar">
                        <div class="rx-rating-fill"
                             style="width: 0%;">
                        </div>
                    </div>
                    <strong>
                        <asp:Label
                            ID="FiveStarLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>
                </div>


                <div class="rx-rating-row">
                    <span>4</span>
                    <span class="rx-rating-star">★</span>
                    <div class="rx-rating-bar">
                        <div class="rx-rating-fill"
                             style="width: 0%;">
                        </div>
                    </div>
                    <strong>
                        <asp:Label
                            ID="FourStarLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>
                </div>


                <div class="rx-rating-row">
                    <span>3</span>
                    <span class="rx-rating-star">★</span>
                    <div class="rx-rating-bar">
                        <div class="rx-rating-fill"
                             style="width: 0%;">
                        </div>
                    </div>
                    <strong>
                        <asp:Label
                            ID="ThreeStarLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>
                </div>


                <div class="rx-rating-row">
                    <span>2</span>
                    <span class="rx-rating-star">★</span>
                    <div class="rx-rating-bar">
                        <div class="rx-rating-fill"
                             style="width: 0%;">
                        </div>
                    </div>
                    <strong>
                        <asp:Label
                            ID="TwoStarLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>
                </div>


                <div class="rx-rating-row">
                    <span>1</span>
                    <span class="rx-rating-star">★</span>
                    <div class="rx-rating-bar">
                        <div class="rx-rating-fill"
                             style="width: 0%;">
                        </div>
                    </div>
                    <strong>
                        <asp:Label
                            ID="OneStarLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>
                </div>

            </div>

        </div>


        <!-- Message -->
        <asp:Label
            ID="ReviewsMessageLabel"
            runat="server"
            CssClass="rx-reviews-message">
        </asp:Label>


        <!-- My Reviews -->
        <div class="rx-reviews-list-card">

            <div class="rx-reviews-section-heading">

                <div>
                    <span>YOUR FEEDBACK</span>

                    <h3>Review History</h3>

                    <p>
                        Reviews you have submitted for your events.
                    </p>
                </div>

            </div>


            <asp:Repeater
                ID="ReviewsRepeater"
                runat="server">

                <ItemTemplate>

                    <div class="rx-review-item">

                        <div class="rx-review-image">

                            <img
                                src='<%# ResolveUrl("~/Content/images/" + Eval("EventImage")) %>'
                                alt='<%# Eval("EventName") %>' />

                        </div>


                        <div class="rx-review-content">

                            <div class="rx-review-top">

                                <div>
                                    <strong>
                                        <%# Eval("EventName") %>
                                    </strong>

                                    <small>
                                        <%# Eval(
                                            "ReviewDate",
                                            "{0:dd MMM yyyy}") %>
                                    </small>
                                </div>

                                <span class="rx-review-stars">
                                    <%# new string('★', Convert.ToInt32(Eval("Rating"))) + new string('☆', 5 - Convert.ToInt32(Eval("Rating"))) %>
                                </span>

                            </div>


                            <p>
                                <%# Eval("Comment") %>
                            </p>


                            <span class="rx-review-status">
                                <%# Convert.ToBoolean(
                                    Eval("IsApproved"))
                                    ? "Approved"
                                    : "Pending Approval" %>
                            </span>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>


            <asp:Panel
                ID="NoReviewsPanel"
                runat="server"
                Visible="false"
                CssClass="rx-reviews-empty">

                <div class="rx-reviews-empty-icon">
                    ★
                </div>

                <h3>No Reviews Yet</h3>

                <p>
                    You haven't submitted a review yet.
                    Share your experience after attending an event.
                </p>

                <asp:Button
    ID="EmptyWriteReviewButton"
    runat="server"
    Text="Write Your First Review"
    CssClass="rx-reviews-write-button"
    CausesValidation="false"
    OnClick="WriteReviewButton_Click" />

            </asp:Panel>

        </div>

    </div>
    <!-- Write Review Popup -->
<asp:Panel ID="WriteReviewPopup"
    runat="server"
    CssClass="rx-review-popup"
    Visible="false">

    <div class="rx-review-popup-overlay"></div>

    <div class="rx-review-popup-card">

        <div class="rx-review-popup-header">
            <div>
                <span class="rx-reviews-eyebrow">
                    SHARE YOUR EXPERIENCE
                </span>

                <h3>Write a Review</h3>

                <p>
                    Tell us about your experience at your event.
                </p>
            </div>

            <asp:Button
                ID="CloseReviewButton"
                runat="server"
                Text="×"
                CssClass="rx-review-popup-close"
                CausesValidation="false"
                OnClick="CloseReviewButton_Click" />
        </div>

        <div class="rx-review-popup-body">

            <!-- Event -->
            <div class="rx-review-form-group">

                <asp:Label
                    ID="ReviewEventLabel"
                    runat="server"
                    Text="Select Event"
                    CssClass="rx-review-form-label">
                </asp:Label>

                <asp:DropDownList
                    ID="ReviewBookingDropDown"
                    runat="server"
                    CssClass="rx-review-form-control">

                    <asp:ListItem
                        Text="Select an event"
                        Value="">
                    </asp:ListItem>

                </asp:DropDownList>

                <asp:RequiredFieldValidator
                    ID="ReviewBookingValidator"
                    runat="server"
                    ControlToValidate="ReviewBookingDropDown"
                    InitialValue=""
                    ErrorMessage="Please select an event."
                    CssClass="rx-review-validation"
                    EnableClientScript="false">
                </asp:RequiredFieldValidator>

            </div>


            <!-- Rating -->
            <div class="rx-review-form-group">

                <asp:Label
                    ID="ReviewRatingLabel"
                    runat="server"
                    Text="Your Rating"
                    CssClass="rx-review-form-label">
                </asp:Label>

                <asp:DropDownList
                    ID="ReviewRatingDropDown"
                    runat="server"
                    CssClass="rx-review-form-control">

                    <asp:ListItem
                        Text="Select a rating"
                        Value="">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="★★★★★  —  Excellent"
                        Value="5">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="★★★★☆  —  Very Good"
                        Value="4">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="★★★☆☆  —  Good"
                        Value="3">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="★★☆☆☆  —  Fair"
                        Value="2">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="★☆☆☆☆  —  Poor"
                        Value="1">
                    </asp:ListItem>

                </asp:DropDownList>

                <asp:RequiredFieldValidator
                    ID="ReviewRatingValidator"
                    runat="server"
                    ControlToValidate="ReviewRatingDropDown"
                    InitialValue=""
                    ErrorMessage="Please select a rating."
                    CssClass="rx-review-validation"
                    EnableClientScript="false">
                </asp:RequiredFieldValidator>

            </div>


            <!-- Comment -->
            <div class="rx-review-form-group">

                <asp:Label
                    ID="ReviewCommentLabel"
                    runat="server"
                    Text="Your Review"
                    CssClass="rx-review-form-label">
                </asp:Label>

                <asp:TextBox
                    ID="ReviewCommentTextBox"
                    runat="server"
                    TextMode="MultiLine"
                    Rows="5"
                    MaxLength="1000"
                    CssClass="rx-review-form-control rx-review-comment"
                    placeholder="Tell us what you enjoyed about your event...">
                </asp:TextBox>

                <asp:RequiredFieldValidator
                    ID="ReviewCommentValidator"
                    runat="server"
                    ControlToValidate="ReviewCommentTextBox"
                    ErrorMessage="Please write a review."
                    CssClass="rx-review-validation"
                    EnableClientScript="false">
                </asp:RequiredFieldValidator>

            </div>


            <!-- Message -->
            <asp:Label
                ID="ReviewFormMessageLabel"
                runat="server"
                CssClass="rx-review-form-message">
            </asp:Label>


            <!-- Buttons -->
            <div class="rx-review-popup-actions">

                <asp:Button
                    ID="CancelReviewButton"
                    runat="server"
                    Text="Cancel"
                    CssClass="rx-review-cancel-button"
                    CausesValidation="false"
                    OnClick="CloseReviewButton_Click" />

                <asp:Button
                    ID="SubmitReviewButton"
                    runat="server"
                    Text="Submit Review"
                    CssClass="rx-reviews-write-button"
                    OnClick="SubmitReviewButton_Click" />

            </div>

        </div>

    </div>

</asp:Panel>

</asp:Content>