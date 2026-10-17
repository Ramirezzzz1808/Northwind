using NorthWind.Membership.Entities.Dtos.UserLogin;
using NorthWind.Membership.Entities.Dtos.UserRegistration;
using NorthWind.Membership.Entities.ValueObjects;
using System.Net.Http.Json;

namespace NorthWind.Membership.Frontend.RazorViews.WebApiGateways;

public class MembershipGateway(HttpClient httpClient)
{
    public async Task RegisterAsync(UserRegistrationDto userData)
    {
        var Response = await httpClient.PostAsJsonAsync(
            Endpoints.Register, userData);

        Response.EnsureSuccessStatusCode();
    }
    public async Task<TokensDto> LoginAsync(UserCredentialsDto userCredentials)
    {
        var Response = await httpClient.PostAsJsonAsync(
            Endpoints.Login, userCredentials);

        return await Response.Content.ReadFromJsonAsync<TokensDto>();
    }
}