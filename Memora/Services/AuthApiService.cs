using Memora.Interfaces;
using Memora.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace Memora.Services;

/// <summary>
/// Used for login. After a successful login, this class sets the token in the ITokenStore implementation.
/// The Token then is added to every API request that requires authentication.
/// </summary>
public class AuthApiService
{
    private readonly HttpClient _http;
    private readonly ITokenStore _tokenStore;

    public AuthApiService(IHttpClientFactory factory, ITokenStore tokenStore) 
    {
        _http = factory.CreateClient("ApiClient");      // creating a named client
        _tokenStore = tokenStore;
    }

    public async Task LoginAsync(LoginRequest request)
    {

        var response = await _http.PostAsJsonAsync("api/Auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error ocurred:: {(int)response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>()
            ?? throw new InvalidOperationException("A login error ocurred.");

        _tokenStore.SetToken(result.Token);
    }
}
