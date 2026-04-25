using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.Services
{
    public class OrderService
    {
        private readonly DbService dbService;

        public OrderService() {
            var context = new EadMidsContext();  
            dbService = new DbService(context);   
        }   

        public bool PlaceOrder(int userId, List<CartItem> cartItems,
            string? deliveryType, string? paymentMethod, string? address)
        {
            // 1. Validate cart
            if (cartItems == null || cartItems.Count == 0)
                return false;

            // 2. Validate products
            foreach (var item in cartItems)
            {
                if (item.Product == null)
                    return false;

                if (item.Quantity <= 0)
                    return false;
            }

            // 3. Calculate total
            decimal total = cartItems.Sum(x => x.TotalPrice);

            // 4. Create Order object
            var order = new Order
            {
                UserId = userId,
                TotalAmount = total,
                Status = "Pending",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                DeliveryType = deliveryType,
                PaymentMethod = paymentMethod,
                Address = address
            };

            // 5. Call DB
            bool result = dbService.PlaceOrder(order, cartItems);

            if (!result)
                return false;

            return true;
        }

        public List<Order> GetUserOrders(int userId)
        {
            return dbService.GetOrdersByUserId(userId);
        }

        public List<Order> GetOrdersByStatus(String status)
        {
            return dbService.GetOrdersByStatus(status);
        }

        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            return dbService.UpdateOrderStatus(orderId, newStatus);
        }
    }
}
