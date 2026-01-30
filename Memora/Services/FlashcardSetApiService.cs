using Memora.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Memora.DTOs;
using System.Windows.Documents;

namespace Memora.Services;

public class FlashcardSetApiService
{
    private readonly HttpClient _http;
    private readonly ITokenStore _tokenStore;
    public FlashcardSetApiService(IHttpClientFactory http, ITokenStore tokenstore)
    {
        _http = http.CreateClient("ApiClient");             // named client
        _tokenStore = tokenstore;
    }

    public async Task<List<FlashcardSetDTO>> GetAllFlashcardSets()
    {
        // commenting to check if it will pass with the MessageHandler
        //var token = _tokenStore.Token;      // setting the token for authorization
        //_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);     // adding the header
        var response = await _http.GetAsync("api/FlashcardSet");
        if(!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error ocurred: {(int)response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<List<FlashcardSetDTO>>();
        if (result == null)
        {
            return new List<FlashcardSetDTO>();     // temporarily returns an empty list
        }

        return result;
        // Further processing of the response can be done here
    }
}
