using RosierBars.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace RosierBars.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel resdata)
        {
            if (!ModelState.IsValid)
                return View(resdata);

            if (resdata.Password != resdata.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(resdata);
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check if email exists
                string emailCheckQuery = "SELECT COUNT(*) FROM tbl_register WHERE EmailID = @EmailID";
                using (SqlCommand checkCmd = new SqlCommand(emailCheckQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@EmailID", resdata.Email);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        ViewBag.RegisterMessage = "That Email ID is taken. Try another.";
                        return View(resdata);
                    }
                }

                // Insert new user
                using (SqlCommand cmd = new SqlCommand("insertregister", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", resdata.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", resdata.LastName);
                    cmd.Parameters.AddWithValue("@Gender", resdata.Gender);
                    cmd.Parameters.AddWithValue("@DOB", resdata.DOB);
                    cmd.Parameters.AddWithValue("@MobileNo", resdata.MobileNo);
                    cmd.Parameters.AddWithValue("@EmailID", resdata.Email);
                    cmd.Parameters.AddWithValue("@Address", resdata.Address);
                    cmd.Parameters.AddWithValue("@Password", resdata.Password);
                    cmd.Parameters.AddWithValue("@ConfirmPassword", resdata.ConfirmPassword);
                    cmd.Parameters.AddWithValue("@IsTermAgree", resdata.IsTermAgree);

                    cmd.ExecuteNonQuery();
                    ViewBag.successRegister = "Registration successful! Redirecting to login...";

                    // Add header to refresh after 3 seconds
                    Response.AddHeader("Refresh", "3;url=" + Url.Action("Login", "Register"));
                }
            }

            return View(resdata);
        }

        public ActionResult List()
        {

            var regusers = new List<RegisterModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select RegisterID,FirstName,LastName,Gender,DOB,MobileNo,EmailID,Address,Password,ConfirmPassword,IsTermAgree,EntryDate,UpdateDate from tbl_register";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    regusers.Add(new RegisterModel
                    {
                        RegisterID = Convert.ToInt32(reader["RegisterID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DOB = reader["DOB"].ToString(),
                        MobileNo = reader["MobileNo"].ToString(),
                        Email = reader["EmailID"].ToString(),
                        Address = reader["Address"].ToString(),
                        Password = reader["Password"].ToString(),
                        ConfirmPassword = reader["ConfirmPassword"].ToString(),
                        IsTermAgree = Convert.ToBoolean(reader["IsTermAgree"]),
                        EntryDate = reader["EntryDate"].ToString(),
                        UpdateDate = reader["UpdateDate"].ToString(),


                    });
                }
                reader.Close();
                return View(regusers);
            }
        }

        public ActionResult RegDetail(int id)
        {
            RegisterModel resdata = new RegisterModel();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select RegisterID,FirstName,LastName,Gender,DOB,MobileNo,EmailID,Address,Password,ConfirmPassword,IsTermAgree,EntryDate,UpdateDate from tbl_register where RegisterID=" + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    resdata.RegisterID = Convert.ToInt32(reader["RegisterID"]);
                    resdata.FirstName = reader["FirstName"].ToString();
                    resdata.LastName = reader["LastName"].ToString();
                    resdata.Gender = reader["Gender"].ToString();
                    resdata.DOB = reader["DOB"].ToString();
                    resdata.MobileNo = reader["MobileNo"].ToString();
                    resdata.Email = reader["EmailID"].ToString();
                    resdata.Address = reader["Address"].ToString();
                    resdata.Password = reader["Password"].ToString();
                    resdata.ConfirmPassword = reader["ConfirmPassword"].ToString();
                    resdata.IsTermAgree = Convert.ToBoolean(reader["IsTermAgree"]);
                    resdata.EntryDate = reader["EntryDate"].ToString();
                    resdata.UpdateDate = reader["UpdateDate"].ToString();
                }
                reader.Close();
            }

            return View(resdata);
        }

        public ActionResult Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "delete from tbl_register where RegisterID=" + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }

            return RedirectToAction("List", "Register");
        }

        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Login(LoginModel logdata)
        {
            if (!ModelState.IsValid)
                return View(logdata);

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select RegisterID,EmailID,FirstName,LastName,Password,MobileNo from tbl_Register where EmailID=" + "'" + logdata.Email + "'" + " and Password=" + "'" + logdata.Password + "'" + " or " + "MobileNo=" + "'" + logdata.Email + "'" + " and Password=" + "'" + logdata.Password + "'";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                var DB_EmailID = "";
                var DB_MobileNo = "";
                var DB_Password = "";
                var DB_FirstName = "";
                var DB_LastName = "";
                var DB_RegisterID = "";

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DB_EmailID = reader["EmailID"].ToString();
                    DB_MobileNo = reader["MobileNo"].ToString();
                    DB_Password = reader["Password"].ToString();
                    DB_FirstName = reader["FirstName"].ToString();
                    DB_LastName = reader["LastName"].ToString();
                    DB_RegisterID = reader["RegisterID"].ToString();

                }
                reader.Close();

                if (DB_EmailID == logdata.Email && DB_Password == logdata.Password)
                {
                    Session["UserName"] = DB_FirstName + " " + DB_LastName;
                    Session["Email"] = DB_EmailID;
                    Session["RegisterID"] = DB_RegisterID; // Store Register ID securely in session

                    TempData["LoginMessage"] = "Login with Email Successfully";
                    TempData["ShouldDelayRedirect"] = true;

                    return RedirectToAction("Index", "Home");
                }

                else if (DB_MobileNo == logdata.Email && DB_Password == logdata.Password)
                {
                    Session["UserName"] = DB_FirstName + "  " + DB_LastName;
                    TempData["LoginMessage"] = "Login with MobileNo Successfull";
                    TempData["RedirectToHome"] = "Redirecting To Home Page";
                    TempData["ShouldDelayRedirect"] = true;
                    return View(logdata); // Stay on login page to show message
                }
                else
                {
                    ViewBag.Message = "Email or Password Incorrect";
                    return View(logdata);
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();        // Removes all keys and values
            Session.Abandon();      // Ends the current session (optional)


            return RedirectToAction("Index", "Home");
            }

        public ActionResult Edit(int id)
        {

            RegisterModel model = new RegisterModel();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select RegisterID,FirstName,LastName,Gender,DOB,MobileNo,EmailID,Address,Password,ConfirmPassword,IsTermAgree,UpdateDate from tbl_register where RegisterID=" + id;

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    model.RegisterID = Convert.ToInt32(reader["RegisterID"]);
                    model.FirstName = reader["FirstName"].ToString();
                    model.LastName = reader["LastName"].ToString();
                    model.Gender = reader["Gender"].ToString();
                    model.DOB = reader["DOB"].ToString();
                    model.MobileNo = reader["MobileNo"].ToString();
                    model.Email = reader["EmailID"].ToString();
                    model.Address = reader["Address"].ToString();
                    model.Password = reader["Password"].ToString();
                    model.ConfirmPassword = reader["ConfirmPassword"].ToString();
                    model.IsTermAgree = Convert.ToBoolean(reader["IsTermAgree"]);
                    model.UpdateDate = reader["UpdateDate"].ToString();
                }
                reader.Close();
                return View(model);

            }
        }

        [HttpPost]
        public ActionResult Edit(RegisterModel resdata)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UpdateReg", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@RegisterID", resdata.RegisterID);
                    cmd.Parameters.AddWithValue("@FirstName", resdata.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", resdata.LastName);
                    cmd.Parameters.AddWithValue("@Gender", resdata.Gender);
                    cmd.Parameters.AddWithValue("@DOB", resdata.DOB);
                    cmd.Parameters.AddWithValue("@MobileNo", resdata.MobileNo);
                    cmd.Parameters.AddWithValue("@EmailID", resdata.Email);
                    cmd.Parameters.AddWithValue("@Address", resdata.Address);
                    cmd.Parameters.AddWithValue("@Password", resdata.Password);
                    cmd.Parameters.AddWithValue("@ConfirmPassword", resdata.ConfirmPassword);
                    cmd.Parameters.AddWithValue("@IsTermAgree", resdata.IsTermAgree);
                    cmd.Parameters.AddWithValue("@UpdateDate", resdata.UpdateDate);

                    cmd.ExecuteNonQuery();

                }
                //return View(empdata);
                return RedirectToAction("List", "Register");
            }
        }

        //public ActionResult Login(LoginModel logdata)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string query = @"
        //    SELECT EmailID, FirstName, LastName, Password, MobileNo 
        //    FROM tbl_Register 
        //    WHERE (EmailID = @Login OR MobileNo = @Login) AND Password = @Password";

        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@Login", logdata.Email);
        //        command.Parameters.AddWithValue("@Password", logdata.Password); // Note: for real apps, never store plain passwords

        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            string firstName = reader["FirstName"].ToString();
        //            string lastName = reader["LastName"].ToString();

        //            Session["UserName"] = firstName + " " + lastName;

        //            ViewBag.Message = "Login successful";
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ViewBag.Message = "Invalid Email/Mobile or Password";
        //        return View(logdata);
        //    }
        //}

    }
}