using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Services;
using MarelibuSoft.WebStore.TagHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Globalization;

namespace MarelibuSoft.WebStore
{
    public class Startup
    {
		private readonly ILogger _logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
			//_logger = logger;
        }

        public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        {
			//_logger.LogInformation("Startup.ConfigureServices -> start");

			//services.AddLogging(logging => logging.AddConfiguration(Configuration));

			services.AddDbContext<ApplicationDbContext>(options => {
				var dbconnection = Configuration.GetConnectionString("DefaultConnection");
				//_logger.LogInformation($"Startup.ConfigureServices -> Database Connection:{dbconnection}"); 
				options.UseMySql(dbconnection,
					mySqlOptionsAction =>
						{
							var mysqlserver = Configuration.GetSection("MySqlServer");
							var version = mysqlserver.GetSection("Version");
							int major = version.GetValue<int>("Major");
							int minor = version.GetValue<int>("Minor");
							int build = version.GetValue<int>("Build");

							//_logger.LogInformation($"Startup.ConfigureServices -> Use MySql Version: {major}.{minor}.{build}");

							mySqlOptionsAction.ServerVersion(new Version(major, minor, build), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)
								.CharSetBehavior(CharSetBehavior.AppendToAllColumns)
								.CharSet(CharSet.Utf8);
						});
					});

			services
				.AddDefaultIdentity<IdentityUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

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


            services.AddTransient<IEmailSender, EmailSender>();
			services.AddSingleton<IMetaService, MetaService>();
			services.AddSingleton<ITagHelperComponent, MetaDataTagHelperComponent>();

			services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc(options =>
            {
                options.SslPort = 6001;
                options.Filters.Add(new RequireHttpsAttribute());
                options.RespectBrowserAcceptHeader = true;
                options.EnableEndpointRouting = false;
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
				.AddNewtonsoftJson();
			services.AddControllers();
			services.AddControllersWithViews();
	
			services.AddHealthChecks();

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
                options.Cookie.HttpOnly = true;
			});

			//_logger.LogInformation("Startup.ConfigureServices -> end");
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			//_logger.LogInformation("Startup.Configure -> start");
			if (env.IsDevelopment() || env.IsStaging())
            {
				//_logger.LogInformation($"Startup.Configure -> env: {env.EnvironmentName}");
				app.UseDeveloperExceptionPage();
				//app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
            }
            else
            {
				//_logger.LogInformation($"Startup.Configure -> env: {env.EnvironmentName}");
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
				//_logger.LogInformation($"Startup.Configure -> supported cultures[{i}]: {supportedCultures[i].DisplayName }"); 
			}

			app.UseRequestLocalization(new RequestLocalizationOptions {
				DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultureInfo),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			app.UseStaticFiles();

			app.UseRouting();
			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //app.UseEndpoints(endpoints =>
            //{
            //	endpoints.MapControllers();
            //});

            //_logger.LogInformation("Startup.Configure -> end");

        }
	}
}
