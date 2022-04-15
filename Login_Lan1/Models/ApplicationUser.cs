using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Login_Lan1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
