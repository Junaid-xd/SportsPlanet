
using Microsoft.Win32;
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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

            // reset errors
            EditNameError.Text = "";
            EditPriceError.Text = "";
            EditQuantityError.Text = "";
            EditImageError.Text = "";

            NameBox.BorderBrush = Brushes.Gray;
            PriceBox.BorderBrush = Brushes.Gray;
            QuantityBox.BorderBrush = Brushes.Gray;
            ImageBox.BorderBrush = Brushes.Gray;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                EditNameError.Text = "Name is required";
                NameBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            if (!decimal.TryParse(PriceBox.Text, out decimal price) || price <= 0)
            {
                EditPriceError.Text = "Valid price required";
                PriceBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            if (!int.TryParse(QuantityBox.Text, out int qty) || qty < 0)
            {
                EditQuantityError.Text = "Valid quantity required";
                QuantityBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(ImageBox.Text))
            {
                EditImageError.Text = "Image is required";
                ImageBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            if (hasError) return;

            selectedProduct.ProductName = NameBox.Text;
            selectedProduct.Price = price;
            selectedProduct.Quantity = qty;
            selectedProduct.ImgPath = ImageBox.Text;
            selectedProduct.Tags = TagsBox.Text;

            bool success = productService.UpdateProduct(selectedProduct);

            if (success)
            {
                EditOverlay.Visibility = Visibility.Collapsed;
                ShowSuccess("Product updated successfully!");
            }
            else
            {
                ShowError("Update failed!");
            }
        }

        private async void ShowSuccess(string msg)
        {
            Toast.Background = new SolidColorBrush(Color.FromRgb(34, 197, 94));
            ToastText.Text = msg;
            Toast.Visibility = Visibility.Visible;

            await Task.Delay(2000);
            Toast.Visibility = Visibility.Collapsed;
        }

        private async void ShowError(string msg)
        {
            Toast.Background = new SolidColorBrush(Color.FromRgb(239, 68, 68));
            ToastText.Text = msg;
            Toast.Visibility = Visibility.Visible;

            await Task.Delay(2000);
            Toast.Visibility = Visibility.Collapsed;
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
                //Filter = "Image Files|*.jpg;*.png;*.jpeg"
                Filter = "Image Files|*.jpg;*.png;*.jpeg;*.webp"
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


        private void QtyText_Loaded(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBlock;
            if (tb == null) return;

            var product = tb.DataContext as Product;
            if (product == null) return;

            tb.Inlines.Clear();

            tb.Inlines.Add(new Run("Qty: ")
            {
                Foreground = Brushes.White
            });

            tb.Inlines.Add(new Run(product.Quantity.ToString())
            {
                Foreground = product.Quantity == 0 ? Brushes.Red : Brushes.LightGreen,
                FontWeight = FontWeights.SemiBold
            });

            if (product.Quantity == 0)
            {
                tb.Foreground = Brushes.Red;
            }
            else
            {
                tb.Foreground = Brushes.LightGreen;
            }
        }

        private void AddProductClick(object sender, RoutedEventArgs e)
        {
            // reset errors + borders
            AddNameError.Text = "";
            AddPriceError.Text = "";
            AddQuantityError.Text = "";
            AddImageError.Text = "";

            AddNameBox.BorderBrush = Brushes.Gray;
            AddPriceBox.BorderBrush = Brushes.Gray;
            AddQuantityBox.BorderBrush = Brushes.Gray;
            AddImageBox.BorderBrush = Brushes.Gray;

            bool hasError = false;

            // Name
            if (string.IsNullOrWhiteSpace(AddNameBox.Text))
            {
                AddNameError.Text = "Name is required";
                AddNameBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Price
            if (!decimal.TryParse(AddPriceBox.Text, out decimal price) || price <= 0)
            {
                AddPriceError.Text = "Valid price required";
                AddPriceBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Quantity
            if (!int.TryParse(AddQuantityBox.Text, out int qty) || qty < 0)
            {
                AddQuantityError.Text = "Valid quantity required";
                AddQuantityBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Image
            if (string.IsNullOrWhiteSpace(AddImageBox.Text))
            {
                AddImageError.Text = "Product image is required";
                AddImageBox.BorderBrush = Brushes.Red;
                hasError = true;
            }
            else if (!System.IO.File.Exists(AddImageBox.Text))
            {
                AddImageError.Text = "Image file not found";
                AddImageBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            if (hasError) return;

            var newProduct = new Product
            {
                ProductName = AddNameBox.Text,
                Price = price,
                Quantity = qty,
                ImgPath = AddImageBox.Text,
                Tags = AddTagsBox.Text,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            bool success = productService.AddProduct(newProduct);

            if (success)
            {
                Products.Add(newProduct);
                AddOverlay.Visibility = Visibility.Collapsed;
                ClearAddFields();

                ShowSuccess("Product added successfully!");
            }
            else
            {
                ShowError("Failed to add product");
            }
        }
    }
}
