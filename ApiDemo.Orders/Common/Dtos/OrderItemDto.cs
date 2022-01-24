namespace ApiDemo.Api.Common
{
    public class OrderItemDto
    {
        public int Id { get; init; }
        public string ItemDescription { get; init; }
        public string ItemSize { get; init; }
        public double Quantity { get; init; }
    }
}
