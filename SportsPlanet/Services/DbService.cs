using Microsoft.EntityFrameworkCore;
using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsPlanet.Services
{
    public class DbService
    {
        private readonly EadMidsContext dbcontext;

        public DbService(EadMidsContext context)
        {
            dbcontext = context;
        }

       
        public List<Product> GetAllProducts()
        {
            return dbcontext.Products.ToList();
        }

        public bool AddNewUser(User user)
        {
            dbcontext.Users.Add(user);
            return dbcontext.SaveChanges() > 0;
        }

        public User? FindByEmail(string email)
        {
            return dbcontext.Users
                           .FirstOrDefault(u => u.Email == email);
        }

       
        public bool PlaceOrder(Order order, List<CartItem> cartItems)
        {
            using var transaction = dbcontext.Database.BeginTransaction();

            try
            {
                foreach (var item in cartItems)
                {
                    // Fetch product from DB
                    var product = dbcontext.Products
                                           .FirstOrDefault(p => p.Id == item.Product.Id);

                    if (product == null)
                        throw new Exception("Product not found");

                    // Check stock availability
                    if (product.Quantity < item.Quantity)
                        throw new Exception($"Not enough stock for product {product.ProductName}");

                    // Deduct quantity
                    product.Quantity -= item.Quantity;

                    // Add order item
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Price = product.Price,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    });
                }

                dbcontext.Orders.Add(order);

                // Save both order + updated product quantities
                dbcontext.SaveChanges();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return dbcontext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.Id)
                .ToList();
        }

        public List<Product> GetProductsByTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return new List<Product>();

            tag = tag.Trim().ToLower();

            return dbcontext.Products
                .Where(p => p.Tags != null &&
                    ("," + p.Tags.ToLower().Replace(" ", "") + ",")
                    .Contains("," + tag + ","))
                .ToList();
        }


        public Product? GetProductById(int id)
        {
            return dbcontext.Products.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return dbcontext.SaveChanges() > 0;
        }

        public bool DeleteProduct(Product product)
        {
            dbcontext.Products.Remove(product);
            return dbcontext.SaveChanges() > 0;
        }

        public bool AddProduct(Product product)
        {
            try
            {
                dbcontext.Products.Add(product);
                return dbcontext.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return new List<Order>();

            status = status.Trim().ToLower();

            return dbcontext.Orders
                .Where(o => o.Status.ToLower() == status)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User) // Including User Info too with every order
                .OrderByDescending(o => o.Id)
                .ToList();
        }

        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var order = dbcontext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                    return false;

                newStatus = newStatus.Trim().ToUpper();

                if (order.Status == newStatus)
                    return true;

                order.Status = newStatus;

                return dbcontext.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}