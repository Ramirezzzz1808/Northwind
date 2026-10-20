using NorthWind.Membership.Entities.Dtos.UserRegistration;
using NorthWind.Sales.Validators.Entities.UserRegistration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddValidators(
        this IServiceCollection services)
    {
        services.AddModelValidator<CreateOrderDto, CreateOrderDtoValidator>();
        services.AddModelValidator<CreateOrderDetailDto, CreateOrderDetailDtoValidator>();

        // Reglas adicionales del registro de usuarios (nombre y apellido).
        services.AddModelValidator<UserRegistrationDto, UserRegistrationNamesValidator>();
        return services;
    }
}
