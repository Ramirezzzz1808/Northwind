using System.Text.RegularExpressions;
using NorthWind.Membership.Entities.Dtos.UserRegistration;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Sales.Validators.Entities.UserRegistration;

/// <summary>
/// Reglas adicionales para el nombre y el apellido del registro de usuarios.
/// La obligatoriedad ya la valida el validador de Membership, por eso estas
/// reglas dan por válido el valor vacío y así no se duplican los mensajes.
/// </summary>
internal class UserRegistrationNamesValidator
    : AbstractModelValidator<UserRegistrationDto>
{
    const int MinimumLength = 2;
    const int MaximumLength = 50;

    // Letras (con tildes y ñ), separadas por un espacio, guion o apóstrofo.
    static readonly Regex NameRegex = new(
        @"^\p{L}+([ '\-]\p{L}+)*$", RegexOptions.Compiled);

    static bool HasMinimumLength(string value) =>
        string.IsNullOrWhiteSpace(value) || value.Trim().Length >= MinimumLength;

    static bool HasMaximumLength(string value) =>
        string.IsNullOrWhiteSpace(value) || value.Trim().Length <= MaximumLength;

    static bool IsValidName(string value) =>
        string.IsNullOrWhiteSpace(value) || NameRegex.IsMatch(value.Trim());

    public UserRegistrationNamesValidator(
        IValidationService<UserRegistrationDto> validationService)
        : base(validationService)
    {
        AddRuleFor(u => u.FirstName)
            .Must(HasMinimumLength, UserRegistrationNamesMessages.FirstNameMinimumLength)
            .Must(HasMaximumLength, UserRegistrationNamesMessages.FirstNameMaximumLength)
            .Must(IsValidName, UserRegistrationNamesMessages.FirstNameOnlyLetters);

        AddRuleFor(u => u.LastName)
            .Must(HasMinimumLength, UserRegistrationNamesMessages.LastNameMinimumLength)
            .Must(HasMaximumLength, UserRegistrationNamesMessages.LastNameMaximumLength)
            .Must(IsValidName, UserRegistrationNamesMessages.LastNameOnlyLetters);
    }
}
