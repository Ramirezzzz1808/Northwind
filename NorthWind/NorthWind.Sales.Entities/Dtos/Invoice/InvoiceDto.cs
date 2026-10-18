namespace NorthWind.Sales.Entities.Dtos.Invoice;

/// <summary>
/// Factura reconstruida: encabezado y detalle de una orden de venta guardada.
/// </summary>
public class InvoiceDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }

    public string CustomerId { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string Identification { get; set; } = "";
    public string Email { get; set; } = "";

    public string ShipAddress { get; set; } = "";
    public string ShipCity { get; set; } = "";
    public string ShipCountry { get; set; } = "";
    public string ShipPostalCode { get; set; } = "";

    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Total { get; set; }

    public List<InvoiceLineDto> Lines { get; set; } = [];
}

public class InvoiceLineDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public decimal LineTotal => Math.Round(UnitPrice * Quantity, 2);
}
