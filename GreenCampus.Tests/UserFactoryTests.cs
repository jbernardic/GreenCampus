using Xunit;
using GreenCampus.Factories;
using GreenCampus.Models;

namespace GreenCampus.Tests
{
    public class UserFactoryTests
    {
        [Fact]
        public void CreateUser_MapsAllPropertiesCorrectly()
        {
            // Arrange
            var model = new UserVM
            {
                FirstName = "Ana",
                LastName = "Novak",
                Email = "ana@proba.com",
                Password = "tajna"
            };
            var factory = new UserFactory();

            // Act
            var user = factory.CreateUser(model, true);

            // Assert
            Assert.Equal(model.FirstName, user.FirstName);
            Assert.Equal(model.LastName, user.LastName);
            Assert.Equal(model.Email, user.Email);
            Assert.Equal(model.Password, user.PasswordHash);
            Assert.True(user.IsAdmin);
        }

        [Fact]
        public void CreateUser_DefaultIsAdminFalse()
        {
            // Arrange
            var model = new UserVM
            {
                FirstName = "Ivan",
                LastName = "Ivic",
                Email = "ivan@proba.com",
                Password = "lozinka"
            };
            var factory = new UserFactory();

            // Act
            var user = factory.CreateUser(model);

            // Assert
            Assert.False(user.IsAdmin);
        }
    }
}
