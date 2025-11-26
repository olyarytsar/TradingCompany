using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.DAL.Concrete
{
    public class EmployeeDAL : GenericDAL<Employee>, IEmployeeDAL
    {
        public EmployeeDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override Employee Create(Employee entity)
        {
            
            throw new NotImplementedException();
        }

        public override List<Employee> GetAll()
        {
            
            throw new NotImplementedException();
        }

        public override Employee GetById(int id)
        {
            
            throw new NotImplementedException();
        }

        public override Employee Update(Employee entity)
        {
           
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            
            throw new NotImplementedException();
        }
        public bool Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public Employee GetByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
