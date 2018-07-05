using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BamboAuto.Data;
using BamboAuto.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BamboAuto.Models
{
	public static class DbInitializer
	{
		public static async Task InitializeAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
		{
			context.Database.EnsureCreated();
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			if (!await RoleManager.RoleExistsAsync(SD.AdminEndUser))
			{
				await RoleManager.CreateAsync(new IdentityRole(SD.AdminEndUser));
			}
			if(!await RoleManager.RoleExistsAsync(SD.CustomerEndUser))
			{
				await RoleManager.CreateAsync(new IdentityRole(SD.CustomerEndUser));
			}

			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			
				var poweruser = new ApplicationUser
				{
					UserName = "admin@admin.com",
					Email = "admin@admin.com"
				};
				var createPowerUser = await userManager.CreateAsync(poweruser, "@Dmin123");
				if (createPowerUser.Succeeded)
				{
					await userManager.AddToRoleAsync(poweruser, SD.AdminEndUser);
				}
		}
	}
}
