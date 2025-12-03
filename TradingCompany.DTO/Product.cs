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

       
        public Category? Category { get; set; }
        public Supplier? Supplier { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Price:C}";
        }
    }
}