namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderDetailViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; } = 1;
    public short UnitsInStock { get; set; }

    public decimal LineTotal => Math.Round(UnitPrice * Quantity, 2);
}
