using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BamboAuto.Data;
using BamboAuto.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BamboAuto.Models
{
	public class SeedData
	{
		public static async Task InitializeAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
		{
			context.Database.EnsureCreated();
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			IdentityResult roleResult;
			if (!await RoleManager.RoleExistsAsync(SD.AdminEndUser))
			{
				roleResult = await RoleManager.CreateAsync(new IdentityRole(SD.AdminEndUser));
			}
			if (!await RoleManager.RoleExistsAsync(SD.CustomerEndUser)
			{
				roleResult = await RoleManager.CreateAsync(new IdentityRole(SD.CustomerEndUser));
			}
			var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var _user = await userManager.FindByEmailAsync(config.GetSection("AppSettings")["UserEmail"]);
			if (_user == null)
			{
				var poweruser = new ApplicationUser
				{
					UserName = config.GetSection("AppSettings")["UserEmail"],
					Email = config.GetSection("AppSettings")["UserEmail"]
				};
				string UserPassword = config.GetSection("AppSettings")["UserPassword"];
				var createPowerUser = await userManager.CreateAsync(poweruser, UserPassword);
				if (createPowerUser.Succeeded)
				{
					await userManager.AddToRoleAsync(poweruser, "Admin");
				}
			}


			if (!context.Users.Any())
			{
				context.Users.AddRange(
					new ApplicationUser {
						UserName = "admin@admin.com",
						Email = "admin@admin.com",
						FirstName = "admin",
						LastName = "-----",
						Address = "-----",
						City = "-----",
						PostalCode = "-----",
						PhoneNumber = "-----"
					}
				);
			}

			if (!context.Roles.Any())
			{
				if (!await context.Roles.(SD.CustomerEndUser))
				{
					await context.Roles.CreateAsync(new IdentityRole(SD.CustomerEndUser));
				}
				if (!await context.Roles.(SD.AdminEndUser))
				{
					await context.Roles.CreateAsync(new IdentityRole(SD.AdminEndUser));
				}
			}

			

				context.SaveChanges();
		}
	}
}