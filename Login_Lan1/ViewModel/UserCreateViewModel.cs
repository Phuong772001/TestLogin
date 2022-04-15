using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login_Lan1.ViewModel
{
    public class UserCreateViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "ConfirmPassword not match")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Role")]
        public string RoleId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
