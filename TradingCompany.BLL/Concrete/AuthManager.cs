using System;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
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

        public Employee Login(string login, string password)
        {
            
            bool isValid = _employeeDal.Login(login, password);

            if (isValid)
            {
               
                return _employeeDal.GetByLogin(login);
            }

            return null;
        }

        public bool IsWarehouseManager(Employee employee)
        {
            if (employee == null || employee.Role == null) return false;

            string role = employee.Role.RoleName;

            return role == "Admin" ||
                   role == "Manager" ||
                   role == "Casshier";


        }
    }
}