using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityDemo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurityDemo.Areas.Identity.Pages.Account.Models;
using System.Security.Claims;

namespace SecurityDemo
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
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddDefaultUI()
				.AddDefaultTokenProviders()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddAuthorization(config =>
			{
				config.AddPolicy(ApplicationPolicy.BigCheese, policy =>
				{
					policy.RequireRole(ApplicationRoles.Admin);
					policy.Build();
				});

				config.AddPolicy("CheeseLovers", policy =>
				{
					policy.RequireClaim(ApplicationClaims.FavoriteCheese)
					.Build();
				});

				config.AddPolicy("ProvoloneFans", policy =>
				{
					policy.RequireClaim(ApplicationClaims.FavoriteCheese, new[] { "provolone" })
					.Build();
				});
			});


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			//outside the configuration of the HTTP pipeline
			var t = roleManager.RoleExistsAsync(ApplicationRoles.Admin);
			t.Wait();
			if (!t.Result)
			{
				var createTask = roleManager.CreateAsync(new IdentityRole {
					Name = ApplicationRoles.Admin,
					NormalizedName = ApplicationRoles.Admin
				});

				createTask.Wait();
			}

			//var jeffTask = userManager.FindByEmailAsync("jaivergh@gmail.com");
			//jeffTask.Wait();
			//userManager.AddClaimAsync(jeffTask.Result, new Claim(ApplicationPolicy.BigCheese, "provolone")).Wait();

		}
	}
}
