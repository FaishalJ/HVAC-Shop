namespace HVAC_Shop.Core.Domain.Entities.OrderAggregate
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required OrderedProductItem OrderedProduct { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }
}
