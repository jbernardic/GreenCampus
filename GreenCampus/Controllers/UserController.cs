using GreenCampus.Interfaces;
using GreenCampus.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GreenCampus.Facades;

namespace GreenCampus.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly DatabaseContext db;
        private readonly RegistrationFacade _registrationFacade;
        private readonly AuthenticationFacade _authenticationFacade;

        public UserController(IUserService userService, DatabaseContext db, RegistrationFacade registrationFacade, AuthenticationFacade authenticationFacade)
        {
            _userService = userService;
            this.db = db;
            _registrationFacade = registrationFacade;
            _authenticationFacade = authenticationFacade;
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
                _registrationFacade.RegisterUser(model);
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
                _authenticationFacade.LoginUser(HttpContext, email, password);
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
