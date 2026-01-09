using Xunit;
using Moq;
using GreenCampus.Repositories;
using GreenCampus.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GreenCampus.Tests
{
    public class UserRepositoryTests
    {
        private Mock<DbSet<User>> CreateMockDbSet(List<User> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            return mockSet;
        }

        [Fact]
        public void GetByEmail_ExistingUser_ReturnsUser()
        {
            // Arrange
            var users = new List<User>
            {
                new User { FirstName = "Test", LastName = "User", Email = "findme@test.com", PasswordHash = "pass", IsAdmin = false }
            };
            var mockSet = CreateMockDbSet(users);

            var dbContextMock = new Mock<DatabaseContext>();
            dbContextMock.Setup(db => db.Users).Returns(mockSet.Object);

            var repo = new UserRepository(dbContextMock.Object);

            // Act
            var result = repo.GetByEmail("findme@test.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("findme@test.com", result.Email);
        }

        [Fact]
        public void GetByEmail_NonExistingUser_ReturnsNull()
        {
            // Arrange
            var users = new List<User>();
            var mockSet = CreateMockDbSet(users);

            var dbContextMock = new Mock<DatabaseContext>();
            dbContextMock.Setup(db => db.Users).Returns(mockSet.Object);

            var repo = new UserRepository(dbContextMock.Object);

            // Act
            var result = repo.GetByEmail("notfound@test.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Add_User_CallsAddAndSaveChanges()
        {
            // Arrange
            var mockSet = new Mock<DbSet<User>>();
            var dbContextMock = new Mock<DatabaseContext>();
            dbContextMock.Setup(db => db.Users).Returns(mockSet.Object);

            var repo = new UserRepository(dbContextMock.Object);
            var user = new User { FirstName = "Test", LastName = "User", Email = "test@test.com", PasswordHash = "pass", IsAdmin = false };

            // Act
            repo.Add(user);

            // Assert
            mockSet.Verify(m => m.Add(user), Times.Once);
            dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
        }
    }
}
