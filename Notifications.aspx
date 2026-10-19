<%@ Page Title="Notifications"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="Notifications.aspx.cs"
    Inherits="RxEvents.Notifications" %>


<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">

    Notifications

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-notifications-page">


        <!-- ================= BREADCRUMB ================= -->

        <div class="rx-notifications-breadcrumb">

            <a href="CustomerDashboard.aspx">
                Home
            </a>

            <span>›</span>

            <span>
                Notifications
            </span>

        </div>


        <!-- ================= HEADER ================= -->

        <div class="rx-notifications-header">

            <div>

                <span class="rx-notifications-eyebrow">
                    ACCOUNT
                </span>

                <h2>
                    Notifications
                </h2>

                <p>
                    Stay updated with your bookings, payments and events.
                </p>

            </div>


            <div class="rx-notifications-header-actions">

                <asp:Button
                    ID="MarkAllReadButton"
                    runat="server"
                    Text="✓ Mark All as Read"
                    CssClass="rx-notifications-mark-all"
                    CausesValidation="false"
                    OnClick="MarkAllReadButton_Click" />

            </div>

        </div>


        <!-- ================= NOTIFICATION SUMMARY ================= -->

        <div class="rx-notification-summary">

            <div class="rx-notification-summary-item">

                <div class="rx-notification-summary-icon">
                    🔔
                </div>

                <div>

                    <span>
                        Total Notifications
                    </span>

                    <strong>
                        <asp:Label
                            ID="TotalNotificationsLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>

                </div>

            </div>


            <div class="rx-notification-summary-item">

                <div class="rx-notification-summary-icon unread">
                    ●
                </div>

                <div>

                    <span>
                        Unread
                    </span>

                    <strong>
                        <asp:Label
                            ID="UnreadNotificationsLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>

                </div>

            </div>


            <div class="rx-notification-summary-item">

                <div class="rx-notification-summary-icon read">
                    ✓
                </div>

                <div>

                    <span>
                        Read
                    </span>

                    <strong>
                        <asp:Label
                            ID="ReadNotificationsLabel"
                            runat="server"
                            Text="0">
                        </asp:Label>
                    </strong>

                </div>

            </div>

        </div>


        <!-- ================= MESSAGE ================= -->

        <asp:Label
            ID="MessageLabel"
            runat="server"
            CssClass="rx-notifications-message">
        </asp:Label>


        <!-- ================= NOTIFICATIONS LIST ================= -->

        <div class="rx-notifications-card">

            <div class="rx-notifications-card-header">

                <div>

                    <h3>
                        Recent Notifications
                    </h3>

                    <p>
                        Your latest account activity and updates.
                    </p>

                </div>

            </div>


            <asp:Repeater
                ID="NotificationsRepeater"
                runat="server"
                OnItemCommand="NotificationsRepeater_ItemCommand">

                <ItemTemplate>

                    <div class='<%# Convert.ToBoolean(Eval("IsRead"))
                        ? "rx-notification-item read"
                        : "rx-notification-item unread" %>'>


                        <!-- Icon -->

                        <div class="rx-notification-item-icon">

                            <%#
                                GetNotificationIcon(
                                    Eval("NotificationType").ToString())
                            %>

                        </div>


                        <!-- Content -->

                        <div class="rx-notification-item-content">

                            <div class="rx-notification-item-title">

                                <strong>
                                    <%# Eval("Title") %>
                                </strong>

                                <%# !Convert.ToBoolean(
                                    Eval("IsRead"))
                                    ? "<span class='rx-unread-badge'>New</span>"
                                    : "" %>

                            </div>


                            <p>
                                <%# Eval("Message") %>
                            </p>


                            <small>
                                <%#
                                    FormatNotificationDate(
                                        Convert.ToDateTime(
                                            Eval("CreatedDate")))
                                %>
                            </small>

                        </div>


                        <!-- Actions -->

                        <div class="rx-notification-item-actions">


                            <asp:LinkButton
                                ID="MarkReadButton"
                                runat="server"
                                Text="Mark as read"
                                CssClass="rx-notification-action"
                                CommandName="MarkRead"
                                CommandArgument='<%# Eval("NotificationID") %>'
                                CausesValidation="false"
                                Visible='<%# !Convert.ToBoolean(Eval("IsRead")) %>'>
                            </asp:LinkButton>


                            <asp:LinkButton
                                ID="DeleteNotificationButton"
                                runat="server"
                                Text="Delete"
                                CssClass="rx-notification-delete"
                                CommandName="DeleteNotification"
                                CommandArgument='<%# Eval("NotificationID") %>'
                                CausesValidation="false">
                            </asp:LinkButton>


                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>


            <!-- ================= EMPTY STATE ================= -->

            <asp:Panel
                ID="EmptyNotificationsPanel"
                runat="server"
                CssClass="rx-notifications-empty"
                Visible="false">

                <div class="rx-notifications-empty-icon">
                    🔔
                </div>

                <h3>
                    No notifications yet
                </h3>

                <p>
                    When there is an update about your bookings,
                    payments or events, it will appear here.
                </p>

            </asp:Panel>

        </div>


        <!-- ================= CLEAR READ ================= -->

        <div class="rx-notifications-footer-action">

            <asp:Button
                ID="ClearReadButton"
                runat="server"
                Text="Clear Read Notifications"
                CssClass="rx-notifications-clear"
                CausesValidation="false"
                OnClick="ClearReadButton_Click" />

        </div>


    </div>

</asp:Content>