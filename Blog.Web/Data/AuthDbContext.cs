using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Blog.Web.Data
{
	public class AuthDbContext : IdentityDbContext
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
		{
		}



		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			//seed roles - users and admin

			var adminRoleId = "75197635-bffc-4f77-84e2-65c59700507d";
			var superadminRoleId = "e2dcaaa1-4a40-454a-a53f-295648a9ad34";
			var userRoleId = "b01af098-f4bc-4f8c-a663-9c8eb1d32c4f";

			var roles = new List<IdentityRole>
			{

				new IdentityRole
				{
					Name = "admin",
					NormalizedName = "admin",
					Id = adminRoleId,
					ConcurrencyStamp = adminRoleId
				},
				new IdentityRole
				{
					Name= "SuperAdmin",
					NormalizedName= "SuperAdmin",
					Id= superadminRoleId,
					ConcurrencyStamp = superadminRoleId
				},
				new IdentityRole
				{
					Name = "User",
					NormalizedName = "User",
					Id= userRoleId,
					ConcurrencyStamp= userRoleId
				}

			};

			builder.Entity<IdentityRole>().HasData(roles);

			// seed superadmin

			var superAdminId = "84162b42-163e-4a7d-89e8-71a3c44b739c";
			var superAdminUser = new IdentityUser
			{
				UserName = "superadmin@bloggie.com",
				Email = "superadmin@bloggie.com",
				NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
				NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
				Id = superAdminId
			};

			superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Superadmin@123");

			builder.Entity<IdentityUser>().HasData(superAdminUser);



			//seed all roles to superadmin

			var superAdminRoles = new List<IdentityUserRole<string>>
			{
				new IdentityUserRole<string>
				{
					RoleId = adminRoleId,
					UserId = superAdminId
				},
				new IdentityUserRole<string>
				{
					RoleId = superadminRoleId,
					UserId = superAdminId
				},
				new IdentityUserRole<string>
				{
					RoleId = userRoleId,
					UserId = superAdminId
				},
			};

			builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);


		}

	}
}
