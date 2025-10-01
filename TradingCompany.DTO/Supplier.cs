using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Brand { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return $"{SupplierId}: {Brand} - {Phone} - {Email}";
        }
    }
}
