using Microsoft.EntityFrameworkCore;
using Project_Coffe.Data;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Models.ModelRealization
{
    public class OrderService(ApplicationDbContext context) : IOrderService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task CreateOrderAsync(int userId, List<int> productIds)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = 0,
                OrderProducts = []
            };


            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
            var orderProducts = new List<OrderProduct>();

            foreach (var product in products)
            {
                var orderProduct = new OrderProduct
                {
                    Product = product,
                    Quantity = 1
                };
                orderProducts.Add(orderProduct);
                order.TotalAmount += (decimal)product.Price;
            }

            order.OrderProducts = orderProducts;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
