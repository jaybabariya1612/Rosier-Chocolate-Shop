using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RosierBars.Models
{
    public class LoginModel
    {
        [Required]
        //[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email ID")]
        public string Email{ get; set; }
        [Required]
        public string Password{ get; set; }
    }
}