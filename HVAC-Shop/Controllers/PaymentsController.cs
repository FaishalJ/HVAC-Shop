using HVAC_Shop.Core.Extensions;
using HVAC_Shop.Core.Services;
using HVAC_Shop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Controllers
{
    public class PaymentsController : BaseController
    {
        const string BasketSessionKey = "BasketId";
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdatePaymentIntent(AppDbContext context, PaymentService paymentService)
        {
            var basket = await context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.BasketId == Request.Cookies[BasketSessionKey]);

            if (basket == null) return BadRequest("Problem retrieving basket");

            var paymentIntent = await paymentService.CreateOrUpdatePaymentIntent(basket);

            if (paymentIntent == null) return BadRequest("Problem creating payment intent");

            basket.PaymentIntentId ??= paymentIntent.Id;
            basket.ClientSecret ??= paymentIntent.ClientSecret;

            if (context.ChangeTracker.HasChanges())
            {
                var result = await context.SaveChangesAsync() > 0;
                if (result) return Ok(basket);
                return BadRequest("Problem updating basket with payment intent");
            }

            return Ok(basket.ToBasketDto());
        }
    }
}
