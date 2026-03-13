namespace HVAC_Shop.Core.DTO
{
    public class BasketItemDto
    {
        public int Quantity { get; set; }

        public int ProductId { get; set; }
        // From Product navigation property
        public required string Name { get; set; }
        public long Price { get; set; }
        public required string Brand { get; set; }
        public required string Type { get; set; }
        public required string PictureUrl { get; set; }
    }
}