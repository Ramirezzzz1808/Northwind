using Microsoft.AspNetCore.Components;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Frontend.Views.Components;

public partial class CreateOrderComponent
{
    [Parameter]
    public CreateOrderViewModel Order { get; set; }

    bool ShowCustomerModal;
    bool ShowProductModal;

    // Si no es null, el modal de productos reemplaza esa línea; si es null, agrega.
    CreateOrderDetailViewModel DetailToReplace;

    void OpenCustomerModal() => ShowCustomerModal = true;

    void OpenProductModalForAdd()
    {
        DetailToReplace = null;
        ShowProductModal = true;
    }

    void OpenProductModalForReplace(CreateOrderDetailViewModel detail)
    {
        DetailToReplace = detail;
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

    void ProductSelected(ProductSearchItemDto product)
    {
        if (DetailToReplace == null)
        {
            Order.AddProduct(product);
        }
        else
        {
            Order.ReplaceProduct(DetailToReplace, product);
        }

        CloseModals();
    }
}
