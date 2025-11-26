using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;
using TradingCompany.WPF.Windows;


namespace TradingCompany.WPF.ViewModels
{
    public class WarehouseManagerViewModel : ViewModelBase
    {
        private readonly IProductManager _productManager;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        private Product _selectedProduct;
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

        
        public ICommand LogoutCommand { get; }
        public ICommand RefreshCommand { get; }

        public WarehouseManagerViewModel(IProductManager productManager)
        {
            _productManager = productManager;

            LogoutCommand = new RelayCommand(ExecuteLogout);
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

        private void ExecuteLogout(object obj)
        {
            
            var loginWindow = App.Services.GetService(typeof(LoginWindow)) as Window;
            loginWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is WarehouseManagerWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}