using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TradingCompany.Models;

public partial class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("order_date", TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column("total_amount", TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Orders")]
    public virtual Employee Employee { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
