using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Extensions;
using HVAC_Shop.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HVAC_Shop.Controllers
{
	public class ProductsController(IProductsRepository productsRepository) : BaseController
	{
		[HttpGet]
		public async Task<ActionResult<List<ProductDto>>> GetAllProducts([FromQuery] ProductQueryOptions options)
		{
			var result = await productsRepository.GetAllProductsAsync(options);
            
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            Response.Headers.Append("X-Page-Number", result.PageNumber.ToString());
            Response.Headers.Append("X-Page-Size", result.PageSize.ToString());
            Response.Headers.Append("X-Page-Count", result.PageCount.ToString());

            return result.Items;
		}

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await productsRepository.GetProductAsync(id);

            if (product == null) return NotFound();

            return product.ToProductDto();
        }

        [HttpGet("filters")]
        public ActionResult Filter()
        {
            return Ok(productsRepository.FilterByTypeAndBrand());
        }
    }
}
