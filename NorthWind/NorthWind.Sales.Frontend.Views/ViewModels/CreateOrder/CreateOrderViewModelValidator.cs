using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

internal class CreateOrderViewModelValidator(
    IModelValidatorHub<CreateOrderDto> dtoModelValidatorHub)
    : AbstractViewModelValidator<CreateOrderDto, CreateOrderViewModel>(
        dtoModelValidatorHub,
        ValidationConstraint.AlwaysValidate)
{
}