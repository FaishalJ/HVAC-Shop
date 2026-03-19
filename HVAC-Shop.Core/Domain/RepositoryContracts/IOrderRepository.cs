using HVAC_Shop.Core.Domain.Entities.OrderAggregate;
using HVAC_Shop.Core.DTO;

namespace HVAC_Shop.Core.Domain.RepositoryContracts
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderAsync(int id, string email);
        Task AddOrderAsync(Order order);
    }
}
