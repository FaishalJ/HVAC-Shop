using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.Entities.OrderAggregate;
using HVAC_Shop.Core.Domain.RepositoryContracts;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVAC_Shop.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketRepository _basketRepository;

        public OrderController(IOrderRepository orderRepository, IBasketRepository basketRepository)
        {
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            var order = await _orderRepository.GetOrderAsync(id, User.GetName());
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderDto orderDto)
        {
            var basketCookie = Request.Cookies["BasketId"];
            if (string.IsNullOrWhiteSpace(basketCookie))
                return BadRequest("Problem retrieving basket");

            var basket = await _basketRepository.GetBasketAsync(basketCookie);

            if (basket == null || basket.Items.Count == 0 || string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                return BadRequest("Basket is empty");
            }

            var orderItems = GetItems(basket.Items);

            if (orderItems == null)
            {
                return BadRequest("Your basket is missing some items");
            }

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            long deliveryFee = subtotal > 10000 ? 0 : 500;

            var order = await _orderRepository.GetOrderAsync(order => order.PaymentIntentId == basket.PaymentIntentId);

            if (order == null)
            {

                order = new Order
                {
                    BuyerEmail = User.GetName(),
                    Address = orderDto.Address,
                    OrderItem = orderItems,
                    Subtotal = subtotal,
                    DeliveryFee = deliveryFee,
                    PaymentSummary = orderDto.PaymentSummary,
                    PaymentIntentId = basket.PaymentIntentId
                };
                await _orderRepository.AddOrderAsync(order);
            }
            else
            {
                order.OrderItem = orderItems;
            }


            //await _basketRepository.RemoveBasketAsync(basket);
            //var result = await _basketRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order.ToOrderDto());
        }

        private static List<OrderItem>? GetItems(List<BasketItem> items)
        {
            List<OrderItem> orderItems = [];

            foreach (var item in items)
            {
                if (item.Quantity > item.Product.QuantityInStock) return null;

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
