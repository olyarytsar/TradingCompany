using System.Windows;
using TradingCompany.WPF.ViewModels;

namespace TradingCompany.WPF.Windows
{
    public partial class WarehouseManagerWindow : Window
    {
        public WarehouseManagerWindow(WarehouseManagerViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}