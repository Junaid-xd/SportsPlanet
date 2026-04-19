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
        private DbService dbService = new DbService();

        public List<Product> GetAllProducts()
        {
            return dbService.GetAllProducts();
        }
    }
}
