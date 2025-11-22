using HVAC_Shop.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace HVAC_Shop.Infrastructure.seedDa
{
    public static class UserSeedData
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create admin
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create user
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed User
            if(!userManager.Users.Any())
            {
                var normalUser = new User
                {
                    UserName = "jaffar@test.com",
                    Email = "jaffar@test.com"
                };
                await userManager.CreateAsync(normalUser, "Password@123");
                await userManager.AddToRoleAsync(normalUser, "User");
            }

            // Seed Admin
            if (await userManager.FindByEmailAsync("admin@test.com") == null)
            {
                var adminUser = new User
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };
                await userManager.CreateAsync(adminUser, "Password@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
