using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Controllers
{
    public class BasketController(AppDbContext context) : BaseController
    {
        const string BasketSessionKey = "BasketId";

        [HttpGet]
        public async Task<ActionResult> GetBasket()
        {
            var basket = await RetriveBasket();
            if (basket == null) return NoContent();

            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToBasket(int productId, int quantity)
        {
            var basket = await RetriveBasket();
            basket ??= CreateBasket();

            var product = await context.Products.FindAsync(productId);
            if (product == null) return NotFound();
            basket.AddItem(product, quantity);
            var result = await context.SaveChangesAsync() > 0;

            if (result) return CreatedAtAction(nameof(GetBasket), basket);

            return BadRequest("Problem adding item to basket");
        }

        // Retrieves the current user's basket based on the BasketId stored in cookies
        private async Task<Basket?> RetriveBasket()
        {
            var basket = await context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.BasketId == Request.Cookies[BasketSessionKey]);

            return basket;
        }

        // Create Basket and add to cookie
        private Basket CreateBasket()
        {
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            var basket = new Basket
            {
                BasketId = Guid.NewGuid().ToString()
            };

            context.Baskets.Add(basket);
            Response.Cookies.Append(BasketSessionKey, basket.BasketId, cookieOptions);

            return basket;
        }
    }
}
