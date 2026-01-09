using Xunit;
using Moq;
using GreenCampus.Facades;
using GreenCampus.Services;
using GreenCampus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GreenCampus.Tests
{
    public class AuthenticationFacadeTests
    {
        private readonly AuthenticationFacade _facade;
        private readonly Mock<AuthService> _authServiceMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private bool _signInAsyncCalled;

        public AuthenticationFacadeTests()
        {
            _authServiceMock = new Mock<AuthService>(MockBehavior.Strict, new object[] { null! });
            _httpContextMock = new Mock<HttpContext>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _signInAsyncCalled = false;

            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(_authenticationServiceMock.Object);

            _httpContextMock.SetupGet(c => c.RequestServices).Returns(_serviceProviderMock.Object);

            _authenticationServiceMock
                .Setup(a => a.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()))
                .Callback(() => _signInAsyncCalled = true)
                .Returns(Task.CompletedTask);

            _facade = new AuthenticationFacade(_authServiceMock.Object);
        }

        [Fact]
        public void LoginUser_WithValidCredentials_CallsSignInAsync()
        {
            // Arrange (Given)
            var email = "proba@proba.com";
            var password = "pass123";
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = email,
                PasswordHash = password,
                IsAdmin = false
            };
            _authServiceMock.Setup(s => s.Login(email, password)).Returns(user);

            // Act (When)
            _facade.LoginUser(_httpContextMock.Object, email, password);

            // Assert (Then)
            _authServiceMock.Verify(s => s.Login(email, password), Times.Once);
            Assert.True(_signInAsyncCalled);
        }
    }
}
