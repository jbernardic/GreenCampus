using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GreenCampus.Tests
{
    [TestFixture]
    public class GreenActivityCreateViewTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task CreateActivityPage_ReturnsHtmlWithForm()
        {
            // Act
            var response = await _client.GetAsync("/GreenActivities/Create");
            var html = await response.Content.ReadAsStringAsync();

            // Assert
        }
    }
}