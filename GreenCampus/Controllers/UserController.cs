using GreenCampus.Interfaces;
using GreenCampus.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenCampus.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly DatabaseContext db;

        public UserController(IUserService userService, DatabaseContext db)
        {
            _userService = userService;
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserVM model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _userService.Register(model);
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }


        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (!ModelState.IsValid) return View();
            try
            {
                var user = _userService.Login(email, password);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
                };
                var claimIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                var task = Task.Run(() =>
                {
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity),
                    authProperties);
                });
                task.GetAwaiter().GetResult();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}
