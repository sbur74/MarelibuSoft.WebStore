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

namespace MarelibuSoft.WebStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			//services.Configure<CookiePolicyOptions>(options =>
			//{
			//	// This lambda determines whether user consent for non-essential cookies 
			//	// is needed for a given request.
			//	options.CheckConsentNeeded = context => true;
			//	options.MinimumSameSitePolicy = SameSiteMode.None;
			//});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
					mySqlOptionsAction =>
					{
						mySqlOptionsAction.ServerVersion(new Version(8, 0, 11), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
					}));

			services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

			//services.AddDefaultIdentity<IdentityUser>()
			//	.AddEntityFrameworkStores<ApplicationDbContext>();

			// Add application services.
			services.AddTransient<IEmailSender, EmailSender>();

			services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddDistributedMemoryCache();
			services.AddSession();
			//services.AddTransient<IEmailSender, AuthMessageSender>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			var cultureInfo = new CultureInfo("de-DE");
			cultureInfo.NumberFormat.CurrencySymbol = "€";

			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

			var supportedCultures = new[] { cultureInfo };

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
			//app.UseCookiePolicy();

			app.UseMvc(routes =>
            {
				/*
								 * routes.MapRoute
						(
							name: "Detail",
							url: "{controller}/{id}",
							defaults: new { action = "Details", id = UrlParameter.Optional   },
							constraints: new { id = "[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}" }
						);

					routes.MapRoute(
						name: "Default",
						url: "{controller}/{action}/{id}",
						defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
					);
					*/

				routes.MapRoute(
					name: "areaRoute",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
					);
				routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
