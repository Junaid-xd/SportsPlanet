using Microsoft.EntityFrameworkCore;
using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.Services
{
    public class ProductService
    {
        private readonly DbService dbService;

        public ProductService()
        {
            var context = new EadMidsContext();  
            dbService = new DbService(context);   
        }
        public List<Product> GetAllProducts()
        {
            return dbService.GetAllProducts();
        }

        public List<Product> GetProductsByTag(String tag)
        {
            return dbService.GetProductsByTag(tag);
        }

        public Product? GetProductById(int id)
        {
            return dbService.GetProductById(id);
        }


        public bool UpdateProduct(Product updatedProduct)
        {
            var product = dbService.GetProductById(updatedProduct.Id);

            if (product == null) return false;

            product.ProductName = updatedProduct.ProductName;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;
            product.ImgPath = updatedProduct.ImgPath;
            product.Tags = updatedProduct.Tags;

            return dbService.SaveChanges();
        }

        public bool DeleteProduct(int productId)
        {
            var product = dbService.GetProductById(productId);

            if (product == null) return false;

            return dbService.DeleteProduct(product);
        }

        public bool AddProduct(Product product)
        {
            return dbService.AddProduct(product);
        }
       

    }
}
