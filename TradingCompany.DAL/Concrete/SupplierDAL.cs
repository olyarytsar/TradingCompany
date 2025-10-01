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
    public class SupplierDAL : GenericDAL<Supplier>, ISupplierDAL
    {
        public SupplierDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override Supplier Create(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public override List<Supplier> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Supplier GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Supplier Update(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
