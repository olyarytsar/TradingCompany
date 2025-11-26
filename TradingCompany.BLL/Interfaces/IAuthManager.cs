using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IAuthManager
    {
        Employee Login(string login, string password);

        bool IsWarehouseManager(Employee employee);
    }
}