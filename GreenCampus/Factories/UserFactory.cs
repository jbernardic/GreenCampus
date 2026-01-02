using GreenCampus.Interfaces;
using GreenCampus.Models;

namespace GreenCampus.Factories
{
    public class UserFactory : IUserFactory
    {
        public User CreateUser(UserVM model, bool isAdmin = false)
        {
            return new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password, // In a real application, ensure to hash the password
                IsAdmin = isAdmin
            };
        }
    }
}
