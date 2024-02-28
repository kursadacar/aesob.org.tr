using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using aesob.org.tr.Models;
using aesob.org.tr.Services.Sms;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace aesob.org.tr
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
            Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<AesobDbContext>(delegate(DbContextOptionsBuilder con)
			{
#if DEBUG
				con.UseSqlServer(Configuration.GetConnectionString("AesobDebugSiteConnection"));
#else
				con.UseSqlServer(Configuration.GetConnectionString("AesobReleaseSiteConnection"));
#endif
			});
			services.AddControllers();
			services.AddRazorPages().AddRazorRuntimeCompilation();
			services.AddAuthentication("Cookies").AddCookie((CookieAuthenticationOptions x) =>
			{
				x.LoginPath = "/Login";
			});
			IConfigurationSection smsSection = Configuration.GetSection("SMSConfig");
			TTMesajService.Initialize(smsSection["SmsUserName"], smsSection["SmsPassword"], smsSection["SmsAlias"]);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseAuthentication();
			app.UseEndpoints((IEndpointRouteBuilder endpoints) =>
			{
				endpoints.MapControllers();
				endpoints.MapRazorPages();
				endpoints.MapFallback((HttpContext context) =>
				{
					context.Response.Redirect(NavigationHelper.GetNotFoundPageURL());
					return Task.CompletedTask;
				});
			});
		}
	}
}
