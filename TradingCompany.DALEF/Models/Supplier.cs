using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TradingCompany.DALEF.Models;

public partial class Supplier
{
    [Key]
    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("brand")]
    [StringLength(50)]
    [Unicode(false)]
    public string Brand { get; set; } = null!;

    [Column("phone")]
    [StringLength(50)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("address")]
    [StringLength(50)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
