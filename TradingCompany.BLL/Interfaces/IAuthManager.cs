using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IAuthManager
    {
       
        bool Login(string login, string password);

        Employee GetEmployeeByLogin(string login);
        Employee GetEmployeeById(int id);
        List<Employee> GetEmployees();
        bool HasRole(Employee employee, RoleType roleType);
    }
}