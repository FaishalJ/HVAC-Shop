using HVAC_Shop.Core.Domain.Entities;
using System.Linq.Expressions;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketAsync(string basketId);
        Task<Basket?> GetBasketAsync(Expression<Func<Basket,bool>> predicate);
        Task CreateBasketAsync(Basket basket);
        Task<bool> SaveChangesAsync();

        Task RemoveBasketAsync(Basket basket);
    }
}
