using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Helpers;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IProductsRepository
    {
        Task<PaginationResult<Product>> GetAllProducts(ProductQueryOptions options);
    }
}
