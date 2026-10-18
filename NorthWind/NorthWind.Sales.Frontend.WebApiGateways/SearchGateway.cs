using System.Net.Http.Json;
using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class SearchGateway(HttpClient httpClient) : ISearchGateway
{
    public async Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomersAsync(
        string text, int page, int pageSize) =>
        await GetAsync<CustomerSearchItemDto>(
            Endpoints.SearchCustomers, text, page, pageSize);

    public async Task<PagedResultDto<ProductSearchItemDto>> SearchProductsAsync(
        string text, int page, int pageSize) =>
        await GetAsync<ProductSearchItemDto>(
            Endpoints.SearchProducts, text, page, pageSize);

    public async Task<PagedResultDto<OrderSearchItemDto>> SearchOrdersAsync(
        string text, int page, int pageSize) =>
        await GetAsync<OrderSearchItemDto>(
            Endpoints.SearchOrders, text, page, pageSize);

    public async Task<InvoiceDto?> GetInvoiceAsync(int orderId) =>
        await httpClient.GetFromJsonAsync<InvoiceDto>(
            $"{Endpoints.GetInvoice}?id={orderId}");

    async Task<PagedResultDto<T>> GetAsync<T>(
        string endpoint, string text, int page, int pageSize)
    {
        var Url = $"{endpoint}?text={Uri.EscapeDataString(text ?? "")}" +
            $"&page={page}&pageSize={pageSize}";

        var Result = await httpClient
            .GetFromJsonAsync<PagedResultDto<T>>(Url);

        return Result ?? new PagedResultDto<T>
        {
            Page = page,
            PageSize = pageSize
        };
    }
}
