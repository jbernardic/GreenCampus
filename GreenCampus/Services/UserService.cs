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
                throw new InvalidOperationException("Invalid email or password");
            }

            if (user.PasswordHash != password) // Compare hashed passwords
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            return user;
        }


        public User Register(UserVM model)
        {
            if (_userRepository.GetByEmail(model.Email) is not null)
            {
                throw new InvalidOperationException("Email already in use");
            }

            var user = _userFactory.CreateUser(model, false);
            _userRepository.Add(user);
            return user;
        }

    }
}
