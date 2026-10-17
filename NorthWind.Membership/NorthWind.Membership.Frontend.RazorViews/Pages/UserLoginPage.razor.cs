using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NorthWind.Membership.Frontend.RazorViews.ViewModels.UserLogin;

namespace NorthWind.Membership.Frontend.RazorViews.Pages;

public partial class UserLoginPage
{
    const string RouteTemplate = "/user/login";

    [Inject]
    UserLoginViewModel ViewModel { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    ErrorBoundary ErrorBoundaryRef;

    void Recover()
    {
        ErrorBoundaryRef?.Recover();
    }

    protected override void OnInitialized()
    {
        ViewModel.OnLogin += () => NavigationManager.NavigateTo("/");
    }
}