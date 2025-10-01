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
    public class ProductDAL : GenericDAL<Product>, IProductDAL
    {
        public ProductDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override Product Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public override List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Product Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
