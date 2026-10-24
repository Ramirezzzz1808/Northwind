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

    // field: "all" busca en todos los campos; otra clave busca solo en ese campo.
    Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomers(
        string text, string field, int page, int pageSize);

    // Todos los productos; los de stock cero van al final de la lista.
    Task<PagedResultDto<ProductSearchItemDto>> SearchProducts(
        string text, string field, int page, int pageSize);

    // Facturas guardadas: por número de orden o por datos del cliente.
    Task<PagedResultDto<OrderSearchItemDto>> SearchOrders(
        string text, string field, int page, int pageSize);

    // Encabezado y detalle de una orden; null si no existe.
    Task<InvoiceDto?> GetInvoice(int orderId);
}
