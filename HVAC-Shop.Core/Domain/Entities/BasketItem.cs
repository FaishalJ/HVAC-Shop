using HVAC_Shop.Core.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HVAC_Shop.Core.Domain.Entities
{
    [Table("BasketItems")]
    public class BasketItem : EntityBase
    {
        public int Quantity { get; set; }

        // Navigation property to the Basket
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public int BasketId { get; set; } // Foreign key to Basket
        public Basket Basket { get; set; } = null!;
    }
}
