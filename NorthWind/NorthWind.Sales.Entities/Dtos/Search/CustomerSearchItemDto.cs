namespace NorthWind.Sales.Entities.Dtos.Search;

public class CustomerSearchItemDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Identification { get; set; } = "";
    public string Email { get; set; } = "";
    public decimal CurrentBalance { get; set; }
}
