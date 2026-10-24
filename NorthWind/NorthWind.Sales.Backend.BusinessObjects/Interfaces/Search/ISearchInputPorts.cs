using NorthWind.Sales.Entities.Dtos.Search;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Search;

public interface ISearchCustomersInputPort
{
    Task<PagedResultDto<CustomerSearchItemDto>> Handle(
        string text, string field, int page, int pageSize);
}

public interface ISearchProductsInputPort
{
    Task<PagedResultDto<ProductSearchItemDto>> Handle(
        string text, string field, int page, int pageSize);
}
