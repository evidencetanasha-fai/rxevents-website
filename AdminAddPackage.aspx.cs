using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using RxEvents.Data;

namespace RxEvents
{
    public partial class AdminAddPackage : Page
    {
        private int PackageID
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["PackageID"], out id)
                    ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only logged-in administrators can access this page.
            int roleID;

            if (Session["UserID"] == null ||
                Session["RoleID"] == null ||
                !int.TryParse(Convert.ToString(Session["RoleID"]), out roleID) ||
                roleID != 1)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (PackageID > 0)
                {
                    PageHeadingLiteral.Text = "Edit Package";
                    SavePackageButton.Text = "Update Package";
                    LoadPackageDetails();
                }
                else
                {
                    PackageStatusDropdown.SelectedValue = "1";
                }
            }
        }

        private void LoadPackageDetails()
        {
            try
            {
                using (SqlConnection connection = new DatabaseHelper().GetConnection())
                {
                    const string query = @"
                        SELECT PackageName, PackageType, Description,
                               Price, ImagePath, IsActive
                        FROM Packages
                        WHERE PackageID = @PackageID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@PackageID", SqlDbType.Int)
                            .Value = PackageID;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                ShowMessage("Package not found.", false);
                                SavePackageButton.Enabled = false;
                                return;
                            }

                            PackageNameInput.Text =
                                Convert.ToString(reader["PackageName"]);

                            string packageType =
                                Convert.ToString(reader["PackageType"]);

                            // Support package types already stored in the database.
                            if (!string.IsNullOrWhiteSpace(packageType) &&
                                PackageTypeDropdown.Items.FindByValue(packageType) == null)
                            {
                                PackageTypeDropdown.Items.Add(
                                    new System.Web.UI.WebControls.ListItem(
                                        packageType, packageType));
                            }

                            if (!string.IsNullOrWhiteSpace(packageType))
                            {
                                PackageTypeDropdown.SelectedValue = packageType;
                            }

                            PackageDescriptionInput.Text =
                                reader["Description"] == DBNull.Value
                                    ? ""
                                    : Convert.ToString(reader["Description"]);

                            PackagePriceInput.Text =
                                Convert.ToDecimal(reader["Price"])
                                    .ToString("0.00", CultureInfo.InvariantCulture);

                            PackageStatusDropdown.SelectedValue =
                                Convert.ToBoolean(reader["IsActive"]) ? "1" : "0";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ShowMessage(
                    "Unable to load package details. Please check the database connection.",
                    false);
            }
        }

        protected void SavePackageButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string packageName = PackageNameInput.Text.Trim();
            string packageType = PackageTypeDropdown.SelectedValue;
            string description = PackageDescriptionInput.Text.Trim();
            bool isActive = PackageStatusDropdown.SelectedValue == "1";

            decimal price;

            if (!decimal.TryParse(
                    PackagePriceInput.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out price) ||
                price < 0 ||
                price > 9999999999.99m)
            {
                ShowMessage("Please enter a valid package price.", false);
                return;
            }

            if (description.Length > 500)
            {
                ShowMessage(
                    "The description cannot exceed 500 characters.",
                    false);
                return;
            }

            try
            {
                using (SqlConnection connection = new DatabaseHelper().GetConnection())
                {
                    connection.Open();

                    // Prevent duplicate package names.
                    const string duplicateQuery = @"
                        SELECT COUNT(*)
                        FROM Packages
                        WHERE PackageName = @PackageName
                          AND PackageID <> @PackageID";

                    using (SqlCommand duplicateCommand =
                        new SqlCommand(duplicateQuery, connection))
                    {
                        duplicateCommand.Parameters.Add(
                            "@PackageName", SqlDbType.VarChar, 100)
                            .Value = packageName;

                        duplicateCommand.Parameters.Add(
                            "@PackageID", SqlDbType.Int)
                            .Value = PackageID;

                        int duplicateCount =
                            Convert.ToInt32(duplicateCommand.ExecuteScalar());

                        if (duplicateCount > 0)
                        {
                            ShowMessage(
                                "A package with this name already exists.",
                                false);
                            return;
                        }
                    }

                    // Keep the current image unless a new one is uploaded.
                    string imagePath = GetExistingImagePath(connection);

                    if (PackageImageInput.HasFile)
                    {
                        string extension = Path.GetExtension(
                            PackageImageInput.FileName).ToLowerInvariant();

                        string[] allowedExtensions =
                        {
                            ".jpg", ".jpeg", ".png", ".webp"
                        };

                        if (Array.IndexOf(allowedExtensions, extension) < 0)
                        {
                            ShowMessage(
                                "Please upload a JPG, JPEG, PNG, or WEBP image.",
                                false);
                            return;
                        }

                        const int maxFileSize = 5 * 1024 * 1024;

                        if (PackageImageInput.PostedFile.ContentLength >
                            maxFileSize)
                        {
                            ShowMessage(
                                "The package image must be 5 MB or smaller.",
                                false);
                            return;
                        }

                        string imageFolder =
                            Server.MapPath("~/Content/images/");

                        if (!Directory.Exists(imageFolder))
                        {
                            Directory.CreateDirectory(imageFolder);
                        }

                        string fileName =
                            Guid.NewGuid().ToString("N") + extension;

                        string fullPath =
                            Path.Combine(imageFolder, fileName);

                        PackageImageInput.SaveAs(fullPath);

                        imagePath = "Content/images/" + fileName;
                    }

                    if (PackageID > 0)
                    {
                        const string updateQuery = @"
                            UPDATE Packages
                            SET PackageName = @PackageName,
                                PackageType = @PackageType,
                                Description = @Description,
                                Price = @Price,
                                ImagePath = @ImagePath,
                                IsActive = @IsActive
                            WHERE PackageID = @PackageID";

                        using (SqlCommand command =
                            new SqlCommand(updateQuery, connection))
                        {
                            AddPackageParameters(
                                command, packageName, packageType,
                                description, price, imagePath, isActive);

                            command.Parameters.Add(
                                "@PackageID", SqlDbType.Int).Value = PackageID;

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                ShowMessage("Package not found.", false);
                                return;
                            }
                        }

                        Response.Redirect("~/AdminPackages.aspx");
                    }
                    else
                    {
                        const string insertQuery = @"
                            INSERT INTO Packages
                                (PackageName, PackageType, Description,
                                 Price, ImagePath, IsActive)
                            VALUES
                                (@PackageName, @PackageType, @Description,
                                 @Price, @ImagePath, @IsActive)";

                        using (SqlCommand command =
                            new SqlCommand(insertQuery, connection))
                        {
                            AddPackageParameters(
                                command, packageName, packageType,
                                description, price, imagePath, isActive);

                            command.ExecuteNonQuery();
                        }

                        Response.Redirect("~/AdminPackages.aspx");
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    ShowMessage(
                        "A package with this name already exists.",
                        false);
                }
                else
                {
                    ShowMessage(
                        "Unable to save the package. Please check your database and try again.",
                        false);
                }
            }
            catch (Exception)
            {
                ShowMessage(
                    "An error occurred while saving the package. Please try again.",
                    false);
            }
        }

        private string GetExistingImagePath(SqlConnection connection)
        {
            if (PackageID <= 0)
            {
                return null;
            }

            const string query =
                "SELECT ImagePath FROM Packages WHERE PackageID = @PackageID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PackageID", SqlDbType.Int)
                    .Value = PackageID;

                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToString(result);
            }
        }

        private void AddPackageParameters(
            SqlCommand command,
            string packageName,
            string packageType,
            string description,
            decimal price,
            string imagePath,
            bool isActive)
        {
            command.Parameters.Add(
                "@PackageName", SqlDbType.VarChar, 100)
                .Value = packageName;

            command.Parameters.Add(
                "@PackageType", SqlDbType.VarChar, 50)
                .Value = string.IsNullOrWhiteSpace(packageType)
                    ? (object)DBNull.Value
                    : packageType;

            command.Parameters.Add(
                "@Description", SqlDbType.VarChar, 500)
                .Value = string.IsNullOrWhiteSpace(description)
                    ? (object)DBNull.Value
                    : description;

            SqlParameter priceParameter =
                command.Parameters.Add("@Price", SqlDbType.Decimal);

            priceParameter.Precision = 12;
            priceParameter.Scale = 2;
            priceParameter.Value = price;

            command.Parameters.Add(
                "@ImagePath", SqlDbType.VarChar, 255)
                .Value = string.IsNullOrWhiteSpace(imagePath)
                    ? (object)DBNull.Value
                    : imagePath;

            command.Parameters.Add(
                "@IsActive", SqlDbType.Bit)
                .Value = isActive;
        }

        private void ShowMessage(string message, bool success)
        {
            PackageMessage.Text = Server.HtmlEncode(message);
            PackageMessage.ForeColor = success
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.Firebrick;
        }
    }
}