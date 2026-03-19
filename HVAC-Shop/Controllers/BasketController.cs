using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HVAC_Shop.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductsRepository _productsRepository;

        const string BasketSessionKey = "BasketId";

        public BasketController(IBasketRepository basketRepository, IProductsRepository productsRepository)
        {
            _basketRepository = basketRepository;
            _productsRepository = productsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetriveBasket();

            if (basket == null) return NoContent();

            return basket.ToBasketDto();
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToBasket(int productId, int quantity)
        {
            var basket = await RetriveBasket() ?? await CreateBasket();

            var product = await _productsRepository.GetProductAsync(productId);
            if (product == null) return NotFound($"Product {productId} not found");

            basket.AddItem(product, quantity);
            var isSuccess = await _basketRepository.SaveChangesAsync();

            if (isSuccess) return CreatedAtAction(nameof(GetBasket), basket.ToBasketDto());

            return BadRequest("Problem adding item to basket");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleItems(int productId, int quantity)
        {
            var basket = await RetriveBasket();

            if (basket == null) return NotFound("Basket not found");


            basket.RemoveItem(productId, quantity);

            var isSuccess = await _basketRepository.SaveChangesAsync();

            if (isSuccess) return NoContent();

            return BadRequest("Problem deleting item from basket");
        }

        // Retrieves the current user's basket based on the BasketId stored in cookies
        private async Task<Basket?> RetriveBasket()
        {
            if (!Request.Cookies.TryGetValue(BasketSessionKey, out var basketId) || string.IsNullOrEmpty(basketId))
            {
                return null;
            }

            var basket = await _basketRepository.GetBasketAsync(basketId);

            return basket;
        }

        // Create Basket and add to cookie
        private async Task<Basket> CreateBasket()
        {
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(30),
            };

            var basket = new Basket
            {
                BasketId = Guid.NewGuid().ToString()
            };

            await _basketRepository.CreateBasketAsync(basket);

            Response.Cookies.Append(BasketSessionKey, basket.BasketId, cookieOptions);

            return basket;
        }
    }
}
