using System.Windows;
using TradingCompany.WPF.ViewModels;

namespace TradingCompany.WPF.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel vm)
        {
            InitializeComponent();
            DataContext = vm; 
        }
    }
}