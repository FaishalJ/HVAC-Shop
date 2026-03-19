using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.Extensions;
using HVAC_Shop.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HVAC_Shop.Controllers
{
    public class PaymentsController : BaseController
    {
        const string BasketSessionKey = "BasketId";

        //private readonly AppDbContext _context;
        private readonly PaymentService _paymentService;
        private readonly IBasketRepository _basketRepository;

        public PaymentsController(PaymentService paymentService, IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdatePaymentIntent()
        {
            var basketCookie = Request.Cookies[BasketSessionKey];
            if (string.IsNullOrWhiteSpace(basketCookie))
                return BadRequest("Problem retrieving basket");

            var basket = await _basketRepository.GetBasketAsync(basketCookie);

            if (basket == null) return BadRequest("Problem retrieving basket");

            var paymentIntent = await _paymentService.CreateOrUpdatePaymentIntent(basket);

            if (paymentIntent == null) return BadRequest("Problem creating payment intent");

            basket.PaymentIntentId ??= paymentIntent.Id;
            basket.ClientSecret ??= paymentIntent.ClientSecret;

            //if (_context.ChangeTracker.HasChanges())
            //{
            //    var result = await _basketRepository.SaveChangesAsync();
            //    if (result) return Ok(basket);
            //    return BadRequest("Problem updating basket with payment intent");
            //}

            var result = await _basketRepository.SaveChangesAsync();
            if (!result) return BadRequest("Problem updating basket with payment intent");

            return Ok(basket.ToBasketDto());
        }
    }
}
