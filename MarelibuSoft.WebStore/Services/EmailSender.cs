using MarelibuSoft.WebStore.Common.Statics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;
        private const string FILESFOLDER = "files";
        private MailboxAddress senderAddress;
        private string signature = string.Empty;
        private string user = string.Empty;
        private string password = string.Empty;
        private string hostname = string.Empty;
        private string path = string.Empty;
        private int hostport = 0;


        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _logger = logger;
            _environment = environment;
            signature = StaticEmailSignature.GetEmailSignature();
            senderAddress = new MailboxAddress(_configuration["Email:Email"]);
            user = _configuration["Email:Email"];
            password = _configuration["Email:Password"];
            hostname = _configuration["Email:Host"];
            hostport = int.Parse(_configuration["Email:Port"]);
            path = Path.Combine(_environment.WebRootPath, FILESFOLDER);
            path = Path.Combine(_environment.WebRootPath, FILESFOLDER);
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var emailmessage = new MimeMessage();
            var bodyBuilder = new BodyBuilder();

            emailmessage.From.Add(senderAddress);
            emailmessage.To.Add(new MailboxAddress(email));
            emailmessage.Subject = subject;
            string contenten = CreateContent(message);

            bodyBuilder.HtmlBody = contenten;
            emailmessage.Body = bodyBuilder.ToMessageBody();

            return TrySendMail(emailmessage);
        }

        private string CreateContent(string message)
        {
            return message + signature;
        }

        private Task TrySendMail(MimeMessage emailmessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(hostname, port: hostport, options: SecureSocketOptions.StartTlsWhenAvailable);
                    client.Authenticate(user, password);
                    _logger.LogInformation("SendEmailAsync -> try send email");
                    client.Send(emailmessage);
                    client.Disconnect(true);
                    _logger.LogInformation("SendEmailAsync ->sending completet");
                    return Task.CompletedTask;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "SendEmailAsync -> Error on sending E-Mail", null);
                    return Task.CompletedTask;
                }
            }
        }

        public Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<string> Attachments,string bcc)
		{

            var emailmessage = new MimeMessage();
            var bodyBuilder = new BodyBuilder();

            emailmessage.From.Add(senderAddress);
            emailmessage.To.Add(new MailboxAddress(email));
            emailmessage.Subject = subject;

            if (!string.IsNullOrWhiteSpace(bcc))
            {
                emailmessage.Bcc.Add(new MailboxAddress(bcc));
            }

            if (Attachments != null && Attachments.Count > 0)
            {
                foreach (string attachment in Attachments)
                {
                    bodyBuilder.Attachments.Add(Path.Combine(path, attachment));
                } 
            }

            string contenten = CreateContent(message);

            bodyBuilder.HtmlBody = contenten;
            emailmessage.Body = bodyBuilder.ToMessageBody();

            return TrySendMail(emailmessage);
        }
	}
}
