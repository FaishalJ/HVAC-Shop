using HVAC_Shop.Core.Domain.Entities.OrderAggregate;

namespace HVAC_Shop.Core.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public required ShippingAddress Address { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = [];
        public required string PaymentIntentId { get; set; }
        public required PaymentSummery PaymentSummary { get; set; }
        public decimal Total { get; set; }
    }
}
