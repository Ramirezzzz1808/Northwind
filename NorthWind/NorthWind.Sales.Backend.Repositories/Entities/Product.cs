namespace NorthWind.Sales.Backend.Repositories.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public short UnitsInStock { get; set; }
    public decimal UnitPrice { get; set; }

    // Token de concurrencia optimista (rowversion de SQL Server).
    public byte[] RowVersion { get; set; }
}
