using HVAC_Shop.Core.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Core.Domain.Entities.OrderAggregate
{
    public class OrderItem : EntityBase
    {
        public required OrderedProductItem OrderedProduct { get; set; }
        public int Quantity { get; set; }
		[Precision(10, 2)]
		public decimal Price { get; set; }
    }
}
