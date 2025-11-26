using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.WPF.Commands;
using TradingCompany.WPF.Windows; // Переконайтеся, що LoginWindow знаходиться тут

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

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthManager authManager)
        {
            _authManager = authManager;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox?.Password;

            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both login and password.", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var employee = _authManager.Login(Login, password);

                if (employee != null)
                {
                    if (_authManager.IsWarehouseManager(employee))
                    {
                        MessageBox.Show($"Welcome, {employee.FirstName}! Logging in as Warehouse Manager.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // !!! ОСНОВНА ЗМІНА ТУТ !!!
                        // Ми шукаємо активне вікно логіну і ставимо йому DialogResult = true.
                        // Це автоматично закриває вікно і повідомляє App.xaml.cs, що можна відкривати головне вікно.
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window is LoginWindow)
                            {
                                window.DialogResult = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                       
                        string actualRole = employee.Role != null ? employee.Role.RoleName : "NULL (Роль не знайдена!)";

                        // Виводимо довжину, щоб побачити приховані пробіли (наприклад "Manager   ")
                        string debugInfo = $"Role: '{actualRole}', Length: {actualRole.Length}";

                        MessageBox.Show($"Доступ відхилено!\nСистема бачить вашу роль як:\n{debugInfo}\n\nОчікується: 'Manager'",
                                        "Debug Info", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid login or password.", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}