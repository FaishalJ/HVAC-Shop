using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Infrastructure.Repository;
using HVAC_Shop.Middleware;

namespace HVAC_Shop.StartUp
{
	public static class AddServices
	{
		public static void AddApplicationServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddDatabaseServices(builder.Configuration);
			builder.Services.AddCorsServices();

			// Exception.
			builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
			builder.Services.AddProblemDetails();

            // Repositories.
			builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
            
			// Identity.
			builder.Services.AddIdentityServices();
        }
    }
}
