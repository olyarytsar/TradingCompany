using System.Collections.Generic;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IAuthManagerWarehouse
    {
        bool Login(string username, string password);
        void Logout(Employee employee);
        Employee GetEmployeeByLogin(string login);
        Employee GetEmployeeById(int id);
        List<Employee> GetAllEmployees();
    }
}
