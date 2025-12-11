using GreenCampus.Models;
using Microsoft.AspNetCore.Mvc;

namespace GreenCampus.Controllers
{
    public class UserController : Controller
    {
        DatabaseContext db;

        public UserController(DatabaseContext databaseContext)
        {
            db = databaseContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
