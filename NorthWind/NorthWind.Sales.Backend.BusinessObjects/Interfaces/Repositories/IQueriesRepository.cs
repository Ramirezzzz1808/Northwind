using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface IQueriesRepository
{
    Task<decimal?> GetCustomerCurrentBalance(string customerId);
    Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(
        IEnumerable<int> productIds);

    // Precio vigente en catálogo (clave: ProductId).
    Task<IDictionary<int, decimal>> GetProductsUnitPrices(
        IEnumerable<int> productIds);

    Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomers(
        string text, int page, int pageSize);

    // Solo productos con stock mayor que cero.
    Task<PagedResultDto<ProductSearchItemDto>> SearchProducts(
        string text, int page, int pageSize);

    // Facturas guardadas: por número de orden o por datos del cliente.
    Task<PagedResultDto<OrderSearchItemDto>> SearchOrders(
        string text, int page, int pageSize);

    // Encabezado y detalle de una orden; null si no existe.
    Task<InvoiceDto?> GetInvoice(int orderId);
}
