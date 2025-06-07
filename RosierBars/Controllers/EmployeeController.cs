using RosierBars.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RosierBars.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Employee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Employee(EmployeeForm empdata)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("insertemp", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.AddWithValue("@FirstName", empdata.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", empdata.LastName);
                        cmd.Parameters.AddWithValue("@Email ", empdata.Email);
                        cmd.Parameters.AddWithValue("@PhoneNo", empdata.PhoneNo);
                        cmd.Parameters.AddWithValue("@Address", empdata.Address);
                        cmd.Parameters.AddWithValue("@City", empdata.City);
                        cmd.Parameters.AddWithValue("@State ", empdata.State);
                        cmd.Parameters.AddWithValue("@PostalCode", empdata.PostalCode);
                        cmd.Parameters.AddWithValue("@Country", empdata.Country);
                        cmd.Parameters.AddWithValue("@DateOfBirth", empdata.DateOfBirth);
                        cmd.Parameters.AddWithValue("@Gender", empdata.Gender);
                        cmd.Parameters.AddWithValue("@HireDate", empdata.HireDate);
                        cmd.Parameters.AddWithValue("@Position ", empdata.Position);
                        cmd.Parameters.AddWithValue("@Department", empdata.Department);
                        cmd.Parameters.AddWithValue("@Salary", empdata.Salary);
                        cmd.Parameters.AddWithValue("@Status ", empdata.Status);
                        cmd.Parameters.AddWithValue("@EmergencyContact", empdata.EmergencyContact);
                        cmd.Parameters.AddWithValue("@BankAccount ", empdata.BankAccount);

                        cmd.ExecuteNonQuery();

                    }
                }
                ViewBag.EmployeeAdd = "Employee Added SuccessFully";
            }
            return View(empdata);
        }


        public ActionResult List()
        {

            var users = new List<EmployeeForm>();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select EmployeeID,FirstName,LastName,Email,PhoneNo,Address,City,State,PostalCode,Country,DateOfBirth,Gender,HireDate,Position,Department,Salary,Status,EmergencyContact,BankAccount,CreatedAt,UpdatedAt from tbl_emp";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new EmployeeForm
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNo = reader["PhoneNo"].ToString(),
                        Address = reader["Address"].ToString(),
                        City = reader["City"].ToString(),
                        State = reader["State"].ToString(),
                        PostalCode = reader["PostalCode"].ToString(),
                        Country = reader["Country"].ToString(),
                        DateOfBirth = reader["DateOfBirth"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        HireDate = reader["HireDate"].ToString(),
                        Position = reader["Position"].ToString(),
                        Department = reader["Department"].ToString(),
                        Salary = Convert.ToInt32(reader["Salary"]),
                        Status = reader["Status"].ToString(),
                        EmergencyContact = reader["EmergencyContact"].ToString(),
                        BankAccount = reader["BankAccount"].ToString(),
                        CreatedAt = reader["CreatedAt"].ToString(),
                        UpdatedAt = reader["UpdatedAt"].ToString(),



                    });
                }
                reader.Close();
                return View(users);
            }
        }


        public ActionResult EmpDetail(int id)
        {
            EmployeeForm empdata = new EmployeeForm();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select EmployeeID,FirstName,LastName,Email,PhoneNo,Address,City,State,PostalCode,Country,DateOfBirth,Gender,HireDate,Position,Department,Salary,Status,EmergencyContact,BankAccount,CreatedAt,UpdatedAt from tbl_emp where EmployeeID=" + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    empdata.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    empdata.FirstName = reader["FirstName"].ToString();
                    empdata.LastName = reader["LastName"].ToString();
                    empdata.Email = reader["Email"].ToString();
                    empdata.PhoneNo = reader["PhoneNo"].ToString();
                    empdata.Address = reader["Address"].ToString();
                    empdata.Email = reader["Email"].ToString();
                    empdata.Address = reader["Address"].ToString();
                    empdata.City = reader["City"].ToString();
                    empdata.State = reader["State"].ToString();
                    empdata.PostalCode = reader["PostalCode"].ToString();
                    empdata.Country = reader["Country"].ToString();
                    empdata.DateOfBirth = reader["DateOfBirth"].ToString();
                    empdata.Gender = reader["Gender"].ToString();
                    empdata.HireDate = reader["HireDate"].ToString();
                    empdata.Position = reader["Position"].ToString();
                    empdata.Department = reader["Department"].ToString();
                    empdata.Salary = Convert.ToInt32(reader["Salary"]);
                    empdata.Status = reader["Status"].ToString();
                    empdata.EmergencyContact = reader["EmergencyContact"].ToString();
                    empdata.BankAccount = reader["BankAccount"].ToString();
                    empdata.CreatedAt = reader["CreatedAt"].ToString();
                    empdata.UpdatedAt = reader["UpdatedAt"].ToString();
                }
                reader.Close();
            }

            return View(empdata);
        }

        public ActionResult Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "delete from tbl_emp where EmployeeID=" + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }

            return RedirectToAction("List", "Employee");
        }


        public ActionResult Inactive(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Update tbl_emp set status='Inactive' where EmployeeID=" + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }

            return RedirectToAction("List", "Employee");
        }

        public ActionResult Active(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Update tbl_emp set status='Active' where EmployeeID=" + id;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }

            return RedirectToAction("List", "Employee");
        }

        public ActionResult UpdatePhone(int PhoneNo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Update tbl_emp set PhoneNo='PhoneNo' where PhoneNo=" + PhoneNo;

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }

            return RedirectToAction("List", "Employee"); ;
        }


        public ActionResult Edit(int id)
        {

            EmployeeForm model = new EmployeeForm();

            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select EmployeeID,FirstName,LastName,Email,PhoneNo,Address,City,State,PostalCode,Country,DateOfBirth,Gender,HireDate,Position,Department,Salary,Status,EmergencyContact,BankAccount,UpdatedAt from tbl_emp where employeeID=" + id;

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    model.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    model.FirstName = reader["FirstName"].ToString();
                    model.LastName = reader["LastName"].ToString();
                    model.Email = reader["Email"].ToString();
                    model.PhoneNo = reader["PhoneNo"].ToString();
                    model.Address = reader["Address"].ToString();
                    model.City = reader["City"].ToString();
                    model.State = reader["State"].ToString();
                    model.PostalCode = reader["PostalCode"].ToString();
                    model.Country = reader["Country"].ToString();
                    model.DateOfBirth = reader["DateOfBirth"].ToString();
                    model.Gender = reader["Gender"].ToString();
                    model.HireDate = reader["HireDate"].ToString();
                    model.Position = reader["Position"].ToString();
                    model.Department = reader["Department"].ToString();
                    model.Salary = Convert.ToInt32(reader["Salary"]);
                    model.Status = reader["Status"].ToString();
                    model.EmergencyContact = reader["EmergencyContact"].ToString();
                    model.BankAccount = reader["BankAccount"].ToString();
                    model.UpdatedAt = reader["UpdatedAt"].ToString();
                }
                reader.Close();
                return View(model);

            }
        }

        [HttpPost]
        public ActionResult Edit(EmployeeForm empdata)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UpdateEmp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@EmployeeID", empdata.EmployeeID);
                    cmd.Parameters.AddWithValue("@FirstName", empdata.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", empdata.LastName);
                    cmd.Parameters.AddWithValue("@Email ", empdata.Email);
                    cmd.Parameters.AddWithValue("@PhoneNo", empdata.PhoneNo);
                    cmd.Parameters.AddWithValue("@Address", empdata.Address);
                    cmd.Parameters.AddWithValue("@City", empdata.City);
                    cmd.Parameters.AddWithValue("@State ", empdata.State);
                    cmd.Parameters.AddWithValue("@PostalCode", empdata.PostalCode);
                    cmd.Parameters.AddWithValue("@Country", empdata.Country);
                    cmd.Parameters.AddWithValue("@DateOfBirth", empdata.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", empdata.Gender);
                    cmd.Parameters.AddWithValue("@HireDate", empdata.HireDate);
                    cmd.Parameters.AddWithValue("@Position ", empdata.Position);
                    cmd.Parameters.AddWithValue("@Department", empdata.Department);
                    cmd.Parameters.AddWithValue("@Salary", empdata.Salary);
                    cmd.Parameters.AddWithValue("@Status ", empdata.Status);
                    cmd.Parameters.AddWithValue("@EmergencyContact", empdata.EmergencyContact);
                    cmd.Parameters.AddWithValue("@BankAccount ", empdata.BankAccount);
                    cmd.Parameters.AddWithValue("@UpdatedAt", empdata.UpdatedAt);

                    cmd.ExecuteNonQuery();

                }
                //return View(empdata);
                return RedirectToAction("List", "Employee");
            }
        }
    }
}
