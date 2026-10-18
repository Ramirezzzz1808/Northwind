namespace NorthWind.Sales.Frontend.Views.Resources;

/// <summary>
/// Mensajes personalizados del punto de venta.
/// </summary>
public static class PosMessages
{
    public const string PageTitle = "Punto de venta";
    public const string CustomerSection = "Cliente";
    public const string ShippingSection = "Datos de envío";
    public const string DetailSection = "Detalle de la venta";

    public const string SearchCustomerButton = "Buscar cliente";
    public const string SearchProductButton = "Agregar producto";
    public const string ChangeProductButton = "Cambiar";
    public const string RemoveProductButton = "Quitar";
    public const string SaveOrderButton = "Guardar venta";
    public const string CloseButton = "Cerrar";
    public const string SelectButton = "Seleccionar";
    public const string SearchButton = "Buscar";
    public const string PreviousButton = "Anterior";
    public const string NextButton = "Siguiente";

    public const string CustomerNameLabel = "Cliente";
    public const string IdentificationLabel = "Cédula";
    public const string EmailLabel = "Correo electrónico";
    public const string ProductLabel = "Producto";
    public const string UnitPriceLabel = "Precio";
    public const string QuantityLabel = "Cantidad";
    public const string StockLabel = "Stock";
    public const string LineTotalLabel = "Importe";
    public const string SubtotalLabel = "Subtotal";
    public const string TaxLabel = "IVA (15%)";
    public const string TotalLabel = "Total";

    public const string CustomerSearchTitle = "Búsqueda de clientes";
    public const string ProductSearchTitle = "Búsqueda de productos";
    public const string SearchPlaceholder = "Escriba para buscar...";
    public const string NoResults = "No se encontraron resultados.";
    public const string Loading = "Cargando...";
    public const string NoLines = "Aún no hay productos en la venta.";

    public const string SearchFailed =
        "No se pudo realizar la búsqueda. Intente nuevamente.";
    public const string CustomerRequired =
        "Debe seleccionar un cliente antes de guardar la venta.";
    public const string DetailRequired =
        "Debe agregar al menos un producto a la venta.";
    public const string DuplicateProduct =
        "El producto '{0}' ya está en la venta. Modifique su cantidad en lugar de agregarlo de nuevo.";
    public const string StockExceeded =
        "Solo hay {0} unidades disponibles de '{1}'. La cantidad se ajustó al stock.";
    public const string QuantityMinimum =
        "La cantidad mínima es 1. Para eliminar el producto use el botón Quitar.";
    public const string ProductAdded = "Producto '{0}' agregado a la venta.";
    public const string ProductRemoved = "Producto '{0}' quitado de la venta.";
    public const string ProductChanged = "Producto cambiado por '{0}'.";

    public const string OrderSaved =
        "Transacción completada: la venta #{0} se guardó correctamente.";
    public const string OrderFailed =
        "La transacción no se completó y no se guardó ningún cambio. " +
        "Es posible que el stock haya cambiado; vuelva a buscar los productos e intente de nuevo.";
}
