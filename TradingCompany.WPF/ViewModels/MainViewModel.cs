using System;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;

namespace TradingCompany.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private ViewModelBase _currentViewModel;
        private Employee _currentUser;
        private bool _isLoggedIn;

        // Властивість для приховування меню в XAML
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set { _isLoggedIn = value; OnPropertyChanged(); }
        }

        public Employee CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        // Команди
        public ICommand NavigateToWarehouseCommand { get; }
        public ICommand NavigateToCreateOrderCommand { get; }
        public ICommand NavigateToActiveOrdersCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            NavigateToWarehouseCommand = new RelayCommand(_ => NavigateTo<WarehouseManagerViewModel>());
            NavigateToCreateOrderCommand = new RelayCommand(_ => NavigateTo<CreateOrderViewModel>());
            NavigateToActiveOrdersCommand = new RelayCommand(_ => NavigateTo<ActiveOrdersViewModel>());
            LogoutCommand = new RelayCommand(Logout);

            // ЗАПУСК: Одразу показуємо екран Логіну
            ShowLoginScreen();
        }

        private void ShowLoginScreen()
        {
            IsLoggedIn = false; // Ховає меню
            var loginVm = _serviceProvider.GetRequiredService<LoginViewModel>();

            // Підписуємось на успішний вхід
            loginVm.LoginSuccess = (user) =>
            {
                CurrentUser = user;
                IsLoggedIn = true; // Показує меню
                NavigateTo<WarehouseManagerViewModel>(); // Переходимо на склад
            };

            CurrentViewModel = loginVm;
        }

        private void NavigateTo<T>() where T : ViewModelBase
        {
            var viewModel = _serviceProvider.GetRequiredService<T>();

            if (viewModel is CreateOrderViewModel createOrderVm)
                createOrderVm.SetCurrentUser(CurrentUser);
            else if (viewModel is WarehouseManagerViewModel warehouseVm)
                warehouseVm.SetCurrentUser(CurrentUser);

            CurrentViewModel = viewModel;
        }

        private void Logout(object obj)
        {
            CurrentUser = null;
            ShowLoginScreen(); // Повертаємось на екран входу
        }
    }
}