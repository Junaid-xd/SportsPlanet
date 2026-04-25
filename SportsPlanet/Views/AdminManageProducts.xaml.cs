
using Microsoft.Win32;
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace SportsPlanet.Views
{
    public partial class AdminManageProducts : Page
    {
        private ProductService productService;
        private Frame frame;


        public ObservableCollection<Product> Products { get; set; }
            = new ObservableCollection<Product>();

        private Product? selectedProduct;

        public AdminManageProducts(Frame fra)
        {
            InitializeComponent();
            frame = fra;

            productService = new ProductService();

            HeaderControl.SetFrame(frame);
            HeaderControl.SetActive("Manage Products");

            LoadProducts();
        }

        private void LoadProducts()
        {
            Products.Clear();

            foreach (var p in productService.GetAllProducts())
                Products.Add(p);

            ProductsItemsControl.ItemsSource = Products;
        }

        // ================= EDIT =================
        private void EditProductClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            selectedProduct = btn?.Tag as Product;

            if (selectedProduct == null) return;

            NameBox.Text = selectedProduct.ProductName;
            PriceBox.Text = selectedProduct.Price.ToString();
            QuantityBox.Text = selectedProduct.Quantity.ToString();
            ImageBox.Text = selectedProduct.ImgPath;
            TagsBox.Text = selectedProduct.Tags;

            EditOverlay.Visibility = Visibility.Visible;
        }

        private void CloseOverlay(object sender, RoutedEventArgs e)
        {
            EditOverlay.Visibility = Visibility.Collapsed;
        }

        private void UpdateProductClick(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null) return;

            selectedProduct.ProductName = NameBox.Text;
            selectedProduct.Price = decimal.Parse(PriceBox.Text);
            selectedProduct.Quantity = int.Parse(QuantityBox.Text);
            selectedProduct.ImgPath = ImageBox.Text;
            selectedProduct.Tags = TagsBox.Text;

            bool success = productService.UpdateProduct(selectedProduct);

            if (success)
            {
                // UI auto updates because ObservableCollection + same object reference
                EditOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Update failed!");
            }
        }
        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null) return;

            bool success = productService.DeleteProduct(selectedProduct.Id);

            if (success)
            {
                Products.Remove(selectedProduct); // instant UI update
                EditOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Delete failed!");
            }
        }

        private void BrowseImageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Select Product Image";
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                ImageBox.Text = dialog.FileName;
            }
        }

        private void OpenAddProductPopup(object sender, RoutedEventArgs e)
        {
            AddOverlay.Visibility = Visibility.Visible;
        }

        private void CloseAddPopup(object sender, RoutedEventArgs e)
        {
            AddOverlay.Visibility = Visibility.Collapsed;
        }

        private void BrowseAddImageClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.png;*.jpeg"
            };

            if (dialog.ShowDialog() == true)
            {
                AddImageBox.Text = dialog.FileName;
            }
        }

        private void ClearAddFields()
        {
            AddNameBox.Text = "";
            AddPriceBox.Text = "";
            AddQuantityBox.Text = "";
            AddImageBox.Text = "";
            AddTagsBox.Text = "";
        }

        private void AddProductClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var newProduct = new Product
                {
                    ProductName = AddNameBox.Text,
                    Price = decimal.Parse(AddPriceBox.Text),
                    Quantity = int.Parse(AddQuantityBox.Text),
                    ImgPath = AddImageBox.Text,
                    Tags = AddTagsBox.Text,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };

                bool success = productService.AddProduct(newProduct);

                if (success)
                {
                    Products.Add(newProduct); // instant UI update
                    AddOverlay.Visibility = Visibility.Collapsed;

                    ClearAddFields();
                }
                else
                {
                    MessageBox.Show("Failed to add product");
                }
            }
            catch
            {
                MessageBox.Show("Invalid input!");
            }
        }
    }
}
