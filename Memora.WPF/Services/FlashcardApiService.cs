using Memora.Authentication;
using Memora.Model;
using MemoraWPF.Model;
using MemoraWPF.Model.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;

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

        public async Task<List<Flashcard>> GetAllSharedFlaschardsByIdAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"/set/{id}/shared");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"An error ocurred while retrieving flashcards from a shared set.\nCode message: {response.StatusCode}");
                }
                var result = await response.Content.ReadFromJsonAsync<List<Flashcard>>();
                if (result is null)
                    throw new InvalidOperationException("Error while getting shared flashcards information");
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            // returns empty if failed
            return new List<Flashcard>();
        }

        public async Task CloneFlashcardsToNewSet(int setId, List<Flashcard> flashcards)
        {
            var request = new CloneFlashcardsRequest { SetId = setId, Flashcards = flashcards };
            try
            {
                var response = await _http.PostAsJsonAsync($"/Copy", request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"An error ocurred while cloning flashcards from a shared set.\nCode message: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
