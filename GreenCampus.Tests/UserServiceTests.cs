using Xunit;
using Moq;
using GreenCampus.Services;
using GreenCampus.Interfaces;
using GreenCampus.Models;
using System;

namespace GreenCampus.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "test@test.com";
            var password = "password123";
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = email,
                PasswordHash = password,
                IsAdmin = false
            };
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByEmail(email)).Returns(user);
            var userFactoryMock = new Mock<IUserFactory>();
            var service = new UserService(userRepoMock.Object, userFactoryMock.Object);

            // Act
            var result = service.Login(email, password);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void Login_InvalidEmail_ThrowsException()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByEmail(It.IsAny<string>())).Returns((User)null);
            var userFactoryMock = new Mock<IUserFactory>();
            var service = new UserService(userRepoMock.Object, userFactoryMock.Object);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => service.Login("notfound@test.com", "password"));
            Assert.Equal("Invalid email or password", ex.Message);
        }

        [Fact]
        public void Login_InvalidPassword_ThrowsException()
        {
            // Arrange
            var email = "test@test.com";
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = email,
                PasswordHash = "correct",
                IsAdmin = false
            };
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByEmail(email)).Returns(user);
            var userFactoryMock = new Mock<IUserFactory>();
            var service = new UserService(userRepoMock.Object, userFactoryMock.Object);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => service.Login(email, "wrong"));
            Assert.Equal("Invalid email or password", ex.Message);
        }

        [Fact]
        public void Register_ValidModel_AddsUserAndReturnsUser()
        {
            // Arrange
            var model = new UserVM { FirstName = "Test", LastName = "User", Email = "new@test.com", Password = "pass" };
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password,
                IsAdmin = false
            };
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByEmail(model.Email)).Returns((User)null);
            var userFactoryMock = new Mock<IUserFactory>();
            userFactoryMock.Setup(f => f.CreateUser(model, false)).Returns(user);
            var service = new UserService(userRepoMock.Object, userFactoryMock.Object);

            // Act
            var result = service.Register(model);

            // Assert
            userRepoMock.Verify(r => r.Add(user), Times.Once);
            Assert.Equal(user, result);
        }

        [Fact]
        public void Register_EmailAlreadyExists_ThrowsException()
        {
            // Arrange
            var model = new UserVM { FirstName = "Test", LastName = "User", Email = "exists@test.com", Password = "pass" };
            var existingUser = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = model.Email,
                PasswordHash = "pass",
                IsAdmin = false
            };
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByEmail(model.Email)).Returns(existingUser);
            var userFactoryMock = new Mock<IUserFactory>();
            var service = new UserService(userRepoMock.Object, userFactoryMock.Object);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => service.Register(model));
            Assert.Equal("Email already in use", ex.Message);
        }
    }
}
