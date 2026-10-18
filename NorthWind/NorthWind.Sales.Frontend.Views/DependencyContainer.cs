using NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;
using NorthWind.Sales.Frontend.Views.ViewModels.Invoices;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddViewsServices(
        this IServiceCollection services)
    {
        services.AddScoped<CreateOrderViewModel>();
        services.AddScoped<InvoiceViewModel>();
        services.AddModelValidator<CreateOrderViewModel,
            CreateOrderViewModelValidator>();
        return services;
    }
}
