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
    public class RoleDAL : GenericDAL<Role>, IRoleDAL
    {
        public RoleDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override Role Create(Role entity)
        {
            throw new NotImplementedException();
        }

        public override List<Role> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Role GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Role Update(Role entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
