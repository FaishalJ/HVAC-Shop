namespace HVAC_Shop.Core.Domain.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        Processing,
        Failed,
        Delivered,
        Cancelled
    }
}
