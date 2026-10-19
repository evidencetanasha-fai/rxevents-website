<%@ Page Title="Book Event"
    Language="C#"
    MasterPageFile="~/Customer.Master"
    AutoEventWireup="true"
    CodeBehind="BookEvent.aspx.cs"
    Inherits="RxEvents.BookEvent" MaintainScrollPositionOnPostback ="true" %>

<asp:Content ID="BookEventHeadContent"
    ContentPlaceHolderID="head"
    runat="server">

    <style type="text/css">

    /* ================================
       BOOK EVENT PAGE OVERRIDES
       ================================ */

    .rx-book-event-page {
        padding: 30px;
        max-width: 1500px;
        margin: 0 auto;
        box-sizing: border-box;
    }

    /* HERO */

    .rx-book-event-hero {
        position: relative !important;
        min-height: 230px !important;
        padding: 45px !important;
        margin-bottom: 28px !important;
        border-radius: 22px !important;
        overflow: hidden !important;

        display: flex !important;
        align-items: center !important;

        background:
            linear-gradient(
                90deg,
                rgba(35, 17, 61, 0.94) 0%,
                rgba(48, 24, 75, 0.78) 45%,
                rgba(48, 24, 75, 0.25) 100%
            ),
            url('../Content/images/event-hero.jpg') center/cover no-repeat !important;

        box-shadow: 0 15px 35px rgba(0,0,0,0.25) !important;
        box-sizing: border-box !important;
    }

    .rx-book-event-hero > div {
        position: relative !important;
        z-index: 2 !important;
    }

    .rx-book-event-hero h1 {
        margin: 0 0 12px !important;
        color: #ffffff !important;
        font-size: 38px !important;
        font-weight: 700 !important;
    }

    .rx-book-event-hero p {
        margin: 0 !important;
        color: #ffffff !important;
        font-size: 16px !important;
    }

    /* BREADCRUMB */

    .rx-book-breadcrumb {
        display: flex !important;
        align-items: center !important;
        gap: 7px !important;
        margin-bottom: 20px !important;
        color: #ffffff !important;
    }

    .rx-book-breadcrumb span {
        color: #ffffff !important;
    }

    .rx-book-breadcrumb span:last-child {
        color: #d4ad35 !important;
        font-weight: 600 !important;
    }

    /* STEPS */

    .rx-book-steps {
        display: flex !important;
        align-items: center !important;
        justify-content: space-between !important;
        width: 100% !important;
        margin: 0 0 32px !important;
        box-sizing: border-box !important;
    }

    .rx-book-step {
        display: flex !important;
        align-items: center !important;
        gap: 10px !important;
        flex: 1 !important;
        color: #ffffff !important;
        min-width: 0 !important;
    }

    .rx-book-step-number {
        flex: 0 0 38px !important;
        width: 38px !important;
        height: 38px !important;
        border-radius: 50% !important;
        display: flex !important;
        align-items: center !important;
        justify-content: center !important;
        background: #54227a !important;
        border: 1px solid #c9a227 !important;
        color: #ffffff !important;
        font-weight: 700 !important;
    }

    .rx-book-step.active .rx-book-step-number {
        background: #c9a227 !important;
        border-color: #c9a227 !important;
        color: #ffffff !important;
    }

    .rx-book-step-content {
        display: flex !important;
        flex-direction: column !important;
        gap: 3px !important;
        min-width: 0 !important;
    }

    .rx-book-step-content strong {
        display: block !important;
        color: #ffffff !important;
        font-size: 14px !important;
        white-space: nowrap !important;
    }

    .rx-book-step-content span {
        display: block !important;
        color: #bdb4c9 !important;
        font-size: 11px !important;
        white-space: nowrap !important;
    }

    .rx-book-step-line {
        flex: 0 0 45px !important;
        width: 45px !important;
        height: 1px !important;
        background: #655171 !important;
        margin: 0 12px !important;
    }

    /* MAIN GRID */

    .rx-book-event-grid {
        display: grid !important;
        grid-template-columns: minmax(0, 1.55fr) minmax(320px, 0.85fr) !important;
        gap: 28px !important;
        align-items: start !important;
    }

    /* FORM CARD */

    .rx-book-event-card {
        background: #ffffff !important;
        border-radius: 20px !important;
        padding: 30px !important;
        box-sizing: border-box !important;
        border: 1px solid #eeeaf2 !important;
        box-shadow: 0 10px 30px rgba(0,0,0,0.15) !important;
    }

    .rx-book-section-heading {
        display: flex !important;
        align-items: flex-start !important;
        gap: 12px !important;
        margin-bottom: 24px !important;
    }

    .rx-book-section-icon {
        flex: 0 0 42px !important;
        width: 42px !important;
        height: 42px !important;
        border-radius: 12px !important;
        background: #f2eafb !important;
        color: #54227a !important;
        display: flex !important;
        align-items: center !important;
        justify-content: center !important;
    }

    .rx-book-section-heading h2 {
        margin: 0 !important;
        color: #30233d !important;
        font-size: 21px !important;
    }

    .rx-book-section-heading p {
        margin: 5px 0 0 !important;
        color: #81778c !important;
        font-size: 13px !important;
    }

    /* FORM GRID */

    .rx-book-form-grid {
        display: grid !important;
        grid-template-columns: repeat(2, minmax(0, 1fr)) !important;
        gap: 22px !important;
    }

    .rx-book-form-group {
        display: flex !important;
        flex-direction: column !important;
        min-width: 0 !important;
    }

    .rx-book-form-group.full-width {
        grid-column: 1 / -1 !important;
    }

    .rx-book-form-label {
        display: block !important;
        margin-bottom: 8px !important;
        color: #403548 !important;
        font-size: 13px !important;
        font-weight: 600 !important;
    }

    .rx-book-form-control {
        width: 100% !important;
        min-height: 46px !important;
        padding: 11px 14px !important;
        box-sizing: border-box !important;

        background: #ffffff !important;
        color: #332a3b !important;

        border: 1px solid #ddd7e5 !important;
        border-radius: 10px !important;

        font-family: inherit !important;
        font-size: 14px !important;
    }

    textarea.rx-book-form-control {
        min-height: 120px !important;
        resize: vertical !important;
    }

    /* VALIDATION */

    .rx-book-validation {
        display: block !important;
        margin-top: 6px !important;
        color: #c62828 !important;
        font-size: 12px !important;
    }

    .rx-book-message {
        display: block !important;
        margin-bottom: 18px !important;
        color: #54227a !important;
    }

    /* ACTIONS */

    .rx-book-form-actions {
        display: flex !important;
        justify-content: space-between !important;
        align-items: center !important;
        gap: 15px !important;
        margin-top: 28px !important;
        padding-top: 24px !important;
        border-top: 1px solid #eeeaf2 !important;
    }

    .rx-book-clear-button {
        min-height: 44px !important;
        padding: 0 22px !important;
        border: 1px solid #d8d1df !important;
        border-radius: 10px !important;
        background: #ffffff !important;
        color: #5b5363 !important;
        font-weight: 600 !important;
    }

    .rx-book-next-button {
        min-height: 46px !important;
        padding: 0 28px !important;
        border: none !important;
        border-radius: 10px !important;
        background: #54227a !important;
        color: #ffffff !important;
        font-weight: 600 !important;
    }

    /* PREVIEW */

    .rx-book-preview-column {
        display: flex !important;
        flex-direction: column !important;
        gap: 20px !important;
    }

    .rx-book-preview-card {
        background: #ffffff !important;
        border-radius: 20px !important;
        overflow: hidden !important;
        border: 1px solid #eeeaf2 !important;
        box-shadow: 0 10px 30px rgba(0,0,0,0.15) !important;
    }

    .rx-book-preview-heading {
        padding: 20px 22px !important;
        color: #30233d !important;
        font-size: 18px !important;
        font-weight: 700 !important;
        border-bottom: 1px solid #eeeaf2 !important;
    }

    .rx-book-preview-heading span {
        color: #c9a227 !important;
    }

    .rx-book-preview-image {
        width: 100% !important;
        height: 210px !important;
        object-fit: cover !important;
        display: block !important;
    }

    .rx-book-preview-title {
        display: block !important;
        padding: 20px 22px 8px !important;
        color: #30233d !important;
        font-size: 21px !important;
        font-weight: 700 !important;
    }

    .rx-book-preview-details {
        padding: 5px 22px 22px !important;
    }

    .rx-book-preview-details > div {
        display: flex !important;
        align-items: center !important;
        gap: 10px !important;
        padding: 12px 0 !important;
        border-bottom: 1px solid #eeeaf2 !important;
        color: #403548 !important;
        font-size: 13px !important;
    }

    .rx-book-preview-details > div span:first-child {
        color: #c9a227 !important;
        font-size: 16px !important;
    }

    /* HELP */

    .rx-book-help-card {
        display: flex !important;
        gap: 15px !important;
        padding: 24px !important;
        border-radius: 18px !important;
        background: linear-gradient(135deg, #54227a, #32154d) !important;
        color: #ffffff !important;
    }

    .rx-book-help-card h3,
    .rx-book-help-card p {
        color: #ffffff !important;
    }

    .rx-book-help-card p {
        opacity: 0.8 !important;
    }

    .rx-book-contact-button {
        padding: 10px 18px !important;
        border: 1px solid #c9a227 !important;
        border-radius: 8px !important;
        background: transparent !important;
        color: #ffffff !important;
    }

    /* MOBILE */

    @media (max-width: 1000px) {

        .rx-book-event-grid {
            grid-template-columns: 1fr !important;
        }

    }

    @media (max-width: 700px) {

        .rx-book-form-grid {
            grid-template-columns: 1fr !important;
        }

        .rx-book-form-group.full-width {
            grid-column: auto !important;
        }

        .rx-book-steps {
            overflow-x: auto !important;
            justify-content: flex-start !important;
        }

        .rx-book-step-line {
            flex-basis: 25px !important;
            width: 25px !important;
        }

        .rx-book-event-card {
            padding: 22px !important;
        }

        .rx-book-form-actions {
            flex-direction: column-reverse !important;
            align-items: stretch !important;
        }

        .rx-book-clear-button,
        .rx-book-next-button {
            width: 100% !important;
        }

    }
    /* ================================
   PACKAGE SELECTION
   ================================ */

.rx-book-package-panel {
    background: transparent !important;
    margin-top: 5px !important;
}

.rx-book-package-header {
    display: flex !important;
    align-items: flex-start !important;
    gap: 14px !important;
    margin-bottom: 26px !important;
}

.rx-book-package-header h2 {
    margin: 0 !important;
    color: #30233d !important;
    font-size: 24px !important;
}

.rx-book-package-header p {
    margin: 6px 0 0 !important;
    color: #81778c !important;
    font-size: 14px !important;
}

.rx-book-package-grid {
    display: grid !important;
    grid-template-columns: repeat(3, minmax(0, 1fr)) !important;
    gap: 22px !important;
}

.rx-book-package-card {
    background: #ffffff !important;
    border: 1px solid #eeeaf2 !important;
    border-radius: 18px !important;
    overflow: hidden !important;
    box-shadow: 0 8px 24px rgba(0,0,0,0.10) !important;
    transition: transform 0.2s ease !important;
}

.rx-book-package-image-wrapper {
    width: 100% !important;
    height: 180px !important;
    overflow: hidden !important;
    background: #f2eafb !important;
}

.rx-book-package-image {
    width: 100% !important;
    height: 100% !important;
    object-fit: cover !important;
    display: block !important;
}

.rx-book-package-content {
    padding: 22px !important;
}

.rx-book-package-type {
    display: inline-block !important;
    margin-bottom: 8px !important;
    color: #c9a227 !important;
    font-size: 11px !important;
    font-weight: 700 !important;
    text-transform: uppercase !important;
    letter-spacing: 0.8px !important;
}

.rx-book-package-content h3 {
    margin: 0 0 10px !important;
    color: #30233d !important;
    font-size: 20px !important;
}

.rx-book-package-description {
    min-height: 60px !important;
    margin: 0 0 18px !important;
    color: #81778c !important;
    font-size: 13px !important;
    line-height: 1.6 !important;
}

.rx-book-package-price {
    margin-bottom: 18px !important;
    color: #54227a !important;
    font-size: 20px !important;
    font-weight: 700 !important;
}

.rx-book-package-select-button {
    width: 100% !important;
    min-height: 44px !important;
    border: 1px solid #54227a !important;
    border-radius: 9px !important;
    background: #ffffff !important;
    color: #54227a !important;
    font-weight: 600 !important;
    cursor: pointer !important;
}

.rx-book-package-select-button:hover {
    background: #54227a !important;
    color: #ffffff !important;
}

.rx-book-package-actions {
    display: flex !important;
    justify-content: space-between !important;
    align-items: center !important;
    gap: 15px !important;
    margin-top: 30px !important;
    padding-top: 24px !important;
    border-top: 1px solid #eeeaf2 !important;
}

@media (max-width: 1000px) {

    .rx-book-package-grid {
        grid-template-columns: repeat(2, minmax(0, 1fr)) !important;
    }

}

@media (max-width: 650px) {

    .rx-book-package-grid {
        grid-template-columns: 1fr !important;
    }

    .rx-book-package-actions {
        flex-direction: column !important;
        align-items: stretch !important;
    }

}
.rx-book-package-card.selected {
    border: 2px solid #c9a227 !important;
    box-shadow: 0 12px 30px rgba(201, 162, 39, 0.22) !important;
    transform: translateY(-3px) !important;
}

.rx-book-package-card.selected
.rx-book-package-select-button {
    background: #54227a !important;
    color: #ffffff !important;
    border-color: #54227a !important;
}
/* =========================================
   STEP 3 - ADDITIONAL INFORMATION
   ========================================= */

.rx-book-additional-panel {
    width: 100%;
    margin-top: 24px;
}

.rx-book-additional-panel .rx-book-event-card {
    background: #ffffff;
    border: 1px solid #eeeaf2;
    border-radius: 22px;
    padding: 32px;
    box-shadow: 0 10px 30px rgba(84, 34, 122, 0.08);
}

/* Step 3 Header */
.rx-book-additional-panel .rx-book-package-header {
    display: flex;
    align-items: center;
    gap: 16px;
    margin-bottom: 28px;
    padding-bottom: 22px;
    border-bottom: 1px solid #eeeaf2;
}

.rx-book-additional-panel .rx-book-section-icon {
    width: 52px;
    height: 52px;
    min-width: 52px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 15px;
    background: linear-gradient(135deg, #54227a, #7d3fa3);
    color: #ffffff;
    font-size: 23px;
    box-shadow: 0 8px 18px rgba(84, 34, 122, 0.20);
}

.rx-book-additional-panel .rx-book-package-header h2 {
    margin: 0 0 5px;
    color: #2d2035;
    font-size: 24px;
    font-weight: 700;
}

.rx-book-additional-panel .rx-book-package-header p {
    margin: 0;
    color: #77707d;
    font-size: 14px;
    line-height: 1.6;
}

/* Message */
.rx-book-additional-panel .rx-book-message {
    display: block;
    margin-bottom: 20px;
    color: #54227a;
    font-size: 14px;
    font-weight: 600;
}

/* Form Grid */
.rx-book-additional-panel .rx-book-form-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 24px;
}

.rx-book-additional-panel .rx-book-form-group {
    display: flex;
    flex-direction: column;
}

.rx-book-additional-panel .rx-book-form-group.full-width {
    grid-column: 1 / -1;
}

/* Labels */
.rx-book-additional-panel .rx-book-form-label {
    margin-bottom: 8px;
    color: #392b42;
    font-size: 14px;
    font-weight: 600;
}

/* Inputs */
.rx-book-additional-panel .rx-book-form-control {
    width: 100%;
    box-sizing: border-box;
    min-height: 48px;
    padding: 12px 15px;
    border: 1px solid #ddd7e3;
    border-radius: 12px;
    background: #faf9fc;
    color: #33283a;
    font-family: inherit;
    font-size: 14px;
    outline: none;
    transition: all 0.2s ease;
}

.rx-book-additional-panel textarea.rx-book-form-control {
    min-height: 130px;
    resize: vertical;
    line-height: 1.6;
}

/* Input focus */
.rx-book-additional-panel .rx-book-form-control:focus {
    border-color: #54227a;
    background: #ffffff;
    box-shadow: 0 0 0 3px rgba(84, 34, 122, 0.10);
}

/* Placeholder */
.rx-book-additional-panel .rx-book-form-control::placeholder {
    color: #aaa3ae;
}

/* Character Limit */
.rx-book-character-limit {
    margin-top: 7px;
    text-align: right;
    color: #99919f;
    font-size: 12px;
}

/* Bottom Buttons */
.rx-book-additional-panel .rx-book-package-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
    margin-top: 32px;
    padding-top: 24px;
    border-top: 1px solid #eeeaf2;
}

/* Back Button */
.rx-book-additional-panel .rx-book-clear-button {
    min-height: 46px;
    padding: 11px 22px;
    border: 1px solid #d8d1df;
    border-radius: 11px;
    background: #ffffff;
    color: #5f5366;
    font-family: inherit;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s ease;
}

.rx-book-additional-panel .rx-book-clear-button:hover {
    border-color: #54227a;
    color: #54227a;
    background: #faf7fc;
}

/* Next Button */
.rx-book-additional-panel .rx-book-next-button {
    min-height: 46px;
    padding: 11px 25px;
    border: 1px solid #54227a;
    border-radius: 11px;
    background: #54227a;
    color: #ffffff;
    font-family: inherit;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    box-shadow: 0 6px 16px rgba(84, 34, 122, 0.18);
    transition: all 0.2s ease;
}

.rx-book-additional-panel .rx-book-next-button:hover {
    background: #431b61;
    border-color: #431b61;
    transform: translateY(-1px);
    box-shadow: 0 9px 20px rgba(84, 34, 122, 0.24);
}

/* =========================================
   STEP 3 - DARK MODE
   ========================================= */

.theme-dark .rx-book-additional-panel .rx-book-event-card {
    background: #211b27;
    border-color: #3a3041;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
}

.theme-dark .rx-book-additional-panel .rx-book-package-header {
    border-bottom-color: #3a3041;
}

.theme-dark .rx-book-additional-panel .rx-book-package-header h2 {
    color: #ffffff;
}

.theme-dark .rx-book-additional-panel .rx-book-package-header p {
    color: #b9afc0;
}

.theme-dark .rx-book-additional-panel .rx-book-form-label {
    color: #eee7f1;
}

.theme-dark .rx-book-additional-panel .rx-book-form-control {
    background: #18131d;
    border-color: #403545;
    color: #ffffff;
}

.theme-dark .rx-book-additional-panel .rx-book-form-control:focus {
    background: #1d1722;
    border-color: #c9a227;
    box-shadow: 0 0 0 3px rgba(201, 162, 39, 0.12);
}

.theme-dark .rx-book-additional-panel .rx-book-form-control::placeholder {
    color: #817787;
}

.theme-dark .rx-book-character-limit {
    color: #8f8494;
}

.theme-dark .rx-book-additional-panel .rx-book-package-actions {
    border-top-color: #3a3041;
}

.theme-dark .rx-book-additional-panel .rx-book-clear-button {
    background: transparent;
    border-color: #4a3f50;
    color: #c9bfca;
}

.theme-dark .rx-book-additional-panel .rx-book-clear-button:hover {
    border-color: #c9a227;
    color: #c9a227;
    background: rgba(201, 162, 39, 0.06);
}

/* =========================================
   STEP 3 - RESPONSIVE
   ========================================= */

@media (max-width: 768px) {

    .rx-book-additional-panel .rx-book-event-card {
        padding: 24px 20px;
        border-radius: 18px;
    }

    .rx-book-additional-panel .rx-book-form-grid {
        grid-template-columns: 1fr;
        gap: 20px;
    }

    .rx-book-additional-panel .rx-book-form-group.full-width {
        grid-column: auto;
    }

    .rx-book-additional-panel .rx-book-package-header {
        align-items: flex-start;
    }

    .rx-book-additional-panel .rx-book-package-header h2 {
        font-size: 21px;
    }

    .rx-book-additional-panel .rx-book-package-actions {
        flex-direction: column-reverse;
        align-items: stretch;
    }

    .rx-book-additional-panel .rx-book-clear-button,
    .rx-book-additional-panel .rx-book-next-button {
        width: 100%;
    }
}
/* =========================================
   STEP 4 - CUSTOMIZE YOUR EVENT
   ========================================= */

.rx-book-additional-panel {
    width: 100%;
    margin-top: 24px;
}

.rx-book-additional-panel .rx-book-event-card {
    background: #ffffff;
    border: 1px solid #eeeaf2;
    border-radius: 22px;
    padding: 32px;
    box-shadow: 0 10px 30px rgba(84, 34, 122, 0.08);
}

/* Header */
.rx-book-additional-panel .rx-book-package-header {
    display: flex;
    align-items: center;
    gap: 16px;
    margin-bottom: 28px;
    padding-bottom: 22px;
    border-bottom: 1px solid #eeeaf2;
}

.rx-book-additional-panel .rx-book-section-icon {
    width: 54px;
    height: 54px;
    min-width: 54px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 16px;
    background: linear-gradient(135deg, #54227a, #7d3fa3);
    color: #ffffff;
    font-size: 23px;
    box-shadow: 0 8px 20px rgba(84, 34, 122, 0.20);
}

.rx-book-additional-panel .rx-book-package-header h2 {
    margin: 0 0 5px;
    color: #2d2035;
    font-size: 24px;
    font-weight: 700;
}

.rx-book-additional-panel .rx-book-package-header p {
    margin: 0;
    color: #77707d;
    font-size: 14px;
    line-height: 1.6;
}

/* Message */
.rx-book-additional-panel .rx-book-message {
    display: block;
    margin-bottom: 20px;
    color: #54227a;
    font-size: 14px;
    font-weight: 600;
}

/* Form */
.rx-book-additional-panel .rx-book-form-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 24px;
}

.rx-book-additional-panel .rx-book-form-group {
    display: flex;
    flex-direction: column;
}

.rx-book-additional-panel .rx-book-form-group.full-width {
    grid-column: 1 / -1;
}

/* Labels */
.rx-book-additional-panel .rx-book-form-label {
    margin-bottom: 8px;
    color: #392b42;
    font-size: 14px;
    font-weight: 600;
}

/* Inputs */
.rx-book-additional-panel .rx-book-form-control {
    width: 100%;
    min-height: 48px;
    box-sizing: border-box;
    padding: 12px 15px;
    border: 1px solid #ddd7e3;
    border-radius: 12px;
    background: #faf9fc;
    color: #33283a;
    font-family: inherit;
    font-size: 14px;
    outline: none;
    transition: all 0.2s ease;
}

.rx-book-additional-panel textarea.rx-book-form-control {
    min-height: 135px;
    resize: vertical;
    line-height: 1.6;
}

/* Focus */
.rx-book-additional-panel .rx-book-form-control:focus {
    border-color: #54227a;
    background: #ffffff;
    box-shadow: 0 0 0 3px rgba(84, 34, 122, 0.10);
}

/* Placeholder */
.rx-book-additional-panel .rx-book-form-control::placeholder {
    color: #aaa3ae;
}

/* Character count */
.rx-book-character-limit {
    margin-top: 7px;
    text-align: right;
    color: #99919f;
    font-size: 12px;
}

/* Bottom actions */
.rx-book-additional-panel .rx-book-package-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
    margin-top: 32px;
    padding-top: 24px;
    border-top: 1px solid #eeeaf2;
}

/* Back button */
.rx-book-additional-panel .rx-book-clear-button {
    min-height: 46px;
    padding: 11px 22px;
    border: 1px solid #d8d1df;
    border-radius: 11px;
    background: #ffffff;
    color: #5f5366;
    font-family: inherit;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s ease;
}

.rx-book-additional-panel .rx-book-clear-button:hover {
    border-color: #54227a;
    color: #54227a;
    background: #faf7fc;
}

/* Review button */
.rx-book-additional-panel .rx-book-next-button {
    min-height: 46px;
    padding: 11px 25px;
    border: 1px solid #54227a;
    border-radius: 11px;
    background: #54227a;
    color: #ffffff;
    font-family: inherit;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    box-shadow: 0 6px 16px rgba(84, 34, 122, 0.18);
    transition: all 0.2s ease;
}

.rx-book-additional-panel .rx-book-next-button:hover {
    background: #431b61;
    border-color: #431b61;
    transform: translateY(-1px);
    box-shadow: 0 9px 20px rgba(84, 34, 122, 0.24);
}


/* =========================================
   STEP 4 - DARK MODE
   ========================================= */

.theme-dark .rx-book-additional-panel .rx-book-event-card {
    background: #211b27;
    border-color: #3a3041;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
}

.theme-dark .rx-book-additional-panel .rx-book-package-header {
    border-bottom-color: #3a3041;
}

.theme-dark .rx-book-additional-panel .rx-book-package-header h2 {
    color: #ffffff;
}

.theme-dark .rx-book-additional-panel .rx-book-package-header p {
    color: #b9afc0;
}

.theme-dark .rx-book-additional-panel .rx-book-form-label {
    color: #eee7f1;
}

.theme-dark .rx-book-additional-panel .rx-book-form-control {
    background: #18131d;
    border-color: #403545;
    color: #ffffff;
}

.theme-dark .rx-book-additional-panel .rx-book-form-control:focus {
    background: #1d1722;
    border-color: #c9a227;
    box-shadow: 0 0 0 3px rgba(201, 162, 39, 0.12);
}

.theme-dark .rx-book-additional-panel .rx-book-form-control::placeholder {
    color: #817787;
}

.theme-dark .rx-book-character-limit {
    color: #8f8494;
}

.theme-dark .rx-book-additional-panel .rx-book-package-actions {
    border-top-color: #3a3041;
}

.theme-dark .rx-book-additional-panel .rx-book-clear-button {
    background: transparent;
    border-color: #4a3f50;
    color: #c9bfca;
}

.theme-dark .rx-book-additional-panel .rx-book-clear-button:hover {
    border-color: #c9a227;
    color: #c9a227;
    background: rgba(201, 162, 39, 0.06);
}


/* =========================================
   STEP 4 - RESPONSIVE
   ========================================= */

@media (max-width: 768px) {

    .rx-book-additional-panel .rx-book-event-card {
        padding: 24px 20px;
        border-radius: 18px;
    }

    .rx-book-additional-panel .rx-book-form-grid {
        grid-template-columns: 1fr;
        gap: 20px;
    }

    .rx-book-additional-panel .rx-book-form-group.full-width {
        grid-column: auto;
    }

    .rx-book-additional-panel .rx-book-package-header {
        align-items: flex-start;
    }

    .rx-book-additional-panel .rx-book-package-header h2 {
        font-size: 21px;
    }

    .rx-book-additional-panel .rx-book-package-actions {
        flex-direction: column-reverse;
        align-items: stretch;
    }

    .rx-book-additional-panel .rx-book-clear-button,
    .rx-book-additional-panel .rx-book-next-button {
        width: 100%;
    }
}
/* =========================================
   STEP 5 - REVIEW & CONFIRM
   ========================================= */

.rx-book-review-panel {
    width: 100%;
    margin-top: 24px;
}

.rx-book-review-panel .rx-book-event-card {
    background: #ffffff;
    border: 1px solid #eeeaf2;
    border-radius: 22px;
    padding: 32px;
    box-shadow: 0 10px 30px rgba(84, 34, 122, 0.08);
}

/* Header */
.rx-book-review-panel .rx-book-package-header {
    display: flex;
    align-items: center;
    gap: 16px;
    margin-bottom: 28px;
    padding-bottom: 22px;
    border-bottom: 1px solid #eeeaf2;
}

.rx-book-review-panel .rx-book-section-icon {
    width: 54px;
    height: 54px;
    min-width: 54px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 16px;
    background: linear-gradient(135deg, #c9a227, #e2c45c);
    color: #ffffff;
    font-size: 25px;
    font-weight: 700;
    box-shadow: 0 8px 20px rgba(201, 162, 39, 0.22);
}

.rx-book-review-panel .rx-book-package-header h2 {
    margin: 0 0 5px;
    color: #2d2035;
    font-size: 24px;
    font-weight: 700;
}

.rx-book-review-panel .rx-book-package-header p {
    margin: 0;
    color: #77707d;
    font-size: 14px;
    line-height: 1.6;
}

/* Message */
.rx-book-review-panel .rx-book-message {
    display: block;
    margin-bottom: 20px;
    color: #54227a;
    font-size: 14px;
    font-weight: 600;
}

/* =========================================
   REVIEW SECTIONS
   ========================================= */

.rx-book-review-section {
    margin-bottom: 24px;
    padding: 24px;
    background: #faf9fc;
    border: 1px solid #eeeaf2;
    border-radius: 16px;
}

.rx-book-review-section-title {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 20px;
}

.rx-book-review-section-title span {
    font-size: 20px;
}

.rx-book-review-section-title h3 {
    margin: 0;
    color: #392b42;
    font-size: 17px;
    font-weight: 700;
}

/* =========================================
   EVENT DETAILS GRID
   ========================================= */

.rx-book-review-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 18px;
}

.rx-book-review-item {
    display: flex;
    flex-direction: column;
    gap: 6px;
}

.rx-book-review-item span {
    color: #8a8190;
    font-size: 12px;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.4px;
}

.rx-book-review-item strong {
    color: #33283a;
    font-size: 15px;
    font-weight: 600;
}

/* =========================================
   VENUE / INSPIRATION HIGHLIGHT
   ========================================= */

.rx-book-review-highlight {
    display: flex;
    flex-direction: column;
    gap: 7px;
    padding: 18px;
    border-left: 4px solid #c9a227;
    border-radius: 10px;
    background: #ffffff;
}

.rx-book-review-highlight strong {
    color: #392b42;
    font-size: 16px;
}

.rx-book-review-highlight span {
    color: #77707d;
    font-size: 13px;
    line-height: 1.5;
}

/* =========================================
   PACKAGE
   ========================================= */

.rx-book-review-package {
    display: flex;
    gap: 22px;
    padding: 18px;
    background: #ffffff;
    border: 1px solid #eeeaf2;
    border-radius: 14px;
}

.rx-book-review-package-image {
    width: 150px;
    height: 110px;
    object-fit: cover;
    border-radius: 12px;
    flex-shrink: 0;
}

.rx-book-review-package-content {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
}

.rx-book-review-package-type {
    display: inline-block;
    margin-bottom: 5px;
    color: #c9a227;
    font-size: 11px;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.8px;
}

.rx-book-review-package-content h3 {
    margin: 0 0 7px;
    color: #392b42;
    font-size: 20px;
}

.rx-book-review-package-content p {
    margin: 0 0 10px;
    color: #77707d;
    font-size: 13px;
    line-height: 1.5;
}

.rx-book-review-price {
    color: #54227a;
    font-size: 17px;
    font-weight: 700;
}

/* =========================================
   EVENT PREFERENCES
   ========================================= */

.rx-book-review-preferences {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 18px;
}

.rx-book-review-preferences > div {
    display: flex;
    flex-direction: column;
    gap: 6px;
    padding: 15px;
    background: #ffffff;
    border: 1px solid #eeeaf2;
    border-radius: 11px;
}

.rx-book-review-preferences > div.full-width {
    grid-column: 1 / -1;
}

.rx-book-review-preferences span {
    color: #8a8190;
    font-size: 12px;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.35px;
}

.rx-book-review-preferences strong {
    color: #392b42;
    font-size: 14px;
    font-weight: 600;
    line-height: 1.5;
}

/* =========================================
   TOTAL
   ========================================= */

.rx-book-review-total {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 20px;
    margin-top: 28px;
    padding: 24px;
    border: 1px solid rgba(201, 162, 39, 0.45);
    border-radius: 16px;
    background: linear-gradient(
        135deg,
        rgba(84, 34, 122, 0.06),
        rgba(201, 162, 39, 0.08)
    );
}

.rx-book-review-total > div {
    display: flex;
    flex-direction: column;
    gap: 5px;
}

.rx-book-review-total span {
    color: #392b42;
    font-size: 16px;
    font-weight: 700;
}

.rx-book-review-total small {
    color: #8a8190;
    font-size: 12px;
}

.rx-book-review-total > strong {
    color: #54227a;
    font-size: 24px;
    font-weight: 800;
    white-space: nowrap;
}

/* =========================================
   ACTIONS
   ========================================= */

.rx-book-review-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
    margin-top: 30px;
    padding-top: 24px;
    border-top: 1px solid #eeeaf2;
}

.rx-book-review-actions .rx-book-clear-button {
    min-height: 48px;
    padding: 12px 23px;
    border: 1px solid #d8d1df;
    border-radius: 11px;
    background: #ffffff;
    color: #5f5366;
    font-family: inherit;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s ease;
}

.rx-book-review-actions .rx-book-clear-button:hover {
    border-color: #54227a;
    color: #54227a;
    background: #faf7fc;
}

/* Confirm Booking */
.rx-book-confirm-button {
    min-height: 50px;
    padding: 13px 30px;
    border: 1px solid #c9a227;
    border-radius: 11px;
    background: linear-gradient(135deg, #c9a227, #b58d18);
    color: #ffffff;
    font-family: inherit;
    font-size: 15px;
    font-weight: 700;
    cursor: pointer;
    box-shadow: 0 8px 20px rgba(201, 162, 39, 0.22);
    transition: all 0.2s ease;
}

.rx-book-confirm-button:hover {
    background: linear-gradient(135deg, #b58d18, #9f7913);
    border-color: #9f7913;
    transform: translateY(-1px);
    box-shadow: 0 11px 24px rgba(201, 162, 39, 0.30);
}


/* =========================================
   STEP 5 - DARK MODE
   ========================================= */

.theme-dark .rx-book-review-panel .rx-book-event-card {
    background: #211b27;
    border-color: #3a3041;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
}

.theme-dark .rx-book-review-panel .rx-book-package-header {
    border-bottom-color: #3a3041;
}

.theme-dark .rx-book-review-panel .rx-book-package-header h2 {
    color: #ffffff;
}

.theme-dark .rx-book-review-panel .rx-book-package-header p {
    color: #b9afc0;
}

.theme-dark .rx-book-review-section {
    background: #18131d;
    border-color: #3a3041;
}

.theme-dark .rx-book-review-section-title h3 {
    color: #eee7f1;
}

.theme-dark .rx-book-review-item span,
.theme-dark .rx-book-review-preferences span {
    color: #918594;
}

.theme-dark .rx-book-review-item strong,
.theme-dark .rx-book-review-preferences strong {
    color: #eee7f1;
}

.theme-dark .rx-book-review-highlight {
    background: #211b27;
    border-left-color: #c9a227;
}

.theme-dark .rx-book-review-highlight strong {
    color: #ffffff;
}

.theme-dark .rx-book-review-highlight span {
    color: #aaa0af;
}

.theme-dark .rx-book-review-package {
    background: #211b27;
    border-color: #3a3041;
}

.theme-dark .rx-book-review-package-content h3 {
    color: #ffffff;
}

.theme-dark .rx-book-review-package-content p {
    color: #aaa0af;
}

.theme-dark .rx-book-review-preferences > div {
    background: #211b27;
    border-color: #3a3041;
}

.theme-dark .rx-book-review-total {
    border-color: rgba(201, 162, 39, 0.35);
    background: linear-gradient(
        135deg,
        rgba(84, 34, 122, 0.15),
        rgba(201, 162, 39, 0.08)
    );
}

.theme-dark .rx-book-review-total span {
    color: #eee7f1;
}

.theme-dark .rx-book-review-total small {
    color: #918594;
}

.theme-dark .rx-book-review-actions {
    border-top-color: #3a3041;
}

.theme-dark .rx-book-review-actions .rx-book-clear-button {
    background: transparent;
    border-color: #4a3f50;
    color: #c9bfca;
}

.theme-dark .rx-book-review-actions .rx-book-clear-button:hover {
    border-color: #c9a227;
    color: #c9a227;
}


/* =========================================
   STEP 5 - RESPONSIVE
   ========================================= */

@media (max-width: 768px) {

    .rx-book-review-panel .rx-book-event-card {
        padding: 24px 20px;
        border-radius: 18px;
    }

    .rx-book-review-grid {
        grid-template-columns: 1fr;
    }

    .rx-book-review-preferences {
        grid-template-columns: 1fr;
    }

    .rx-book-review-preferences > div.full-width {
        grid-column: auto;
    }

    .rx-book-review-package {
        flex-direction: column;
    }

    .rx-book-review-package-image {
        width: 100%;
        height: 180px;
    }

    .rx-book-review-total {
        flex-direction: column;
        align-items: flex-start;
    }

    .rx-book-review-total > strong {
        font-size: 21px;
    }

    .rx-book-review-actions {
        flex-direction: column-reverse;
        align-items: stretch;
    }

    .rx-book-review-actions .rx-book-clear-button,
    .rx-book-review-actions .rx-book-confirm-button {
        width: 100%;
    }
}
/* =========================================
   5-STEP BOOKING PROGRESS
   ========================================= */

.rx-book-progress {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 0;
    margin: 0 auto 30px;
    padding: 8px 10px;
}

.rx-book-step {
    display: flex;
    align-items: center;
    gap: 9px;
    min-width: 0;
    flex: 0 1 auto;
}

.rx-book-step-number {
    width: 38px;
    height: 38px;
    min-width: 38px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 50%;
    background: #54227a;
    border: 1px solid #c9a227;
    color: #ffffff;
    font-size: 15px;
    font-weight: 700;
}

.rx-book-step.active .rx-book-step-number {
    background: #c9a227;
    border-color: #c9a227;
    color: #ffffff;
    box-shadow: 0 5px 15px rgba(201, 162, 39, 0.30);
}

.rx-book-step-info {
    display: flex;
    flex-direction: column;
    gap: 2px;
    white-space: nowrap;
}

.rx-book-step-info strong {
    color: #ffffff;
    font-size: 13px;
    font-weight: 700;
}

.rx-book-step-info span {
    color: #b9afc0;
    font-size: 11px;
}

.rx-book-progress-line {
    width: 38px;
    height: 1px;
    margin: 0 10px;
    background: #6b5b74;
}

/* Mobile */
@media (max-width: 900px) {

    .rx-book-progress {
        overflow-x: auto;
        justify-content: flex-start;
        padding-bottom: 12px;
    }

    .rx-book-step-info {
        display: none;
    }

    .rx-book-progress-line {
        width: 22px;
        margin: 0 6px;
    }
}
/* =========================================
   STEP 3 - INSPIRATION FOLDERS
   ========================================= */

.rx-book-inspiration-panel {
    width: 100%;
    margin-top: 24px;
}

.rx-book-inspiration-panel .rx-book-event-card {
    background: #ffffff;
    border: 1px solid #eeeaf2;
    border-radius: 22px;
    padding: 30px;
    box-shadow: 0 10px 30px rgba(84, 34, 122, 0.08);
}

.rx-book-inspiration-info {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    margin-bottom: 24px;
    padding: 15px 17px;
    border-radius: 12px;
    background: #faf7fc;
    border: 1px solid #eee5f3;
}

.rx-book-inspiration-info-icon {
    font-size: 19px;
}

.rx-book-inspiration-info strong {
    display: block;
    margin-bottom: 3px;
    color: #392b42;
    font-size: 14px;
}

.rx-book-inspiration-info p {
    margin: 0;
    color: #77707d;
    font-size: 12px;
    line-height: 1.5;
}

/* Folder grid */
.rx-book-inspiration-grid {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr));
    gap: 18px;
}

/* Folder card */
.rx-book-inspiration-card {
    overflow: hidden;
    background: #ffffff;
    border: 1px solid #e6e0ea;
    border-radius: 14px;
    box-shadow: 0 5px 15px rgba(84, 34, 122, 0.06);
    transition: all 0.2s ease;
}

.rx-book-inspiration-card:hover {
    transform: translateY(-3px);
    border-color: #b9a4c8;
    box-shadow: 0 10px 22px rgba(84, 34, 122, 0.12);
}

/* Selected folder */
.rx-book-inspiration-card.selected {
    border: 2px solid #c9a227;
    box-shadow: 0 10px 25px rgba(201, 162, 39, 0.20);
}

/* Smaller image */
.rx-book-inspiration-cover {
    width: 100%;
    height: 145px;
    overflow: hidden;
    background: #f1edf4;
}

.rx-book-inspiration-image {
    width: 100%;
    height: 100%;
    display: block;
    object-fit: cover;
}

/* Content */
.rx-book-inspiration-content {
    padding: 15px;
}

.rx-book-inspiration-category {
    color: #c9a227;
    font-size: 10px;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.7px;
}

.rx-book-inspiration-content h3 {
    margin: 5px 0 9px;
    color: #392b42;
    font-size: 16px;
    font-weight: 700;
}

.rx-book-inspiration-details {
    display: flex;
    flex-direction: column;
    gap: 5px;
    margin-bottom: 13px;
}

.rx-book-inspiration-details span {
    color: #77707d;
    font-size: 11px;
}

/* Attach button */
.rx-book-inspiration-select-button {
    width: 100%;
    min-height: 38px;
    padding: 8px 12px;
    border: 1px solid #54227a;
    border-radius: 9px;
    background: #ffffff;
    color: #54227a;
    font-family: inherit;
    font-size: 12px;
    font-weight: 700;
    cursor: pointer;
    transition: all 0.2s ease;
}

.rx-book-inspiration-select-button:hover {
    background: #54227a;
    color: #ffffff;
}

.rx-book-inspiration-card.selected
.rx-book-inspiration-select-button {
    background: #c9a227;
    border-color: #c9a227;
    color: #ffffff;
}

/* =========================================
   STEP 3 BUTTONS
   ========================================= */

.rx-book-inspiration-actions {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 20px;
    margin-top: 28px;
    padding-top: 22px;
    border-top: 1px solid #eeeaf2;
}

.rx-book-inspiration-next-actions {
    display: flex;
    align-items: center;
    gap: 12px;
}

/* Keep all buttons on one line */
.rx-book-inspiration-actions .rx-book-clear-button,
.rx-book-inspiration-actions .rx-book-skip-button,
.rx-book-inspiration-actions .rx-book-next-button {
    white-space: nowrap;
    min-height: 44px;
}

/* Skip */
.rx-book-skip-button {
    min-height: 44px;
    padding: 10px 19px;
    border: 1px solid #d8d1df;
    border-radius: 10px;
    background: #ffffff;
    color: #655a6b;
    font-family: inherit;
    font-size: 13px;
    font-weight: 600;
    cursor: pointer;
}

.rx-book-skip-button:hover {
    border-color: #54227a;
    color: #54227a;
    background: #faf7fc;
}

/* =========================================
   STEP 3 - DARK MODE
   ========================================= */

.theme-dark .rx-book-inspiration-panel .rx-book-event-card {
    background: #211b27;
    border-color: #3a3041;
}

.theme-dark .rx-book-inspiration-info {
    background: #18131d;
    border-color: #3a3041;
}

.theme-dark .rx-book-inspiration-info strong {
    color: #eee7f1;
}

.theme-dark .rx-book-inspiration-info p {
    color: #aaa0af;
}

.theme-dark .rx-book-inspiration-card {
    background: #18131d;
    border-color: #3a3041;
}

.theme-dark .rx-book-inspiration-card.selected {
    border-color: #c9a227;
}

.theme-dark .rx-book-inspiration-content h3 {
    color: #ffffff;
}

.theme-dark .rx-book-inspiration-details span {
    color: #aaa0af;
}

.theme-dark .rx-book-inspiration-actions {
    border-top-color: #3a3041;
}

.theme-dark .rx-book-skip-button {
    background: transparent;
    border-color: #4a3f50;
    color: #c9bfca;
}

/* Responsive */
@media (max-width: 900px) {
    .rx-book-inspiration-grid {
        grid-template-columns: repeat(2, minmax(0, 1fr));
    }
}

@media (max-width: 600px) {

    .rx-book-inspiration-grid {
        grid-template-columns: 1fr;
    }

    .rx-book-inspiration-actions {
        flex-direction: column;
        align-items: stretch;
    }

    .rx-book-inspiration-next-actions {
        width: 100%;
        justify-content: stretch;
    }

    .rx-book-inspiration-next-actions > * {
        flex: 1;
    }

    .rx-book-inspiration-actions .rx-book-clear-button {
        width: 100%;
    }
}   

</style>

</asp:Content>

<asp:Content ID="BookEventMainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="rx-book-event-page">

        <!-- BREADCRUMB -->
        <div class="rx-book-breadcrumb">
            <span>Home</span>
            <span class="rx-book-breadcrumb-arrow">›</span>
            <span>Book Event</span>
        </div>

        <!-- PAGE HEADER -->
        <div class="rx-book-event-hero">

    <asp:Image
        ID="BookEventHeroImage"
        runat="server"
        ImageUrl="~/Content/images/event-hero.jpg"
        CssClass="rx-book-hero-image" />

    <div class="rx-book-hero-overlay"></div>

    <div class="rx-book-hero-content">
        <h1>Book an Event</h1>

        <p>
            Plan your perfect event in just a few steps.
        </p>
    </div>

</div>

        <!-- PROGRESS STEPS -->
<div class="rx-book-progress">

    <!-- STEP 1 -->
    <div class='<%= GetProgressStepClass(1) %>'>
        <div class="rx-book-step-number">1</div>
        <div class="rx-book-step-info">
            <strong>Event Details</strong>
            <span>Plan your event</span>
        </div>
    </div>

    <div class="rx-book-progress-line"></div>

    <!-- STEP 2 -->
    <div class='<%= GetProgressStepClass(2) %>'>
        <div class="rx-book-step-number">2</div>
        <div class="rx-book-step-info">
            <strong>Select Package</strong>
            <span>Choose your package</span>
        </div>
    </div>

    <div class="rx-book-progress-line"></div>

    <!-- STEP 3 -->
    <div class='<%= GetProgressStepClass(3) %>'>
        <div class="rx-book-step-number">3</div>
        <div class="rx-book-step-info">
            <strong>Inspiration</strong>
            <span>Share your vision</span>
        </div>
    </div>

    <div class="rx-book-progress-line"></div>

    <!-- STEP 4 -->
    <div class='<%= GetProgressStepClass(4) %>'>
        <div class="rx-book-step-number">4</div>
        <div class="rx-book-step-info">
            <strong>Customize Event</strong>
            <span>Add your preferences</span>
        </div>
    </div>

    <div class="rx-book-progress-line"></div>

    <!-- STEP 5 -->
    <div class='<%= GetProgressStepClass(5) %>'>
        <div class="rx-book-step-number">5</div>
        <div class="rx-book-step-info">
            <strong>Review &amp; Confirm</strong>
            <span>Complete your booking</span>
        </div>
    </div>

</div>

        <!-- STEP 1: EVENT DETAILS -->
<asp:Panel
    ID="Step1Panel"
    runat="server"
    CssClass="rx-book-step-panel">

    <div class="rx-book-event-grid">

            <!-- FORM -->
            <section class="rx-book-event-card">

                <div class="rx-book-section-heading">

                    <div class="rx-book-section-icon">
                        📅
                    </div>

                    <div>
                        <h2>Event Information</h2>

                        <p>
                            Fill in the details below to get started.
                        </p>
                    </div>

                </div>

                <asp:Label
                    ID="BookEventMessageLabel"
                    runat="server"
                    CssClass="rx-book-message"
                    EnableViewState="false">
                </asp:Label>

                <div class="rx-book-form-grid">

                    <!-- EVENT TYPE -->
                    <div class="rx-book-form-group">

                        <asp:Label
                            ID="EventCategoryLabel"
                            runat="server"
                            AssociatedControlID="EventCategoryDropDown"
                            Text="Event Type *"
                            CssClass="rx-book-form-label">
                        </asp:Label>

                        <asp:DropDownList
                            ID="EventCategoryDropDown"
                            runat="server"
                            CssClass="rx-book-form-control"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="EventCategoryDropDown_SelectedIndexChanged">
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator
                            ID="EventCategoryValidator"
                            runat="server"
                            ControlToValidate="EventCategoryDropDown"
                            InitialValue=""
                            ErrorMessage="Please select an event type."
                            CssClass="rx-book-validation"
                            EnableClientScript="false"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>

                    </div>

                    <!-- EVENT TITLE -->
                    <div class="rx-book-form-group">

                        <asp:Label
                            ID="EventLabel"
                            runat="server"
                            AssociatedControlID="EventDropDown"
                            Text="Event Title *"
                            CssClass="rx-book-form-label">
                        </asp:Label>

                        <asp:DropDownList
                            ID="EventDropDown"
                            runat="server"
                            CssClass="rx-book-form-control"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="EventDropDown_SelectedIndexChanged">
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator
                            ID="EventValidator"
                            runat="server"
                            ControlToValidate="EventDropDown"
                            InitialValue=""
                            ErrorMessage="Please select an event."
                            CssClass="rx-book-validation"
                            EnableClientScript="false"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>

                    </div>

                    <!-- EVENT DATE -->
                    <div class="rx-book-form-group">

                        <asp:Label
                            ID="EventDateLabel"
                            runat="server"
                            AssociatedControlID="EventDateTextBox"
                            Text="Event Date *"
                            CssClass="rx-book-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="EventDateTextBox"
                            runat="server"
                            TextMode="Date"
                            CssClass="rx-book-form-control">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="EventDateValidator"
                            runat="server"
                            ControlToValidate="EventDateTextBox"
                            ErrorMessage="Please select an event date."
                            CssClass="rx-book-validation"
                            EnableClientScript="false"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>

                    </div>

                    <!-- EVENT TIME -->
                    <div class="rx-book-form-group">

                        <asp:Label
                            ID="EventTimeLabel"
                            runat="server"
                            AssociatedControlID="EventTimeTextBox"
                            Text="Event Time *"
                            CssClass="rx-book-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="EventTimeTextBox"
                            runat="server"
                            TextMode="Time"
                            CssClass="rx-book-form-control">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="EventTimeValidator"
                            runat="server"
                            ControlToValidate="EventTimeTextBox"
                            ErrorMessage="Please select an event time."
                            CssClass="rx-book-validation"
                            EnableClientScript="false"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>

                    </div>

                    <!-- GUEST COUNT -->
                    <div class="rx-book-form-group">

                        <asp:Label
                            ID="GuestCountLabel"
                            runat="server"
                            AssociatedControlID="GuestCountTextBox"
                            Text="Expected Number of Guests *"
                            CssClass="rx-book-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="GuestCountTextBox"
                            runat="server"
                            TextMode="Number"
                            CssClass="rx-book-form-control">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="GuestCountRequiredValidator"
                            runat="server"
                            ControlToValidate="GuestCountTextBox"
                            ErrorMessage="Please enter the number of guests."
                            CssClass="rx-book-validation"
                            EnableClientScript="false"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>

                        <asp:RangeValidator
                            ID="GuestCountRangeValidator"
                            runat="server"
                            ControlToValidate="GuestCountTextBox"
                            MinimumValue="1"
                            MaximumValue="10000"
                            Type="Integer"
                            ErrorMessage="Guest count must be between 1 and 10,000."
                            CssClass="rx-book-validation"
                            EnableClientScript="false"
                            Display="Dynamic">
                        </asp:RangeValidator>

                    </div>

                    <!-- VENUE -->
<div class="rx-book-form-group">

    <asp:Label
        ID="VenueLabel"
        runat="server"
        AssociatedControlID="VenueDropDown"
        Text="Preferred Venue *"
        CssClass="rx-book-form-label">
    </asp:Label>

    <asp:DropDownList
        ID="VenueDropDown"
        runat="server"
        CssClass="rx-book-form-control"
        AutoPostBack="true"
        OnSelectedIndexChanged="VenueDropDown_SelectedIndexChanged">
    </asp:DropDownList>

    <asp:RequiredFieldValidator
        ID="VenueValidator"
        runat="server"
        ControlToValidate="VenueDropDown"
        InitialValue=""
        ErrorMessage="Please select a venue."
        CssClass="rx-book-validation"
        EnableClientScript="false"
        Display="Dynamic">
    </asp:RequiredFieldValidator>

</div>

<!-- CUSTOMER VENUE ADDRESS -->
<asp:Panel
    ID="OwnVenueAddressPanel"
    runat="server"
    CssClass="rx-book-own-venue-panel"
    Visible="false">

    <div class="rx-book-own-venue-header">

        <div>
            <h3>Your Event Location</h3>

            <p>
                Since your event will be held at your own location,
                please provide the address for our event planning team.
            </p>
        </div>

    </div>

    <asp:Label
        ID="VenueAddressLabel"
        runat="server"
        AssociatedControlID="VenueAddressTextBox"
        Text="Event Address *"
        CssClass="rx-book-form-label">
    </asp:Label>

    <asp:TextBox
        ID="VenueAddressTextBox"
        runat="server"
        TextMode="MultiLine"
        Rows="4"
        CssClass="rx-book-form-control"
        placeholder="Enter the full address where the event will take place...">
    </asp:TextBox>

    <asp:RequiredFieldValidator
    ID="VenueAddressValidator"
    runat="server"
    ControlToValidate="VenueAddressTextBox"
    ErrorMessage="Please enter the event address."
    CssClass="rx-book-validation"
    EnableClientScript="false"
    Display="Dynamic"
    Enabled="false">
</asp:RequiredFieldValidator>

</asp:Panel>

                    <!-- NOTES -->
                    <div class="rx-book-form-group full-width">

                        <asp:Label
                            ID="AdditionalNotesLabel"
                            runat="server"
                            AssociatedControlID="AdditionalNotesTextBox"
                            Text="Additional Notes"
                            CssClass="rx-book-form-label">
                        </asp:Label>

                        <asp:TextBox
                            ID="AdditionalNotesTextBox"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="4"
                            MaxLength="1000"
                            CssClass="rx-book-form-control rx-book-notes"
                            placeholder="Share any special requirements, themes or ideas for your event...">
                        </asp:TextBox>

                        <div class="rx-book-character-limit">
                            Maximum 1000 characters
                        </div>

                    </div>

                </div>

                <!-- ACTIONS -->
                <div class="rx-book-form-actions">

                    <asp:Button
                        ID="ClearFormButton"
                        runat="server"
                        Text="Clear Form"
                        CssClass="rx-book-clear-button"
                        CausesValidation="false"
                        OnClick="ClearFormButton_Click">
                    </asp:Button>

                    <asp:Button
                        ID="NextPackageButton"
                        runat="server"
                        Text="Next: Select Package →"
                        CssClass="rx-book-next-button"
                        OnClick="NextPackageButton_Click">
                    </asp:Button>

                </div>

            </section>

            <!-- RIGHT PREVIEW -->
            <aside class="rx-book-preview-column">

                <div class="rx-book-preview-card">

                    <div class="rx-book-preview-heading">
                        <span>◉</span>
                        Event Preview
                    </div>

                    <asp:Image
                        ID="EventPreviewImage"
                        runat="server"
                        CssClass="rx-book-preview-image"
                        ImageUrl="~/Content/images/rx-events-logo.png">
                    </asp:Image>

                    <asp:Label
                        ID="EventPreviewNameLabel"
                        runat="server"
                        Text="Choose an event"
                        CssClass="rx-book-preview-title">
                    </asp:Label>

                    <div class="rx-book-preview-details">

                        <div>
                            <span>⌖</span>
                            <asp:Label
                                ID="VenuePreviewLabel"
                                runat="server"
                                Text="Select a venue">
                            </asp:Label>
                        </div>

                        <div>
                            <span>◷</span>
                            <asp:Label
                                ID="TimePreviewLabel"
                                runat="server"
                                Text="Select a time">
                            </asp:Label>
                        </div>

                        <div>
                            <span>♟</span>
                            <asp:Label
                                ID="GuestsPreviewLabel"
                                runat="server"
                                Text="0 Guests">
                            </asp:Label>
                        </div>

                        <div>
                            <span>◆</span>
                            <asp:Label
                                ID="PricePreviewLabel"
                                runat="server"
                                Text="Starting from UGX 0">
                            </asp:Label>
                        </div>

                    </div>

                </div>

                <!-- HELP CARD -->
                <div class="rx-book-help-card">

                    <div class="rx-book-help-icon">
                        ?
                    </div>

                    <div>
                        <h3>Need Help?</h3>

                        <p>
                            Not sure what to choose?
                            Our event planners are here to help.
                        </p>

                        <asp:Button
                            ID="ContactPlannerButton"
                            runat="server"
                            Text="Contact an Event Planner"
                            CssClass="rx-book-contact-button"
                            CausesValidation="false">
                        </asp:Button>
                    </div>

                </div>

            </aside>

           </div>

</asp:Panel>

        <!-- STEP 2: SELECT PACKAGE -->
<asp:Panel
    ID="Step2Panel"
    runat="server"
    CssClass="rx-book-package-panel"
    Visible="false">

    <div class="rx-book-package-header">

        <div class="rx-book-section-icon">
            💎
        </div>

        <div>
            <h2>Select Your Package</h2>

            <p>
                Choose the package that best matches your event needs.
            </p>
        </div>

    </div>

    <asp:Label
        ID="PackageSelectionMessageLabel"
        runat="server"
        CssClass="rx-book-message">
    </asp:Label>

    <div class="rx-book-package-grid">

        <asp:Repeater
            ID="PackagesRepeater"
            runat="server"
            OnItemCommand="PackagesRepeater_ItemCommand">

            <ItemTemplate>

                <div class='<%# GetPackageCardClass(Eval("PackageID")) %>'>

                    <div class="rx-book-package-image-wrapper">

                        <asp:Image
    ID="PackageImage"
    runat="server"
    ImageUrl='<%# GetPackageImageUrl(Eval("ImagePath")) %>'
    CssClass="rx-book-package-image" />

                    </div>

                    <div class="rx-book-package-content">

                        <span class="rx-book-package-type">
                            <%# Eval("PackageType") %>
                        </span>

                        <h3>
                            <%# Eval("PackageName") %>
                        </h3>

                        <p class="rx-book-package-description">
                            <%# Eval("Description") %>
                        </p>

                        <div class="rx-book-package-price">
                            UGX
                            <%# Convert.ToDecimal(Eval("Price")).ToString("N0") %>
                        </div>

                        <asp:Button
                            ID="SelectPackageButton"
                            runat="server"
                            Text="Select Package"
                            CssClass="rx-book-package-select-button"
                            CommandName="SelectPackage"
                            CommandArgument='<%# Eval("PackageID") %>'
                            CausesValidation="false" />

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>

    </div>

    <div class="rx-book-package-actions">

        <asp:Button
            ID="BackToDetailsButton"
            runat="server"
            Text="← Back to Event Details"
            CssClass="rx-book-clear-button"
            CausesValidation="false"
            OnClick="BackToDetailsButton_Click">
        </asp:Button>

        <asp:Button
    ID="NextAdditionalInfoButton"
    runat="server"
    Text="Next: Additional Info →"
    CssClass="rx-book-next-button"
    CausesValidation="false"
    OnClick="NextAdditionalInfoButton_Click" />

    </div>

</asp:Panel>
        <!-- STEP 3: ATTACH INSPIRATION -->
<asp:Panel
    ID="Step3Panel"
    runat="server"
    CssClass="rx-book-inspiration-panel"
    Visible="false">

    <div class="rx-book-event-card">

        <div class="rx-book-package-header">
            <div class="rx-book-section-icon">📌</div>

            <div>
                <h2>Attach Your Inspiration</h2>
                <p>
                    Choose an inspiration folder to help our event planners
                    understand your vision.
                </p>
            </div>
        </div>

        <asp:Label
            ID="InspirationMessageLabel"
            runat="server"
            CssClass="rx-book-message">
        </asp:Label>

        <div class="rx-book-inspiration-info">
            <span class="rx-book-inspiration-info-icon">💡</span>

            <div>
                <strong>Make your vision easier to understand</strong>
                <p>
                    Your selected inspiration folder will be attached to
                    your booking so our planning team can use your ideas
                    when preparing your event.
                </p>
            </div>
        </div>

        <div class="rx-book-inspiration-grid">

            <asp:Repeater
                ID="WishlistFoldersRepeater"
                runat="server"
                OnItemCommand="WishlistFoldersRepeater_ItemCommand">

                <ItemTemplate>

                    <div class='<%# GetInspirationFolderCardClass(Eval("WishlistFolderID")) %>'>

                        <div class="rx-book-inspiration-cover">

                            <asp:Image
                                ID="FolderCoverImage"
                                runat="server"
                                ImageUrl='<%# GetWishlistCoverImageUrl(Eval("CoverImage")) %>'
                                CssClass="rx-book-inspiration-image" />

                        </div>

                        <div class="rx-book-inspiration-content">

                            <span class="rx-book-inspiration-category">
                                <%# Eval("CategoryName") %>
                            </span>

                            <h3>
                                <%# Eval("FolderName") %>
                            </h3>

                            <div class="rx-book-inspiration-details">

                                <span>
                                    🖼 <%# Eval("ImageCount") %> inspiration images
                                </span>

                                <span>
                                    💰
                                    <%#
                                        Eval("Budget") == DBNull.Value ||
                                        Eval("Budget") == null
                                        ? "No budget set"
                                        : "UGX " + Convert.ToDecimal(Eval("Budget")).ToString("N0")
                                    %>
                                </span>

                            </div>

                            <asp:Button
                                ID="SelectInspirationButton"
                                runat="server"
                                Text="Attach This Folder"
                                CssClass="rx-book-inspiration-select-button"
                                CommandName="SelectInspiration"
                                CommandArgument='<%# Eval("WishlistFolderID") %>'
                                CausesValidation="false" />

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>

        <asp:Panel
            ID="NoWishlistFoldersPanel"
            runat="server"
            CssClass="rx-book-no-inspiration"
            Visible="false">

            <div class="rx-book-no-inspiration-icon">
                📁
            </div>

            <h3>No Inspiration Folders Yet</h3>

            <p>
                You haven't created an inspiration folder yet.
                You can continue without one and add your ideas later.
            </p>

        </asp:Panel>

        <div class="rx-book-inspiration-actions">

            <asp:Button
                ID="BackToPackageButton"
                runat="server"
                Text="← Back to Packages"
                CssClass="rx-book-clear-button"
                CausesValidation="false"
                OnClick="BackToPackageButton_Click" />

            <div class="rx-book-inspiration-next-actions">

                <asp:Button
                    ID="SkipInspirationButton"
                    runat="server"
                    Text="Skip for Now"
                    CssClass="rx-book-skip-button"
                    CausesValidation="false"
                    OnClick="SkipInspirationButton_Click" />

                <asp:Button
                    ID="Button1"
                    runat="server"
                    Text="Next: Customize Event →"
                    CssClass="rx-book-next-button"
                    CausesValidation="false"
                    OnClick="NextCustomizeEventButton_Click" />

            </div>

        </div>

    </div>

</asp:Panel>
        <!-- STEP 4: CUSTOMIZE YOUR EVENT -->
<asp:Panel
    ID="Step4Panel"
    runat="server"
    CssClass="rx-book-additional-panel"
    Visible="false">

    <div class="rx-book-event-card">

        <div class="rx-book-package-header">

            <div class="rx-book-section-icon">
                ✨
            </div>

            <div>
                <h2>Customize Your Event</h2>

                <p>
                    Tell us about your preferences so we can make your
                    event special.
                </p>
            </div>

        </div>

        <asp:Label
            ID="AdditionalInfoMessageLabel"
            runat="server"
            CssClass="rx-book-message">
        </asp:Label>

        <div class="rx-book-form-grid">

            <!-- Preferred Theme -->
            <div class="rx-book-form-group">

                <asp:Label
                    ID="EventThemeLabel"
                    runat="server"
                    AssociatedControlID="EventThemeTextBox"
                    Text="Preferred Theme"
                    CssClass="rx-book-form-label">
                </asp:Label>

                <asp:TextBox
                    ID="EventThemeTextBox"
                    runat="server"
                    CssClass="rx-book-form-control"
                    MaxLength="150"
                    placeholder="e.g. Royal Purple &amp; Gold">
                </asp:TextBox>

            </div>


            <!-- Decoration Style -->
            <div class="rx-book-form-group">

                <asp:Label
                    ID="DecorationStyleLabel"
                    runat="server"
                    AssociatedControlID="DecorationStyleDropDown"
                    Text="Decoration Style"
                    CssClass="rx-book-form-label">
                </asp:Label>

                <asp:DropDownList
                    ID="DecorationStyleDropDown"
                    runat="server"
                    CssClass="rx-book-form-control">

                    <asp:ListItem
                        Text="Select a style"
                        Value="">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="Elegant &amp; Classic"
                        Value="Elegant &amp; Classic">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="Luxury &amp; Glamorous"
                        Value="Luxury &amp; Glamorous">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="Modern &amp; Minimal"
                        Value="Modern &amp; Minimal">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="Traditional"
                        Value="Traditional">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="Garden &amp; Floral"
                        Value="Garden &amp; Floral">
                    </asp:ListItem>

                </asp:DropDownList>

            </div>


            <!-- Entertainment -->
            <div class="rx-book-form-group">

                <asp:Label
                    ID="EntertainmentLabel"
                    runat="server"
                    AssociatedControlID="EntertainmentTextBox"
                    Text="Entertainment Preferences"
                    CssClass="rx-book-form-label">
                </asp:Label>

                <asp:TextBox
                    ID="EntertainmentTextBox"
                    runat="server"
                    CssClass="rx-book-form-control"
                    MaxLength="250"
                    placeholder="e.g. DJ, live band, MC...">
                </asp:TextBox>

            </div>


            <!-- Dietary Requirements -->
            <div class="rx-book-form-group">

                <asp:Label
                    ID="DietaryRequirementsLabel"
                    runat="server"
                    AssociatedControlID="DietaryRequirementsTextBox"
                    Text="Catering / Dietary Requirements"
                    CssClass="rx-book-form-label">
                </asp:Label>

                <asp:TextBox
                    ID="DietaryRequirementsTextBox"
                    runat="server"
                    CssClass="rx-book-form-control"
                    MaxLength="500"
                    placeholder="e.g. Vegetarian, halal, allergies...">
                </asp:TextBox>

            </div>


            <!-- Special Requests -->
            <div class="rx-book-form-group full-width">

                <asp:Label
                    ID="SpecialRequestsLabel"
                    runat="server"
                    AssociatedControlID="SpecialRequestsTextBox"
                    Text="Special Requests"
                    CssClass="rx-book-form-label">
                </asp:Label>

                <asp:TextBox
                    ID="SpecialRequestsTextBox"
                    runat="server"
                    CssClass="rx-book-form-control"
                    TextMode="MultiLine"
                    Rows="5"
                    MaxLength="1500"
                    placeholder="Tell us anything else you would like us to know...">
                </asp:TextBox>

                <div class="rx-book-character-limit">
                    Maximum 1500 characters
                </div>

            </div>

        </div>


        <!-- Buttons -->
        <div class="rx-book-package-actions">

            <asp:Button
                ID="BackToInspirationButton"
                runat="server"
                Text="← Back to Inspiration"
                CssClass="rx-book-clear-button"
                CausesValidation="false"
                OnClick="BackToInspirationButton_Click" />

            <asp:Button
                ID="NextReviewButton"
                runat="server"
                Text="Next: Review &amp; Confirm →"
                CssClass="rx-book-next-button"
                CausesValidation="false"
                OnClick="NextReviewButton_Click" />

        </div>

    </div>

</asp:Panel>
        <!-- STEP 5: REVIEW & CONFIRM -->
<asp:Panel
    ID="Step5Panel"
    runat="server"
    CssClass="rx-book-review-panel"
    Visible="false">

    <div class="rx-book-event-card">

        <div class="rx-book-package-header">

            <div class="rx-book-section-icon">
                ✓
            </div>

            <div>
                <h2>Review &amp; Confirm</h2>
                <p>
                    Review your event details before confirming your booking.
                </p>
            </div>

        </div>

        <asp:Label
            ID="ReviewMessageLabel"
            runat="server"
            CssClass="rx-book-message">
        </asp:Label>


        <!-- EVENT DETAILS -->
        <div class="rx-book-review-section">

            <div class="rx-book-review-section-title">
                <span>📅</span>
                <h3>Event Details</h3>
            </div>

            <div class="rx-book-review-grid">

                <div class="rx-book-review-item">
                    <span>Event</span>
                    <strong>
                        <asp:Label
                            ID="ReviewEventNameLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div class="rx-book-review-item">
                    <span>Category</span>
                    <strong>
                        <asp:Label
                            ID="ReviewCategoryLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div class="rx-book-review-item">
                    <span>Date</span>
                    <strong>
                        <asp:Label
                            ID="ReviewDateLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div class="rx-book-review-item">
                    <span>Time</span>
                    <strong>
                        <asp:Label
                            ID="ReviewTimeLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div class="rx-book-review-item">
                    <span>Guests</span>
                    <strong>
                        <asp:Label
                            ID="ReviewGuestsLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

            </div>

        </div>


        <!-- VENUE -->
        <div class="rx-book-review-section">

            <div class="rx-book-review-section-title">
                <span>📍</span>
                <h3>Venue</h3>
            </div>

            <div class="rx-book-review-highlight">

                <strong>
                    <asp:Label
                        ID="ReviewVenueLabel"
                        runat="server">
                    </asp:Label>
                </strong>

                <span>
                    <asp:Label
                        ID="ReviewVenueAddressLabel"
                        runat="server">
                    </asp:Label>
                </span>

            </div>

        </div>


        <!-- PACKAGE -->
        <div class="rx-book-review-section">

            <div class="rx-book-review-section-title">
                <span>💎</span>
                <h3>Selected Package</h3>
            </div>

            <div class="rx-book-review-package">

                <asp:Image
                    ID="ReviewPackageImage"
                    runat="server"
                    CssClass="rx-book-review-package-image" />

                <div class="rx-book-review-package-content">

                    <span class="rx-book-review-package-type">
                        <asp:Label
                            ID="ReviewPackageTypeLabel"
                            runat="server">
                        </asp:Label>
                    </span>

                    <h3>
                        <asp:Label
                            ID="ReviewPackageNameLabel"
                            runat="server">
                        </asp:Label>
                    </h3>

                    <p>
                        <asp:Label
                            ID="ReviewPackageDescriptionLabel"
                            runat="server">
                        </asp:Label>
                    </p>

                    <strong class="rx-book-review-price">
                        UGX
                        <asp:Label
                            ID="ReviewPackagePriceLabel"
                            runat="server">
                        </asp:Label>
                    </strong>

                </div>

            </div>

        </div>


        <!-- INSPIRATION -->
        <div class="rx-book-review-section">

            <div class="rx-book-review-section-title">
                <span>📌</span>
                <h3>Inspiration</h3>
            </div>

            <div class="rx-book-review-highlight">

                <strong>
                    <asp:Label
                        ID="ReviewWishlistFolderLabel"
                        runat="server">
                    </asp:Label>
                </strong>

                <span>
                    <asp:Label
                        ID="ReviewWishlistStatusLabel"
                        runat="server">
                    </asp:Label>
                </span>

            </div>

        </div>


        <!-- CUSTOMIZATION -->
        <div class="rx-book-review-section">

            <div class="rx-book-review-section-title">
                <span>✨</span>
                <h3>Event Preferences</h3>
            </div>

            <div class="rx-book-review-preferences">

                <div>
                    <span>Preferred Theme</span>
                    <strong>
                        <asp:Label
                            ID="ReviewThemeLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div>
                    <span>Decoration Style</span>
                    <strong>
                        <asp:Label
                            ID="ReviewDecorationLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div>
                    <span>Entertainment</span>
                    <strong>
                        <asp:Label
                            ID="ReviewEntertainmentLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div>
                    <span>Catering / Dietary</span>
                    <strong>
                        <asp:Label
                            ID="ReviewDietaryLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

                <div class="full-width">
                    <span>Special Requests</span>
                    <strong>
                        <asp:Label
                            ID="ReviewSpecialRequestsLabel"
                            runat="server">
                        </asp:Label>
                    </strong>
                </div>

            </div>

        </div>


        <!-- TOTAL -->
        <div class="rx-book-review-total">

            <div>
                <span>Estimated Package Total</span>
                <small>
                    Final event costs may vary depending on additional requirements.
                </small>
            </div>

            <strong>
                UGX
                <asp:Label
                    ID="ReviewTotalLabel"
                    runat="server">
                </asp:Label>
            </strong>

        </div>


        <!-- ACTIONS -->
        <div class="rx-book-review-actions">

            <asp:Button
                ID="BackToCustomizeButton"
                runat="server"
                Text="← Back to Customize"
                CssClass="rx-book-clear-button"
                CausesValidation="false"
                OnClick="BackToCustomizeButton_Click" />

            <asp:Button
                ID="ConfirmBookingButton"
                runat="server"
                Text="✓ Confirm Booking"
                CssClass="rx-book-confirm-button"
                CausesValidation="false"
                OnClick="ConfirmBookingButton_Click" />

        </div>

    </div>

</asp:Panel>

</div>

</asp:Content>