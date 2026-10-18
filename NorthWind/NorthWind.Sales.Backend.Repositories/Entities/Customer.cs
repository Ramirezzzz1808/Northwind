namespace NorthWind.Sales.Backend.Repositories.Entities;

public class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal CurrentBalance { get; set; }

    // Cédula (10 dígitos) o RUC (13 dígitos).
    public string Identification { get; set; }
    public string Email { get; set; }
}
