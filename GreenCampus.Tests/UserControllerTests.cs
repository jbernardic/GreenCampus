using Xunit;
using Moq;
using GreenCampus.Controllers;
using GreenCampus.Interfaces;
using GreenCampus.Facades;
using GreenCampus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GreenCampus.Services;

namespace GreenCampus.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<DatabaseContext> _dbMock;
        private readonly Mock<RegistrationFacade> _registrationFacadeMock;
        private readonly Mock<AuthenticationFacade> _authenticationFacadeMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _dbMock = new Mock<DatabaseContext>();
            _registrationFacadeMock = new Mock<RegistrationFacade>(MockBehavior.Strict, _userServiceMock.Object, Mock.Of<IEmailService>());

            var authServiceMock = new Mock<AuthService>(Mock.Of<IAuthStrategy>());
            _authenticationFacadeMock = new Mock<AuthenticationFacade>(MockBehavior.Strict, authServiceMock.Object);

            _controller = new UserController(
                _userServiceMock.Object,
                _dbMock.Object,
                _registrationFacadeMock.Object,
                _authenticationFacadeMock.Object
            );
        }

        [Fact]
        public void Register_Get_ReturnsView()
        {
            // Act
            var result = _controller.Register();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Register_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");
            var model = new UserVM();

            // Act
            var result = _controller.Register(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public void Register_Post_ValidModel_RedirectsToHome()
        {
            // Arrange
            var model = new UserVM { FirstName = "Test", LastName = "User", Email = "test@test.com", Password = "pass" };
            _registrationFacadeMock.Setup(f => f.RegisterUser(model));

            // Act
            var result = _controller.Register(model);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
            _registrationFacadeMock.Verify(f => f.RegisterUser(model), Times.Once);
        }

        [Fact]
        public void Register_Post_Exception_ReturnsViewWithError()
        {
            // Arrange
            var model = new UserVM { FirstName = "Test", LastName = "User", Email = "test@test.com", Password = "pass" };
            _registrationFacadeMock.Setup(f => f.RegisterUser(model)).Throws(new System.Exception("Error"));

            // Act
            var result = _controller.Register(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public void Login_Get_ReturnsView()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Login_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");

            // Act
            var result = _controller.Login("test@test.com", "pass");

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Login_Post_ValidModel_RedirectsToHome()
        {
            // Arrange
            var email = "test@test.com";
            var password = "pass";
            _authenticationFacadeMock.Setup(f => f.LoginUser(It.IsAny<HttpContext>(), email, password));

            // Act
            var result = _controller.Login(email, password);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
            _authenticationFacadeMock.Verify(f => f.LoginUser(It.IsAny<HttpContext>(), email, password), Times.Once);
        }

        [Fact]
        public void Login_Post_Exception_ReturnsViewWithError()
        {
            // Arrange
            var email = "test@test.com";
            var password = "pass";
            _authenticationFacadeMock.Setup(f => f.LoginUser(It.IsAny<HttpContext>(), email, password))
                .Throws(new System.Exception("Error"));

            // Act
            var result = _controller.Login(email, password);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }
    }
}
