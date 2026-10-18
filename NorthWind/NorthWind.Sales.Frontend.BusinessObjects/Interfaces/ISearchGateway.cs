using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

/// <summary>
/// Búsquedas paginadas y consulta de facturas usadas por el punto de venta.
/// </summary>
public interface ISearchGateway
{
    Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomersAsync(
        string text, int page, int pageSize);

    Task<PagedResultDto<ProductSearchItemDto>> SearchProductsAsync(
        string text, int page, int pageSize);

    Task<PagedResultDto<OrderSearchItemDto>> SearchOrdersAsync(
        string text, int page, int pageSize);

    Task<InvoiceDto?> GetInvoiceAsync(int orderId);
}
