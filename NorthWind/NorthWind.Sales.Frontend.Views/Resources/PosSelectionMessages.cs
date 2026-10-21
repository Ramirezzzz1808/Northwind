namespace NorthWind.Sales.Frontend.Views.Resources;

/// <summary>
/// Mensajes de la selección múltiple de productos y de la confirmación
/// para quitar todo el detalle.
/// </summary>
public static class PosSelectionMessages
{
    public const string SelectedBadge = "Seleccionado";
    public const string OutOfStockBadge = "Sin stock";
    public const string AcceptButton = "Aceptar";
    public const string CancelButton = "Cancelar";
    public const string PickColumn = "Elegir";
    public const string SelectedCount = "{0} producto(s) seleccionado(s)";

    public const string RemoveAllButton = "Quitar todo";
    public const string RemoveAllTitle = "Quitar todos los productos";
    public const string RemoveAllQuestion =
        "¿Está seguro de quitar todos los productos de la venta? " +
        "Esta acción no se puede deshacer.";
    public const string RemoveAllYes = "Sí, quitar todo";
    public const string RemoveAllNo = "No";

    public const string RemoveOneTitle = "Quitar producto";
    public const string RemoveOneQuestion =
        "¿Está seguro de quitar el producto '{0}' de la venta?";
    public const string RemoveOneYes = "Sí, quitar";
    public const string RemoveOneNo = "No";

    public const string ProductsAdded =
        "{0} producto(s) agregado(s) a la venta.";
    public const string ProductsSkipped =
        " {0} no se agregó (ya estaba en la venta o no tiene stock).";
    public const string AllRemoved =
        "Se quitaron todos los productos de la venta.";
    public const string OutOfStockProduct =
        "El producto '{0}' no tiene stock disponible.";
}
