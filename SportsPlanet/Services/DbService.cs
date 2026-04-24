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

        //public bool PlaceOrder(Order order, List<CartItem> cartItems)
        //{
        //    using var transaction = dbcontext.Database.BeginTransaction();

        //    try
        //    {
        //        foreach (var item in cartItems)
        //        {
        //            order.OrderItems.Add(new OrderItem
        //            {
        //                ProductId = item.Product.Id,
        //                Quantity = item.Quantity,
        //                Price = item.Product.Price,
        //                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        //            });
        //        }

        //        dbcontext.Orders.Add(order);
        //        dbcontext.SaveChanges();

        //        transaction.Commit();
        //        return true;
        //    }
        //    catch
        //    {
        //        transaction.Rollback();
        //        return false;
        //    }
        //}


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
    }
}