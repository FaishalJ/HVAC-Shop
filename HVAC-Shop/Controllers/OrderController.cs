using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.Entities.OrderAggregate;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Extensions;
using HVAC_Shop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Controllers
{
    [Authorize]
    public class OrderController(AppDbContext context) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await context.Orders
                .Include(x => x.OrderItem).ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            var orders = await context.Orders
                .Include(x => x.OrderItem)
                .Where(x => x.BuyerEmail == User.GetName() && (x.Id == id))
                .FirstOrDefaultAsync();

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderDto orderDto)
        {
            var basket = await context.Baskets
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BasketId == Request.Cookies["BasketId"]);

            if (basket == null || basket.Items.Count < 0 || String.IsNullOrEmpty(basket.PaymentIntentId))
            {
                return BadRequest("Basket is empty");
            }

            var orderItems = GetItems(basket.Items);

            if(orderItems == null)
            {
                return BadRequest("Your basket is empty");
            }

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            long deliveryFee = subtotal > 10000 ? 0 : 500;

            var order = new Order
            {
                BuyerEmail = User.GetName(),
                Address = orderDto.Address,
                OrderItem = orderItems,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                PaymentSummary = orderDto.PaymentSummary,
                PaymentIntentId = basket.PaymentIntentId ?? string.Empty
            };
            context.Orders.Add(order);
            context.Baskets.Remove(basket);
            var result = await context.SaveChangesAsync() > 0;

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        private static List<OrderItem>? GetItems(List<BasketItem> items)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in items)
            {
                if (item.Quantity < item.Product.QuantityInStock) return null;

                var orderedProductItem = new OrderedProductItem
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    PictureUrl = item.Product.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    OrderedProduct = orderedProductItem,
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                };

                orderItems.Add(orderItem);
            }
            return orderItems;
        }
    }
}
