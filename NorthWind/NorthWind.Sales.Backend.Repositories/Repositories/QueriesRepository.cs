using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;
using NorthWind.Sales.Entities.ValueObjects;

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
        string text, string field, int page, int pageSize)
    {
        IQueryable<Entities.Customer> Filtered = context.Customers;

        if (!string.IsNullOrWhiteSpace(text))
        {
            // "all" busca en todos los campos; las demás claves, solo en uno.
            Filtered = field switch
            {
                SearchFields.Id =>
                    Filtered.Where(c => c.Id.Contains(text)),
                SearchFields.Name =>
                    Filtered.Where(c => c.Name.Contains(text)),
                SearchFields.Identification =>
                    Filtered.Where(c => c.Identification.Contains(text)),
                SearchFields.Email =>
                    Filtered.Where(c => c.Email.Contains(text)),
                _ => Filtered.Where(c =>
                    c.Id.Contains(text) ||
                    c.Name.Contains(text) ||
                    c.Identification.Contains(text) ||
                    c.Email.Contains(text))
            };
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
        string text, string field, int page, int pageSize)
    {
        // Se incluyen todos los productos; los de stock cero se envían
        // al final de la lista y la pantalla los muestra bloqueados.
        IQueryable<Entities.Product> Filtered = context.Products;

        if (!string.IsNullOrWhiteSpace(text))
        {
            bool IsNumber = int.TryParse(text, out int ProductId);

            // Los decimales se guardan con punto; se acepta coma al escribir.
            string Decimal = text.Replace(',', '.');

            Filtered = field switch
            {
                SearchFields.Id =>
                    Filtered.Where(p => IsNumber && p.Id == ProductId),
                SearchFields.Name =>
                    Filtered.Where(p => p.Name.Contains(text)),
                SearchFields.UnitPrice =>
                    Filtered.Where(p => p.UnitPrice.ToString().Contains(Decimal)),
                SearchFields.UnitsInStock =>
                    Filtered.Where(p => p.UnitsInStock.ToString().Contains(text)),
                _ => Filtered.Where(p =>
                    p.Name.Contains(text) ||
                    (IsNumber && p.Id == ProductId) ||
                    p.UnitPrice.ToString().Contains(Decimal) ||
                    p.UnitsInStock.ToString().Contains(text))
            };
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
        string text, string field, int page, int pageSize)
    {
        var Joined =
            from o in context.Orders
            join c in context.Customers on o.CustomerId equals c.Id
            select new { Order = o, Customer = c };

        if (!string.IsNullOrWhiteSpace(text))
        {
            bool IsNumber = int.TryParse(text, out int OrderId);

            // Los decimales se guardan con punto; se acepta coma al escribir.
            string Decimal = text.Replace(',', '.');

            Joined = field switch
            {
                SearchFields.OrderNumber =>
                    Joined.Where(x => IsNumber && x.Order.Id == OrderId),
                SearchFields.CustomerName =>
                    Joined.Where(x => x.Customer.Name.Contains(text)),
                SearchFields.Identification =>
                    Joined.Where(x => x.Customer.Identification.Contains(text)),
                SearchFields.Total =>
                    Joined.Where(x => x.Order.Total.ToString().Contains(Decimal)),
                _ => Joined.Where(x =>
                    (IsNumber && x.Order.Id == OrderId) ||
                    x.Customer.Name.Contains(text) ||
                    x.Customer.Identification.Contains(text) ||
                    x.Customer.Id.Contains(text) ||
                    x.Order.Total.ToString().Contains(Decimal))
            };
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
