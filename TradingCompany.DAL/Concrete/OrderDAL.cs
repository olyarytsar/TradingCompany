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
        public override List<Order> GetAll()
        {
            var orders = new List<Order>();

            using (var connection = new SqlConnection(_connStr))
            {
                connection.Open();
                string sql = "SELECT * FROM dbo.Orders";
                using (var command = new SqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var order = new Order
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("order_id")),
                            EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("total_amount"))
                        };

                        orders.Add(order);
                    }
                }
            }

            return orders;
        }

    }
}
