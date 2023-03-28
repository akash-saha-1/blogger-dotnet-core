using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        private readonly IConfiguration configuration;
        public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (User, Admin, SuperAdmin)
            //Generated from C# Interactive Window by clicking on View -> Other Window -> C# Interactive Window -> type "Console.WriteLine(Guid.NewGuid())"
            var userRoleId = "5dc3dc87-3e19-4488-827e-227f145d0bf1";  
            var adminRoleId = "a31637ec-5045-4fc3-96ab-1ab7855ff45b";
            var superAdminRoleId = "5ed5dd9c-507c-4035-a514-cafbebeda960";

            var roles = new List<IdentityRole> {
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdminUser
            var superAdminId = "a7ff0d74-4ab0-4f30-bc03-e49eed4f8298";
            var superAdminUser = new IdentityUser
            {
                UserName = configuration.GetSection("SuperAdminDetails")["SuperAdminUsername"],
                NormalizedUserName = configuration.GetSection("SuperAdminDetails")["SuperAdminUsername"]?.ToUpper(),
                Email = configuration.GetSection("SuperAdminDetails")["SuperAdminEmail"],
                NormalizedEmail = configuration.GetSection("SuperAdminDetails")["SuperAdminEmail"]?.ToUpper(),
                Id = superAdminId
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, configuration.GetSection("SuperAdminDetails")["SuperAdminPassword"]);

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add all the roles to SuperAdmin
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> {
                    RoleId = userRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string> {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string> {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
