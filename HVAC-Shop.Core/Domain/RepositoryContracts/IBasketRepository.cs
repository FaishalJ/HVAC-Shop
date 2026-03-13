using HVAC_Shop.Core.Domain.Entities;
using System.Threading.Tasks;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IBasketRepository
    {
        Task<Basket?> RetriveBasketAsync(string basketId);
        Task<bool> AddItemsToBasketAsync(Product product, int quantity, string basketId);
    }
}
