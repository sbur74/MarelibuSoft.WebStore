using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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



			var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
					.AddFilter("Microsoft", LogLevel.Warning)
					.AddFilter("System", LogLevel.Warning)
					.AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
					.AddConsole()
					.AddEventLog();
			});

			var Log = loggerFactory.CreateLogger<Program>();

			try
			{
				Log.LogInformation("Starte logging!", args);
				var host = CreateHostBuilder(args, configuration).Build();
				var logger = host.Services.GetRequiredService<ILogger<Program>>();
				logger.LogInformation("From Program. Running the host now..");
				host.Run();

				return 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Log.LogError(ex, "Fehler beim erstellen des Hosts!");

				return 1;
			}
			finally
			{
				Log.LogInformation("Beende das Logging.");
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configuration) =>
				Host.CreateDefaultBuilder(args)
					.ConfigureLogging(logging =>
					{
						logging.ClearProviders()
						.AddFilter("Microsoft", LogLevel.Warning)
						.AddFilter("System", LogLevel.Warning)
						.AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
						.AddConsole()
						.AddEventLog();
					})
					.ConfigureWebHostDefaults(webBuilder =>
					{
						webBuilder
						.UseKestrel(options =>
						{
							var certificateSettings = configuration.GetSection("certificateSettings");
							string certificateFileName = certificateSettings.GetValue<string>("filename");
							string certificatePassword = certificateSettings.GetValue<string>("password");

							var certificate = new X509Certificate2(certificateFileName, certificatePassword);

							options.Listen(IPAddress.Loopback, 6001, listenOptions =>
							{
								//Log.Debug("Listen options -> start");
								listenOptions.UseHttps(certificate);

								//Log.Debug("Listen options -> end");
							});
							options.AddServerHeader = true;
						})
					.UseConfiguration(configuration)
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseIISIntegration()
					.UseStartup<Startup>()
					.UseUrls("https://localhost:6001");
					});

		//public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot config) =>
		//		WebHost.CreateDefaultBuilder(args)
		//				.UseKestrel(options =>
		//				{
		//					var certificateSettings = config.GetSection("certificateSettings");
		//					string certificateFileName = certificateSettings.GetValue<string>("filename");
		//					string certificatePassword = certificateSettings.GetValue<string>("password");

		//					var certificate = new X509Certificate2(certificateFileName, certificatePassword);

		//					options.Listen(IPAddress.Loopback, 6001, listenOptions =>
		//					{
		//						Log.Debug("Listen options -> start");
		//						listenOptions.UseHttps(certificate);

		//						Log.Debug("Listen options -> end");
		//					});
		//					options.AddServerHeader = true;
		//				})
		//				.UseConfiguration(config)
		//				.UseContentRoot(Directory.GetCurrentDirectory())
		//				.UseIISIntegration()
		//				.UseStartup<Startup>()
		//				.UseSerilog((ctx, cfg) => cfg.ReadFrom.ConfigurationSection(ctx.Configuration.GetSection("Serilog")))
		//				.UseUrls("https://localhost:6001");
	}
}
