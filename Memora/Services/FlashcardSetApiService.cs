using Memora.Interfaces;
using Memora.Model;
using System.Net.Http;
using System.Net.Http.Json;

namespace Memora.Services;

public class FlashcardSetApiService
{
    private readonly HttpClient _http;
    private readonly ITokenStorage _tokenStore;
    public FlashcardSetApiService(IHttpClientFactory http, ITokenStorage tokenstore)
    {
        _http = http.CreateClient("ApiClient");             // named client
        _tokenStore = tokenstore;
    }

    public async Task<List<FlashcardSet>> GetAllFlashcardSets()
    {
        // Header is added in the MessagingHandler with each request that is made. Refer to App.xaml.cs for services configuration
        var response = await _http.GetAsync("api/FlashcardSet");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error ocurred: {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<List<FlashcardSet>>();
        if (result == null)
        {
            return new List<FlashcardSet>();     // temporarily returns an empty list
        }

        return result;
    }
}
