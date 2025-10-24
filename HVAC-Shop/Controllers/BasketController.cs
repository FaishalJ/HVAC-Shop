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
            if (basket == null) return NotFound();

            return Ok(basket);
        }

        private async Task<Basket?> RetriveBasket()
        {
            var basket = await context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.BasketId == Request.Cookies[BasketSessionKey]);

            return basket;
        }
    }
}
