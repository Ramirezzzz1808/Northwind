namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal class NorthWindSalesCommandsDataContext(
    IOptions<DBOptions> dbOptions)
    : NorthWindSalesContext(dbOptions), INorthWindSalesCommandsDataContext
{
    public async Task AddOrderAsync(Order order) =>
        await AddAsync(order);

    public async Task AddOrderDetailsAsync(
        IEnumerable<Repositories.Entities.OrderDetail> orderDetails) =>
        await AddRangeAsync(orderDetails);

    public async Task DecreaseProductsStockAsync(
        IDictionary<int, short> quantitiesByProduct)
    {
        int[] ProductIds = quantitiesByProduct.Keys.ToArray();

        // Entidades rastreadas: el RowVersion leído viaja en el UPDATE,
        // si otro usuario modificó el producto se lanza una excepción
        // de concurrencia y la transacción se revierte.
        List<Product> Products = await this.Products
            .Where(p => ProductIds.Contains(p.Id))
            .ToListAsync();

        foreach (Product Product in Products)
        {
            Product.UnitsInStock -= quantitiesByProduct[Product.Id];
        }
    }

    public async Task SaveChangesAsync() =>
        await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}
