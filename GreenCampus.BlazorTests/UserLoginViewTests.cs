using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GreenCampus.Tests
{
    [TestFixture]
    public class UserLoginViewTests
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
        public async Task LoginPage_ReturnsHtmlWithForm()
        {
            // Act
            var response = await _client.GetAsync("/User/Login");
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(html, Does.Contain("form"));
            Assert.That(html, Does.Contain("Enter your email"));
            Assert.That(html, Does.Contain("Enter your password"));
            Assert.That(html, Does.Contain("Sign In"));
            Assert.That(html, Does.Contain("Create Account"));
        }
    }
}

