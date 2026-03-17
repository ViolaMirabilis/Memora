using Memora.Interfaces;
using Memora.Model;
using System.Net.Http;
using System.Net.Http.Json;

namespace Memora.Services;

public class FlashcardSetApiService
{
    private readonly HttpClient _http;
    public FlashcardSetApiService(IHttpClientFactory http)
    {
        _http = http.CreateClient("ApiClient");             // named client
    }

    /// <summary>
    /// Get all flashcards for current user
    /// </summary>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
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

    /// <summary>
    /// Creating a new, empty flashcard set with a given name.
    /// Does not return an object of FlashcardSet, because UI updates are handled by WPF in this case.
    /// </summary>
    /// <param name="set"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public async Task CreateFlashcardSet(FlashcardSet set)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/FlashcardSet", set);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error ocurred while uploading a flashcard set.\nCode message: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// Updates the name of an existing flashcard set
    /// </summary>
    /// <param name="updatedSet"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public async Task UpdateFlashcardSetName(UpdatedNameFlashcardSet updatedSet)
    {
        try
        {
            var response = await _http.PatchAsJsonAsync("/api/FlashcardSet", updatedSet);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error ocurred while updating the flashcard set name.\nCode message: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    public async Task DeleteFlashcardSet(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"/api/FlashcardSet/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error ocurred while deleting the flashcard set.\nCode message: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {

        }

    }
}
