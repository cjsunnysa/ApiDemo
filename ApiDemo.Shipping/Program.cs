var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapPost("/api/packing-orders", async (http) =>
{
    var packingOrder = await http.Request.ReadFromJsonAsync<PackingOrder>(http.RequestAborted);
    
    http.Response.StatusCode = StatusCodes.Status204NoContent;
});

app.Run();

internal class PackingOrder
{
    public string CustomerName { get; init; } = string.Empty;
    public string DeliveryAddressLine1 { get; init; } = string.Empty;
    public string DeliveryAddressLine2 { get; init; } = string.Empty;
    public string DeliveryAddressSuburb { get; init; } = string.Empty;
    public string DeliveryAddressProvince { get; init; } = string.Empty;
    public int DeliveryAddressPostalCode { get; init; }
    public IEnumerable<PackingItem> Items { get; init; } = Array.Empty<PackingItem>();
}

internal class PackingItem
{
    public string Sku { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public double Quanity { get; init; }
}
