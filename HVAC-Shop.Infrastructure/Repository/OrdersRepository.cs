using HVAC_Shop.Core.Domain.Entities.OrderAggregate;
using HVAC_Shop.Core.Domain.IdentityEntities;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Infrastructure.Repository
{
    public class OrdersRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrdersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.Select(o=>o.ToOrderDto())
                .AsNoTracking()
                .ToListAsync();

            return orders;
        }

        public async Task<OrderDto?> GetOrderAsync(int id, string email)
        {
            var order = await _context.Orders.Select(o => o.ToOrderDto())
                .Where(x => x.BuyerEmail == email && (x.Id == id))
                .FirstOrDefaultAsync();

            return order;
        }
    }
}
