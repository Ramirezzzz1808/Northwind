namespace NorthWind.Sales.Validators.Entities.CreateOrder;

/// <summary>
/// Mensajes de validación de los datos de envío.
/// </summary>
internal static class ShipInfoValidationMessages
{
    public const string ShipAddressMinimumLength =
        "La dirección debe tener al menos 5 caracteres.";
    public const string ShipAddressInvalidCharacters =
        "La dirección solo puede contener letras, números, espacios y los signos . , # - /";

    public const string ShipCityOnlyLetters =
        "La ciudad solo puede contener letras.";
    public const string ShipCountryOnlyLetters =
        "El país solo puede contener letras.";

    public const string ShipPostalCodeRequired =
        "El código postal es obligatorio.";
    public const string ShipPostalCodeMinimumLength =
        "El código postal debe tener al menos 4 dígitos.";
    public const string ShipPostalCodeOnlyDigits =
        "El código postal solo puede contener números.";
}
