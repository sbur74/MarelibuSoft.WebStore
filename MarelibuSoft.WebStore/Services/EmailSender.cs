using MarelibuSoft.WebStore.Common.Statics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
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
		private readonly IHostingEnvironment _environment;
		private const string FILESFOLDER = "files";

		public EmailSender(IConfiguration configuration, ILogger<EmailSender>logger, IHostingEnvironment environment)
		{
			_configuration = configuration;
			_logger = logger;
			_environment = environment;
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

					message += StaticEmailSignature.GetEmailSignature();
					emailMessage.IsBodyHtml = true;
					emailMessage.To.Add(new MailAddress(email));
					emailMessage.Bcc.Add(new MailAddress("petra@marelibudesign.de"));
					emailMessage.Bcc.Add(new MailAddress("pburon@t-online.de"));
					emailMessage.From = new MailAddress(_configuration["Email:Email"]);
					emailMessage.Subject = subject;
					emailMessage.Body = message;

					ServicePointManager.ServerCertificateValidationCallback =
						delegate (object s, X509Certificate certificate,
								 X509Chain chain, SslPolicyErrors sslPolicyErrors)
						{ return true; };

					try
					{
						_logger.LogDebug("Try send EMail");
						client.Send(emailMessage);
					}
					catch (Exception e)
					{
						_logger.LogError(e, "Error on sending E-Mail", null);
					}
				}
			}
            return Task.CompletedTask;
        }

		public Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<string> Attachments)
		{
			using (var client = new SmtpClient())
			{
				var credential = new NetworkCredential
				{
					UserName = _configuration["Email:Email"],
					Password = _configuration["Email:Password"]
				};

				client.Credentials = credential;
				client.Host = _configuration["Email:Host"];
				client.Port = int.Parse(_configuration["Email:Port"]);
				client.EnableSsl = true;

			

				using (var emailMessage = new MailMessage())
				{
					message += StaticEmailSignature.GetEmailSignature();
					emailMessage.IsBodyHtml = true;
					emailMessage.To.Add(new MailAddress(email));
					emailMessage.Bcc.Add(new MailAddress("petra@marelibudesign.de"));
					emailMessage.Bcc.Add(new MailAddress("service@marelibudesign.de"));
					emailMessage.Bcc.Add(new MailAddress("pburon@t-online.de"));
					emailMessage.From = new MailAddress(_configuration["Email:Email"]);
					emailMessage.Subject = subject;
					emailMessage.Body = message;

					foreach (string file in Attachments)
					{

						string path = Path.Combine(_environment.WebRootPath, FILESFOLDER);
						string filepath = Path.Combine(path, file);
						Attachment data = new Attachment(filepath, MediaTypeNames.Application.Octet);
						// Add time stamp information for the file.
						ContentDisposition disposition = data.ContentDisposition;
						disposition.CreationDate = System.IO.File.GetCreationTime(filepath);
						disposition.ModificationDate = System.IO.File.GetLastWriteTime(filepath);
						disposition.ReadDate = System.IO.File.GetLastAccessTime(filepath);
						emailMessage.Attachments.Add(data);
					}

					ServicePointManager.ServerCertificateValidationCallback =
						delegate (object s, X509Certificate certificate,
								 X509Chain chain, SslPolicyErrors sslPolicyErrors)
						{ return true; };

					try
					{
						_logger.LogDebug("Try send EMail");
						client.Send(emailMessage);
					}
					catch (Exception e)
					{
						_logger.LogError(e, "Fehler beim E-Mail Versand:", null);
					}
				}
			}
			return Task.CompletedTask;
		}
	}
}
