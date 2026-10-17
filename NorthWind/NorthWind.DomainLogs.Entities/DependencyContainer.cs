namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddDomainLogger(
        this IServiceCollection services)
    {
        services.AddScoped<IDomainLogger, DomainLogger>();
        return services;
    }
}