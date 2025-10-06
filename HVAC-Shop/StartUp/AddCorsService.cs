namespace HVAC_Shop.StartUp
{
	public static class AddCorsService
	{
		public static void AddCorsServices(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyOrigin()
						  .AllowAnyMethod()
						  .AllowAnyHeader();
				});
			});
        }
    }
}
