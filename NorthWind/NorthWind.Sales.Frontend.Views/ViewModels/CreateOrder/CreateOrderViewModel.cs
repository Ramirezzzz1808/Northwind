using NorthWind.RazorComponents.Validators;
using NorthWind.Sales.Entities.Dtos.Search;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderViewModel(
    ICreateOrderGateway gateway,
    ISearchGateway searchGateway,
    IModelValidatorHub<CreateOrderViewModel> validator)
{
    // Solo para mostrar en pantalla; el servidor recalcula los totales.
    const decimal TaxRate = 0.15m;

    #region Propiedades relacionadas a CreateOrderDto
    public string CustomerId { get; set; }
    public string ShipAddress { get; set; }
    public string ShipCity { get; set; }
    public string ShipCountry { get; set; }
    public string ShipPostalCode { get; set; }
    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];
    #endregion

    #region Datos del cliente (solo lectura en pantalla)
    public string CustomerName { get; private set; } = "";
    public string CustomerIdentification { get; private set; } = "";
    public string CustomerEmail { get; private set; } = "";
    #endregion

    #region Totales
    public decimal Subtotal =>
        Math.Round(OrderDetails.Sum(d => d.UnitPrice * d.Quantity), 2);
    public decimal TaxAmount => Math.Round(Subtotal * TaxRate, 2);
    public decimal Total => Subtotal + TaxAmount;
    #endregion

    public IModelValidatorHub<CreateOrderViewModel> Validator => validator;

    public ModelValidator<CreateOrderViewModel> ModelValidatorComponentReference { get; set; }

    public string InformationMessage { get; private set; }
    public string ErrorMessage { get; private set; }
    public int? LastOrderId { get; private set; }

    #region Búsquedas (usadas por las ventanas emergentes)
    public Task<PagedResultDto<CustomerSearchItemDto>> SearchCustomers(
        string text, int page, int pageSize) =>
        searchGateway.SearchCustomersAsync(text, page, pageSize);

    public Task<PagedResultDto<ProductSearchItemDto>> SearchProducts(
        string text, int page, int pageSize) =>
        searchGateway.SearchProductsAsync(text, page, pageSize);
    #endregion

    public void SelectCustomer(CustomerSearchItemDto customer)
    {
        CustomerId = customer.Id;
        CustomerName = customer.Name;
        CustomerIdentification = customer.Identification;
        CustomerEmail = customer.Email;
        ClearMessages();
    }

    /// <summary>
    /// Agrega un producto al detalle. Un producto solo puede aparecer una vez.
    /// </summary>
    public bool AddProduct(ProductSearchItemDto product)
    {
        ClearMessages();

        if (OrderDetails.Any(d => d.ProductId == product.Id))
        {
            ErrorMessage = string.Format(
                PosMessages.DuplicateProduct, product.Name);
            return false;
        }

        OrderDetails.Add(new CreateOrderDetailViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            UnitPrice = product.UnitPrice,
            UnitsInStock = product.UnitsInStock,
            Quantity = 1
        });

        InformationMessage = string.Format(
            PosMessages.ProductAdded, product.Name);
        return true;
    }

    /// <summary>
    /// Cambia el producto de una línea sin perder el resto de los datos.
    /// </summary>
    public bool ReplaceProduct(
        CreateOrderDetailViewModel detail, ProductSearchItemDto product)
    {
        ClearMessages();

        if (OrderDetails.Any(d =>
            !ReferenceEquals(d, detail) && d.ProductId == product.Id))
        {
            ErrorMessage = string.Format(
                PosMessages.DuplicateProduct, product.Name);
            return false;
        }

        detail.ProductId = product.Id;
        detail.ProductName = product.Name;
        detail.UnitPrice = product.UnitPrice;
        detail.UnitsInStock = product.UnitsInStock;
        EnforceQuantity(detail);

        if (ErrorMessage == null)
        {
            InformationMessage = string.Format(
                PosMessages.ProductChanged, product.Name);
        }

        return true;
    }

    public void RemoveDetail(CreateOrderDetailViewModel detail)
    {
        ClearMessages();
        OrderDetails.Remove(detail);
        InformationMessage = string.Format(
            PosMessages.ProductRemoved, detail.ProductName);
    }

    /// <summary>
    /// La cantidad no puede ser menor que 1 ni superar el stock.
    /// </summary>
    public void EnforceQuantity(CreateOrderDetailViewModel detail)
    {
        ClearMessages();

        if (detail.Quantity < 1)
        {
            detail.Quantity = 1;
            ErrorMessage = PosMessages.QuantityMinimum;
        }
        else if (detail.Quantity > detail.UnitsInStock)
        {
            detail.Quantity = detail.UnitsInStock;
            ErrorMessage = string.Format(
                PosMessages.StockExceeded,
                detail.UnitsInStock, detail.ProductName);
        }
    }

    public async Task Send()
    {
        ClearMessages();

        if (string.IsNullOrWhiteSpace(CustomerId))
        {
            ErrorMessage = PosMessages.CustomerRequired;
            return;
        }

        if (OrderDetails.Count == 0)
        {
            ErrorMessage = PosMessages.DetailRequired;
            return;
        }

        try
        {
            var OrderId = await gateway.CreateOrderAsync((CreateOrderDto)this);
            LastOrderId = OrderId;
            InformationMessage = string.Format(
                PosMessages.OrderSaved, OrderId);
            ResetOrder();
        }
        catch (HttpRequestException ex)
        {
            if (ex.Data.Contains("Errors"))
            {
                IEnumerable<ValidationError> Errors =
                    ex.Data["Errors"] as IEnumerable<ValidationError>;
                ModelValidatorComponentReference.AddErrors(Errors);
            }
            else
            {
                ErrorMessage = PosMessages.OrderFailed;
            }
        }
    }

    void ResetOrder()
    {
        CustomerId = null;
        CustomerName = "";
        CustomerIdentification = "";
        CustomerEmail = "";
        ShipAddress = null;
        ShipCity = null;
        ShipCountry = null;
        ShipPostalCode = null;
        OrderDetails.Clear();
    }

    void ClearMessages()
    {
        InformationMessage = null;
        ErrorMessage = null;
    }

    public static explicit operator CreateOrderDto(CreateOrderViewModel model) =>
        new CreateOrderDto(
            model.CustomerId, model.ShipAddress, model.ShipCity,
            model.ShipCountry, model.ShipPostalCode,
            model.OrderDetails.Select(d => new CreateOrderDetailDto(
                d.ProductId, d.UnitPrice, d.Quantity)));
}
