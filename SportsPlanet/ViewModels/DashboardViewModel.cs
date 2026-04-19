using SportsPlanet.Models;
using SportsPlanet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.ViewModels
{
    public class DashboardViewModel
    {
        private ProductService productService = new ProductService();

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public DashboardViewModel()
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            var list = productService.GetAllProducts();

            Products.Clear();
            foreach (var item in list)
                Products.Add(item);
        }
    }
}
