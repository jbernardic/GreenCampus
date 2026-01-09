using GreenCampus.Interfaces;
using GreenCampus.Models;

namespace GreenCampus.Services
{
    public class AuthService
    {
        private readonly IAuthStrategy _strategy;

        public AuthService(IAuthStrategy strategy)
        {
            _strategy = strategy;
        }

        public virtual User Login(string email, string password)
        {
            var user = _strategy.Authenticate(email, password);
            if (user == null)
                throw new Exception("Invalid email or password");
            return user;
        }
    }
}
