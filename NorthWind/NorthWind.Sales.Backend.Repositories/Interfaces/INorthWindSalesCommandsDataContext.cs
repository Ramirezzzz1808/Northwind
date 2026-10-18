using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesCommandsDataContext
{
    Task AddOrderAsync(Order order);
    Task AddOrderDetailsAsync(IEnumerable<Entities.OrderDetail> orderDetails);

    // Resta del inventario las cantidades vendidas (clave: ProductId).
    Task DecreaseProductsStockAsync(IDictionary<int, short> quantitiesByProduct);

    Task SaveChangesAsync();
}
