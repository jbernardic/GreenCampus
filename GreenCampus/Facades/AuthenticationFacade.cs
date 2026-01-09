using GreenCampus.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using GreenCampus.Services;

namespace GreenCampus.Facades
{
    public class AuthenticationFacade
    {
        private readonly AuthService _authService;

        public AuthenticationFacade(AuthService authService)
        {
            _authService = authService;
        }

        public virtual void LoginUser(HttpContext httpContext, string email, string password)
        {
            var user = _authService.Login(email, password);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };
            var claimIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity),
                authProperties).GetAwaiter().GetResult();
        }
    }
}
