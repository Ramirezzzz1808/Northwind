namespace NorthWind.Sales.Entities.Dtos.Invoice;

/// <summary>
/// Fila del resultado de búsqueda de facturas.
/// </summary>
public class OrderSearchItemDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerId { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string Identification { get; set; } = "";
    public decimal Total { get; set; }
}
