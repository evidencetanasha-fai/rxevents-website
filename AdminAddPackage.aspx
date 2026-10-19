<%@ Page Title="Add Package" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminAddPackage.aspx.cs"
    Inherits="RxEvents.AdminAddPackage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/css/admin.css") %>"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="add-package-page">

        <div class="add-package-header">
            <div>
                <h1>
                    <asp:Literal ID="PageHeadingLiteral" runat="server"
                        Text="Add New Package" />
                </h1>
                <p>Create and manage event packages for your customers.</p>
            </div>

            <asp:HyperLink ID="BackToPackagesLink" runat="server"
                NavigateUrl="~/AdminPackages.aspx"
                CssClass="add-package-back">
                Back to Packages
            </asp:HyperLink>
        </div>

        <asp:Label ID="PackageMessage" runat="server"
            CssClass="add-package-message" EnableViewState="false" />

        <div class="add-package-card">

            <div class="add-package-section-title">
                <h2>Package Information</h2>
                <p>Enter the details of the event package.</p>
            </div>

            <div class="add-package-form-grid">

                <div class="add-package-field">
                    <asp:Label ID="PackageNameLabel" runat="server"
                        AssociatedControlID="PackageNameInput"
                        Text="Package Name *" />

                    <asp:TextBox ID="PackageNameInput" runat="server"
                        CssClass="add-package-input"
                        MaxLength="100"
                        placeholder="e.g. Platinum Package" />

                    <asp:RequiredFieldValidator ID="PackageNameValidator"
                        runat="server"
                        ControlToValidate="PackageNameInput"
                        ErrorMessage="Package name is required."
                        CssClass="add-package-error"
                        Display="Dynamic" />
                </div>

                <div class="add-package-field">
                    <asp:Label ID="PackageTypeLabel" runat="server"
                        AssociatedControlID="PackageTypeDropdown"
                        Text="Package Type *" />

                    <asp:DropDownList ID="PackageTypeDropdown" runat="server"
                        CssClass="add-package-input">
                        <asp:ListItem Text="Select package type"
                            Value="" />
                        <asp:ListItem Text="Premium++" Value="Premium++" />
                        <asp:ListItem Text="Premium" Value="Premium" />
                        <asp:ListItem Text="Standard" Value="Standard" />
                        <asp:ListItem Text="Basic" Value="Basic" />
                        <asp:ListItem Text="Custom" Value="Custom" />
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="PackageTypeValidator"
                        runat="server"
                        ControlToValidate="PackageTypeDropdown"
                        InitialValue=""
                        ErrorMessage="Please select a package type."
                        CssClass="add-package-error"
                        Display="Dynamic" />
                </div>

                <div class="add-package-field add-package-full">
                    <asp:Label ID="PackageDescriptionLabel" runat="server"
                        AssociatedControlID="PackageDescriptionInput"
                        Text="Description" />

                    <asp:TextBox ID="PackageDescriptionInput" runat="server"
                        CssClass="add-package-input"
                        TextMode="MultiLine"
                        Rows="5"
                        MaxLength="500"
                        placeholder="Describe what this package offers..." />
                </div>

                <div class="add-package-field">
                    <asp:Label ID="PackagePriceLabel" runat="server"
                        AssociatedControlID="PackagePriceInput"
                        Text="Package Price *" />

                    <asp:TextBox ID="PackagePriceInput" runat="server"
                        CssClass="add-package-input"
                        TextMode="Number"
                        placeholder="Enter package price" />

                    <asp:RequiredFieldValidator ID="PackagePriceRequiredValidator"
                        runat="server"
                        ControlToValidate="PackagePriceInput"
                        ErrorMessage="Package price is required."
                        CssClass="add-package-error"
                        Display="Dynamic" />

                    <asp:RangeValidator ID="PackagePriceRangeValidator"
                        runat="server"
                        ControlToValidate="PackagePriceInput"
                        Type="Double"
                        MinimumValue="0"
                        MaximumValue="999999999"
                        ErrorMessage="Enter a valid non-negative price."
                        CssClass="add-package-error"
                        Display="Dynamic" />
                </div>

                <div class="add-package-field">
                    <asp:Label ID="PackageStatusLabel" runat="server"
                        AssociatedControlID="PackageStatusDropdown"
                        Text="Package Status" />

                    <asp:DropDownList ID="PackageStatusDropdown" runat="server"
                        CssClass="add-package-input">
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>

                <div class="add-package-field add-package-full">
                    <asp:Label ID="PackageImageLabel" runat="server"
                        AssociatedControlID="PackageImageInput"
                        Text="Package Image" />

                    <asp:FileUpload ID="PackageImageInput" runat="server"
                        CssClass="add-package-input" />

                    <p class="add-package-help">
                        Choose an image for this package. If you edit a package
                        without uploading a new image, the existing image will be kept.
                    </p>
                </div>

            </div>

            <div class="add-package-actions">
                <asp:HyperLink ID="CancelPackageLink" runat="server"
                    NavigateUrl="~/AdminPackages.aspx"
                    CssClass="add-package-cancel">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="SavePackageButton" runat="server"
                    Text="Save Package"
                    CssClass="add-package-save"
                    OnClick="SavePackageButton_Click" />
            </div>

        </div>

    </div>

</asp:Content>