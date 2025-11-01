using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.Helpers;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Infrastructure.Repository
{
    public class ProductsRepository(AppDbContext context) : IProductsRepository
    {
        public async Task<List<Product>> GetAllProducts(ProductQueryOptions options)
        {
            var query = context.Products.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(options.FilterBy) && !string.IsNullOrWhiteSpace(options.FilterValue)
)
            {
                query = options.FilterBy.ToLower() switch
                {
                    "type" => query.Where(p => p.Type.Equals(options.FilterValue.ToLower())),
                    "brand" => query.Where(p => p.Brand.Equals(options.FilterValue.ToLower())),
                    _ => query
                };
            }

            // Sorting
            query = options.SortBy?.ToLower() switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };


            return await query.ToListAsync();
        }
    }
}
