using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Services;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Threading;
using System.IO;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MarelibuSoft.WebStore.TagHelpers;

namespace MarelibuSoft.WebStore
{
    public class Startup
    {
		private readonly ILogger _logger;
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
			_logger = logger;
        }

        public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        {
			_logger.LogInformation("Startup.ConfigureServices -> start");

			services.AddDbContext<ApplicationDbContext>(options => {
				var dbconnection = Configuration.GetConnectionString("DefaultConnection");
				_logger.LogInformation($"Startup.ConfigureServices -> Database Connection:{dbconnection}"); 
				options.UseMySql(dbconnection,
					mySqlOptionsAction =>
						{
							var mysqlserver = Configuration.GetSection("MySqlServer");
							var version = mysqlserver.GetSection("Version");
							int major = version.GetValue<int>("Major");
							int minor = version.GetValue<int>("Minor");
							int build = version.GetValue<int>("Build");

							_logger.LogInformation($"Startup.ConfigureServices -> Use MySql Version: {major}.{minor}.{build}");

							mySqlOptionsAction.ServerVersion(new Version(major, minor, build), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)
								.CharSetBehavior(CharSetBehavior.AppendToAllColumns)
								.AnsiCharSet(CharSet.Latin1)
								.UnicodeCharSet(CharSet.Utf8mb4);
						});
					});

			services.Configure<IdentityOptions>(options =>
			{
				// Default Password settings.
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 0;
			});

			services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

			services.AddTransient<IEmailSender, EmailSender>();
			services.AddSingleton<IMetaService, MetaService>();
			services.AddSingleton<ITagHelperComponent, MetaDataTagHelperComponent>();

			services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
			services.AddMvc(options =>
			{
				options.SslPort = 6001;
				options.Filters.Add(new RequireHttpsAttribute());
				options.RespectBrowserAcceptHeader = true;
			})
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddHttpsRedirection(options =>
			{
				options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
				options.HttpsPort = 6001;
			});

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				var expiration = TimeSpan.FromMinutes(25);
				options.IdleTimeout = expiration;
			});

			_logger.LogInformation("Startup.ConfigureServices -> end");
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			_logger.LogInformation("Startup.Configure -> start");
			if (env.IsDevelopment() || env.IsStaging())
            {
				_logger.LogInformation($"Startup.Configure -> env: {env.EnvironmentName}");
				app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
				_logger.LogInformation($"Startup.Configure -> env: {env.EnvironmentName}");
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			var cultureInfo = new CultureInfo("de-DE");
			cultureInfo.NumberFormat.CurrencySymbol = "€";

			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

			var supportedCultures = new[] { cultureInfo };
			for (int i = 0; i < supportedCultures.Length; i++)
			{
				_logger.LogInformation($"Startup.Configure -> supported cultures[{i}]: {supportedCultures[i].DisplayName }"); 
			}

			app.UseRequestLocalization(new RequestLocalizationOptions {
				DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultureInfo),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			app.UseStaticFiles();

			app.UseAuthentication();
			
			app.UseSession();
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "areaRoute",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
					);

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			_logger.LogInformation("Startup.Configure -> end");

		}
	}
}
