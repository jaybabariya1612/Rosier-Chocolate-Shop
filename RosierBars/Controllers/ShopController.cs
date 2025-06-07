using RosierBars.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace RosierBars.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Shop()
        {
            var ProductList = new List<ProductModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.ProductId, b.Brand,p.ModelName,p.ProductName,p.Description,p.Price,p.OriginalPrice,p.DiscountPercent,p.Rating,p.TotalRatings,p.TotalReviews,p.Available,p.Stock,p.Flavor,pt.Type,fp.FoodPreference,p.PackOf,p.NetQuantity,p.ShelfLife,p.IsGourmet,p.IsHomemade,p.IsGiftPack,s.Seller,p.NoReturnsAllowed,p.GstInvoiceAvailable,m.ManufacturedBy,p.Ingredients,p.NutritionInfo,p.WidthCm,p.HeightCm,p.DepthCm,p.WeightG,p.ImageUrl FROM Products p JOIN Brands b ON p.BrandId = b.BrandId JOIN ProductTypes pt ON p.TypeId = pt.TypeId JOIN FoodPreferences fp ON p.FoodPreferenceId = fp.FoodPreferenceId JOIN Sellers s ON p.SellerId = s.SellerId JOIN Manufacturers m ON p.ManufacturerId = m.ManufacturerId";

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
                        ImageUrl = reader["ImageUrl"]?.ToString()

                    });
                }

                reader.Close();
            }
            return View(ProductList);

        }

        public ActionResult Product(int id)
        {
            ProductModel model = new ProductModel();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT b.Brand,p.ModelName,p.ProductName,p.Description,p.Price,p.OriginalPrice,p.DiscountPercent,p.Rating,p.TotalRatings,p.TotalReviews,p.Available,p.Stock,p.Flavor,pt.Type,fp.FoodPreference,p.PackOf,p.NetQuantity,p.ShelfLife,p.IsGourmet,p.IsHomemade,p.IsGiftPack,s.Seller,p.NoReturnsAllowed,p.GstInvoiceAvailable,m.ManufacturedBy,p.Ingredients,p.NutritionInfo,p.WidthCm,p.HeightCm,p.DepthCm,p.WeightG,p.ImageUrl FROM Products p JOIN Brands b ON p.BrandId = b.BrandId JOIN ProductTypes pt ON p.TypeId = pt.TypeId JOIN FoodPreferences fp ON p.FoodPreferenceId = fp.FoodPreferenceId JOIN Sellers s ON p.SellerId = s.SellerId JOIN Manufacturers m ON p.ManufacturerId = m.ManufacturerId where productID=" + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
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
                }
                reader.Close();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            if (Session["RegisterID"] == null)
            {
                return RedirectToAction("Login", "Register");
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            int customerId = Convert.ToInt32(Session["RegisterID"]);

            // Get product price from database
            decimal price = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Price FROM Products WHERE ProductId = @ProductId", conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    price = Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }

            // Insert/update cart item
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("InsertCartItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    cmd.Parameters.AddWithValue("@OrderQuantity", quantity);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["CartMessage"] = "Product added to cart successfully!";
            return RedirectToAction("Product", "Shop", new { id = id });
        }
        public ActionResult CartCount()
        {
            int count = 0;

            if (Session["RegisterID"] != null)
            {
                int customerId = Convert.ToInt32(Session["RegisterID"]);
                string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Cart WHERE CustomerId = @CustomerId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }

            TempData["cartCount"] = count;
            return Content(count.ToString());
        }

        public ActionResult Cart()
        {
            if (Session["Username"] == null)
            {
                return View(new List<ProductModel>());
            }

            int customerId = Convert.ToInt32(Session["RegisterID"]);
            var cartItems = GetCartItems(customerId);
            return View(cartItems);
        }
        [HttpPost]
        public JsonResult UpdateQuantity(int productId, int OrderQuantity)
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return Json(new { success = false, message = "Please log in to update your cart" });
                }

                if (OrderQuantity < 1)
                {
                    return Json(new { success = false, message = "Quantity must be at least 1" });
                }

                int customerId = Convert.ToInt32(Session["RegisterID"]);
                string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // FIX: Removed the non-breaking space after @OrderQuantity
                    string query = @"UPDATE Cart SET OrderQuantity = @OrderQuantity
                                 WHERE CustomerId = @CustomerId AND ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderQuantity", OrderQuantity);
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@ProductId", productId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            return Json(new { success = false, message = "Item not found in your cart" });
                        }
                    }
                }

                decimal newSubtotal = CalculateCartSubtotal(customerId);
                return Json(new
                {
                    success = true,
                    newSubtotal = newSubtotal,
                    message = "Quantity updated successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating quantity: " + ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult RemoveItem(int productId)
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return Json(new { success = false, message = "Please log in to modify your cart" });
                }

                int customerId = Convert.ToInt32(Session["RegisterID"]);
                string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Cart WHERE CustomerId = @CustomerId AND ProductId = @ProductId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@ProductId", productId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            return Json(new { success = false, message = "Item not found in your cart" });
                        }
                    }
                }

                decimal newSubtotal = CalculateCartSubtotal(customerId);
                bool isEmpty = !GetCartItems(customerId).Any();

                return Json(new
                {
                    success = true,
                    newSubtotal = newSubtotal,
                    isEmpty = isEmpty,
                    message = "Item removed from cart"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while removing item: " + ex.Message
                });
            }
        }



        private List<ProductModel> GetCartItems(int customerId)
        {
            var cart = new List<ProductModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT p.ProductId, p.ProductName, p.ImageUrl, p.Price, p.OriginalPrice, 
                                    p.DiscountPercent, p.Flavor, p.PackOf, p.NetQuantity, s.Seller, c.OrderQuantity 
                             FROM Cart c 
                             JOIN Products p ON c.ProductId = p.ProductId 
                             JOIN Sellers s ON p.SellerId = s.SellerId 
                             WHERE c.CustomerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cart.Add(new ProductModel
                            {
                                ProductId = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                ImageUrl = reader.GetString(2),
                                Price = reader.GetInt32(3), // Assuming Price is decimal in DB
                                OriginalPrice = reader.GetDecimal(4),
                                DiscountPercent = reader.GetInt32(5),
                                Flavor = reader.GetString(6),
                                PackOf = reader.GetInt32(7),
                                NetQuantity = reader.GetString(8),
                                Seller = reader.GetString(9),
                                OrderQuantity = reader.GetInt32(10)
                            });
                        }
                    }
                }
            }
            return cart;
        }

        private decimal CalculateCartSubtotal(int customerId)
        {
            decimal subtotal = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT SUM(p.Price * c.OrderQuantity)
                             FROM Cart c
                             JOIN Products p ON c.ProductId = p.ProductId
                             WHERE c.CustomerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        subtotal = Convert.ToDecimal(result);
                    }
                }
            }
            return subtotal;
        }
        public ActionResult BackToProduct(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Product", "Shop", new { id = id });
        }

        public ActionResult CartUpdate(int id)
        {



            return RedirectToAction("Product", "Shop", new { id = id });
        }


        [HttpGet]
        public ActionResult Checkout()
        {
            if (Session["RegisterID"] == null)
                return RedirectToAction("Login", "Register");

            int customerId = Convert.ToInt32(Session["RegisterID"]);
            var cartItems = GetCartItems(customerId);
            if (!cartItems.Any())
                return RedirectToAction("Cart");

            var model = new OrderModel
            {
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(c => c.Price * c.OrderQuantity)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Checkout(OrderModel model)
        {
            if (Session["RegisterID"] == null)
                return RedirectToAction("Login", "Register");

            int customerId = Convert.ToInt32(Session["RegisterID"]);
            var cartItems = GetCartItems(customerId);

            if (cartItems == null || cartItems.Count == 0)
                return RedirectToAction("Cart");

            decimal totalAmount = cartItems.Sum(item => item.Price * item.OrderQuantity);

            int orderId;
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string orderQuery = @"INSERT INTO Orders 
                    (CustomerId, OrderDate, ShippingAddress, Mobile, TotalAmount, PaymentStatus, OrderStatus, CreatedAt, UpdatedAt)
                    OUTPUT INSERTED.OrderId
                    VALUES (@CustomerId, @OrderDate, @ShippingAddress, @Mobile, @TotalAmount, @PaymentStatus, @OrderStatus, GETDATE(), GETDATE())";

                        SqlCommand cmdOrder = new SqlCommand(orderQuery, conn, transaction);
                        cmdOrder.Parameters.AddWithValue("@CustomerId", customerId);
                        cmdOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        cmdOrder.Parameters.AddWithValue("@ShippingAddress", model.ShippingAddress);
                        cmdOrder.Parameters.AddWithValue("@Mobile", model.Mobile);
                        cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        cmdOrder.Parameters.AddWithValue("@PaymentStatus", "Pending");
                        cmdOrder.Parameters.AddWithValue("@OrderStatus", "Processing");

                        orderId = (int)cmdOrder.ExecuteScalar();

                        foreach (var item in cartItems)
                        {
                            // Insert OrderItem
                            string itemQuery = @"INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
                         VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                            SqlCommand cmdItem = new SqlCommand(itemQuery, conn, transaction);
                            cmdItem.Parameters.AddWithValue("@OrderId", orderId);
                            cmdItem.Parameters.AddWithValue("@ProductId", item.ProductId);
                            cmdItem.Parameters.AddWithValue("@Quantity", item.OrderQuantity);
                            cmdItem.Parameters.AddWithValue("@Price", item.Price);
                            cmdItem.ExecuteNonQuery();

                            // Update Product Stock
                            string updateStockQuery = @"UPDATE Products 
                                SET Stock = Stock - @Quantity 
                                WHERE ProductId = @ProductId AND Stock >= @Quantity";

                            SqlCommand cmdUpdateStock = new SqlCommand(updateStockQuery, conn, transaction);
                            cmdUpdateStock.Parameters.AddWithValue("@ProductId", item.ProductId);
                            cmdUpdateStock.Parameters.AddWithValue("@Quantity", item.OrderQuantity);
                            int rowsAffected = cmdUpdateStock.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                throw new Exception("Insufficient stock for product ID: " + item.ProductId);
                            }
                        }


                        SqlCommand cmdClearCart = new SqlCommand("DELETE FROM Cart WHERE CustomerId = @CustomerId", conn, transaction);
                        cmdClearCart.Parameters.AddWithValue("@CustomerId", customerId);
                        cmdClearCart.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        TempData["OrderError"] = "Something went wrong! Please try again.";
                        model.CartItems = cartItems;
                        model.TotalAmount = totalAmount;
                        return View(model);
                    }
                }
            }

            TempData["OrderSuccess"] = $"Order placed successfully! Your Order ID is #{orderId}";
            return RedirectToAction("OrderConfirmation", new { id = orderId });
        }

        public ActionResult OrderConfirmation(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
        [HttpPost]
        public ActionResult CancelOrder(int id)  // id = OrderId
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Step 1: Get all OrderItems for this Order
                        string selectItemsQuery = @"SELECT ProductId, Quantity FROM OrderItems WHERE OrderId = @OrderId";
                        SqlCommand cmdSelect = new SqlCommand(selectItemsQuery, conn, transaction);
                        cmdSelect.Parameters.AddWithValue("@OrderId", id);

                        List<(int productId, int quantity)> itemList = new List<(int, int)>();
                        using (SqlDataReader reader = cmdSelect.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int productId = Convert.ToInt32(reader["ProductId"]);
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                itemList.Add((productId, quantity));
                            }
                        }

                        // Step 2: Update product stock
                        foreach (var item in itemList)
                        {
                            string updateStockQuery = @"UPDATE Products 
                                                SET Stock = Stock + @Quantity 
                                                WHERE ProductId = @ProductId";

                            SqlCommand cmdUpdateStock = new SqlCommand(updateStockQuery, conn, transaction);
                            cmdUpdateStock.Parameters.AddWithValue("@ProductId", item.productId);
                            cmdUpdateStock.Parameters.AddWithValue("@Quantity", item.quantity);
                            cmdUpdateStock.ExecuteNonQuery();
                        }

                        // Step 3: Update order status to 'Cancelled'
                        string updateOrderQuery = @"UPDATE Orders 
                                            SET OrderStatus = 'Cancelled', UpdatedAt = GETDATE() 
                                            WHERE OrderId = @OrderId";

                        SqlCommand cmdUpdateOrder = new SqlCommand(updateOrderQuery, conn, transaction);
                        cmdUpdateOrder.Parameters.AddWithValue("@OrderId", id);
                        cmdUpdateOrder.ExecuteNonQuery();

                        transaction.Commit();
                        TempData["OrderCancelSuccess"] = $"Order #{id} cancelled successfully.";
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        TempData["OrderCancelError"] = "Failed to cancel the order. Try again.";
                    }
                }
            }

            return RedirectToAction("Login", "Register");
        }
    }
}