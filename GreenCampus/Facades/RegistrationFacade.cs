using GreenCampus.Interfaces;
using GreenCampus.Models;

namespace GreenCampus.Facades
{
    public class RegistrationFacade
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public RegistrationFacade(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public void RegisterUser(UserVM model)
        {
            var user = _userService.Register(model);
            _emailService.SendWelcomeEmail(user.Email);
        }
    }
}
