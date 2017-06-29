using System.Threading.Tasks;
using BookReader.Data.Helpers;
using BookReader.Data.Repositories.Abstract;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BookReader.Data.Repositories
{
	public class EmailSender : IEmailSender
	{
		public EmailSender(IOptions<EmailSettings> emailSettings)
		{
			EmailSettings = emailSettings.Value;
		}

		public EmailSettings EmailSettings { get; }

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var mimeMessage = new MimeMessage();
			mimeMessage.From.Add(new MailboxAddress(EmailSettings.UsernameEmail));
			mimeMessage.To.Add(new MailboxAddress(email));
			mimeMessage.Subject = subject;
			mimeMessage.Body = new TextPart("plain") { Text = message };

			using (var smtp = new SmtpClient())
			{
				smtp.Connect(EmailSettings.Domain, EmailSettings.Port, false);
				smtp.Authenticate(EmailSettings.UsernameEmail, EmailSettings.UsernamePassword);
				await smtp.SendAsync(mimeMessage).ConfigureAwait(false);
				await smtp.DisconnectAsync(true).ConfigureAwait(false);
			}
		}
	}
}
