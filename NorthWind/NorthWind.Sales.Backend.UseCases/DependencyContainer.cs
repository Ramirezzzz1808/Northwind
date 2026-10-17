using Microsoft.Extensions.DependencyInjection;
using NorthWind.Sales.Backend.UseCases.CreateOrder;

namespace NorthWind.Sales.Backend.UseCases;

public static class DependencyContainer
{
    public static IServiceCollection AddUseCasesServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderInputPort, CreateOrderInteractor>();

        services.AddModelValidator<CreateOrderDto, CreateOrderCustomerValidator>();
        services.AddModelValidator<CreateOrderDto, CreateOrderProductValidator>();

        services.AddScoped<IDomainEventHandler<SpecialOrderCreatedEvent>,
            SendEmailWhenSpecialOrderCreatedEventHandler>();

        return services;
    }
}