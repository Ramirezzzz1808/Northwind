using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;
using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesQueriesDataContext
{
    IQueryable<Customer> Customers { get; }
    IQueryable<Product> Products { get; }
    IQueryable<Order> Orders { get; }

    // Nombre completo: OrderDetail también existe en ValueObjects.
    IQueryable<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> OrderDetails { get; }

    Task<ReturnType> FirstOrDefaultAsync<ReturnType>(
        IQueryable<ReturnType> queryable);
    Task<IEnumerable<ReturnType>> ToListAsync<ReturnType>(
        IQueryable<ReturnType> queryable);
    Task<int> CountAsync<ReturnType>(
        IQueryable<ReturnType> queryable);
}
