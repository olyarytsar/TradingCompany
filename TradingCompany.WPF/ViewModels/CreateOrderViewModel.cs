using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;  
using TradingCompany.WPF.Templates; 

namespace TradingCompany.WPF.ViewModels
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => (Product?.Price ?? 0) * Quantity;
    }

    public class CreateOrderViewModel : ViewModelBase
    {
        private readonly IProductManager _productManager;
        private readonly ISupplyManager _supplyManager;
        private Employee _currentUser;

        public ObservableCollection<Supplier> Suppliers { get; set; } = new ObservableCollection<Supplier>();
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<CartItem> OrderItems { get; set; } = new ObservableCollection<CartItem>();

        private Supplier _selectedSupplier;
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (_selectedSupplier != value)
                {
                    _selectedSupplier = value;
                    OnPropertyChanged();
                    LoadProductsForSupplier();
                    OrderItems.Clear();
                    OnPropertyChanged(nameof(TotalOrderSum));
                }
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();

                ClearErrors(nameof(SelectedProduct));
                if (_selectedProduct == null)
                {
                    AddError(nameof(SelectedProduct), "Product is required");
                }
            }
        }

        private int _quantityToAdd = 1;
        public int QuantityToAdd
        {
            get => _quantityToAdd;
            set
            {
                _quantityToAdd = value;
                OnPropertyChanged();

                ClearErrors(nameof(QuantityToAdd));
                if (_quantityToAdd <= 0)
                {
                    AddError(nameof(QuantityToAdd), "Quantity must be greater than 0");
                }
                else if (_quantityToAdd > 1000)
                {
                    AddError(nameof(QuantityToAdd), "Quantity allows max 1000 items");
                }
            }
        }

        private CartItem _selectedCartItem;
        public CartItem SelectedCartItem
        {
            get => _selectedCartItem;
            set { _selectedCartItem = value; OnPropertyChanged(); }
        }

        public decimal TotalOrderSum => OrderItems.Sum(x => x.TotalPrice);

        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand SubmitOrderCommand { get; }

        public CreateOrderViewModel(IProductManager productManager, ISupplyManager supplyManager)
        {
            _productManager = productManager;
            _supplyManager = supplyManager;

            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand(RemoveItem);
            SubmitOrderCommand = new RelayCommand(SubmitOrder);

            LoadSuppliers();
        }

        public void SetCurrentUser(Employee employee)
        {
            _currentUser = employee;
        }

        private void LoadSuppliers()
        {
            Suppliers.Clear();
            var list = _supplyManager.GetAllSuppliers();
            foreach (var s in list) Suppliers.Add(s);
        }

        private void LoadProductsForSupplier()
        {
            Products.Clear();
            if (SelectedSupplier != null)
            {
                var allProducts = _productManager.GetAllProducts();
                var filtered = allProducts.Where(p => p.SupplierId == SelectedSupplier.SupplierId);
                foreach (var p in filtered) Products.Add(p);
            }
        }

        private void AddItem(object obj)
        {
            if (SelectedProduct == null || QuantityToAdd <= 0 || HasErrors)
            {
                MessageBox.Show("Please correct errors before adding item (check red fields).", "Validation Error");
                return;
            }

            var existing = OrderItems.FirstOrDefault(x => x.Product.ProductId == SelectedProduct.ProductId);
            if (existing != null)
            {
                existing.Quantity += QuantityToAdd;
                OrderItems.Remove(existing);
                OrderItems.Add(existing);
            }
            else
            {
                OrderItems.Add(new CartItem
                {
                    Product = SelectedProduct,
                    Quantity = QuantityToAdd
                });
            }

            OnPropertyChanged(nameof(TotalOrderSum));
        }

        private void RemoveItem(object obj)
        {
            if (SelectedCartItem != null)
            {
                OrderItems.Remove(SelectedCartItem);
                OnPropertyChanged(nameof(TotalOrderSum));
            }
        }

        private void SubmitOrder(object obj)
        {
            if (_currentUser == null) return;
            if (SelectedSupplier == null || OrderItems.Count == 0)
            {
                MessageBox.Show("Order is empty or supplier not selected.");
                return;
            }

            try
            {
                var itemsDict = new Dictionary<int, int>();
                foreach (var item in OrderItems)
                {
                    if (itemsDict.ContainsKey(item.Product.ProductId))
                        itemsDict[item.Product.ProductId] += item.Quantity;
                    else
                        itemsDict.Add(item.Product.ProductId, item.Quantity);
                }

                _supplyManager.CreateSupplyOrder(_currentUser.EmployeeId, SelectedSupplier.SupplierId, itemsDict);

                MessageBox.Show($"Order created successfully! Total: {TotalOrderSum:C}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                
                OrderItems.Clear();
                SelectedProduct = null;
                QuantityToAdd = 1;
                OnPropertyChanged(nameof(TotalOrderSum));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}