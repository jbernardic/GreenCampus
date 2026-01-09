using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GreenCampus.Tests
{
    [TestFixture]
    public class UserRegisterViewTests
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
        public async Task RegisterPage_ReturnsHtmlWithForm()
        {
            // Act
            var response = await _client.GetAsync("/User/Register");
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(html, Does.Contain("form"));
            Assert.That(html, Does.Contain("Enter your first name"));
            Assert.That(html, Does.Contain("Enter your last name"));
            Assert.That(html, Does.Contain("your.email@university.edu"));
            Assert.That(html, Does.Contain("Create a strong password"));
            Assert.That(html, Does.Contain("Create Account"));
            Assert.That(html, Does.Contain("Sign In"));
        }
    }
}
