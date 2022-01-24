namespace ApiDemo.Api.Domain.Entities
{
    public class StockItem
    {
        public int Id { get; init; }
        public string Description { get; set; }
        public string Size { get; set; }
        public string Sku { get; set; }
    }
}