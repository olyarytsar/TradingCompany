using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{OrderItemId}: Order {OrderId}, Product {ProductId}, Quantity: {Quantity}";
        }
    }
}
