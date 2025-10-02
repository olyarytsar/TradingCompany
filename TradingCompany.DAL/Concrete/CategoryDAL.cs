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
    public class CategoryDAL : GenericDAL<Category>, ICategoryDAL
    {
        public CategoryDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override Category Create(Category entity)
        {
            throw new NotImplementedException();
        }

        public override List<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Category GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Category Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            
            throw new NotImplementedException();
        }
    }
}
