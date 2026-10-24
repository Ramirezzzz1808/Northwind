using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Search;
using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;
using NorthWind.Sales.Entities.ValueObjects;

namespace NorthWind.Sales.Backend.UseCases.Search;

internal static class SearchParameters
{
    public const int MaxPageSize = 50;
    public const int DefaultPageSize = 10;
    public const int MaxTextLength = 50;

    public static (string Text, string Field, int Page, int PageSize) Normalize(
        string? text, string? field, int page, int pageSize)
    {
        string CleanText = (text ?? "").Trim();

        if (CleanText.Length > MaxTextLength)
        {
            CleanText = CleanText[..MaxTextLength];
        }

        string CleanField = SearchFields.Normalize(field);

        int CleanPage = Math.Max(page, 1);

        int CleanPageSize = pageSize <= 0
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);

        return (CleanText, CleanField, CleanPage, CleanPageSize);
    }
}

internal class SearchCustomersInteractor(IQueriesRepository repository)
    : ISearchCustomersInputPort
{
    public async Task<PagedResultDto<CustomerSearchItemDto>> Handle(
        string text, string field, int page, int pageSize)
    {
        var Parameters = SearchParameters.Normalize(text, field, page, pageSize);

        return await repository.SearchCustomers(
            Parameters.Text, Parameters.Field, Parameters.Page, Parameters.PageSize);
    }
}

internal class SearchProductsInteractor(IQueriesRepository repository)
    : ISearchProductsInputPort
{
    public async Task<PagedResultDto<ProductSearchItemDto>> Handle(
        string text, string field, int page, int pageSize)
    {
        var Parameters = SearchParameters.Normalize(text, field, page, pageSize);

        return await repository.SearchProducts(
            Parameters.Text, Parameters.Field, Parameters.Page, Parameters.PageSize);
    }
}

internal class SearchOrdersInteractor(IQueriesRepository repository)
    : ISearchOrdersInputPort
{
    public async Task<PagedResultDto<OrderSearchItemDto>> Handle(
        string text, string field, int page, int pageSize)
    {
        var Parameters = SearchParameters.Normalize(text, field, page, pageSize);

        return await repository.SearchOrders(
            Parameters.Text, Parameters.Field, Parameters.Page, Parameters.PageSize);
    }
}

internal class GetInvoiceInteractor(IQueriesRepository repository)
    : IGetInvoiceInputPort
{
    public async Task<InvoiceDto?> Handle(int orderId) =>
        await repository.GetInvoice(orderId);
}
