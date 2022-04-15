using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Login_Lan1.Controllers
{
    
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin,Supper Admin")]
        public IActionResult Admin()
        {
            return View();
        }
    }

}
