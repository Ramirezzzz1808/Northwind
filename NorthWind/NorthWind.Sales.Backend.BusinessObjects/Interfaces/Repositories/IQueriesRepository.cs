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
}
