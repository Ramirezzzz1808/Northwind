using Microsoft.AspNetCore.Components;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Frontend.Views.Components;

public partial class CreateOrderComponent
{
    [Parameter]
    public CreateOrderViewModel Order { get; set; }

    bool ShowCustomerModal;
    bool ShowProductModal;
    bool ShowClearConfirmation;

    // Línea que se quiere quitar (si no es null, se muestra la confirmación).
    CreateOrderDetailViewModel DetailToRemove;

    // Productos que ya están en la venta (se muestran bloqueados en el modal).
    HashSet<int> ProductsInOrder = [];

    // Si no es null, el modal de productos reemplaza esa línea (selección única);
    // si es null, agrega varios productos (selección múltiple).
    CreateOrderDetailViewModel DetailToReplace;

    void OpenCustomerModal() => ShowCustomerModal = true;

    void OpenProductModalForAdd()
    {
        DetailToReplace = null;
        ProductsInOrder = Order.OrderDetails.Select(d => d.ProductId).ToHashSet();
        ShowProductModal = true;
    }

    void OpenProductModalForReplace(CreateOrderDetailViewModel detail)
    {
        DetailToReplace = detail;
        ProductsInOrder = Order.OrderDetails.Select(d => d.ProductId).ToHashSet();
        ShowProductModal = true;
    }

    void CloseModals()
    {
        ShowCustomerModal = false;
        ShowProductModal = false;
        DetailToReplace = null;
    }

    void CustomerSelected(CustomerSearchItemDto customer)
    {
        Order.SelectCustomer(customer);
        CloseModals();
    }

    void ProductsConfirmed(List<ProductSearchItemDto> products)
    {
        if (DetailToReplace == null)
        {
            Order.AddProducts(products);
        }
        else if (products.Count > 0)
        {
            Order.ReplaceProduct(DetailToReplace, products[0]);
        }

        CloseModals();
    }

    void AskRemoveConfirmation(CreateOrderDetailViewModel detail) =>
        DetailToRemove = detail;

    void CancelRemove() => DetailToRemove = null;

    void ConfirmRemove()
    {
        if (DetailToRemove != null)
        {
            Order.RemoveDetail(DetailToRemove);
        }

        DetailToRemove = null;
    }

    void AskClearConfirmation() => ShowClearConfirmation = true;

    void CancelClear() => ShowClearConfirmation = false;

    void ConfirmClear()
    {
        Order.ClearDetails();
        ShowClearConfirmation = false;
    }
}
