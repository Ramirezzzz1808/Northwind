namespace NorthWind.Sales.Backend.BusinessObjects.Aggregates;

public class OrderAggregate : Order
{
    // IVA vigente (15%).
    public const decimal TaxRate = 0.15m;

    readonly List<OrderDetail> OrderDetailsField = [];
    public IReadOnlyCollection<OrderDetail> OrderDetails => OrderDetailsField;

    public void AddDetail(int productId, decimal unitPrice, short quantity)
    {
        var ExistingOrderDetail =
            OrderDetailsField
            .FirstOrDefault(o => o.ProductId == productId);

        if (ExistingOrderDetail != default)
        {
            quantity += ExistingOrderDetail.Quantity;
            OrderDetailsField.Remove(ExistingOrderDetail);
        }

        OrderDetailsField.Add(
            new OrderDetail(productId, unitPrice, quantity));
    }

    public void CalculateTotals()
    {
        Subtotal = Math.Round(
            OrderDetailsField.Sum(d => d.UnitPrice * d.Quantity), 2);
        TaxAmount = Math.Round(Subtotal * TaxRate, 2);
        Total = Subtotal + TaxAmount;
    }

    public static OrderAggregate From(CreateOrderDto orderDto)
    {
        OrderAggregate OrderAggregate = new OrderAggregate
        {
            CustomerId = orderDto.CustomerId,
            ShipAddress = orderDto.ShipAddress,
            ShipCity = orderDto.ShipCity,
            ShipCountry = orderDto.ShipCountry,
            ShipPostalCode = orderDto.ShipPostalCode
        };

        foreach (var Item in orderDto.OrderDetails)
        {
            OrderAggregate.AddDetail(Item.ProductId,
                Item.UnitPrice, Item.Quantity);
        }

        OrderAggregate.CalculateTotals();

        return OrderAggregate;
    }
}
