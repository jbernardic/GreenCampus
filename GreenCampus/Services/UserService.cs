using GreenCampus.Interfaces;
using GreenCampus.Models;
using TripManagerWebApp.Security;


namespace GreenCampus.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        public UserService(IUserRepository userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
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

            var user = _userFactory.CreateUser(model, false);
            _userRepository.Add(user);
            return user;
        }
    }
}
