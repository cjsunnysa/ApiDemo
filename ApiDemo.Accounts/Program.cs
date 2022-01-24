var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("api/customers/{id}", async (int id) => 
    await Task.FromResult(
        id <= 0 ? Results.BadRequest(id) :
            id != 1 ? Results.NotFound(id) : 
                Results.Ok(CreateCustomer())
    )
);

app.Run();

static Customer CreateCustomer()
{
    return new Customer
    {
        Id = 1,
        TitleId = 1,
        FirstName = "Sammi",
        LastName = "Sosa",
        ResidentialAddresses = new[]
        {
            new Address
            {
                Id = 531,
                Line1 = "291 Oak Ave",
                Suburb = "Randburg",
                ProvinceId = 1,
                PostalCode = 2194,
                IsActive = false,
                IsPrimary = false,
                CreatedDate = new DateTime(1983, 11, 28, 13, 23, 45)
            },
            new Address
            {
                Id = 768,
                Line1 = "12 Leopards Leap Lane",
                Suburb = "Boskruin",
                ProvinceId = 1,
                PostalCode = 2190,
                IsActive = false,
                IsPrimary = false,
                CreatedDate= new DateTime(1989, 1, 15, 11, 03, 12)
            },
            new Address
            {
                Id = 10287,
                Line1 = "2 Almer Road",
                Suburb = "Woodbridge Island",
                ProvinceId = 2,
                PostalCode = 1710,
                IsActive = true,
                IsPrimary = true,
                CreatedDate= new DateTime(2002, 4, 3, 9, 56, 34)
            }
        },
    };
}

internal class Customer
{
    public int Id { get; init; } = 0;
    public int TitleId { get; init; } = 0;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public IEnumerable<Address> ResidentialAddresses { get; init; } = new List<Address>();
    public IEnumerable<Address> PostalAddresses { get; init; } = new List<Address>();
}

internal class Address
{
    public int Id { get; init; } = 0;
    public string Line1 { get; init; } = string.Empty;
    public string? Line2 { get; init; } = null;
    public string Suburb { get; init; } = string.Empty;
    public int ProvinceId { get; init; } = 0;
    public int PostalCode { get; init; } = 0;
    public bool IsPrimary { get; init; } = false;
    public bool IsActive { get; init; } = false;
    public DateTime CreatedDate { get; init; } = DateTime.Now;
}
