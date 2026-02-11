using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.DTO;

namespace HVAC_Shop.Core.Extensions
{
    public static class UserAddressExtension
    {
        public static UserAddressDto ToDto(this Address address)
        {
            return new UserAddressDto
            {
                Id = address.Id,
                Name = address.Name,
                City = address.City,
                Country = address.Country,
                Line1 = address.Line1,
                Line2 = address.Line2,
                State = address.State,
                PostalCode = address.PostalCode
            };
        }
    }
}
