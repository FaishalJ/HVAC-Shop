using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Helpers;
using Microsoft.Extensions.Options;
using Stripe;

namespace HVAC_Shop.Core.Services
{
    public class PaymentService(IOptions<StripeOptions> options)
    {
        public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;

            long basketAmount = basket.Items.Sum(item => item.Product.Price * item.Quantity);
            long deliveryFee = basketAmount > 10000 ? 0 : 500;
            var totalAmount = basketAmount + deliveryFee;
            //long discount = 0;

            var intent = new PaymentIntent();
            var service = new PaymentIntentService();

            if (!String.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = totalAmount,
                };

                intent = await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = basketAmount + deliveryFee,
                Currency = "usd"
            };

            intent = await service.CreateAsync(createOptions);

            return intent;
        }
    }
}
