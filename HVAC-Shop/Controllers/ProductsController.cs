using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HVAC_Shop.Controllers
{
	public class ProductsController(IProductsRepository productsRepository) : BaseController
	{
		[HttpGet]
		public async Task<ActionResult<List<Product>>> GetAllProducts([FromQuery] ProductQueryOptions options)
		{
			return await productsRepository.GetAllProducts(options);
		}

   //     [HttpGet("{id}")]
   //     public async Task<ActionResult<Product>> GetProduct(int id)
   //     {
   //         var product = await context.Products.FindAsync(id);

			//if (product == null) return NotFound();

			//return product;
   //     }
    }
}
