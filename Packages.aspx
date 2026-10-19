<%@ Page Title="Packages" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Packages.aspx.cs"
    Inherits="RxEvents.Packages" %>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="rx-packages-page">

        <div class="rx-packages-heading">

            <div class="rx-packages-label">
                <span></span>
                CHOOSE YOUR EXPERIENCE
                <span></span>
            </div>

            <h1>Our Packages</h1>

            <p>
                Choose the perfect package for your special occasion.
                Every package is designed to make your event memorable.
            </p>

        </div>

        <div class="rx-packages-grid">

            <asp:Repeater ID="PackagesRepeater" runat="server">

                <ItemTemplate>

                    <article class="rx-package-card">

                        <!-- Package Image -->
                        <div class="rx-package-image">
                            <img src='<%# ResolveUrl("~/Content/images/" + Eval("ImagePath")) %>'
                                 alt='<%# Eval("PackageName") %>' />
                        </div>

                        <!-- Package Type -->
                        <span class="rx-package-type">
                            <%# Eval("PackageType") %>
                        </span>

                        <!-- Package Name -->
                        <h2>
                            <%# Eval("PackageName") %>
                        </h2>

                        <!-- Price -->
                        <div class="rx-package-price">
                            <span>₹</span><%# String.Format("{0:N0}", Eval("Price")) %>
                        </div>

                        <!-- Description -->
                        <p class="rx-package-description">
                            <%# Eval("Description") %>
                        </p>

                        <div class="rx-package-divider"></div>

                        <!-- Included Services -->
                        <h3>Included Services</h3>

                        <ul class="rx-package-services">

                            <asp:Repeater
                                ID="PackageServicesRepeater"
                                runat="server"
                                DataSource='<%# Eval("IncludedServices") %>'>

                                <ItemTemplate>

                                    <li>
                                        <span class="rx-check">✓</span>

                                        <span>
                                            <%# Eval("ServiceName") %>
                                        </span>
                                    </li>

                                </ItemTemplate>

                            </asp:Repeater>

                        </ul>

                        <!-- Choose Package -->
                        <a href="#"
                           class="rx-package-button">
                            Choose Package
                            <span>→</span>
                        </a>

                    </article>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </section>

</asp:Content>