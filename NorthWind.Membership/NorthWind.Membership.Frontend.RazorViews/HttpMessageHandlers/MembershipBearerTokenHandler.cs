using System.Net.Http.Headers;
using NorthWind.Membership.Frontend.RazorViews.Interfaces;

namespace NorthWind.Membership.Frontend.RazorViews.HttpMessageHandlers;

internal class MembershipBearerTokenHandler(ITokenStorage storage)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var Tokens = await storage.GetTokensAsync();

        if (Tokens != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer", Tokens.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}