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
    public class OrderItemDAL : GenericDAL<OrderItem>, IOrderItemDAL
    {
        public OrderItemDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override OrderItem Create(OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public override List<OrderItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public override OrderItem GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override OrderItem Update(OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
