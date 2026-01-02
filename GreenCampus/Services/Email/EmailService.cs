using GreenCampus.Interfaces;

namespace GreenCampus.Services.Email
{
    public class EmailService : IEmailService
    {
        public void SendWelcomeEmail(string toEmail)
        {
            // Implementation for sending a welcome email
            Console.WriteLine($"Welcome email sent to {toEmail}");
        }
    }
}
