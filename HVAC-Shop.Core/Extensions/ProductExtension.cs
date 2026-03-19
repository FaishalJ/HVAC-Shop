using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.DTO;

namespace HVAC_Shop.Core.Extensions
{
    public static class ProductExtension
    {
        extension(Product product)
        {
            public ProductDto ToProductDto()
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
                    PublicId = product.PublicId
                };
            }
        }
    }
}
