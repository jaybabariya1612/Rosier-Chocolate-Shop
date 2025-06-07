using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RosierBars.Models
{
    public class EmployeeForm

    {
        public int EmployeeID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email ID")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]
        public string PhoneNo { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        public string City { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        public string State { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        public string PostalCode { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        [Required]
        public string Country { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string HireDate { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        public int Salary { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]
        public string EmergencyContact { get; set; }
        [Required]
        [RegularExpression(@"^\d{9,18}$", ErrorMessage = "Please enter a valid Indian bank account number (9 to 18 digits).")]
        public string BankAccount { get; set; }

        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}