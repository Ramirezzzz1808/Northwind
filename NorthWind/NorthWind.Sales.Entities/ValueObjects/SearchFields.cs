namespace NorthWind.Sales.Entities.ValueObjects;

/// <summary>
/// Claves de los filtros de búsqueda. "all" busca en todos los campos;
/// las demás buscan solo en el campo indicado.
/// </summary>
public static class SearchFields
{
    public const string All = "all";

    // Clientes y productos.
    public const string Id = "id";
    public const string Name = "name";

    // Clientes.
    public const string Identification = "identification";
    public const string Email = "email";

    // Productos.
    public const string UnitPrice = "price";
    public const string UnitsInStock = "stock";

    // Facturas.
    public const string OrderNumber = "number";
    public const string CustomerName = "customer";
    public const string Total = "total";

    static readonly HashSet<string> Valid =
    [
        All, Id, Name, Identification, Email,
        UnitPrice, UnitsInStock, OrderNumber, CustomerName, Total
    ];

    /// <summary>
    /// Devuelve la clave en minúsculas; si es desconocida o vacía, "all".
    /// </summary>
    public static string Normalize(string? field)
    {
        string Clean = (field ?? "").Trim().ToLowerInvariant();
        return Valid.Contains(Clean) ? Clean : All;
    }
}
