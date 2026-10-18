using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Search;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Backend.UseCases.Search;

internal static class SearchParameters
{
    public const int MaxPageSize = 50;
    public const int DefaultPageSize = 10;
    public const int MaxTextLength = 50;

    public static (string Text, int Page, int PageSize) Normalize(
        string? text, int page, int pageSize)
    {
        string CleanText = (text ?? "").Trim();

        if (CleanText.Length > MaxTextLength)
        {
            CleanText = CleanText[..MaxTextLength];
        }

        int CleanPage = Math.Max(page, 1);

        int CleanPageSize = pageSize <= 0
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);

        return (CleanText, CleanPage, CleanPageSize);
    }
}

internal class SearchCustomersInteractor(IQueriesRepository repository)
    : ISearchCustomersInputPort
{
    public async Task<PagedResultDto<CustomerSearchItemDto>> Handle(
        string text, int page, int pageSize)
    {
        var Parameters = SearchParameters.Normalize(text, page, pageSize);

        return await repository.SearchCustomers(
            Parameters.Text, Parameters.Page, Parameters.PageSize);
    }
}

internal class SearchProductsInteractor(IQueriesRepository repository)
    : ISearchProductsInputPort
{
    public async Task<PagedResultDto<ProductSearchItemDto>> Handle(
        string text, int page, int pageSize)
    {
        var Parameters = SearchParameters.Normalize(text, page, pageSize);

        return await repository.SearchProducts(
            Parameters.Text, Parameters.Page, Parameters.PageSize);
    }
}
