using Microsoft.AspNetCore.Components;
using NorthWind.Membership.Frontend.RazorViews.AuthenticationStateProviders;

namespace NorthWind.Membership.Frontend.RazorViews.Pages;

public partial class LogoutPage
{
    [Inject]
    JWTAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    async Task Logout()
    {
        await AuthenticationStateProvider.LogoutAsync();
        NavigationManager.NavigateTo("/");
    }
}