using HVAC_Shop.Core.Domain.Entities;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketAsync(string basketId);
        Task CreateBasketAsync(Basket basket);
        Task<bool> SaveChangesAsync();

        Task RemoveBasketAsync(Basket basket);
    }
}
