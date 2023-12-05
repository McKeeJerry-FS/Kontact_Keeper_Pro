using Kontact_Keeper_Pro.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Kontact_Keeper_Pro.Services
{
    public class EmailService : IEmailSender
    {

		private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        #region SendEmailAsync
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			try
			{
				var emailAddress = _emailSettings.EmailAddress ?? Environment.GetEnvironmentVariable("EmailAddress");
				var emailPassword = _emailSettings.EmailPassword ?? Environment.GetEnvironmentVariable("EmailPassword");
				var emailHost = _emailSettings.EmailHost ?? Environment.GetEnvironmentVariable("EmailHost");
				var emailPort = _emailSettings.EmailPort != 0 ? _emailSettings.EmailPort : int.Parse(Environment.GetEnvironmentVariable("EmailPort")!);

				MimeMessage newEmail = new MimeMessage();
				newEmail.Sender = MailboxAddress.Parse(emailAddress);
				foreach (string address in email.Split(";"))
				{
					newEmail.To.Add(MailboxAddress.Parse(address));
				}

				// Set the subject
				newEmail.Subject = subject;

				// set the message
				BodyBuilder emailBody = new BodyBuilder();
				emailBody.HtmlBody = htmlMessage;
				newEmail.Body = emailBody.ToMessageBody();

				// send the email

				using SmtpClient smtpClient = new();
				try
				{
                    await smtpClient.ConnectAsync(emailHost, emailPort, SecureSocketOptions.StartTls);
					await smtpClient.AuthenticateAsync(emailAddress, emailPassword);
					await smtpClient.SendAsync(newEmail);

					await smtpClient.DisconnectAsync(true);

					// For testing - comment out later
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("****************** SUCCESS *****************");
                    Console.WriteLine($"Email Successfully sent!!!!!!");
                    Console.WriteLine("****************** SUCCESS *****************");
                    Console.ResetColor();
                }
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("****************** ERROR *****************");
					Console.WriteLine($"Failure sending email with Google Provider Error: {ex.Message}");
                    Console.WriteLine("****************** ERROR *****************");
					Console.ResetColor();

                    throw;
				}



			}
			catch (Exception)
			{

				throw;
			}
		}

		#endregion    
	}
}
