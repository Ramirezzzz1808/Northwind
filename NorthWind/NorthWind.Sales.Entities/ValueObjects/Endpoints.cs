namespace NorthWind.Sales.Entities.ValueObjects;

public class Endpoints
{
    public const string CreateOrder = $"/{nameof(CreateOrder)}";
    public const string SearchCustomers = $"/{nameof(SearchCustomers)}";
    public const string SearchProducts = $"/{nameof(SearchProducts)}";
    public const string SearchOrders = $"/{nameof(SearchOrders)}";
    public const string GetInvoice = $"/{nameof(GetInvoice)}";
}
