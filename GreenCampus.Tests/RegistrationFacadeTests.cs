using Xunit;
using Moq;
using GreenCampus.Facades;
using GreenCampus.Interfaces;
using GreenCampus.Models;

namespace GreenCampus.Tests
{
    public class RegistrationFacadeTests
    {
        private readonly RegistrationFacade _facade;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public RegistrationFacadeTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _emailServiceMock = new Mock<IEmailService>();
            _facade = new RegistrationFacade(_userServiceMock.Object, _emailServiceMock.Object);
        }

        [Fact]
        public void RegisterUser_ValidModel_CallsRegisterAndSendWelcomeEmail()
        {
            // Arrange (Given)
            var model = new UserVM
            {
                FirstName = "Test",
                LastName = "User",
                Email = "proba@proba.com",
                Password = "pass123"
            };
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password,
                IsAdmin = false
            };
            _userServiceMock.Setup(s => s.Register(model)).Returns(user);

            // Act (When)
            _facade.RegisterUser(model);

            // Assert (Then)
            _userServiceMock.Verify(s => s.Register(model), Times.Once);
            _emailServiceMock.Verify(e => e.SendWelcomeEmail(user.Email), Times.Once);
        }
    }
}
