using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.DTO;
using System.Xml.Linq;

namespace HVAC_Shop.Core.Extensions
{
    public static class ProductExtension
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                Type = product.Type,
                Brand = product.Brand,
                QuantityInStock = product.QuantityInStock,
                PublicId = product.PublicId,
                Items = [.. product.Items.Select(x => new BasketItemDto
                {
                    ProductId = x.ProductId,
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    Brand = x.Product.Brand,
                    Type = x.Product.Type,
                    PictureUrl = x.Product.PictureUrl,
                    Quantity = x.Quantity
                })]
            };
        }
    }
}
