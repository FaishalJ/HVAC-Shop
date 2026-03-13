using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Infrastructure.Repository
{
    public class BasketRepository(AppDbContext context) : IBasketRepository
    {
        public async Task<bool> AddItemsToBasketAsync(Product product, int quantity, string basketId)
        {
            var basket = await RetriveBasketAsync(basketId);
            basket?.AddItem(product, quantity);

            var result = await context.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<Basket?> RetriveBasketAsync(string basketId)
        {
            var basket = await context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.BasketId == basketId);

            return basket;
        }
    }
}
