using Xunit;
using GreenCampus.Services.Email;

namespace GreenCampus.Tests
{
    public class EmailServiceTests
    {
        [Fact]
        public void SendWelcomeEmail_DoesNotThrow()
        {
            // Arrange
            var service = new EmailService();

            // Act & Assert
            var exception = Record.Exception(() => service.SendWelcomeEmail("test@test.com"));
            Assert.Null(exception);
        }
    }
}
