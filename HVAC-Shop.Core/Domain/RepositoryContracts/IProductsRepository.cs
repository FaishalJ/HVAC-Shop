using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Helpers;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllProducts(ProductQueryOptions options);
    }
}
