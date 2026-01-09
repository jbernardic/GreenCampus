using Xunit;
using Moq;
using GreenCampus.Services;
using GreenCampus.Interfaces;
using GreenCampus.Models;
using System;

namespace GreenCampus.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "test@test.com";
            var password = "pass";
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = email,
                PasswordHash = password,
                IsAdmin = false
            };
            var strategyMock = new Mock<IAuthStrategy>();
            strategyMock.Setup(s => s.Authenticate(email, password)).Returns(user);

            var service = new AuthService(strategyMock.Object);

            // Act
            var result = service.Login(email, password);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void Login_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var email = "test@test.com";
            var password = "wrong";
            var strategyMock = new Mock<IAuthStrategy>();
            strategyMock.Setup(s => s.Authenticate(email, password)).Returns((User)null);

            var service = new AuthService(strategyMock.Object);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => service.Login(email, password));
            Assert.Equal("Invalid email or password", ex.Message);
        }
    }
}
