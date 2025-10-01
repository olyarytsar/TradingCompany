using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }

        public override string ToString()
        {
            return $"{ProductId}: {Name} - {Price:C} (Stock: {QuantityInStock})";
        }
    }
}