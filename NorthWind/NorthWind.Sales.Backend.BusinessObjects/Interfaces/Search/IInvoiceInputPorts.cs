using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Search;

public interface ISearchOrdersInputPort
{
    Task<PagedResultDto<OrderSearchItemDto>> Handle(
        string text, string field, int page, int pageSize);
}

public interface IGetInvoiceInputPort
{
    // Devuelve null si la factura no existe.
    Task<InvoiceDto?> Handle(int orderId);
}
