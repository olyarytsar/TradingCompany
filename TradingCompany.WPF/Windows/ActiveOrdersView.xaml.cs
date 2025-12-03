using System.Windows;
using System.Windows.Controls;

namespace TradingCompany.WPF.Windows
{
    public partial class ActiveOrdersView : UserControl
    {
        public ActiveOrdersView()
        {
            InitializeComponent();
        }
        private void CloseDetails_Click(object sender, RoutedEventArgs e)
        {
            
            OrdersGrid.SelectedItem = null;
        }
    }
}