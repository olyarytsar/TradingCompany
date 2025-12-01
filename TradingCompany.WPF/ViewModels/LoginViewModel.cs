using System;
using System.Windows; // Потрібно для Application
using System.Windows.Controls;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;

namespace TradingCompany.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthManager _authManager;

        private string _login;
        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(); }
        }

        // Подія успішного входу
        public Action<Employee> LoginSuccess { get; set; }

        public ICommand LoginCommand { get; }

        // Змінили назву на ExitCommand для ясності
        public ICommand ExitCommand { get; }

        public LoginViewModel(IAuthManager authManager)
        {
            _authManager = authManager;

            LoginCommand = new RelayCommand(ExecuteLogin);

            // Ця команда повністю закриває програму
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        private void ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox?.Password;

            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter credentials.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var employee = _authManager.Login(Login, password);

                if (employee != null)
                {
                    if (_authManager.IsWarehouseManager(employee))
                    {
                        LoginSuccess?.Invoke(employee);
                    }
                    else
                    {
                        MessageBox.Show("Access Denied.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid login/password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}