namespace NorthWind.Sales.Validators.Entities.UserRegistration;

/// <summary>
/// Mensajes de validación del nombre, el apellido y el correo en el registro de usuarios.
/// </summary>
internal static class UserRegistrationNamesMessages
{
    public const string FirstNameMinimumLength =
        "El nombre debe tener al menos 2 caracteres.";
    public const string FirstNameMaximumLength =
        "El nombre no puede tener más de 50 caracteres.";
    public const string FirstNameOnlyLetters =
        "El nombre solo puede contener letras. Entre palabras se permite un espacio, guion o apóstrofo.";

    public const string LastNameMinimumLength =
        "El apellido debe tener al menos 2 caracteres.";
    public const string LastNameMaximumLength =
        "El apellido no puede tener más de 50 caracteres.";
    public const string LastNameOnlyLetters =
        "El apellido solo puede contener letras. Entre palabras se permite un espacio, guion o apóstrofo.";

    public const string EmailInvalid =
        "El correo electrónico no es válido. Debe terminar en un dominio como .com, sin punto al final.";
}
