namespace HVAC_Shop.Core.Domain.Entities.OrderAggregate
{
    public class Order
    {
        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public required ShippingAddress Address { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public long Subtotal { get; set; }
        public long DeliveryFee { get; set; }
        public long Discount { get; set; }
        public required string PaymentIntentId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> OrderItem { get; set; } = [];
        public required PaymentSummery PaymentSummary { get; set; }
        public long GetTotal()
        {
            return Subtotal + DeliveryFee - Discount;
        }
    }
}
