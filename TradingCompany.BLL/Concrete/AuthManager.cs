using System;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concrete
{
    public class AuthManager : IAuthManager
    {
        private readonly IEmployeeDAL _employeeDal;

        public AuthManager(IEmployeeDAL employeeDal)
        {
            _employeeDal = employeeDal;
        }

        public bool Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;

            return _employeeDal.Login(login, password);
        }

        public Employee GetEmployeeByLogin(string login)
        {
            return _employeeDal.GetByLogin(login);
        }

        public Employee GetEmployeeById(int id)
        {
            return _employeeDal.GetById(id);
        }

        public List<Employee> GetEmployees()
        {
            return _employeeDal.GetAll();
        }

        public bool HasRole(Employee employee, RoleType roleType)
        {
            if (employee == null || employee.Role == null)
                return false;

            return employee.Role.RoleName.Equals(roleType.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}