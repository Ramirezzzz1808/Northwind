using System.Text.RegularExpressions;
using NorthWind.Membership.Entities.Dtos.UserRegistration;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Sales.Validators.Entities.UserRegistration;

/// <summary>
/// Reglas adicionales para el nombre, el apellido y el correo del registro de usuarios.
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

    // Forma general de un correo: algo@algo (sin espacios).
    static readonly Regex EmailShapeRegex = new(
        @"^[^@\s]+@[^@\s]+$", RegexOptions.Compiled);

    // Correo estricto: el dominio termina en una extensión de letras (.com, .ec, ...),
    // sin punto al final ni puntos seguidos.
    static readonly Regex EmailRegex = new(
        @"^[A-Za-z0-9_%+\-]+(\.[A-Za-z0-9_%+\-]+)*@([A-Za-z0-9]([A-Za-z0-9\-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$",
        RegexOptions.Compiled);

    static bool HasMinimumLength(string value) =>
        string.IsNullOrWhiteSpace(value) || value.Trim().Length >= MinimumLength;

    static bool HasMaximumLength(string value) =>
        string.IsNullOrWhiteSpace(value) || value.Trim().Length <= MaximumLength;

    static bool IsValidName(string value) =>
        string.IsNullOrWhiteSpace(value) || NameRegex.IsMatch(value.Trim());

    // Solo revisa los valores que tienen forma de correo; los demás ya los
    // rechaza la regla de Membership con su propio mensaje.
    static bool IsValidEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        string Email = value.Trim();
        return !EmailShapeRegex.IsMatch(Email) || EmailRegex.IsMatch(Email);
    }

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

        AddRuleFor(u => u.Email)
            .Must(IsValidEmail, UserRegistrationNamesMessages.EmailInvalid);
    }
}
