using BookReader.Data.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookReader.Data.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using BookReader.Web.Helpers;

namespace BookReader.Web
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile("appsettings.local.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<BookReaderDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookReaderConnection")));
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IAuthorRepository, AuthorRepository>();

			// Add framework services.
			services.AddMvc();

			services.AddAuthorization(options =>
			{
				options.AddPolicy(BookReaderPolicies.AdminPolicy, policy => policy.RequireRole(BookReaderRoles.Admin));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationScheme = "Cookies",
				CookieName = ".BookReaderAuth",
				ExpireTimeSpan = TimeSpan.FromDays(60),
				LoginPath = new PathString("/User/Login"),
				AccessDeniedPath = new PathString("/Home/Forbidden"),
				AutomaticAuthenticate = true,
				AutomaticChallenge = true
			});

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
