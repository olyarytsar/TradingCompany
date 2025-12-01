using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;

namespace TradingCompany.WPF.ViewModels
{
    public class WarehouseManagerViewModel : ViewModelBase
    {
        private readonly IProductManager _productManager;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        private Product _selectedProduct;
        private Employee _currentUser;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                LoadData();
            }
        }

       
        public ICommand RefreshCommand { get; }

        public WarehouseManagerViewModel(IProductManager productManager)
        {
            _productManager = productManager;

            RefreshCommand = new RelayCommand(obj => LoadData());

            LoadData();
        }

        private void LoadData()
        {
            Products.Clear();
            var productsFromDb = _productManager.GetProducts(SearchText, "Name_Asc");

            foreach (var p in productsFromDb)
            {
                Products.Add(p);
            }
        }

        public void SetCurrentUser(Employee employee)
        {
            _currentUser = employee;
        }
    }
}