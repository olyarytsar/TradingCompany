using System.Collections.Generic;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IWarehouseManager
    {
        List<Product> ViewProducts(string search = null, string sortBy = null);
        Order CreateOrder(Employee employee, int supplierId, Dictionary<int, int> productQuantities);
        List<Order> ViewActiveOrders();
        Order UpdateOrder(Order order);
    }
}

