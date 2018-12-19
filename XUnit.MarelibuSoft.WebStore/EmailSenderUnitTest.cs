using System;
using Xunit;
using MarelibuSoft.WebStore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Moq;

namespace XUnit.MarelibuSoft.WebStore
{
	public class EmailSenderUnitTest
	{
		IEmailSender _emailSender;
		IConfiguration _configuration;
		IHostingEnvironment _environment;

		public EmailSenderUnitTest()
		{

		}

		[Fact]
		public async void Test1()
		{
			var mock = new Mock<ILogger<EmailSender>>();
			var sender = new EmailSender(_configuration, mock.Object , _environment);
			await sender.SendEmailAsync("sburon@t-online.de", "test email", "Test Email!");
		}
	}
}
