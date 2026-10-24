using Microsoft.AspNetCore.Http;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Search;
using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class SearchController
{
    public static WebApplication UseSearchController(
        this WebApplication app)
    {
        app.MapGet(Endpoints.SearchCustomers, SearchCustomers)
            .RequireAuthorization();

        app.MapGet(Endpoints.SearchProducts, SearchProducts)
            .RequireAuthorization();

        app.MapGet(Endpoints.SearchOrders, SearchOrders)
            .RequireAuthorization();

        app.MapGet(Endpoints.GetInvoice, GetInvoice)
            .RequireAuthorization();

        return app;
    }

    public static async Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomers(
        ISearchCustomersInputPort inputPort,
        string? text = null,
        string? field = null,
        int page = 1,
        int pageSize = 10) =>
        await inputPort.Handle(text ?? "", field ?? SearchFields.All, page, pageSize);

    public static async Task<PagedResultDto<ProductSearchItemDto>> SearchProducts(
        ISearchProductsInputPort inputPort,
        string? text = null,
        string? field = null,
        int page = 1,
        int pageSize = 10) =>
        await inputPort.Handle(text ?? "", field ?? SearchFields.All, page, pageSize);

    public static async Task<PagedResultDto<OrderSearchItemDto>> SearchOrders(
        ISearchOrdersInputPort inputPort,
        string? text = null,
        string? field = null,
        int page = 1,
        int pageSize = 10) =>
        await inputPort.Handle(text ?? "", field ?? SearchFields.All, page, pageSize);

    public static async Task<IResult> GetInvoice(
        IGetInvoiceInputPort inputPort,
        int id)
    {
        var Invoice = await inputPort.Handle(id);

        return Invoice == null
            ? Results.NotFound($"La factura #{id} no existe.")
            : Results.Ok(Invoice);
    }
}
