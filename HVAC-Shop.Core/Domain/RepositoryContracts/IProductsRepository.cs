using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Helpers;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IProductsRepository
    {
        Task<PaginationResult<ProductDto>> GetAllProductsAsync(ProductQueryOptions options);
        Task<Product?> GetProductAsync(int productId);
        Object FilterByTypeAndBrand();
    }
}
