using HVAC_Shop.Core.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Core.Domain.Entities.OrderAggregate
{
    public class Order : EntityBase
    {
        public required string BuyerEmail { get; set; }
        public required ShippingAddress Address { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Precision(10, 2)]
        public decimal Subtotal { get; set; }

        [Precision(10, 2)]
        public decimal DeliveryFee { get; set; }

        [Precision(10, 2)]
        public decimal Discount { get; set; }
        public required string PaymentIntentId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> OrderItem { get; set; } = [];
        public required PaymentSummery PaymentSummary { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + DeliveryFee - Discount;
        }
    }
}
