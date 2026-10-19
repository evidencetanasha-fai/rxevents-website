
<%@ Page Title="Complete Payment"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="Payment.aspx.cs"
    Inherits="RxEvents.Payment" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="head"
    runat="server">
    <style>
        .rx-payment-page {
            max-width: 850px;
            margin: 0 auto;
            padding: 30px 22px;
            color: #30243a;
        }

        .rx-pay-card {
            padding: 28px;
            margin-top: 22px;
            background: #fff;
            border: 1px solid #eee6f2;
            border-radius: 18px;
            box-shadow: 0 8px 25px rgba(58,30,74,.06);
        }

        .rx-pay-page-title {
            margin: 0 0 8px;
            font-size: 30px;
            font-weight: 750;
        }

        .rx-pay-muted {
            color: #817689;
        }

        .rx-pay-summary {
            padding: 18px;
            margin: 20px 0;
            background: #faf7fc;
            border-radius: 12px;
        }

        .rx-pay-amount {
            display: block;
            margin-top: 8px;
            color: #54227a;
            font-size: 30px;
            font-weight: 750;
        }

        .rx-pay-methods label {
            margin-left: 8px;
        }

        .rx-pay-methods td {
            padding: 10px 8px;
        }

        .rx-pay-button {
            padding: 13px 24px;
            margin-top: 20px;
            color: #24142e;
            background: #d7b84c;
            border: 0;
            border-radius: 9px;
            font-weight: 700;
            cursor: pointer;
        }

        .rx-pay-message {
            display: block;
            margin: 15px 0;
        }

        .theme-dark .rx-payment-page {
            color: #f2eaf6;
        }

        .theme-dark .rx-pay-card {
            background: #211b27;
            border-color: #3a3041;
        }

        .theme-dark .rx-pay-summary {
            background: #18131d;
        }

        .theme-dark .rx-pay-amount {
            color: #d7b84c;
        }

        @media(max-width:600px) {
            .rx-payment-page { padding: 18px 12px; }
            .rx-pay-card { padding: 20px; }
        }
        
/* ===== PAYMENT METHOD FORMS ===== */

.rx-payment-page h3 {
    margin: 24px 0 16px;
    color: #54227a;
    font-size: 18px;
    font-weight: 750;
}

.rx-pay-card {
    overflow: hidden;
}

.rx-pay-methods {
    width: 100%;
    margin: 16px 0 22px;
    border-collapse: separate;
    border-spacing: 0 10px;
}

.rx-pay-methods tr {
    background: #faf7fc;
    border: 1px solid #eee6f2;
}

.rx-pay-methods td {
    padding: 15px 16px;
    border-top: 1px solid #eee6f2;
    border-bottom: 1px solid #eee6f2;
}

.rx-pay-methods td:first-child {
    width: 42px;
    border-left: 1px solid #eee6f2;
    border-radius: 10px 0 0 10px;
}

.rx-pay-methods td:last-child {
    border-right: 1px solid #eee6f2;
    border-radius: 0 10px 10px 0;
}

.rx-pay-methods input[type="radio"] {
    accent-color: #54227a;
    width: 17px;
    height: 17px;
    cursor: pointer;
}

.rx-pay-methods label {
    display: inline-block;
    color: #30243a;
    font-weight: 600;
    cursor: pointer;
}

/* Individual payment forms */

.rx-payment-form {
    padding: 24px;
    margin: 20px 0;
    background: #fcfaff;
    border: 1px solid #e8dcef;
    border-radius: 15px;
}

.rx-payment-form h3 {
    margin-top: 0;
}

.rx-payment-form p {
    color: #817689;
    font-size: 14px;
    line-height: 1.7;
}

/* Labels and text inputs */

.rx-payment-form label,
.rx-payment-form .rx-field-label {
    display: block;
    margin: 16px 0 7px;
    color: #4a3b55;
    font-size: 13px;
    font-weight: 700;
}

.rx-payment-form input[type="text"],
.rx-payment-form input[type="password"],
.rx-payment-form input[type="tel"],
.rx-payment-form input[type="email"] {
    box-sizing: border-box;
    width: 100%;
    max-width: 100%;
    padding: 13px 14px;
    color: #30243a;
    background: #fff;
    border: 1px solid #ddd0e7;
    border-radius: 9px;
    outline: none;
    font-family: inherit;
    font-size: 14px;
    transition: border-color .2s, box-shadow .2s;
}

.rx-payment-form input:focus {
    border-color: #7b3fa0;
    box-shadow: 0 0 0 3px rgba(123, 63, 160, .12);
}

/* Card expiry and CVV row */

.rx-card-details-row {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 16px;
}

/* Demo payment information */

.rx-payment-demo-note {
    padding: 13px 15px;
    margin-top: 18px;
    color: #725a16;
    background: #fff8df;
    border: 1px solid #f0df9c;
    border-radius: 9px;
    font-size: 12px;
    line-height: 1.7;
}

.rx-payment-demo-id {
    display: block;
    padding: 14px;
    margin: 15px 0;
    color: #54227a;
    background: #f5edfa;
    border: 1px dashed #b99acb;
    border-radius: 9px;
    font-weight: 700;
    overflow-wrap: anywhere;
}

/* Wallet confirmation checkbox */

.rx-payment-form input[type="checkbox"] {
    width: 17px;
    height: 17px;
    margin-right: 8px;
    accent-color: #54227a;
    vertical-align: middle;
}

.rx-payment-form .rx-wallet-confirmation {
    display: flex;
    align-items: flex-start;
    gap: 9px;
    margin-top: 18px;
    color: #4a3b55;
    font-size: 13px;
    line-height: 1.6;
}

/* Validation and feedback */

.rx-payment-form span[style*="color:Red"],
.rx-payment-form span[style*="color: red"] {
    display: block;
    margin-top: 6px;
    font-size: 12px;
}

.rx-pay-message {
    display: block;
    padding: 12px 14px;
    margin: 15px 0;
    border-radius: 9px;
    line-height: 1.6;
}

/* Payment action button */

.rx-pay-button {
    display: inline-block;
    width: 100%;
    padding: 15px 20px;
    margin-top: 18px;
    color: #24142e;
    background: linear-gradient(135deg, #e6cc70, #d7b84c);
    border: 1px solid #c7a63c;
    border-radius: 10px;
    box-shadow: 0 5px 14px rgba(130, 96, 20, .13);
    font-size: 14px;
    font-weight: 800;
    cursor: pointer;
    transition: transform .2s, box-shadow .2s;
}

.rx-pay-button:hover {
    transform: translateY(-1px);
    box-shadow: 0 8px 18px rgba(130, 96, 20, .2);
}

/* Dark theme */

.theme-dark .rx-payment-page h3 {
    color: #d7b84c;
}

.theme-dark .rx-pay-methods tr {
    background: #18131d;
}

.theme-dark .rx-pay-methods td {
    border-color: #3a3041;
}

.theme-dark .rx-pay-methods label {
    color: #f2eaf6;
}

.theme-dark .rx-payment-form {
    background: #18131d;
    border-color: #3a3041;
}

.theme-dark .rx-payment-form label,
.theme-dark .rx-payment-form .rx-field-label,
.theme-dark .rx-payment-form .rx-wallet-confirmation {
    color: #e9dfef;
}

.theme-dark .rx-payment-form input[type="text"],
.theme-dark .rx-payment-form input[type="password"],
.theme-dark .rx-payment-form input[type="tel"],
.theme-dark .rx-payment-form input[type="email"] {
    color: #f2eaf6;
    background: #211b27;
    border-color: #51415d;
}

.theme-dark .rx-payment-demo-id {
    color: #e5d4f1;
    background: #292033;
    border-color: #6e4c83;
}

/* Mobile layout */

@media (max-width: 600px) {
    .rx-payment-form {
        padding: 17px;
    }

    .rx-card-details-row {
        grid-template-columns: 1fr;
        gap: 4px;
    }

    .rx-pay-button {
        padding: 14px;
    }
}

    </style>
</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-payment-page">
        <h1 class="rx-pay-page-title">Complete Your Payment</h1>
        <p class="rx-pay-muted">
            Review your booking and select a payment method.
            This is a simulated transaction for demonstration purposes.
        </p>

        <div class="rx-pay-card">
            <h2>Booking Summary</h2>

            <p>
                Booking reference:
                <strong>
                    <asp:Label ID="BookingReferenceLabel"
                        runat="server" />
                </strong>
            </p>

            <p>
                Booking status:
                <asp:Label ID="BookingStatusLabel"
                    runat="server" />
            </p>

            <div class="rx-pay-summary">
                <span class="rx-pay-muted">Amount due</span>
                <asp:Label ID="AmountDueLabel"
                    runat="server"
                    CssClass="rx-pay-amount" />
            </div>

            
<h3>Choose a payment method</h3>

<asp:RadioButtonList ID="PaymentMethodList"
    runat="server"
    AutoPostBack="true"
    OnSelectedIndexChanged="PaymentMethodList_SelectedIndexChanged"
    RepeatDirection="Vertical">
    <asp:ListItem Value="Card">Credit / Debit Card</asp:ListItem>
    <asp:ListItem Value="UPI">UPI / Online Payment</asp:ListItem>
    <asp:ListItem Value="Wallet">Digital Wallet</asp:ListItem>
    <asp:ListItem Value="Cash">Cash Payment</asp:ListItem>
</asp:RadioButtonList>

<asp:RequiredFieldValidator
    ID="PaymentMethodValidator"
    runat="server"
    ControlToValidate="PaymentMethodList"
    ErrorMessage="Please select a payment method."
    ForeColor="Red"
    EnableClientScript="false"
    Display="Dynamic" />

<asp:Panel ID="CardPaymentPanel" runat="server" Visible="false">
    <h3>Card Details — Demo Only</h3>

    <asp:Label runat="server" Text="Cardholder Name" />
    <asp:TextBox ID="CardholderNameTextBox" runat="server"
        MaxLength="100" />

    <asp:RequiredFieldValidator ID="CardholderNameValidator"
        runat="server" ControlToValidate="CardholderNameTextBox"
        ErrorMessage="Enter the cardholder name."
        ForeColor="Red" EnableClientScript="false"
        ValidationGroup="Payment" />

    <br />

    <asp:Label runat="server" Text="Demo Card Number" />
    <asp:TextBox ID="CardNumberTextBox" runat="server"
        MaxLength="19" />

    <asp:RequiredFieldValidator ID="CardNumberValidator"
        runat="server" ControlToValidate="CardNumberTextBox"
        ErrorMessage="Enter the demo card number."
        ForeColor="Red" EnableClientScript="false"
        ValidationGroup="Payment" />

    <br />

    <asp:Label runat="server" Text="Expiry (MM/YY)" />
    <asp:TextBox ID="CardExpiryTextBox" runat="server"
        MaxLength="5" />

    <asp:Label runat="server" Text="CVV (test value only)" />
    <asp:TextBox ID="CardCvvTextBox" runat="server"
        TextMode="Password" MaxLength="4" />

    <p class="rx-pay-muted">
        Use fictional test details only. Do not enter real card information.
        These fields will not be stored.
    </p>
</asp:Panel>

<asp:Panel ID="UpiPaymentPanel" runat="server" Visible="false">
    <h3>UPI Demo Payment</h3>
    <p>Demo UPI ID: rxevents.demo@example</p>
    <asp:Label runat="server" Text="Demo UPI ID" />
    <asp:TextBox ID="UpiIdTextBox" runat="server"
        MaxLength="100" />
</asp:Panel>

<asp:Panel ID="WalletPaymentPanel" runat="server" Visible="false">
    <h3>Wallet Demo</h3>
    <p>This demo will simulate a wallet confirmation. No real wallet is connected.</p>
    <asp:CheckBox ID="WalletConfirmationCheckBox" runat="server"
        Text="I confirm this is a simulated wallet transaction." />
</asp:Panel>

<asp:Panel ID="CashPaymentPanel" runat="server" Visible="false">
    <h3>Cash Payment</h3>
    <p>Payment will remain pending until cash is received.</p>
</asp:Panel>

<asp:Label ID="PaymentMessageLabel"
    runat="server" CssClass="rx-pay-message" />

<asp:Button ID="PayNowButton"
    runat="server"
    Text="Simulate Payment"
    CssClass="rx-pay-button"
    OnClick="PayNowButton_Click"
    ValidationGroup="Payment" />

        </div>
    </div>
</asp:Content>
