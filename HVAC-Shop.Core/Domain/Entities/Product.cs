using HVAC_Shop.Core.Domain.Entities.Base;

namespace HVAC_Shop.Core.Domain.Entities
{
	public class Product: EntityBase
    {
		public required string Name { get; set; }
		public required string Description { get; set; }
		public long Price { get; set; }
		public required string PictureUrl { get; set; }
		public required string Type { get; set; }
		public required string Brand { get; set; }
		public int QuantityInStock { get; set; }
		public string? PublicId { get; set; }

        public List<BasketItem?> Items { get; set; } = [];
    }
}
