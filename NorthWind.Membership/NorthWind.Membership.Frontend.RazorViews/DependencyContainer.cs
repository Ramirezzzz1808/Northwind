using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NorthWind.Membership.Frontend.RazorViews.AuthenticationStateProviders;
using NorthWind.Membership.Frontend.RazorViews.HttpMessageHandlers;
using NorthWind.Membership.Frontend.RazorViews.Interfaces;
using NorthWind.Membership.Frontend.RazorViews.Services;
using NorthWind.Membership.Frontend.RazorViews.ViewModels.UserRegistration;
using NorthWind.Membership.Frontend.RazorViews.WebApiGateways;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddMembershipServices(
        this IServiceCollection services,
        Action<HttpClient> configureMembershipHttpClient)
    {
        services.AddAuthorizationCore();

        services.AddScoped<ITokenStorage, TokenLocalStorage>();
        services.AddScoped<JWTAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(provider =>
            provider.GetService<JWTAuthenticationStateProvider>());

        services.AddHttpClient<MembershipGateway>(configureMembershipHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();

        services.AddScoped<UserRegistrationViewModel>();
        services.AddScoped<UserLoginViewModel>();

        services.AddMembershipValidators();
        services.AddModelValidator<UserRegistrationViewModel,
            UserRegistrationViewModelDtoValidator>();
        services.AddModelValidator<UserRegistrationViewModel,
            UserRegistrationViewModelValidator>();
        services.AddModelValidator<UserLoginViewModel,
            UserLoginViewModelDtoValidator>();

        return services;
    }

    public static IHttpClientBuilder AddMembershipBearerTokenHandler(
        this IHttpClientBuilder builder)
    {
        builder.Services.TryAddTransient<MembershipBearerTokenHandler>();
        builder.AddHttpMessageHandler<MembershipBearerTokenHandler>();

        return builder;
    }
}