using System;
using System.Collections.Generic;

namespace TradingCompany.DTO
{
    public class Order
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsActive { get; set; }

      
        public Employee? Employee { get; set; }
        public Category? Category { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public override string ToString()
        {
            return $"{OrderId}: Date: {OrderDate.ToShortDateString()}, Total: {TotalAmount:C}, Active: {IsActive}";
        }
    }
}