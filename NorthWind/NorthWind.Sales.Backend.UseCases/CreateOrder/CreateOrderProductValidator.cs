using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Backend.UseCases.Resources;
using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

internal class CreateOrderProductValidator(IQueriesRepository repository)
    : IModelValidator<CreateOrderDto>
{
    readonly List<ValidationError> ErrorsField = [];

    public IEnumerable<ValidationError> Errors => ErrorsField;

    public ValidationConstraint Constraint =>
        ValidationConstraint.ValidateIfThereAreNoPreviousErrors;

    public async Task<bool> Validate(CreateOrderDto model)
    {
        IEnumerable<ProductUnitsInStock> RequiredQuantities =
            model.OrderDetails
                .GroupBy(d => d.ProductId)
                .Select(d => new ProductUnitsInStock(
                    d.Key, (short)d.Sum(x => x.Quantity)));

        IEnumerable<int> ProductIds =
            RequiredQuantities.Select(d => d.ProductId);

        IEnumerable<ProductUnitsInStock> InStockQuantities =
            await repository.GetProductsUnitsInStock(ProductIds);

        var RequiredVsInStock = RequiredQuantities
            .GroupJoin(InStockQuantities,
                required => required.ProductId,
                inStock => inStock.ProductId,
                (oneRequired, manyInStock) =>
                    new { oneRequired, manyInStock })
            .SelectMany(groupResult =>
                groupResult.manyInStock.DefaultIfEmpty(),
                (groupResult, singleInStock) => new
                {
                    groupResult.oneRequired.ProductId,
                    Required = groupResult.oneRequired.UnitsInStock,
                    InStock = singleInStock?.UnitsInStock
                });

        foreach (var Item in RequiredVsInStock)
        {
            if (!Item.InStock.HasValue)
            {
                string PropertyName = GetPropertyNameWithIndex(
                    model, Item.ProductId,
                    nameof(CreateOrderDetailDto.ProductId));

                ErrorsField.Add(new ValidationError(PropertyName,
                    string.Format(
                        CreateOrderMessages.ProductIdNotFoundErrorTemplate,
                        Item.ProductId)));
            }
            else if (Item.InStock < Item.Required)
            {
                string PropertyName = GetPropertyNameWithIndex(
                    model, Item.ProductId,
                    nameof(CreateOrderDetailDto.Quantity));

                ErrorsField.Add(new ValidationError(PropertyName,
                    string.Format(
                        CreateOrderMessages.UnitsInStockLessThanQuantityErrorTemplate,
                        Item.Required, Item.InStock, Item.ProductId)));
            }
        }

        return !ErrorsField.Any();
    }

    string GetPropertyNameWithIndex(CreateOrderDto model,
        int productId, string propertyName)
    {
        var Index = model.OrderDetails
            .ToList()
            .FindIndex(d => d.ProductId == productId);

        return $"OrderDetails[{Index}].{propertyName}";
    }
}