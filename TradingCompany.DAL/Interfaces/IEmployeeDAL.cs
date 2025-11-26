using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DAL.Interfaces
{
    public interface IEmployeeDAL : IGenericDAL<Employee>
    {
            bool Login(string login, string password);
            Employee GetByLogin(string login);
        
    }
}
