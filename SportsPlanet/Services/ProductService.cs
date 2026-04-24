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
    }
}
