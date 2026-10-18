using Microsoft.Extensions.DependencyInjection;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Search;
using NorthWind.Sales.Backend.UseCases.CreateOrder;
using NorthWind.Sales.Backend.UseCases.Search;

namespace NorthWind.Sales.Backend.UseCases;

public static class DependencyContainer
{
    public static IServiceCollection AddUseCasesServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderInputPort, CreateOrderInteractor>();
        services.AddScoped<ISearchCustomersInputPort, SearchCustomersInteractor>();
        services.AddScoped<ISearchProductsInputPort, SearchProductsInteractor>();

        // El orden de registro es el orden de ejecución.
        services.AddModelValidator<CreateOrderDto, CreateOrderDuplicateProductValidator>();
        services.AddModelValidator<CreateOrderDto, CreateOrderCustomerValidator>();
        services.AddModelValidator<CreateOrderDto, CreateOrderProductValidator>();

        services.AddScoped<IDomainEventHandler<SpecialOrderCreatedEvent>,
            SendEmailWhenSpecialOrderCreatedEventHandler>();

        return services;
    }
}
