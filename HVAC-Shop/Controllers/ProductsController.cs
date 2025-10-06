using HVAC_Shop.Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Controllers
{
	public class ProductsController(AppDbContext context) : BaseController
	{
		[HttpGet]
		public async Task<ActionResult> GetAllProducts()
		{
			var products = await context.Products.ToListAsync();
			return Ok(products);
		}

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);

			if (product == null) return NotFound();

			return product;
        }
    }
}
