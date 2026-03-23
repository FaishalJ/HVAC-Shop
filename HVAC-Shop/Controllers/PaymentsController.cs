using HVAC_Shop.Core.Domain.Entities.OrderAggregate;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.Extensions;
using HVAC_Shop.Core.Helpers;
using HVAC_Shop.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace HVAC_Shop.Controllers
{
    public class PaymentsController : BaseController
    {
        const string BasketSessionKey = "BasketId";

        private readonly PaymentService _paymentService;
        private readonly IBasketRepository _basketRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductsRepository _productRepository;
        private readonly IOptions<StripeOptions> _options;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(PaymentService paymentService, IOptions<StripeOptions> options, IBasketRepository basketRepository, IOrderRepository orderRepository, IProductsRepository productsRepository, ILogger<PaymentsController> logger)
        {
            _basketRepository = basketRepository;
            _orderRepository = orderRepository;
            _productRepository = productsRepository;
            _paymentService = paymentService;
            _options = options;
            _logger = logger;
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

            var result = await _basketRepository.SaveChangesAsync();
            if (!result) return BadRequest("Problem updating basket with payment intent");

            return Ok(basket.ToBasketDto());
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            // 1. Read the raw JSON payload sent by Stripe
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                // 2. Verify the event using Stripe’s signature and your webhook secret
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _options.Value.WhSecret
                );

                if (stripeEvent.Data.Object is PaymentIntent intent)
                {
                    if (string.Equals(intent.Status, "succeeded", StringComparison.OrdinalIgnoreCase))
                        await HandlePaymentIntentSucceeded(intent);
                    else
                        await HandlePaymentIntentFailed(intent);
                }
                else
                {
                    _logger.LogInformation("Received Stripe event with unsupported object type: {ObjectType}",
                        stripeEvent.Data.Object?.GetType().Name ?? "null");
                }

                // 4. Always return 200 OK so Stripe knows you received the event
                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe webhook error: {Message}", e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error has occured");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            var order = (await _orderRepository.GetAllOrdersAsync()).FirstOrDefault(x => x.PaymentIntentId == intent.Id) ?? throw new Exception("Order not found");

            if (order.Total != intent.Amount)
            {
                order.Status = OrderStatus.PaymentMisMatch;
            }
            else
            {
                order.Status = OrderStatus.PaymentReceived;
            }

            var basket = await _basketRepository.GetBasketAsync(b => b.PaymentIntentId == intent.Id);

            if (basket != null)
            {
                await _basketRepository.RemoveBasketAsync(basket);
            }
            await _basketRepository.SaveChangesAsync();
        }

        private async Task HandlePaymentIntentFailed(PaymentIntent intent)
        {
            var order = (await _orderRepository.GetAllOrdersAsync()).FirstOrDefault(x => x.PaymentIntentId == intent.Id) ?? throw new Exception("Order not found");

            // loop over the order items and update the product stock
            foreach (var item in order.OrderItems)
            {
                var productOrdered = await _productRepository.GetProductAsync(item.OrderedProduct.ProductId) ?? throw new Exception("Problem updating order stock, Product not found"); ;
                productOrdered.QuantityInStock += item.Quantity;
            }

            order.Status = OrderStatus.Failed;

            await _basketRepository.SaveChangesAsync();
        }

    }
}
