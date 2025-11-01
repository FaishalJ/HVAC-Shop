using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.DTO;

namespace HVAC_Shop.Core.Extensions
{
    public static class BasketToDtoExtention
    {
        public static BasketDTO ToBasketDto(this Basket basket)
        {
            return new BasketDTO
            {
                BasketId = basket.BasketId,
                Items = basket.Items.Select(x => new BasketItemDto
                {
                    ProductId = x.ProductId,
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    Brand = x.Product.Brand,
                    Type = x.Product.Type,
                    PictureUrl = x.Product.PictureUrl,
                    Quantity = x.Quantity
                }).ToList(),
            };
        }
    }
}
