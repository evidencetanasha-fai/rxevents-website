
<%@ Page Title="My Payments"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="MyPayments.aspx.cs"
    Inherits="RxEvents.MyPayments" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="head"
    runat="server">

    <style>
        .rx-payments-page {
            padding: 28px;
            color: #30243a;
        }

        .rx-payments-heading {
            margin-bottom: 24px;
        }

        .rx-payments-heading h1 {
            margin: 0 0 8px;
            font-size: 30px;
            font-weight: 750;
        }

        .rx-payments-heading p {
            margin: 0;
            color: #817689;
        }

        .rx-payment-summary {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 18px;
            margin-bottom: 25px;
        }

        .rx-payment-stat {
            padding: 22px;
            background: #ffffff;
            border: 1px solid #eee6f2;
            border-radius: 16px;
            box-shadow: 0 5px 18px rgba(58, 30, 74, .05);
        }

        .rx-payment-stat span {
            display: block;
            margin-bottom: 10px;
            color: #817689;
            font-size: 13px;
        }

        .rx-payment-stat strong {
            color: #54227a;
            font-size: 25px;
        }

        .rx-payment-history {
            padding: 24px;
            background: #ffffff;
            border: 1px solid #eee6f2;
            border-radius: 18px;
        }

        .rx-payment-history h2 {
            margin: 0 0 20px;
            font-size: 20px;
        }

        .rx-payment-table-wrapper {
            overflow-x: auto;
        }

        .rx-payment-table {
            width: 100%;
            border-collapse: collapse;
            min-width: 650px;
        }

        .rx-payment-table th {
            padding: 14px 12px;
            text-align: left;
            color: #817689;
            background: #faf7fc;
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: .4px;
        }

        .rx-payment-table td {
            padding: 16px 12px;
            border-bottom: 1px solid #f0ebf3;
            font-size: 13px;
        }

        .rx-payment-reference {
            color: #54227a;
            font-weight: 700;
        }

        .rx-payment-status {
            display: inline-block;
            padding: 6px 10px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 700;
        }

        .rx-payment-status-paid {
            background: #e8f7ee;
            color: #247a45;
        }

        .rx-payment-status-pending {
            background: #fff5d9;
            color: #946800;
        }

        .rx-payment-status-failed,
        .rx-payment-status-refunded {
            background: #fdeaea;
            color: #a62d2d;
        }

        .rx-payment-empty {
            display: block;
            padding: 35px 15px;
            color: #817689;
            text-align: center;
            line-height: 1.7;
        }

        .rx-payment-empty strong {
            display: block;
            margin-bottom: 7px;
            color: #54227a;
            font-size: 18px;
        }

        .theme-dark .rx-payments-page {
            color: #f2eaf6;
        }

        .theme-dark .rx-payment-stat,
        .theme-dark .rx-payment-history {
            background: #211b27;
            border-color: #3a3041;
        }

        .theme-dark .rx-payment-stat span {
            color: #b9afc0;
        }

        .theme-dark .rx-payment-stat strong,
        .theme-dark .rx-payment-empty strong {
            color: #d7b84c;
        }

        .theme-dark .rx-payment-table th {
            background: #18131d;
            color: #b9afc0;
        }

        .theme-dark .rx-payment-table td {
            border-bottom-color: #3a3041;
            color: #eee7f1;
        }

        .theme-dark .rx-payment-reference {
            color: #d7b84c;
        }

        .theme-dark .rx-payment-empty {
            color: #b9afc0;
        }

        @media (max-width: 850px) {
            .rx-payment-summary {
                grid-template-columns: 1fr;
            }

            .rx-payments-page {
                padding: 18px;
            }

            .rx-payment-history {
                padding: 16px;
            }
        }
    </style>

</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-payments-page">

        <div class="rx-payments-heading">
            <h1>My Payments</h1>
            <p>Track your event payments and transaction history.</p>
        </div>

        <div class="rx-payment-summary">

            <div class="rx-payment-stat">
                <span>Total Payments</span>
                <strong>
                    <asp:Label ID="TotalPaymentsLabel"
                        runat="server" Text="0" />
                </strong>
            </div>

            <div class="rx-payment-stat">
                <span>Total Amount Paid</span>
                <strong>
                    <asp:Label ID="TotalPaidLabel"
                        runat="server" Text="UGX 0" />
                </strong>
            </div>

            <div class="rx-payment-stat">
                <span>Pending Payments</span>
                <strong>
                    <asp:Label ID="PendingPaymentsLabel"
                        runat="server" Text="0" />
                </strong>
            </div>

        </div>

        <div class="rx-payment-history">

            <h2>Payment History</h2>

            <asp:Label ID="PaymentMessageLabel"
                runat="server"
                CssClass="rx-payment-empty"
                Visible="false" />

            <div class="rx-payment-table-wrapper">

                <asp:GridView
                    ID="PaymentsGridView"
                    runat="server"
                    AutoGenerateColumns="false"
                    CssClass="rx-payment-table"
                    GridLines="None"
                    EmptyDataText="No payments found.">

                    <Columns>

                        <asp:BoundField
                            DataField="PaymentReference"
                            HeaderText="Reference" />

                        <asp:BoundField
                            DataField="BookingID"
                            HeaderText="Booking" />

                        <asp:BoundField
                            DataField="PaymentDate"
                            HeaderText="Date"
                            DataFormatString="{0:dd MMM yyyy}" />

                        <asp:BoundField
                            DataField="PaymentMethod"
                            HeaderText="Method" />

                        <asp:BoundField
                            DataField="Amount"
                            HeaderText="Amount (UGX)"
                            DataFormatString="{0:N0}" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# GetPaymentStatusClass(Eval("PaymentStatus")) %>'>
                                    <%#: Eval("PaymentStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>

</asp:Content>
