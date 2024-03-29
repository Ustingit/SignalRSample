using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRSample.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRSample.Hubs;

namespace SignalRSample
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
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.AddControllersWithViews();

			var azureConnection = Configuration.GetConnectionString("SignalRHub");
			Console.WriteLine($"Azure connection is: {azureConnection}");

			if(string.IsNullOrEmpty(azureConnection))
				throw new ArgumentNullException("ensure azure  signal r connection string");

			services.AddSignalR();
				//.AddAzureSignalR(azureConnection);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
				endpoints.MapHub<UserHub>("/hubs/userCount");
				endpoints.MapHub<DeathlyHallowHub>("/hubs/deathlyhallows");
				endpoints.MapHub<HouseGroupHub>("/hubs/housegroup");
				endpoints.MapHub<NotificationHub>("/hubs/notification");
				endpoints.MapHub<ChatHub>("/hubs/chat");
				endpoints.MapHub<OrderHub>("/hubs/order");
				endpoints.MapHub<AdvancedChatHub>("/hubs/advancedChat");
			});
		}
	}
}
