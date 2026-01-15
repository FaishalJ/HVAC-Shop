using HVAC_Shop.Core.Domain.Entities.OrderAggregate;

namespace HVAC_Shop.Core.DTO
{
    public class CreateOrderDto
    {
        public required ShippingAddress Address { get; set; }
        public required PaymentSummery PaymentSummary { get; set; }
    }
}
