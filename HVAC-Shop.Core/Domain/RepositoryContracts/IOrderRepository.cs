using HVAC_Shop.Core.Domain.Entities.OrderAggregate;
using HVAC_Shop.Core.DTO;
using System.Linq.Expressions;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderAsync(int id, string email);
        Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> predicate);
        Task AddOrderAsync(Order order);
    }
}
