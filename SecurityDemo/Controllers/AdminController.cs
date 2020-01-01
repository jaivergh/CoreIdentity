using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecurityDemo.Areas.Identity.Pages.Account.Models;

namespace SecurityDemo.Controllers
{
    [Authorize(Policy = ApplicationPolicy.BigCheese)]
    public class AdminController : Controller
    {
        private UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Claims = await _userManager.GetClaimsAsync(user);
            return View(user);
        }
    }
}