using System;
using System.Collections.Generic;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Packages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPackages();
            }
        }

        private void LoadPackages()
        {
            try
            {
                PackageRepository repository = new PackageRepository();

                List<Package> packages = repository.GetActivePackages();

                foreach (Package package in packages)
                {
                    package.IncludedServices =
                        repository.GetPackageServices(package.PackageID);
                }

                PackagesRepeater.DataSource = packages;
                PackagesRepeater.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<p style='color:red;'>Unable to load packages: "
                    + ex.Message +
                    "</p>"
                );
            }
        }
    }
}