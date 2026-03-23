using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HVAC_Shop.Infrastructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _context;

        public BasketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Basket?> GetBasketAsync(string basketId)
        {
            var query = _context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product);

            return await query.FirstOrDefaultAsync(b => b.BasketId == basketId);
        }

        public async Task<Basket?> GetBasketAsync(Expression<Func<Basket, bool>> predicate)
        {
            var query = _context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task CreateBasketAsync(Basket basket)
        {
            await _context.Baskets.AddAsync(basket);
        }

        //public async Task UpdateBasketAsync(Basket basket)
        //{
        //    _context.Baskets.Update(basket);
        //    await Task.CompletedTask;
        //}

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task RemoveBasketAsync(Basket basket)
        {
            var existingasket = await _context.Baskets.FirstOrDefaultAsync(b => b.Id == basket.Id);

            if (existingasket != null)
            {
                _context.Baskets.Remove(existingasket);
            }
        }
    }
}
