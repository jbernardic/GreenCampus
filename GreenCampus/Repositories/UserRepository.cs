using GreenCampus.Interfaces;
using GreenCampus.Models;

namespace GreenCampus.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _db;
        public UserRepository(DatabaseContext db)
        {
            _db = db;
        }
        public void Add(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User? GetByEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
