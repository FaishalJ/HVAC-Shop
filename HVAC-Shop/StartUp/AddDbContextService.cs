using HVAC_Shop.Infrastructure;
using Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.StartUp
{
    public static class AddDbContextService
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
					maxRetryDelay: TimeSpan.FromSeconds(30),
					errorNumbersToAdd: null))
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    var dbContext = (AppDbContext)context;

                    // Seed Products
                    if (!dbContext.Products.Any())
                    {
                        dbContext.Products.AddRange(InitialSeedData.GetProducts());
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                })
            );
        }
    }
}
