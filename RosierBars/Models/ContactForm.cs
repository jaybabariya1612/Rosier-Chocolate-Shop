using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RosierBars.Models
{
    public class ContactForm
    {

        //public int ContactID { get; set; }

        [Required(ErrorMessage ="Please Enter Your FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Your LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Select Your Gender")]
        public string Gender{ get; set; }
        [Required(ErrorMessage = "Please Enter Your EmailID")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",ErrorMessage = "Invalid Email ID")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Your MobileNo")]
        [RegularExpression(@"^[0-9]{10}$",ErrorMessage = "Invalid Mobile Number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter Your City")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please Enter Your State")]
        public string State { get; set; }
        [Required(ErrorMessage = "Please Enter Your Country")]
        public string Country { get; set; }


    }
}