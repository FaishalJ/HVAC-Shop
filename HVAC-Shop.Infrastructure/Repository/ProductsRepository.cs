using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Extensions;
using HVAC_Shop.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Infrastructure.Repository
{
    public class ProductsRepository(AppDbContext context) : IProductsRepository
    {
        public async Task<PaginationResult<ProductDto>> GetAllProductsAsync(ProductQueryOptions options)
        {
            IQueryable<Product> query = context.Products;
            //var query = context.Products.AsQueryable();

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

            // Total count before pagination
            var totalCount = await query.CountAsync();

            // Sorting
            query = options.SortBy?.ToLower() switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            // Pagination
            query = query
                .Skip((options.PageNumber - 1) * options.PageSize)
                .Take(options.PageSize);

            return new PaginationResult<ProductDto>
            {
                Items = await query.Select(x => x.ToProductDto()).ToListAsync(),
                TotalCount = totalCount,
                PageNumber = options.PageNumber,
                PageSize = options.PageSize
            };
        }
        public async Task<Product?> GetProductAsync(int productId)
        {
            return await context.Products.FindAsync(productId);
        }
        public Object FilterByTypeAndBrand()
        {
            var type = context.Products.Select(x => x.Type).Distinct();
            var brand = context.Products.Select(x => x.Brand).Distinct();

            return new { Type = type, Brand = brand };
        }

    }
}
