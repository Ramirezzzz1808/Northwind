namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddExceptionDelegatingHandler(
        this IServiceCollection services)
    {
        services.AddTransient<ExceptionDelegatingHandler>();
        return services;
    }
}