using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class Order
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"{OrderId}: Employee {Employee}, Date: {OrderDate.ToShortDateString()}, Total: {TotalAmount:C}, Active: {IsActive}";
        }
    }
}
