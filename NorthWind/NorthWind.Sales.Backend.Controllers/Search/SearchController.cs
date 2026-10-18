using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Search;
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

        return app;
    }

    public static async Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomers(
        ISearchCustomersInputPort inputPort,
        string? text = null,
        int page = 1,
        int pageSize = 10) =>
        await inputPort.Handle(text ?? "", page, pageSize);

    public static async Task<PagedResultDto<ProductSearchItemDto>> SearchProducts(
        ISearchProductsInputPort inputPort,
        string? text = null,
        int page = 1,
        int pageSize = 10) =>
        await inputPort.Handle(text ?? "", page, pageSize);
}
