<%@ Page Title="Payments" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminPayments.aspx.cs"
    Inherits="RxEvents.AdminPayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-payments">

        <!-- Page Heading -->
        <div class="payments-header">
            <div>
                <h1>Payments Management</h1>
                <p>Monitor transactions, payment statuses, and revenue.</p>
            </div>

            <asp:Button ID="ExportPaymentsButton" runat="server"
                Text="Export CSV"
                CssClass="payment-export-btn"
                OnClick="ExportPaymentsButton_Click"
                CausesValidation="false" />
        </div>

        <!-- Summary Cards -->
        <div class="payment-stats">

            <div class="payment-stat-card">
                <div class="payment-stat-icon">₱</div>
                <div>
                    <p>Total Payments</p>
                    <h2>
                        <asp:Label ID="TotalPaymentsLabel" runat="server"
                            Text="0" />
                    </h2>
                    <span>All recorded transactions</span>
                </div>
            </div>

            <div class="payment-stat-card">
                <div class="payment-stat-icon">₹</div>
                <div>
                    <p>Total Revenue</p>
                    <h2>
                        <asp:Label ID="TotalRevenueLabel" runat="server"
                            Text="₹0.00" />
                    </h2>
                    <span>Payments marked as paid</span>
                </div>
            </div>

            <div class="payment-stat-card">
                <div class="payment-stat-icon">◷</div>
                <div>
                    <p>Pending Payments</p>
                    <h2>
                        <asp:Label ID="PendingPaymentsLabel" runat="server"
                            Text="0" />
                    </h2>
                    <span>Awaiting payment completion</span>
                </div>
            </div>

            <div class="payment-stat-card">
                <div class="payment-stat-icon">✓</div>
                <div>
                    <p>Successful Payments</p>
                    <h2>
                        <asp:Label ID="SuccessfulPaymentsLabel" runat="server"
                            Text="0" />
                    </h2>
                    <span>Payments marked as paid</span>
                </div>
            </div>

        </div>

        <!-- Filters -->
        <div class="payment-filter-card">

            <div class="payment-filter-heading">
                <div>
                    <h2>Transaction Records</h2>
                    <p>Search and filter payment transactions.</p>
                </div>
            </div>

            <div class="payment-filter-grid">

                <div class="payment-filter-field">
                    <asp:Label ID="SearchLabel" runat="server"
                        AssociatedControlID="SearchTextBox"
                        Text="Search" />

                    <asp:TextBox ID="SearchTextBox" runat="server"
                        CssClass="payment-input"
                        placeholder="Payment, booking, customer..." />
                </div>

                <div class="payment-filter-field">
                    <asp:Label ID="StatusLabel" runat="server"
                        AssociatedControlID="StatusDropDown"
                        Text="Payment Status" />

                    <asp:DropDownList ID="StatusDropDown" runat="server"
                        CssClass="payment-input">
                        <asp:ListItem Text="All Statuses" Value="" />
                        <asp:ListItem Text="Pending" Value="Pending" />
                        <asp:ListItem Text="Paid" Value="Paid" />
                        <asp:ListItem Text="Failed" Value="Failed" />
                        <asp:ListItem Text="Refunded" Value="Refunded" />
                    </asp:DropDownList>
                </div>

                <div class="payment-filter-field">
                    <asp:Label ID="MethodLabel" runat="server"
                        AssociatedControlID="MethodDropDown"
                        Text="Payment Method" />

                    <asp:DropDownList ID="MethodDropDown" runat="server"
                        CssClass="payment-input">
                        <asp:ListItem Text="All Methods" Value="" />
                    </asp:DropDownList>
                </div>

                <div class="payment-filter-field">
                    <asp:Label ID="StartDateLabel" runat="server"
                        AssociatedControlID="StartDateTextBox"
                        Text="From Date" />

                    <asp:TextBox ID="StartDateTextBox" runat="server"
                        CssClass="payment-input"
                        TextMode="Date" />
                </div>

                <div class="payment-filter-field">
                    <asp:Label ID="EndDateLabel" runat="server"
                        AssociatedControlID="EndDateTextBox"
                        Text="To Date" />

                    <asp:TextBox ID="EndDateTextBox" runat="server"
                        CssClass="payment-input"
                        TextMode="Date" />
                </div>

                <div class="payment-filter-actions">
                    <asp:Button ID="ApplyFilterButton" runat="server"
                        Text="Apply Filters"
                        CssClass="payment-primary-btn"
                        OnClick="ApplyFilterButton_Click"
                        CausesValidation="false" />

                    <asp:Button ID="ResetFilterButton" runat="server"
                        Text="Reset"
                        CssClass="payment-reset-btn"
                        OnClick="ResetFilterButton_Click"
                        CausesValidation="false" />
                </div>

            </div>
        </div>

        <!-- Payments Table -->
        <div class="payment-table-card">

            <div class="payment-table-heading">
                <h2>All Transactions</h2>

                <asp:Label ID="RecordCountLabel" runat="server"
                    CssClass="payment-record-count"
                    Text="0 records" />
            </div>

            <div class="payment-table-wrapper">

                <asp:GridView ID="PaymentsGridView" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="payment-table"
                    GridLines="None"
                    DataKeyNames="PaymentID"
                    AllowPaging="True"
                    PageSize="10"
                    OnPageIndexChanging="PaymentsGridView_PageIndexChanging"
                    OnRowCommand="PaymentsGridView_RowCommand"
                    EmptyDataText="No payment records found.">

                    <Columns>

                        <asp:BoundField DataField="PaymentReference"
                            HeaderText="Payment Reference" />

                        <asp:BoundField DataField="BookingReference"
                            HeaderText="Booking Reference" />

                        <asp:BoundField DataField="CustomerName"
                            HeaderText="Customer" />

                        <asp:BoundField DataField="EventName"
                            HeaderText="Event" />

                        <asp:BoundField DataField="Amount"
                            HeaderText="Amount"
                            DataFormatString="{0:N2}"
                            HtmlEncode="False" />

                        <asp:BoundField DataField="PaymentMethod"
                            HeaderText="Method" />

                        <asp:BoundField DataField="PaymentDate"
                            HeaderText="Payment Date"
                            DataFormatString="{0:dd MMM yyyy}" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="PaymentStatusLabel"
                                    runat="server"
                                    Text='<%# Eval("PaymentStatus") %>'
                                    CssClass='<%# GetPaymentStatusClass(
                                        Eval("PaymentStatus")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="ViewPaymentButton"
                                    runat="server"
                                    Text="View"
                                    CssClass="payment-view-btn"
                                    CommandName="ViewPayment"
                                    CommandArgument='<%# Eval("PaymentID") %>'
                                    CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                    <PagerStyle CssClass="payment-pager" />
                    <HeaderStyle CssClass="payment-table-header" />
                    <RowStyle CssClass="payment-table-row" />
                    <EmptyDataRowStyle CssClass="payment-empty-row" />

                </asp:GridView>

            </div>

        </div>

        <asp:Label ID="MessageLabel" runat="server"
            CssClass="payment-message" />

    </div>

</asp:Content>