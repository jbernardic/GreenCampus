using GreenCampus.Models;

namespace GreenCampus.Interfaces
{
    public interface IUserFactory
    {
        User CreateUser(UserVM model, bool isAdmin = false);
    }
}
