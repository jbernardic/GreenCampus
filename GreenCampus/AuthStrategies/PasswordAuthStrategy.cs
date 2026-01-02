using GreenCampus.Interfaces;
using GreenCampus.Models;

namespace GreenCampus.AuthStrategies
{
    public class PasswordAuthStrategy : IAuthStrategy
    {
        private readonly IUserRepository _userRepository;

        public PasswordAuthStrategy(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Authenticate(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user != null && user.PasswordHash == password) // Dodaj hashiranje u praksi!
                return user;
            return null;
        }
    }
}
