namespace TradingCompany.DTO
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product? Product { get; set; }

        public override string ToString()
        {
            return $"{OrderItemId}: Product {Product?.Name}, Qty: {Quantity}";
        }
    }
}