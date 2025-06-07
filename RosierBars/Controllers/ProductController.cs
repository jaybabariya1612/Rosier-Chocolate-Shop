using CsvHelper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RosierBars.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace RosierBars.Controllers
{
    public class ProductController : Controller
    {

        // GET: Product
        public ActionResult Product()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Product(ProductModel model, IEnumerable<HttpPostedFileBase> ImageFiles)
        {
            // Debugging: Log all model state errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Debug.WriteLine($"ModelState Error: {error.ErrorMessage}");
            }

            // First check if any files were uploaded (if this is required)
            if (ImageFiles == null || !ImageFiles.Any(f => f != null && f.ContentLength > 0))
            {
                ModelState.AddModelError("ImageFiles", "Please upload at least one product image");
            }

            // Check if model state is valid after all validations
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                List<string> savedFileNames = new List<string>();

                // Process uploaded files
                foreach (var file in ImageFiles.Take(5))
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        try
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            string path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                            file.SaveAs(path);
                            savedFileNames.Add(fileName);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("ImageFiles", "Error saving image: " + ex.Message);
                            return View(model);
                        }
                    }
                }

                model.ImageUrl = string.Join(",", savedFileNames);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Check for existing product
                        string productCheckQuery = "SELECT COUNT(*) FROM Products WHERE ProductName = @ProductName";
                        using (SqlCommand checkCmd = new SqlCommand(productCheckQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                            int count = (int)checkCmd.ExecuteScalar();

                            if (count > 0)
                            {
                                ViewBag.ProductMessage = "That Product is already Added.";
                                return View(model);
                            }
                        }

                        // Insert new product
                        using (SqlCommand cmd = new SqlCommand("InsertProducts", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Add parameters with null checks
                            cmd.Parameters.AddWithValue("@ProductName", model.ProductName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Brand", model.Brand ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ModelName", model.ModelName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Price", model.Price);
                            cmd.Parameters.AddWithValue("@OriginalPrice", model.OriginalPrice);
                            cmd.Parameters.AddWithValue("@DiscountPercent", model.DiscountPercent);
                            cmd.Parameters.AddWithValue("@Available", model.Available ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Stock", model.Stock);
                            cmd.Parameters.AddWithValue("@Flavor", model.Flavor ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Type", model.Type ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@FoodPreference", model.FoodPreference ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@PackOf", model.PackOf);
                            cmd.Parameters.AddWithValue("@NetQuantity", model.NetQuantity ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ShelfLife", model.ShelfLife ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@IsGourmet", model.IsGourmet ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@IsHomemade", model.IsHomemade ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@IsGiftPack", model.IsGiftPack ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Seller", model.Seller ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@NoReturnsAllowed", model.NoReturnsAllowed ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@GstInvoiceAvailable", model.GstInvoiceAvailable ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ManufacturedBy", model.ManufacturedBy ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ingredients", model.Ingredients ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@NutritionInfo", model.NutritionInfo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@WidthCm", model.WidthCm);
                            cmd.Parameters.AddWithValue("@HeightCm", model.HeightCm);
                            cmd.Parameters.AddWithValue("@DepthCm", model.DepthCm);
                            cmd.Parameters.AddWithValue("@WeightG", model.WeightG);
                            cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }

                        TempData["Success"] = "Product Added Successfully!";
                        return RedirectToAction("Product", "Product");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while saving the product: " + ex.Message);
                        return View(model);
                    }
                }
            }

            // If we got this far, something failed - redisplay form with errors
            // Add this to ensure validation messages are visible
            ViewBag.ValidationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return View(model);
        }

        public ActionResult ProductList()
        {
            var ProductList = new List<ProductModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.ProductId,b.Brand,p.ModelName,p.ProductName,p.Description,p.Price,p.OriginalPrice,p.DiscountPercent,p.Rating,p.TotalRatings,p.TotalReviews,p.Available,p.Stock,p.Flavor,pt.Type,fp.FoodPreference,p.PackOf,p.NetQuantity,p.ShelfLife,p.IsGourmet,p.IsHomemade,p.IsGiftPack,s.Seller,p.NoReturnsAllowed,p.GstInvoiceAvailable,m.ManufacturedBy,p.Ingredients,p.NutritionInfo,p.WidthCm,p.HeightCm,p.DepthCm,p.WeightG,p.ImageUrl,p.CreatedAt,p.UpdatedAt FROM Products p\r\nJOIN Brands b ON p.BrandId = b.BrandId JOIN ProductTypes pt ON p.TypeId = pt.TypeId JOIN FoodPreferences fp ON p.FoodPreferenceId = fp.FoodPreferenceId JOIN Sellers s ON p.SellerId = s.SellerId JOIN Manufacturers m ON p.ManufacturerId = m.ManufacturerId;";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductList.Add(new ProductModel
                    {
                        ProductId = reader["ProductId"] != DBNull.Value ? Convert.ToInt32(reader["ProductId"]) : 0,
                        Brand = reader["Brand"]?.ToString(),
                        ModelName = reader["ModelName"]?.ToString(),
                        ProductName = reader["ProductName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        Price = reader["Price"] != DBNull.Value ? Convert.ToInt32(reader["Price"]) : 0,
                        OriginalPrice = reader["OriginalPrice"] != DBNull.Value ? Convert.ToInt32(reader["OriginalPrice"]) : 0,
                        DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToInt32(reader["DiscountPercent"]) : 0,
                        Rating = reader["Rating"] != DBNull.Value ? Convert.ToInt32(reader["Rating"]) : 0,
                        TotalRatings = reader["TotalRatings"] != DBNull.Value ? Convert.ToInt32(reader["TotalRatings"]) : 0,
                        TotalReviews = reader["TotalReviews"] != DBNull.Value ? Convert.ToInt32(reader["TotalReviews"]) : 0,
                        Available = reader["Available"]?.ToString(),
                        Stock = reader["Stock"] != DBNull.Value ? Convert.ToInt32(reader["Stock"]) : 0,
                        Flavor = reader["Flavor"]?.ToString(),
                        Type = reader["Type"]?.ToString(),
                        FoodPreference = reader["FoodPreference"]?.ToString(),
                        PackOf = reader["PackOf"] != DBNull.Value ? Convert.ToInt32(reader["PackOf"]) : 0,
                        NetQuantity = reader["NetQuantity"]?.ToString(),
                        ShelfLife = reader["ShelfLife"]?.ToString(),
                        IsGourmet = reader["IsGourmet"]?.ToString(),
                        IsHomemade = reader["IsHomemade"]?.ToString(),
                        IsGiftPack = reader["IsGiftPack"]?.ToString(),
                        Seller = reader["Seller"]?.ToString(),
                        NoReturnsAllowed = reader["NoReturnsAllowed"]?.ToString(),
                        GstInvoiceAvailable = reader["GstInvoiceAvailable"]?.ToString(),
                        ManufacturedBy = reader["ManufacturedBy"]?.ToString(),
                        Ingredients = reader["Ingredients"]?.ToString(),
                        NutritionInfo = reader["NutritionInfo"]?.ToString(),
                        WidthCm = reader["WidthCm"] != DBNull.Value ? Convert.ToDecimal(reader["WidthCm"]) : 0,
                        HeightCm = reader["HeightCm"] != DBNull.Value ? Convert.ToDecimal(reader["HeightCm"]) : 0,
                        DepthCm = reader["DepthCm"] != DBNull.Value ? Convert.ToDecimal(reader["DepthCm"]) : 0,
                        WeightG = reader["WeightG"] != DBNull.Value ? Convert.ToDecimal(reader["WeightG"]) : 0,
                        ImageUrl = reader["ImageUrl"]?.ToString(),
                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue,
                        UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : DateTime.MinValue

                    });
                }
                reader.Close();
                return View(ProductList);
            }
        }

        public ActionResult ProductEdit(int id)
        {
            ProductModel model = new ProductModel();
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.ProductId, b.Brand, p.ModelName, p.ProductName, p.Description, p.Price, p.OriginalPrice, p.DiscountPercent, p.Rating, p.TotalRatings, p.TotalReviews, p.Available, p.Stock, p.Flavor, pt.Type, fp.FoodPreference, p.PackOf, p.NetQuantity, p.ShelfLife, p.IsGourmet, p.IsHomemade, p.IsGiftPack, s.Seller, p.NoReturnsAllowed, p.GstInvoiceAvailable, m.ManufacturedBy, p.Ingredients, p.NutritionInfo, p.WidthCm, p.HeightCm, p.DepthCm, p.WeightG, p.ImageUrl, p.CreatedAt, p.UpdatedAt FROM Products p JOIN Brands b ON p.BrandId = b.BrandId JOIN ProductTypes pt ON p.TypeId = pt.TypeId JOIN FoodPreferences fp ON p.FoodPreferenceId = fp.FoodPreferenceId JOIN Sellers s ON p.SellerId = s.SellerId JOIN Manufacturers m ON p.ManufacturerId = m.ManufacturerId WHERE ProductID = @ProductId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    model.ProductId = reader["ProductId"] != DBNull.Value ? Convert.ToInt32(reader["ProductId"]) : 0;
                    model.Brand = reader["Brand"]?.ToString();
                    model.ModelName = reader["ModelName"]?.ToString();
                    model.ProductName = reader["ProductName"]?.ToString();
                    model.Description = reader["Description"]?.ToString();
                    model.Price = reader["Price"] != DBNull.Value ? Convert.ToInt32(reader["Price"]) : 0;
                    model.OriginalPrice = reader["OriginalPrice"] != DBNull.Value ? Convert.ToInt32(reader["OriginalPrice"]) : 0;
                    model.DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToInt32(reader["DiscountPercent"]) : 0;
                    model.Rating = reader["Rating"] != DBNull.Value ? Convert.ToInt32(reader["Rating"]) : 0;
                    model.TotalRatings = reader["TotalRatings"] != DBNull.Value ? Convert.ToInt32(reader["TotalRatings"]) : 0;
                    model.TotalReviews = reader["TotalReviews"] != DBNull.Value ? Convert.ToInt32(reader["TotalReviews"]) : 0;
                    model.Available = reader["Available"]?.ToString();
                    model.Stock = reader["Stock"] != DBNull.Value ? Convert.ToInt32(reader["Stock"]) : 0;
                    model.Flavor = reader["Flavor"]?.ToString();
                    model.Type = reader["Type"]?.ToString();
                    model.FoodPreference = reader["FoodPreference"]?.ToString();
                    model.PackOf = reader["PackOf"] != DBNull.Value ? Convert.ToInt32(reader["PackOf"]) : 0;
                    model.NetQuantity = reader["NetQuantity"]?.ToString();
                    model.ShelfLife = reader["ShelfLife"]?.ToString();
                    model.IsGourmet = reader["IsGourmet"]?.ToString();
                    model.IsHomemade = reader["IsHomemade"]?.ToString();
                    model.IsGiftPack = reader["IsGiftPack"]?.ToString();
                    model.Seller = reader["Seller"]?.ToString();
                    model.NoReturnsAllowed = reader["NoReturnsAllowed"]?.ToString();
                    model.GstInvoiceAvailable = reader["GstInvoiceAvailable"]?.ToString();
                    model.ManufacturedBy = reader["ManufacturedBy"]?.ToString();
                    model.Ingredients = reader["Ingredients"]?.ToString();
                    model.NutritionInfo = reader["NutritionInfo"]?.ToString();
                    model.WidthCm = reader["WidthCm"] != DBNull.Value ? Convert.ToDecimal(reader["WidthCm"]) : 0;
                    model.HeightCm = reader["HeightCm"] != DBNull.Value ? Convert.ToDecimal(reader["HeightCm"]) : 0;
                    model.DepthCm = reader["DepthCm"] != DBNull.Value ? Convert.ToDecimal(reader["DepthCm"]) : 0;
                    model.WeightG = reader["WeightG"] != DBNull.Value ? Convert.ToDecimal(reader["WeightG"]) : 0;
                    model.ImageUrl = reader["ImageUrl"]?.ToString();
                    model.UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : DateTime.MinValue;
                }
                reader.Close();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductEdit(ProductModel model, IEnumerable<HttpPostedFileBase> ImageFiles)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            List<string> newImageNames = new List<string>();

            try
            {
                // Handle image uploads
                if (ImageFiles != null && ImageFiles.Any(f => f != null))
                {
                    foreach (var file in ImageFiles.Take(5 - (model.ImageUrl?.Split(',').Count() ?? 0)))
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Validate file type and size (max 5MB)
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                            var extension = Path.GetExtension(file.FileName).ToLower();

                            if (!allowedExtensions.Contains(extension))
                            {
                                ModelState.AddModelError("", $"Invalid file type: {file.FileName}. Only JPG, PNG, GIF are allowed.");
                                continue;
                            }

                            if (file.ContentLength > 5 * 1024 * 1024) // 5MB
                            {
                                ModelState.AddModelError("", $"File too large: {file.FileName}. Max 5MB allowed.");
                                continue;
                            }

                            // Generate unique filename
                            string fileName = $"{Guid.NewGuid()}{extension}";
                            string path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);

                            try
                            {
                                file.SaveAs(path);
                                newImageNames.Add(fileName);
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", $"Error saving {file.FileName}: {ex.Message}");
                                continue;
                            }
                        }
                    }
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Get existing images from DB
                    List<string> oldImages = new List<string>();
                    using (SqlCommand getImgCmd = new SqlCommand("SELECT ImageUrl FROM Products WHERE ProductId = @ProductId", conn))
                    {
                        getImgCmd.Parameters.AddWithValue("@ProductId", model.ProductId);
                        using (SqlDataReader reader = getImgCmd.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(0))
                            {
                                string imageData = reader.GetString(0);
                                oldImages = imageData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            }
                        }
                    }

                    // Current images retained from UI
                    List<string> updatedImages = (model.ImageUrl ?? "")
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    // Add new uploaded images
                    updatedImages.AddRange(newImageNames);

                    // Remove duplicates and empty entries
                    model.ImageUrl = string.Join(",", updatedImages.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());

                    // Delete removed images from disk
                    List<string> removedImages = oldImages.Except(updatedImages).ToList();
                    foreach (var img in removedImages)
                    {
                        try
                        {
                            string fullPath = Path.Combine(Server.MapPath("~/Content/Images/"), img);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log error but continue
                            System.Diagnostics.Debug.WriteLine($"Error deleting image {img}: {ex.Message}");
                        }
                    }

                    // Update product in database
                    using (SqlCommand cmd = new SqlCommand("UpdateProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Helper method to handle nulls
                        object GetValue(object value)
                        {
                            return value ?? DBNull.Value;
                        }

                        // Strings
                        cmd.Parameters.AddWithValue("@ProductName", GetValue(model.ProductName));
                        cmd.Parameters.AddWithValue("@Brand", GetValue(model.Brand));
                        cmd.Parameters.AddWithValue("@ModelName", GetValue(model.ModelName));
                        cmd.Parameters.AddWithValue("@Description", GetValue(model.Description));
                        cmd.Parameters.AddWithValue("@Flavor", GetValue(model.Flavor));
                        cmd.Parameters.AddWithValue("@Type", GetValue(model.Type));
                        cmd.Parameters.AddWithValue("@FoodPreference", GetValue(model.FoodPreference));
                        cmd.Parameters.AddWithValue("@Seller", GetValue(model.Seller));
                        cmd.Parameters.AddWithValue("@ManufacturedBy", GetValue(model.ManufacturedBy));
                        cmd.Parameters.AddWithValue("@Ingredients", GetValue(model.Ingredients));
                        cmd.Parameters.AddWithValue("@NutritionInfo", GetValue(model.NutritionInfo));
                        cmd.Parameters.AddWithValue("@ImageUrl", GetValue(model.ImageUrl));

                        // Value types
                        cmd.Parameters.AddWithValue("@ProductId", model.ProductId);
                        cmd.Parameters.AddWithValue("@Price", (object)model.Price ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@OriginalPrice", (object)model.OriginalPrice ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DiscountPercent", (object)model.DiscountPercent ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Available", (object)model.Available ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Stock", (object)model.Stock ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PackOf", (object)model.PackOf ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NetQuantity", (object)model.NetQuantity ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ShelfLife", (object)model.ShelfLife ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@WidthCm", (object)model.WidthCm ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HeightCm", (object)model.HeightCm ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DepthCm", (object)model.DepthCm ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@WeightG", (object)model.WeightG ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Rating", (object)model.Rating ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotalRatings", (object)model.TotalRatings ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotalReviews", (object)model.TotalReviews ?? DBNull.Value);

                        // Boolean flags
                        cmd.Parameters.AddWithValue("@IsGourmet", model.IsGourmet);
                        cmd.Parameters.AddWithValue("@IsHomemade", model.IsHomemade);
                        cmd.Parameters.AddWithValue("@IsGiftPack", model.IsGiftPack);
                        cmd.Parameters.AddWithValue("@NoReturnsAllowed", model.NoReturnsAllowed);
                        cmd.Parameters.AddWithValue("@GstInvoiceAvailable", model.GstInvoiceAvailable);

                        // DateTime
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        //cmd.Parameters.AddWithValue("@CreatedAt", model.CreatedAt == DateTime.MinValue ? (object)DBNull.Value : model.CreatedAt);

                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Success"] = "Product Updated Successfully!";
                return RedirectToAction("ProductList", "Product");
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error updating product: {ex}");

                // Add error to model state
                ModelState.AddModelError("", "An error occurred while updating the product. Please try again.");

                // Return to view with model to show errors
                return View(model);
            }
        }


        public ActionResult ProductDetails(int id)
        {
            ProductModel model = new ProductModel();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.ProductId,b.Brand,p.ModelName,p.ProductName,p.Description,p.Price,p.OriginalPrice,p.DiscountPercent,p.Rating,p.TotalRatings,p.TotalReviews,p.Available,p.Stock,p.Flavor,pt.Type,fp.FoodPreference,p.PackOf,p.NetQuantity,p.ShelfLife,p.IsGourmet,p.IsHomemade,p.IsGiftPack,s.Seller,p.NoReturnsAllowed,p.GstInvoiceAvailable,m.ManufacturedBy,p.Ingredients,p.NutritionInfo,p.WidthCm,p.HeightCm,p.DepthCm,p.WeightG,p.ImageUrl,p.CreatedAt,p.UpdatedAt FROM Products p JOIN Brands b ON p.BrandId = b.BrandId JOIN ProductTypes pt ON p.TypeId = pt.TypeId JOIN FoodPreferences fp ON p.FoodPreferenceId = fp.FoodPreferenceId JOIN Sellers s ON p.SellerId = s.SellerId JOIN Manufacturers m ON p.ManufacturerId = m.ManufacturerId where ProductID=" + id;

                SqlCommand command = new SqlCommand(query, connection); 
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    model.ProductId = reader["ProductId"] != DBNull.Value ? Convert.ToInt32(reader["ProductId"]) : 0;
                    model.Brand = reader["Brand"]?.ToString();
                    model.ModelName = reader["ModelName"]?.ToString();
                    model.ProductName = reader["ProductName"]?.ToString();
                    model.Description = reader["Description"]?.ToString();
                    model.Price = reader["Price"] != DBNull.Value ? Convert.ToInt32(reader["Price"]) : 0;
                    model.OriginalPrice = reader["OriginalPrice"] != DBNull.Value ? Convert.ToInt32(reader["OriginalPrice"]) : 0;
                    model.DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToInt32(reader["DiscountPercent"]) : 0;
                    model.Rating = reader["Rating"] != DBNull.Value ? Convert.ToInt32(reader["Rating"]) : 0;
                    model.TotalRatings = reader["TotalRatings"] != DBNull.Value ? Convert.ToInt32(reader["TotalRatings"]) : 0;
                    model.TotalReviews = reader["TotalReviews"] != DBNull.Value ? Convert.ToInt32(reader["TotalReviews"]) : 0;
                    model.Available = reader["Available"]?.ToString();
                    model.Stock = reader["Stock"] != DBNull.Value ? Convert.ToInt32(reader["Stock"]) : 0;
                    model.Flavor = reader["Flavor"]?.ToString();
                    model.Type = reader["Type"]?.ToString();
                    model.FoodPreference = reader["FoodPreference"]?.ToString();
                    model.PackOf = reader["PackOf"] != DBNull.Value ? Convert.ToInt32(reader["PackOf"]) : 0;
                    model.NetQuantity = reader["NetQuantity"]?.ToString();
                    model.ShelfLife = reader["ShelfLife"]?.ToString();
                    model.IsGourmet = reader["IsGourmet"]?.ToString();
                    model.IsHomemade = reader["IsHomemade"]?.ToString();
                    model.IsGiftPack = reader["IsGiftPack"]?.ToString();
                    model.Seller = reader["Seller"]?.ToString();
                    model.NoReturnsAllowed = reader["NoReturnsAllowed"]?.ToString();
                    model.GstInvoiceAvailable = reader["GstInvoiceAvailable"]?.ToString();
                    model.ManufacturedBy = reader["ManufacturedBy"]?.ToString();
                    model.Ingredients = reader["Ingredients"]?.ToString();
                    model.NutritionInfo = reader["NutritionInfo"]?.ToString();
                    model.WidthCm = reader["WidthCm"] != DBNull.Value ? Convert.ToDecimal(reader["WidthCm"]) : 0;
                    model.HeightCm = reader["HeightCm"] != DBNull.Value ? Convert.ToDecimal(reader["HeightCm"]) : 0;
                    model.DepthCm = reader["DepthCm"] != DBNull.Value ? Convert.ToDecimal(reader["DepthCm"]) : 0;
                    model.WeightG = reader["WeightG"] != DBNull.Value ? Convert.ToDecimal(reader["WeightG"]) : 0;
                    model.ImageUrl = reader["ImageUrl"]?.ToString();
                    model.UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : DateTime.MinValue;
                    model.CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue;

                }
                reader.Close();
            }
            return View(model);
        }
        public ActionResult ProductDelete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Transaction to ensure all deletions happen together
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Delete the product from the Products table
                        string deleteProductQuery = "DELETE FROM Products WHERE ProductId = @ProductId";
                        SqlCommand deleteProductCommand = new SqlCommand(deleteProductQuery, connection, transaction);
                        deleteProductCommand.Parameters.AddWithValue("@ProductId", id);
                        deleteProductCommand.ExecuteNonQuery();

                        // Delete related entries from other tables (Brands, ProductTypes, FoodPreferences, Sellers, Manufacturers)
                        string deleteBrandsQuery = "DELETE FROM Brands WHERE BrandId NOT IN (SELECT BrandId FROM Products)";
                        SqlCommand deleteBrandsCommand = new SqlCommand(deleteBrandsQuery, connection, transaction);
                        deleteBrandsCommand.ExecuteNonQuery();

                        string deleteProductTypesQuery = "DELETE FROM ProductTypes WHERE TypeId NOT IN (SELECT TypeId FROM Products)";
                        SqlCommand deleteProductTypesCommand = new SqlCommand(deleteProductTypesQuery, connection, transaction);
                        deleteProductTypesCommand.ExecuteNonQuery();

                        string deleteFoodPreferencesQuery = "DELETE FROM FoodPreferences WHERE FoodPreferenceId NOT IN (SELECT FoodPreferenceId FROM Products)";
                        SqlCommand deleteFoodPreferencesCommand = new SqlCommand(deleteFoodPreferencesQuery, connection, transaction);
                        deleteFoodPreferencesCommand.ExecuteNonQuery();

                        string deleteSellersQuery = "DELETE FROM Sellers WHERE SellerId NOT IN (SELECT SellerId FROM Products)";
                        SqlCommand deleteSellersCommand = new SqlCommand(deleteSellersQuery, connection, transaction);
                        deleteSellersCommand.ExecuteNonQuery();

                        string deleteManufacturersQuery = "DELETE FROM Manufacturers WHERE ManufacturerId NOT IN (SELECT ManufacturerId FROM Products)";
                        SqlCommand deleteManufacturersCommand = new SqlCommand(deleteManufacturersQuery, connection, transaction);
                        deleteManufacturersCommand.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback if any error occurs
                        transaction.Rollback();
                        // Log or handle the error
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            // Redirect to the Product List after deletion
            return RedirectToAction("ProductList", "Product");
        }

        [HttpPost]
        public ActionResult ImportProducts(HttpPostedFileBase file)
        {
            // Initialize counters and logging
            int totalRecords = 0;
            int successCount = 0;
            int skipCount = 0;
            int errorCount = 0;
            StringBuilder debugLog = new StringBuilder();
            DataTable dt = new DataTable();

            try
            {
                // 1. File validation
                if (file == null || file.ContentLength == 0)
                {
                    return Json(new { success = false, message = "No file selected" });
                }

                debugLog.AppendLine($"Starting import of {file.FileName}");

                // 2. File processing
                var extension = Path.GetExtension(file.FileName).ToLower();
                try
                {
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        using (var stream = file.InputStream)
                        {
                            IWorkbook workbook;
                            if (extension == ".xlsx")
                            {
                                workbook = new XSSFWorkbook(stream);
                            }
                            else
                            {
                                workbook = new HSSFWorkbook(stream);
                            }

                            ISheet sheet = workbook.GetSheetAt(0);
                            dt = ConvertSheetToDataTable(sheet, debugLog);
                        }
                    }
                    else if (extension == ".csv")
                    {
                        using (var reader = new StreamReader(file.InputStream))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            using (var dr = new CsvDataReader(csv))
                            {
                                dt.Load(dr);
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid file format" });
                    }
                }
                catch (Exception ex)
                {
                    debugLog.AppendLine($"File processing error: {ex.ToString()}");
                    throw new Exception("Failed to process file. Please ensure it's a valid Excel or CSV file.", ex);
                }

                totalRecords = dt.Rows.Count;
                debugLog.AppendLine($"Found {totalRecords} records to process");

                // 3. Database operations
                string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        debugLog.AppendLine("Database connection opened successfully");

                        if (!StoredProcedureExists(conn, "InsertProducts", debugLog))
                        {
                            string error = "InsertProducts stored procedure not found";
                            debugLog.AppendLine(error);
                            return Json(new { success = false, message = error });
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            string productName = row["ProductName"]?.ToString();
                            debugLog.AppendLine($"Processing product: {productName}");

                            try
                            {
                                bool exists = ProductExists(conn, productName, debugLog);
                                if (exists)
                                {
                                    skipCount++;
                                    debugLog.AppendLine($"Product exists, skipping: {productName}");
                                    continue;
                                }

                                using (SqlCommand cmd = new SqlCommand("InsertProducts", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    AddParameterIfExists(cmd, "@ProductName", row, "ProductName", debugLog);
                                    AddParameterIfExists(cmd, "@Brand", row, "Brand", debugLog);
                                    AddParameterIfExists(cmd, "@ModelName", row, "ModelName", debugLog);
                                    AddParameterIfExists(cmd, "@Description", row, "Description", debugLog);
                                    AddParameterIfExists(cmd, "@Price", row, "Price", debugLog, isDecimal: true);
                                    AddParameterIfExists(cmd, "@OriginalPrice", row, "OriginalPrice", debugLog, isDecimal: true);
                                    AddParameterIfExists(cmd, "@DiscountPercent", row, "DiscountPercent", debugLog, isDecimal: true);
                                    AddParameterIfExists(cmd, "@Available", row, "Available", debugLog);
                                    AddParameterIfExists(cmd, "@Stock", row, "Stock", debugLog);
                                    AddParameterIfExists(cmd, "@Flavor", row, "Flavor", debugLog);
                                    AddParameterIfExists(cmd, "@Type", row, "Type", debugLog);
                                    AddParameterIfExists(cmd, "@FoodPreference", row, "FoodPreference", debugLog);
                                    AddParameterIfExists(cmd, "@PackOf", row, "PackOf", debugLog);
                                    AddParameterIfExists(cmd, "@NetQuantity", row, "NetQuantity", debugLog);
                                    AddParameterIfExists(cmd, "@ShelfLife", row, "ShelfLife", debugLog);
                                    AddParameterIfExists(cmd, "@IsGourmet", row, "IsGourmet", debugLog);
                                    AddParameterIfExists(cmd, "@IsHomemade", row, "IsHomemade", debugLog);
                                    AddParameterIfExists(cmd, "@IsGiftPack", row, "IsGiftPack", debugLog);
                                    AddParameterIfExists(cmd, "@Seller", row, "Seller", debugLog);
                                    AddParameterIfExists(cmd, "@NoReturnsAllowed", row, "NoReturnsAllowed", debugLog);
                                    AddParameterIfExists(cmd, "@GstInvoiceAvailable", row, "GstInvoiceAvailable", debugLog);
                                    AddParameterIfExists(cmd, "@ManufacturedBy", row, "ManufacturedBy", debugLog);
                                    AddParameterIfExists(cmd, "@Ingredients", row, "Ingredients", debugLog);
                                    AddParameterIfExists(cmd, "@NutritionInfo", row, "NutritionInfo", debugLog);
                                    AddParameterIfExists(cmd, "@WidthCm", row, "WidthCm", debugLog, isDecimal: true);
                                    AddParameterIfExists(cmd, "@HeightCm", row, "HeightCm", debugLog, isDecimal: true);
                                    AddParameterIfExists(cmd, "@DepthCm", row, "DepthCm", debugLog, isDecimal: true);
                                    AddParameterIfExists(cmd, "@WeightG", row, "WeightG", debugLog, isDecimal: true);
                                    AddParameterIfExists(cmd, "@ImageUrl", row, "ImageUrl", debugLog);

                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        successCount++;
                                        debugLog.AppendLine($"Successfully imported: {productName}");
                                    }
                                    else
                                    {
                                        errorCount++;
                                        debugLog.AppendLine($"No rows affected for: {productName}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                errorCount++;
                                debugLog.AppendLine($"ERROR importing {productName}: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        debugLog.AppendLine($"Database error: {ex.ToString()}");
                        throw;
                    }
                }

                string resultMessage = $"Processed {totalRecords} records: {successCount} imported, {skipCount} skipped, {errorCount} failed";
                debugLog.AppendLine(resultMessage);

                return Json(new
                {
                    success = errorCount == 0,
                    message = resultMessage,
                    totalRecords,
                    importedCount = successCount,
                    skippedCount = skipCount,
                    errorCount,
                    detailedLog = debugLog.ToString()
                });
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"FATAL ERROR: {ex.ToString()}");
                return Json(new
                {
                    success = false,
                    message = $"Import failed: {ex.Message}",
                    detailedLog = debugLog.ToString()
                });
            }
        }

        #region Helper Methods with debugLog parameter
        private DataTable ConvertSheetToDataTable(ISheet sheet, StringBuilder debugLog)
        {
            DataTable dt = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(0);
                foreach (ICell cell in headerRow.Cells)
                {
                    dt.Columns.Add(cell.ToString());
                }

                for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
                {
                    IRow row = sheet.GetRow(rowIdx);
                    if (row == null) continue;

                    DataRow dataRow = dt.NewRow();
                    for (int colIdx = 0; colIdx < headerRow.Cells.Count; colIdx++)
                    {
                        ICell cell = row.GetCell(colIdx);
                        dataRow[colIdx] = cell?.ToString() ?? string.Empty;
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error converting sheet to DataTable: {ex.Message}");
                throw;
            }
            return dt;
        }

        private bool StoredProcedureExists(SqlConnection conn, string spName, StringBuilder debugLog)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES 
                        WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = @spName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@spName", spName);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error checking for stored procedure: {ex.Message}");
                throw;
            }
        }

        private bool ProductExists(SqlConnection conn, string productName, StringBuilder debugLog)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Products WHERE ProductName = @ProductName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error checking for product existence: {ex.Message}");
                throw;
            }
        }

        private void AddParameterIfExists(SqlCommand cmd, string paramName, DataRow row, string columnName,
                                        StringBuilder debugLog, bool isDecimal = false, bool isInt = false, bool isBool = false)
        {
            try
            {
                if (!row.Table.Columns.Contains(columnName))
                {
                    cmd.Parameters.AddWithValue(paramName, DBNull.Value);
                    debugLog.AppendLine($"Warning: Column '{columnName}' not found - using NULL");
                    return;
                }

                object value = row[columnName];

                if (isDecimal)
                {
                    cmd.Parameters.AddWithValue(paramName, GetDecimalValue(value, debugLog));
                }
                else if (isInt)
                {
                    cmd.Parameters.AddWithValue(paramName, GetIntValue(value, debugLog));
                }
                else if (isBool)
                {
                    cmd.Parameters.AddWithValue(paramName, GetBooleanValue(value, debugLog));
                }
                else
                {
                    cmd.Parameters.AddWithValue(paramName, GetStringValue(value, debugLog));
                }
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error adding parameter {paramName}: {ex.Message}");
                throw;
            }
        }

        private string GetStringValue(object value, StringBuilder debugLog)
        {
            try
            {
                return (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                    ? null
                    : value.ToString();
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error getting string value: {ex.Message}");
                return null;
            }
        }

        private decimal GetDecimalValue(object value, StringBuilder debugLog)
        {
            try
            {
                if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                    return 0m;

                decimal result;
                return decimal.TryParse(value.ToString(), out result) ? result : 0m;
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error getting decimal value: {ex.Message}");
                return 0m;
            }
        }

        private int GetIntValue(object value, StringBuilder debugLog)
        {
            try
            {
                if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                    return 0;

                int result;
                return int.TryParse(value.ToString(), out result) ? result : 0;
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error getting int value: {ex.Message}");
                return 0;
            }
        }

        private bool GetBooleanValue(object value, StringBuilder debugLog)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                    return false;

                string strValue = value.ToString().ToLower().Trim();
                return strValue == "true" || strValue == "yes" || strValue == "1" || strValue == "y";
            }
            catch (Exception ex)
            {
                debugLog.AppendLine($"Error getting boolean value: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}