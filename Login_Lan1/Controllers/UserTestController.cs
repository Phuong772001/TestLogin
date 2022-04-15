using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login_Lan1.Models;
using Login_Lan1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Login_Lan1.Controllers
{
    [Authorize(Roles = "Admin,Supper Admin")]
    public class UserTestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserTestController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var users = _userManager.Users;
            if (users != null && users.Any())
            {
                var model = new List<UserViewModel>();
                model = users.Select(u => new UserViewModel()
                {
                    UserId = u.Id,
                    Address = u.Address,
                    City = u.City,
                    Email = u.Email
                }).ToList();
                foreach (var user in model)
                {
                    user.RoleName = GetRoleName(user.UserId);
                }
                return View(model);
            }

            return View();

        }

        public string GetRoleName(string userId)
        {

            var user = _userManager.FindByIdAsync(userId).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            return roles != null ? string.Join(",", roles) : string.Empty;

        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Address = model.Address,
                    City = model.City,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.RoleId))
                    {
                        var role = await _roleManager.FindByIdAsync(model.RoleId);
                        var addRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                        if (addRoleResult.Succeeded)
                        {
                            return RedirectToAction("Index", "UserTest");
                        }

                        foreach (var error in addRoleResult.Errors)
                        {
                            ModelState.AddModelError("lỗi rồi", error.Description);
                        }
                    }

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("lỗi rồi", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var model = new EditUserViewModel()
                {
                    Address = user.Address,
                    City = user.City,
                    Email = user.Email,
                    UserId = user.Id
                };
                var roleName = await _userManager.GetRolesAsync(user);
                if (roleName != null && roleName.Any())
                {
                    var role = await _roleManager.FindByNameAsync(roleName.FirstOrDefault());
                    model.RoleId = role.Id;
                }
                ViewBag.Roles = _roleManager.Roles;
                return View(model);
            }
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.Id = model.UserId;
                    user.City = model.City;
                    user.Address = model.Address;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var rolesName = await _userManager.GetRolesAsync(user);
                       
                        if (!string.IsNullOrEmpty(model.RoleId))
                        {
                            var delRole = await _userManager.RemoveFromRoleAsync(user, rolesName.ToString());
                            var role = await _roleManager.FindByIdAsync(model.RoleId);
                            var addRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                            if (addRoleResult.Succeeded)
                            {
                                return RedirectToAction("Index", "UserTest");
                            }

                            foreach (var error in addRoleResult.Errors)
                            {
                                ModelState.AddModelError("lỗi rồi", error.Description);
                            }
                        }

                        return RedirectToAction("Index", "UserTest");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }


            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var delRole = await _userManager.FindByIdAsync(id);
            if (delRole != null)
            {
                var result = await _userManager.DeleteAsync(delRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "UserTest");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
    }
}