using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login_Lan1.ViewModel
{
    public class UserViewModel
    {
       
        public string  UserId { get; set; }
        public string  Email { get; set; }
        public string  RoleName { get; set; }
        public string  City { get; set; }
        public string  Address { get; set; }
    }
}
