using HVAC_Shop.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HVAC_Shop.Core.Domain.IdentityEntities
{
    public class User : IdentityUser
    {
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
    }
}
