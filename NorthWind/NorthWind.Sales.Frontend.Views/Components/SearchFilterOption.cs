using NorthWind.Sales.Entities.ValueObjects;

namespace NorthWind.Sales.Frontend.Views.Components;

/// <summary>
/// Una opción del filtro de búsqueda: la clave que se envía al servidor
/// y el texto que ve el usuario.
/// </summary>
public record SearchFilterOption(string Key, string Label)
{
    public static readonly SearchFilterOption All =
        new(SearchFields.All, SearchFilterMessages.AllFields);

    public static List<SearchFilterOption> ForCustomers() =>
    [
        All,
        new(SearchFields.Id, SearchFilterMessages.Code),
        new(SearchFields.Name, SearchFilterMessages.Name),
        new(SearchFields.Identification, SearchFilterMessages.Identification),
        new(SearchFields.Email, SearchFilterMessages.Email)
    ];

    public static List<SearchFilterOption> ForProducts() =>
    [
        All,
        new(SearchFields.Id, SearchFilterMessages.Code),
        new(SearchFields.Name, SearchFilterMessages.Name),
        new(SearchFields.UnitPrice, SearchFilterMessages.UnitPrice),
        new(SearchFields.UnitsInStock, SearchFilterMessages.Stock)
    ];

    public static List<SearchFilterOption> ForInvoices() =>
    [
        All,
        new(SearchFields.OrderNumber, SearchFilterMessages.InvoiceNumber),
        new(SearchFields.CustomerName, SearchFilterMessages.Customer),
        new(SearchFields.Identification, SearchFilterMessages.Identification),
        new(SearchFields.Total, SearchFilterMessages.Total)
    ];
}
