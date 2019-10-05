using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
		Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<string> Attachments, string bcc);

	}
}
