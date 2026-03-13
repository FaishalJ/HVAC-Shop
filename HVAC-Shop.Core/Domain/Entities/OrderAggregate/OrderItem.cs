using HVAC_Shop.Core.Domain.Entities.Base;

namespace HVAC_Shop.Core.Domain.Entities.OrderAggregate
{
    public class OrderItem : EntityBase
    {
        public required OrderedProductItem OrderedProduct { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }
}
