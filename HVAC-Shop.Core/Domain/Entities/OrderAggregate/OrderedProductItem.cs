using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Core.Domain.Entities.OrderAggregate
{
    [Owned]
    public class OrderedProductItem
    {
        public int ProductId { get; set; }
        public required string PictureUrl { get; set; }
        public required string Name { get; set; }
    }
}
