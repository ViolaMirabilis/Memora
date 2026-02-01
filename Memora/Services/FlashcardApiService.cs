using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Memora.Model;
using Memora.Authentication;

namespace Memora.Services
{
    public class FlashcardApiService
    {
        private readonly HttpClient _http;

        public FlashcardApiService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<List<Flashcard>> GetAllFlashcardsByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/Flashcard/set/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Flashcard>>();
            if (result is null)
                throw new InvalidOperationException("Error while getting Flashcards information");
            return result;
        }
    }
}
