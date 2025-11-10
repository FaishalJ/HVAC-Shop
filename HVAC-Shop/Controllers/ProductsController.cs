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
			var result = await productsRepository.GetAllProducts(options);

   //         var result = new PaginationResult<Product>
			//{
			//	PageNumber = options.PageNumber,
			//	PageSize = options.PageSize,
			//	TotalCount = products.Count,
   //         };

            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            Response.Headers.Append("X-Page-Number", result.PageNumber.ToString());
            Response.Headers.Append("X-Page-Size", result.PageSize.ToString());
            Response.Headers.Append("X-Page-Count", result.PageCount.ToString());


            return result.Items;
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
