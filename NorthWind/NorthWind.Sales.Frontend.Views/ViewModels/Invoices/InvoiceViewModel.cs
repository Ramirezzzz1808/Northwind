using NorthWind.Sales.Entities.Dtos.Invoice;
using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Frontend.Views.ViewModels.Invoices;

public class InvoiceViewModel(ISearchGateway gateway)
{
    public InvoiceDto? Invoice { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool IsLoading { get; private set; }

    public Task<PagedResultDto<OrderSearchItemDto>> SearchOrders(
        string text, string field, int page, int pageSize) =>
        gateway.SearchOrdersAsync(text, field, page, pageSize);

    public async Task LoadInvoice(int orderId)
    {
        ErrorMessage = null;
        IsLoading = true;
        try
        {
            Invoice = await gateway.GetInvoiceAsync(orderId);

            if (Invoice == null)
            {
                ErrorMessage = string.Format(
                    InvoiceMessages.NotFound, orderId);
            }
        }
        catch (HttpRequestException)
        {
            Invoice = null;
            ErrorMessage = string.Format(
                InvoiceMessages.NotFound, orderId);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
