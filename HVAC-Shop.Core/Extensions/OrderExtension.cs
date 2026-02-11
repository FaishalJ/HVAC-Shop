using HVAC_Shop.Core.Domain.Entities.OrderAggregate;
using HVAC_Shop.Core.DTO;

namespace HVAC_Shop.Core.Extensions
{
    public static class OrderExtension
    {
        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                Address = order.Address,
                OrderDate = order.OrderDate,
                Subtotal = order.Subtotal,
                DeliveryFee = order.DeliveryFee,
                Discount = order.Discount,
                Status = order.Status,
                PaymentSummary = order.PaymentSummary,
                PaymentIntentId = order.PaymentIntentId,
                OrderItems = order.OrderItem.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    OrderedProduct = item.OrderedProduct,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList(),
                Total = order.GetTotal()
            };
        }
    }
}
