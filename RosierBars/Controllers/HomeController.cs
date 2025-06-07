using RosierBars.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace RosierBars.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactForm contactdata)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("insertinquries", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.AddWithValue("@FirstName", contactdata.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", contactdata.LastName);
                        cmd.Parameters.AddWithValue("@Gender", contactdata.Gender);
                        cmd.Parameters.AddWithValue("@Phone", contactdata.Phone);
                        cmd.Parameters.AddWithValue("@Email", contactdata.Email);
                        cmd.Parameters.AddWithValue("@Address", contactdata.Address);
                        cmd.Parameters.AddWithValue("@City", contactdata.City);
                        cmd.Parameters.AddWithValue("@State", contactdata.State);
                        cmd.Parameters.AddWithValue("@Country", contactdata.Country);

                        cmd.ExecuteNonQuery();

                        ViewBag.Message = "Your message has been sent successfully!";
                        ViewBag.ShowMessage = true;

                        // Clear the form if the submission was successful
                        ModelState.Clear();
                        return View(new ContactForm());
                    }
                }
            }
            return View(contactdata);
        }


        public ActionResult FAQ()
        {

            return View();
        }

        public ActionResult PrivacyPolicy()
        {

            return View();
        }

        public ActionResult TermsofService()
        {

            return View();
        }
        public ActionResult ProductList()
        {
            var ProductList = new List<ProductModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.ProductName,p.ImageUrl,p.Price FROM Products p JOIN Brands b ON p.BrandId = b.BrandId JOIN ProductTypes pt ON p.TypeId = pt.TypeId JOIN FoodPreferences fp ON p.FoodPreferenceId = fp.FoodPreferenceId JOIN Sellers s ON p.SellerId = s.SellerId JOIN Manufacturers m ON p.ManufacturerId = m.ManufacturerId;\r\n";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductList.Add(new ProductModel
                    {
                        ProductName = reader["ProductName"]?.ToString(),
                        Price = reader["Price"] != DBNull.Value ? Convert.ToInt32(reader["Price"]) : 0,
                        ImageUrl = reader["ImageUrl"]?.ToString(),

                    });
                }
                reader.Close();
                return PartialView("_ProductList", ProductList);
            }
        }


    }
}