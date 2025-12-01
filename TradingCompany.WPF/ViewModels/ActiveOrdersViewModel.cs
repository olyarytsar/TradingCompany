using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;

namespace TradingCompany.WPF.ViewModels
{
    public class ActiveOrdersViewModel : ViewModelBase
    {
        private readonly ISupplyManager _supplyManager;

        public ObservableCollection<Order> ActiveOrders { get; set; } = new ObservableCollection<Order>();

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set { _selectedOrder = value; OnPropertyChanged(); }
        }

        public ICommand CancelOrderCommand { get; }
        public ICommand SaveChangesCommand { get; }

        public ActiveOrdersViewModel(ISupplyManager supplyManager)
        {
            _supplyManager = supplyManager;
            CancelOrderCommand = new RelayCommand(CancelOrder);
            SaveChangesCommand = new RelayCommand(SaveChanges);

            LoadOrders();
        }

        private void LoadOrders()
        {
            ActiveOrders.Clear();
            var orders = _supplyManager.GetActiveSupplyOrders();
            foreach (var order in orders)
            {
                ActiveOrders.Add(order);
            }
        }

        private void CancelOrder(object obj)
        {
            if (SelectedOrder == null) return;

            var result = MessageBox.Show($"Ви впевнені, що хочете скасувати замовлення #{SelectedOrder.OrderId}?",
                                         "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Логіка скасування: ставимо IsActive = false
                SelectedOrder.IsActive = false;
                _supplyManager.UpdateOrder(SelectedOrder);
                LoadOrders(); // Оновлюємо список (скасоване замовлення зникне зі списку активних)
            }
        }

        private void SaveChanges(object obj)
        {
            if (SelectedOrder != null)
            {
                try
                {
                    _supplyManager.UpdateOrder(SelectedOrder);
                    MessageBox.Show("Зміни збережено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Помилка: {ex.Message}");
                }
            }
        }
    }
}