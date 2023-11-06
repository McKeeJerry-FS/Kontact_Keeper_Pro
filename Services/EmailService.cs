using Microsoft.AspNetCore.Identity.UI.Services;

namespace Kontact_Keeper_Pro.Services
{
    public class EmailService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
