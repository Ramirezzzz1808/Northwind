namespace NorthWind.Sales.Frontend.Views.Resources;

/// <summary>
/// Mensajes personalizados de la consulta de facturas.
/// </summary>
public static class InvoiceMessages
{
    public const string PageTitle = "Facturas";
    public const string SearchInvoiceButton = "Buscar factura";
    public const string SearchInvoiceTitle = "Búsqueda de facturas";
    public const string PrintButton = "Imprimir / Guardar PDF";
    public const string NewSaleButton = "Nueva venta";
    public const string ViewInvoiceLink = "Ver factura";

    public const string InvoiceNumberLabel = "Factura N°";
    public const string DateLabel = "Fecha";
    public const string CustomerLabel = "Cliente";
    public const string IdentificationLabel = "Cédula";
    public const string EmailLabel = "Correo electrónico";
    public const string ShippingLabel = "Dirección de envío";
    public const string ProductLabel = "Producto";
    public const string UnitPriceLabel = "Precio";
    public const string QuantityLabel = "Cantidad";
    public const string LineTotalLabel = "Importe";
    public const string SubtotalLabel = "Subtotal";
    public const string TaxLabel = "IVA (15%)";
    public const string TotalLabel = "Total";

    public const string EmptyState =
        "Busque una factura por su número o por el cliente.";
    public const string Loading = "Cargando factura...";
    public const string NotFound =
        "La factura #{0} no existe o no se pudo cargar. Verifique el número e intente de nuevo.";
}
