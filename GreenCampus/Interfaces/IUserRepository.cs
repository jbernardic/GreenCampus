using GreenCampus.Models;

namespace GreenCampus.Interfaces
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        void Add(User user);
    }
}
