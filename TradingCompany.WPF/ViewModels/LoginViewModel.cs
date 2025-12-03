using System;
using System.Threading.Tasks; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;  
using TradingCompany.WPF.Templates; 

namespace TradingCompany.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthManager _authManager;

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();

                ClearErrors(nameof(Login));
                if (string.IsNullOrWhiteSpace(_login))
                {
                    AddError(nameof(Login), "Login cannot be empty");
                }
            }
        }

        public Action<Employee> LoginSuccess { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        public LoginViewModel(IAuthManager authManager)
        {
            _authManager = authManager;

            LoginCommand = new AsyncRelayCommand(ExecuteLogin);

            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        private async Task ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox?.Password;

            
            if (HasErrors || string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please check your login and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                
                Employee employee = await Task.Run(() => _authManager.Login(Login, password));

                if (employee != null)
                {
                    if (_authManager.IsWarehouseManager(employee))
                    {
                        LoginSuccess?.Invoke(employee);
                    }
                    else
                    {
                        MessageBox.Show("Access Denied. You do not have the Warehouse Manager role.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid login or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"System Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}