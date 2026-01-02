using GreenCampus.Models;

namespace GreenCampus.Interfaces
{
    public interface IAuthStrategy
    {
        User? Authenticate(string email, string password);
    }
}
