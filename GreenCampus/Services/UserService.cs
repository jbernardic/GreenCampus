using GreenCampus.Interfaces;
using GreenCampus.Models;
using TripManagerWebApp.Security;


namespace GreenCampus.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User Login(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user is null)
            {
                throw new Exception("Invalid email or password");
            }
            //var pwdHash = PasswordHashProvider.GetHash(password, user.Salt);
            if (user.PasswordHash != password) // Compare hashed passwords
            {
                throw new Exception("Invalid email or password");
            }
            return user;

        }

        public User Register(UserVM model)
        {
            if (_userRepository.GetByEmail(model.Email) is not null)
            {
                throw new Exception("Email already in use");
            }

            //var salt = PasswordHashProvider.GetSalt();
            //var pwdHash = PasswordHashProvider.GetHash(model.Password, salt);

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsAdmin = false,
                PasswordHash = model.Password, // Hashed password should be stored (implement hashing)

            };
            _userRepository.Add(user);
            return user;
        }
    }
}
