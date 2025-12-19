using GreenCampus.Models;

namespace GreenCampus.Interfaces
{
    public interface IUserService
    {
        User Register(UserVM model);
        User Login(string email, string password);
    }
}
