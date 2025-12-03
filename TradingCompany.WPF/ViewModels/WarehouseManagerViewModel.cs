using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands; 
using TradingCompany.WPF.Templates; 

namespace TradingCompany.WPF.ViewModels
{
    public class WarehouseManagerViewModel : ViewModelBase
    {
        private readonly IProductManager _productManager;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        private Product _selectedProduct;
        private Employee _currentUser;

        private string _sortOrder = "Name_Asc"; 
        public string SortOrder
        {
            get => _sortOrder;
            set
            {
                _sortOrder = value;
                OnPropertyChanged();
                LoadData(); 
            }
        }

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
        public ICommand SortCommand { get; } 

        public WarehouseManagerViewModel(IProductManager productManager)
        {
            _productManager = productManager;

            RefreshCommand = new RelayCommand(obj => LoadData());

            SortCommand = new RelayCommand<string>(sortKey => SortOrder = sortKey);

            LoadData();
        }

        private void LoadData()
        {
            Products.Clear();
           
            var productsFromDb = _productManager.GetProducts(SearchText, SortOrder);

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