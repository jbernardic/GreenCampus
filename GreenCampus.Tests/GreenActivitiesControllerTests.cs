using Xunit;
using Moq;
using GreenCampus.Controllers;
using GreenCampus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCampus.Tests
{
    public class GreenActivitiesControllerTests
    {
        private Mock<DbSet<GreenActivity>> CreateMockDbSet(List<GreenActivity> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<GreenActivity>>();
            mockSet.As<IQueryable<GreenActivity>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<GreenActivity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<GreenActivity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<GreenActivity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            return mockSet;
        }

        [Fact]
        public async Task Index_ReturnsViewWithActivities()
        {
            // Arrange
            var activities = new List<GreenActivity>
            {
                new GreenActivity { GreenActivityId = 1, Name = "A", Description = "Desc" }
            };
            var mockSet = CreateMockDbSet(activities);
            var dbContextMock = new Mock<DatabaseContext>();
            dbContextMock.Setup(db => db.GreenActivities).Returns(mockSet.Object);

            var controller = new GreenActivitiesController(dbContextMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(activities, viewResult.Model);
        }

        [Fact]
        public async Task Details_NullId_ReturnsNotFound()
        {
            var dbContextMock = new Mock<DatabaseContext>();
            var controller = new GreenActivitiesController(dbContextMock.Object);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_NonExistingId_ReturnsNotFound()
        {
            var mockSet = CreateMockDbSet(new List<GreenActivity>());
            var dbContextMock = new Mock<DatabaseContext>();
            dbContextMock.Setup(db => db.GreenActivities).Returns(mockSet.Object);

            var controller = new GreenActivitiesController(dbContextMock.Object);

            var result = await controller.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ExistingId_ReturnsViewWithActivity()
        {
            var activity = new GreenActivity { GreenActivityId = 1, Name = "A", Description = "Desc" };
            var mockSet = CreateMockDbSet(new List<GreenActivity> { activity });
            var dbContextMock = new Mock<DatabaseContext>();
            dbContextMock.Setup(db => db.GreenActivities).Returns(mockSet.Object);

            var controller = new GreenActivitiesController(dbContextMock.Object);

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(activity, viewResult.Model);
        }

        [Fact]
        public void Create_Get_ReturnsView()
        {
            var dbContextMock = new Mock<DatabaseContext>();
            var controller = new GreenActivitiesController(dbContextMock.Object);

            var result = controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var mockSet = new Mock<DbSet<GreenActivity>>();
            var dbContextMock = new Mock<DatabaseContext>();
            dbContextMock.Setup(db => db.GreenActivities).Returns(mockSet.Object);

            var controller = new GreenActivitiesController(dbContextMock.Object);
            var activity = new GreenActivity { Name = "A", Description = "Desc" };

            controller.ModelState.Clear(); // osiguraj da je ModelState ispravan

            var result = await controller.Create(activity);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            var dbContextMock = new Mock<DatabaseContext>();
            var controller = new GreenActivitiesController(dbContextMock.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Inicijaliziraj required property-je s praznim stringovima
            var activity = new GreenActivity { Name = "", Description = "" };

            var result = await controller.Create(activity);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(activity, viewResult.Model);
        }
    }
}
