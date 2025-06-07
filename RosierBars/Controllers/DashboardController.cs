using RosierBars.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace RosierBars.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard



        public ActionResult Account()
        {
            var model = new DashboardViewModel
            {
                UserName = Session["UserName"]?.ToString() ?? "Guest",
                Email = Session["Email"]?.ToString() ?? "Not Available",
                RecentOrders = GetRecentOrders()
            };

            return PartialView("_Account", model);
        }

        // Reusable data-fetching method
        public List<OrderModel> GetRecentOrders()
        {
            var orders = new List<OrderModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            // Get current logged-in user/customer ID
            var userId = Convert.ToInt32(Session["RegisterID"]);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
        WITH RecentOrders AS (
            SELECT TOP 2 OrderId, OrderDate, ShippingAddress, Mobile, TotalAmount, OrderStatus, PaymentStatus
            FROM Orders
            WHERE CustomerId = @CustomerId
            ORDER BY OrderDate DESC
        )
        SELECT 
            O.OrderId, O.OrderDate, O.ShippingAddress, O.Mobile, O.TotalAmount, O.OrderStatus, O.PaymentStatus,
            OI.OrderItemId, P.ProductName, P.ImageUrl, OI.Quantity, OI.Price
        FROM RecentOrders O
        JOIN OrderItems OI ON O.OrderId = OI.OrderId
        JOIN Products P ON OI.ProductId = P.ProductId
        ORDER BY O.OrderDate DESC;
        ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", userId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new OrderModel
                            {
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                OrderItemId = Convert.ToInt32(reader["OrderItemId"]),
                                Mobile = reader["Mobile"].ToString(),
                                ShippingAddress = reader["ShippingAddress"].ToString(),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                ProductName = reader["ProductName"].ToString(),
                                ImageUrl = reader["ImageUrl"].ToString(),
                                OrderStatus = reader["OrderStatus"].ToString(),
                                PaymentStatus = reader["PaymentStatus"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                Price = Convert.ToInt32(reader["Price"])
                            });
                        }
                    }
                }
            }

            return orders;
        }


        public ActionResult Orders()
        {
            var userId = Convert.ToInt32(Session["RegisterID"]); // Assuming you store UserId in session
            var orders = new List<OrderModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                O.OrderId, O.OrderDate, O.ShippingAddress, O.Mobile, O.TotalAmount,
                O.OrderStatus, O.PaymentStatus,
                OI.OrderItemId, P.ProductName, P.ImageUrl, OI.Quantity, OI.Price
            FROM Orders O
            JOIN OrderItems OI ON O.OrderId = OI.OrderId
            JOIN Products P ON OI.ProductId = P.ProductId
            WHERE O.CustomerId = @CustomerId
            ORDER BY O.OrderDate DESC, O.OrderId;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new OrderModel
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderItemId = Convert.ToInt32(reader["OrderItemId"]),
                        Mobile = reader["Mobile"].ToString(),
                        ShippingAddress = reader["ShippingAddress"].ToString(),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        ProductName = reader["ProductName"].ToString(),
                        ImageUrl = reader["ImageUrl"].ToString(),
                        OrderStatus = reader["OrderStatus"].ToString(),
                        PaymentStatus = reader["PaymentStatus"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        Price = Convert.ToInt32(reader["Price"]),
                    });
                }
                reader.Close();
            }

            return PartialView("_Orders", orders);
        }

        public ActionResult _OrderDetails(int id)
        {
            List<OrderDetailModel> details = new List<OrderDetailModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                oi.OrderId,
                r.FirstName + ' ' + r.LastName AS CustomerName,
                o.OrderDate,
                o.ShippingAddress,
                o.Mobile,
                o.TotalAmount,
                o.OrderStatus,
                o.PaymentStatus,
                p.ProductName,
				p.ImageUrl,
                oi.Quantity,
                oi.Price
            FROM 
                OrderItems oi
            INNER JOIN 
                Orders o ON oi.OrderId = o.OrderId
            INNER JOIN 
                tbl_register r ON o.CustomerId = r.RegisterID 
            INNER JOIN 
                Products p ON oi.ProductId = p.ProductId
            WHERE 
                oi.OrderId = " + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OrderDetailModel model = new OrderDetailModel
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        CustomerName = reader["CustomerName"].ToString(),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ShippingAddress = reader["ShippingAddress"].ToString(),
                        Mobile = reader["Mobile"].ToString(),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        OrderStatus = reader["OrderStatus"].ToString(),
                        imageurl = reader["ImageUrl"].ToString(),
                        PaymentStatus = reader["PaymentStatus"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"])
                    };

                    details.Add(model);
                }

                reader.Close();
            }

            return PartialView("_OrderDetails", details);
        }




    }
}

