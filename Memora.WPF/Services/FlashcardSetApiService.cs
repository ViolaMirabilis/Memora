using Memora.Interfaces;
using Memora.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

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
            MessageBox.Show(ex.ToString());
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
            MessageBox.Show(ex.ToString());
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
            MessageBox.Show(ex.ToString());
        }

    }

    /// <summary>
    /// Sends a request to share the set
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<bool> ShareFlashcardSet(int id)
    {
        try
        {
            var response = await _http.PostAsync($"/api/FlashcardSet/{id}/sharing", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error ocurred while sharing the flashcard set.\nCode message: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<bool>();
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        return false;
    }

    public async Task<string> GetFlashcardSetSharingCode(int id)
    {
        try
        {
            // temporary endpoint, will be changed later
            var response = await _http.GetAsync($"/api/FlashcardSet/{id}/sharing-code");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error ocurred while getting the shared code.\nCode message: {response.StatusCode}");
            }
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        return null;
    }

    /// <summary>
    /// Gets a flashcard set BY the sharing code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<FlashcardSet> GetFlashcardSetByCode(string code)
    {
        try
        {
            var response = await _http.GetAsync($"api/FlashcardSet/{code}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("An error ocurred while retrieving a flashcard set by code");
            }

            var result = await response.Content.ReadFromJsonAsync<FlashcardSet>();

            if (result == null)
            {
                return new FlashcardSet();     // temporarily returns an empty list
            }

            return result;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        return null;
    }

    public async Task<FlashcardSet> GetLastFlashcard()
    {
        try
        {
            var response = await _http.GetAsync("LastAdded");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("An error ocurred while retrieving the last flashcard set");
            }

            var result = await response.Content.ReadFromJsonAsync<FlashcardSet>();
            return result;

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        return new FlashcardSet();
    }
}
