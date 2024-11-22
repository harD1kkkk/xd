﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using Project_Coffe.Models.ModelRealization;
using System.Security.Claims;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModel model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = new Order
            {
                UserId = userId,
                TotalAmount = model.TotalAmount,
                OrderProducts = model.Products.Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            await _orderService.CreateOrderAsync(order);
            return Ok(order);
        }
    }
}
