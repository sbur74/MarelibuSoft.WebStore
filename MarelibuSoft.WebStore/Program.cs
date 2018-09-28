using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace MarelibuSoft.WebStore
{
	public class Program
	{
		
		public static int Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appsettings.json")
						.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration.GetSection("Serilog"))
				.Enrich.FromLogContext()
				.CreateLogger();

			try
			{
				Log.Information("Starting web host");
				CreateWebHostBuilder(args, configuration).Build().Run();
				return 0;
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Host terminated unexpectedly");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot config) =>
			WebHost.CreateDefaultBuilder(args)
					.UseKestrel(options =>
					{
						var certificateSettings = config.GetSection("certificateSettings");
						string certificateFileName = certificateSettings.GetValue<string>("filename");
						string certificatePassword = certificateSettings.GetValue<string>("password");

						var certificate = new X509Certificate2(certificateFileName, certificatePassword);
						
						options.Listen(IPAddress.Loopback, 5001, listenOptions =>
						{
							Log.Debug("Listen options -> start");
							listenOptions.UseHttps(certificate);

							Log.Debug("Listen options -> end");
						});
						options.AddServerHeader = true;
					})
					.UseConfiguration(config)
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseStartup<Startup>()
					.UseSerilog()
					.UseUrls("https://localhost:5001");
	}
}
