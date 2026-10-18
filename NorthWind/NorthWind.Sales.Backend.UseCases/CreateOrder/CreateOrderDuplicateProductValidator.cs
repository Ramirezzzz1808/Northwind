using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

/// <summary>
/// Un producto debe aparecer una sola vez en la orden.
/// </summary>
internal class CreateOrderDuplicateProductValidator
    : IModelValidator<CreateOrderDto>
{
    const string DuplicatedProductTemplate =
        "El producto {0} está repetido en la orden. " +
        "Modifique la cantidad de la línea existente.";

    readonly List<ValidationError> ErrorsField = [];

    public IEnumerable<ValidationError> Errors => ErrorsField;

    public ValidationConstraint Constraint =>
        ValidationConstraint.ValidateIfThereAreNoPreviousErrors;

    public Task<bool> Validate(CreateOrderDto model)
    {
        List<CreateOrderDetailDto> Details = model.OrderDetails.ToList();
        HashSet<int> Seen = [];

        for (int Index = 0; Index < Details.Count; Index++)
        {
            int ProductId = Details[Index].ProductId;

            if (!Seen.Add(ProductId))
            {
                ErrorsField.Add(new ValidationError(
                    $"OrderDetails[{Index}].{nameof(CreateOrderDetailDto.ProductId)}",
                    string.Format(DuplicatedProductTemplate, ProductId)));
            }
        }

        return Task.FromResult(!ErrorsField.Any());
    }
}
