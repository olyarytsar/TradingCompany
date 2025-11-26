using System.Collections.Generic;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface ISupplyManager
    {
        List<Supplier> GetAllSuppliers();
        List<Order> GetActiveSupplyOrders();

        void CreateSupplyOrder(int employeeId, int supplierId, Dictionary<int, int> productQuantities);

        void UpdateOrder(Order order);
    }
}