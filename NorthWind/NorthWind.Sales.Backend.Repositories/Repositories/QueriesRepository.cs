using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class QueriesRepository(INorthWindSalesQueriesDataContext context)
    : IQueriesRepository
{
    public async Task<decimal?> GetCustomerCurrentBalance(string customerId)
    {
        var Queryable = context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => new { c.CurrentBalance });

        var Result = await context.FirstOrDefaultAsync(Queryable);

        return Result?.CurrentBalance;
    }

    public async Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(
        IEnumerable<int> productIds)
    {
        var Queryable = context.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new ProductUnitsInStock(p.Id, p.UnitsInStock));

        return await context.ToListAsync(Queryable);
    }

    public async Task<IDictionary<int, decimal>> GetProductsUnitPrices(
        IEnumerable<int> productIds)
    {
        int[] Ids = productIds.ToArray();

        var Queryable = context.Products
            .Where(p => Ids.Contains(p.Id))
            .Select(p => new { p.Id, p.UnitPrice });

        var Result = await context.ToListAsync(Queryable);

        return Result.ToDictionary(p => p.Id, p => p.UnitPrice);
    }

    public async Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomers(
        string text, int page, int pageSize)
    {
        IQueryable<Entities.Customer> Filtered = context.Customers;

        if (!string.IsNullOrWhiteSpace(text))
        {
            // Búsqueda por varios campos: código, nombre, cédula o correo.
            Filtered = Filtered.Where(c =>
                c.Id.Contains(text) ||
                c.Name.Contains(text) ||
                c.Identification.Contains(text) ||
                c.Email.Contains(text));
        }

        int TotalCount = await context.CountAsync(Filtered);

        var PageQuery = Filtered
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerSearchItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Identification = c.Identification ?? "",
                Email = c.Email ?? "",
                CurrentBalance = c.CurrentBalance
            });

        var Items = await context.ToListAsync(PageQuery);

        return new PagedResultDto<CustomerSearchItemDto>
        {
            Items = Items.ToList(),
            TotalCount = TotalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PagedResultDto<ProductSearchItemDto>> SearchProducts(
        string text, int page, int pageSize)
    {
        // Solo se muestran productos con stock mayor que cero.
        IQueryable<Entities.Product> Filtered =
            context.Products.Where(p => p.UnitsInStock > 0);

        if (!string.IsNullOrWhiteSpace(text))
        {
            // Búsqueda por dos campos: nombre o código del producto.
            if (int.TryParse(text, out int ProductId))
            {
                Filtered = Filtered.Where(p =>
                    p.Name.Contains(text) || p.Id == ProductId);
            }
            else
            {
                Filtered = Filtered.Where(p => p.Name.Contains(text));
            }
        }

        int TotalCount = await context.CountAsync(Filtered);

        var PageQuery = Filtered
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductSearchItemDto
            {
                Id = p.Id,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock
            });

        var Items = await context.ToListAsync(PageQuery);

        return new PagedResultDto<ProductSearchItemDto>
        {
            Items = Items.ToList(),
            TotalCount = TotalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
