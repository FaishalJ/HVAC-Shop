using HVAC_Shop.Core.Domain.IdentityEntities;
using HVAC_Shop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HVAC_Shop.StartUp
{
    public static class IdentityServices
    {
        public static void AddIdentityServices(this IServiceCollection services)
        {
            // Identity Core
            services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            // Identity Core
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            // Authorization
            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();
            //});
        }
    }
}
