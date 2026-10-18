using System.Text.RegularExpressions;
using NorthWind.Sales.Entities.Dtos.CreateOrder;
using NorthWind.Sales.Validators.Entities.Resources;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Sales.Validators.Entities.CreateOrder;

internal class CreateOrderDtoValidator
    : AbstractModelValidator<CreateOrderDto>
{
    // Letras (con tildes y ñ), separadas por un espacio, guion o apóstrofo.
    static readonly Regex OnlyLettersRegex = new(
        @"^\p{L}+([ '\-]\p{L}+)*$", RegexOptions.Compiled);

    // Letras, números, espacios y signos comunes de una dirección.
    static readonly Regex AddressRegex = new(
        @"^[\p{L}\p{N} .,#°/\-]+$", RegexOptions.Compiled);

    static readonly Regex OnlyDigitsRegex = new(
        @"^\d+$", RegexOptions.Compiled);

    // Los predicados devuelven true cuando el valor está vacío: de ese caso
    // se encarga la regla NotEmpty, para no duplicar mensajes.
    static bool IsOnlyLetters(string value) =>
        string.IsNullOrWhiteSpace(value) || OnlyLettersRegex.IsMatch(value.Trim());

    static bool IsValidAddress(string value) =>
        string.IsNullOrWhiteSpace(value) || AddressRegex.IsMatch(value.Trim());

    static bool IsOnlyDigits(string value) =>
        string.IsNullOrWhiteSpace(value) || OnlyDigitsRegex.IsMatch(value.Trim());

    public CreateOrderDtoValidator(
        IValidationService<CreateOrderDto> validationService,
        IModelValidator<CreateOrderDetailDto> detailValidator)
        : base(validationService)
    {
        AddRuleFor(c => c.CustomerId)
            .NotEmpty(CreateOrderMessages.CustomerIdRequired)
            .Length(5, CreateOrderMessages.CustomerIdRequiredLength);

        AddRuleFor(c => c.ShipAddress)
            .NotEmpty(CreateOrderMessages.ShipAddressRequired)
            .MinimumLength(5, ShipInfoValidationMessages.ShipAddressMinimumLength)
            .MaximumLength(60, CreateOrderMessages.ShipAddressMaximumLength)
            .Must(IsValidAddress, ShipInfoValidationMessages.ShipAddressInvalidCharacters);

        AddRuleFor(c => c.ShipCity)
            .NotEmpty(CreateOrderMessages.ShipCityRequired)
            .MinimumLength(3, CreateOrderMessages.ShipCityMinimumLength)
            .MaximumLength(15, CreateOrderMessages.ShipCityMaximumLength)
            .Must(IsOnlyLetters, ShipInfoValidationMessages.ShipCityOnlyLetters);

        AddRuleFor(c => c.ShipCountry)
            .NotEmpty(CreateOrderMessages.ShipCountryRequired)
            .MinimumLength(3, CreateOrderMessages.ShipCountryMinimumLength)
            .MaximumLength(15, CreateOrderMessages.ShipCountryMaximumLength)
            .Must(IsOnlyLetters, ShipInfoValidationMessages.ShipCountryOnlyLetters);

        AddRuleFor(c => c.ShipPostalCode)
            .NotEmpty(ShipInfoValidationMessages.ShipPostalCodeRequired)
            .MinimumLength(4, ShipInfoValidationMessages.ShipPostalCodeMinimumLength)
            .MaximumLength(10, CreateOrderMessages.ShipPostalCodeMaximumLength)
            .Must(IsOnlyDigits, ShipInfoValidationMessages.ShipPostalCodeOnlyDigits);

        AddRuleFor(c => c.OrderDetails)
            .NotNull(CreateOrderMessages.OrderDetailsRequired)
            .NotEmpty(CreateOrderMessages.OrderDetailsNotEmpty);

        AddRuleForEach(c => c.OrderDetails)
            .SetValidator(detailValidator);
    }
}
