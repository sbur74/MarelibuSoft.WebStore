using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
		private IConfiguration _configuration;
		private readonly ILogger _logger;
		public EmailSender(IConfiguration configuration, ILogger<EmailSender>logger)
		{
			_configuration = configuration;
			_logger = logger;
		}
        public Task SendEmailAsync(string email, string subject, string message)
        {
			using (var client = new SmtpClient())
			{
				var credential = new NetworkCredential {
					UserName = _configuration["Email:Email"],
					Password = _configuration["Email:Password"]
				};

				client.Credentials = credential;
				client.Host = _configuration["Email:Host"];
				client.Port = int.Parse(_configuration["Email:Port"]);
				client.EnableSsl = true;

				using (var emailMessage = new MailMessage())
				{
					emailMessage.IsBodyHtml = true;
					emailMessage.To.Add(new MailAddress(email));
					emailMessage.From = new MailAddress(_configuration["Email:Email"]);
					emailMessage.Subject = subject;
					emailMessage.Body = message;

					ServicePointManager.ServerCertificateValidationCallback =
						delegate (object s, X509Certificate certificate,
								 X509Chain chain, SslPolicyErrors sslPolicyErrors)
						{ return true; };

					//try
					//{
					_logger.LogDebug("Try send EMail");
						client.Send(emailMessage);
					//}
					//catch (Exception e)
					//{
					//	_logger.LogError(e, "Error on sending E-Mail", null);
					//	throw e;
					//}
				}
			}
            return Task.CompletedTask;
        }
    }
}
