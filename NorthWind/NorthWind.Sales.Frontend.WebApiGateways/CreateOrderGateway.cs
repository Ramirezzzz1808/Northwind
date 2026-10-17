using System.Net.Http.Json;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class CreateOrderGateway(HttpClient httpClient) : ICreateOrderGateway
{
    public async Task<int> CreateOrderAsync(CreateOrderDto order)
    {
        var Response = await httpClient.PostAsJsonAsync(
            Endpoints.CreateOrder, order);

        return await Response.Content.ReadFromJsonAsync<int>();
    }
}