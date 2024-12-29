using Microsoft.AspNetCore.Identity.UI.Services;

namespace TopSpeed.Application.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;

            //Go and register in the program.cs
        }
    }
}
