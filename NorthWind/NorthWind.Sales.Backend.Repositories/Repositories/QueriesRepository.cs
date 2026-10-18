using NorthWind.Sales.Entities.Dtos.Invoice;
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
        // Se incluyen todos los productos; los de stock cero se envían
        // al final de la lista y la pantalla los muestra bloqueados.
        IQueryable<Entities.Product> Filtered = context.Products;

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
            .OrderBy(p => p.UnitsInStock <= 0)
            .ThenBy(p => p.Name)
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

    public async Task<PagedResultDto<OrderSearchItemDto>> SearchOrders(
        string text, int page, int pageSize)
    {
        var Joined =
            from o in context.Orders
            join c in context.Customers on o.CustomerId equals c.Id
            select new { Order = o, Customer = c };

        if (!string.IsNullOrWhiteSpace(text))
        {
            // Por número de factura o por datos del cliente
            // (código, nombre o cédula).
            if (int.TryParse(text, out int OrderId))
            {
                Joined = Joined.Where(x =>
                    x.Order.Id == OrderId ||
                    x.Customer.Name.Contains(text) ||
                    x.Customer.Identification.Contains(text) ||
                    x.Customer.Id.Contains(text));
            }
            else
            {
                Joined = Joined.Where(x =>
                    x.Customer.Name.Contains(text) ||
                    x.Customer.Identification.Contains(text) ||
                    x.Customer.Id.Contains(text));
            }
        }

        int TotalCount = await context.CountAsync(Joined);

        var PageQuery = Joined
            .OrderByDescending(x => x.Order.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new OrderSearchItemDto
            {
                Id = x.Order.Id,
                OrderDate = x.Order.OrderDate,
                CustomerId = x.Customer.Id,
                CustomerName = x.Customer.Name,
                Identification = x.Customer.Identification ?? "",
                Total = x.Order.Total
            });

        var Items = await context.ToListAsync(PageQuery);

        return new PagedResultDto<OrderSearchItemDto>
        {
            Items = Items.ToList(),
            TotalCount = TotalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<InvoiceDto?> GetInvoice(int orderId)
    {
        var HeaderQuery =
            from o in context.Orders
            where o.Id == orderId
            join c in context.Customers on o.CustomerId equals c.Id
            select new InvoiceDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                CustomerId = c.Id,
                CustomerName = c.Name,
                Identification = c.Identification ?? "",
                Email = c.Email ?? "",
                ShipAddress = o.ShipAddress ?? "",
                ShipCity = o.ShipCity ?? "",
                ShipCountry = o.ShipCountry ?? "",
                ShipPostalCode = o.ShipPostalCode ?? "",
                Subtotal = o.Subtotal,
                TaxAmount = o.TaxAmount,
                Total = o.Total
            };

        var Invoice = await context.FirstOrDefaultAsync(HeaderQuery);

        if (Invoice == null)
        {
            return null;
        }

        var LinesQuery =
            from d in context.OrderDetails
            where d.OrderId == orderId
            join p in context.Products on d.ProductId equals p.Id
            orderby p.Name
            select new InvoiceLineDto
            {
                ProductId = d.ProductId,
                ProductName = p.Name,
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity
            };

        Invoice.Lines = (await context.ToListAsync(LinesQuery)).ToList();

        return Invoice;
    }
}
