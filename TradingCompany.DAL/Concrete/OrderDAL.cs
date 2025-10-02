using AutoMapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.DAL.Concrete
{
    public class OrderDAL : GenericDAL<Order>, IOrderDAL
    {
        public OrderDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override Order Create(Order entity)
        {
            throw new NotImplementedException();
        }
        public override List<Order> GetAll()
        {
            throw new NotImplementedException();
        }
        public override Order GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Order Update(Order entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }
       
    }
}
