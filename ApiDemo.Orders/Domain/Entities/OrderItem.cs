namespace ApiDemo.Api.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; init; }
        public StockItem Item { get; set; }
        public double Quantity { get; set; }
    }
}
