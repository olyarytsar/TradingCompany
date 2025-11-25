using System;

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

        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }

        public override string ToString()
        {
            string categoryName = Category != null ? Category.CategoryName : $"CategoryId: {CategoryId}";
            string supplierName = Supplier != null ? Supplier.Brand : $"SupplierId: {SupplierId}";

            return $"{ProductId}: {Name} - {Price:C} (Stock: {QuantityInStock}), Category: {categoryName}, Supplier: {supplierName}";
        }

    }
}
