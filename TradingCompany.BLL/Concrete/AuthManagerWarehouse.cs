using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concrete
{
    public class AuthManagerWarehouse : IAuthManagerWarehouse
    {
        private readonly EmployeeDALEF _employeeDal;
        private Employee _loggedInEmployee;

        public AuthManagerWarehouse(EmployeeDALEF employeeDal)
        {
            _employeeDal = employeeDal;
        }

        public bool Login(string username, string password)
        {
            var employee = _employeeDal.GetAll()
                .FirstOrDefault(e => e.Login.Equals(username, StringComparison.OrdinalIgnoreCase)
                                     && e.Password == password);

            if (employee == null)
            {
                Console.WriteLine("Login failed: invalid credentials.");
                return false;
            }

            _loggedInEmployee = employee;
            Console.WriteLine($"Employee {employee.FirstName} logged in successfully at {DateTime.Now}.");
            return true;
        }

        public void Logout(Employee employee)
        {
            if (_loggedInEmployee == null)
            {
                Console.WriteLine("No employee is currently logged in.");
                return;
            }

            Console.WriteLine($"Employee {employee.FirstName} logged out at {DateTime.Now}.");
            _loggedInEmployee = null;
        }

        public Employee GetEmployeeByLogin(string login)
        {
            return _employeeDal.GetAll()
                .FirstOrDefault(e => e.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }

        public Employee GetEmployeeById(int id)
        {
            return _employeeDal.GetById(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeDal.GetAll();
        }
    }
}
