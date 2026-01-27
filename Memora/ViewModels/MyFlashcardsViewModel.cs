using Memora.Core;
using Memora.DTOs;
using Memora.Interfaces;
using Memora.Services;
using System.Net.Http;
using System.Windows;
using Memora.DTOs;
using System.Collections.ObjectModel;

namespace Memora.ViewModels
{
    public class MyFlashcardsViewModel : ViewModel
    {
        private readonly FlashcardSetApiService _flashcardSetService;
        public ObservableCollection<FlashcardSetDTO> FlashcardSets { get; set; } = new ObservableCollection<FlashcardSetDTO>();

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public MyFlashcardsViewModel(INavigationService navService, FlashcardSetApiService flashcardSetService)
        {
            _navigation = navService;
            _flashcardSetService = flashcardSetService;

            _ = LoadFlaschardSetsAsync();      // fire and forget with the "discard" operator
        }


        private async Task<List<FlashcardSetDTO>> GetAllFlashcardSets()
        {
            try
            {
                var result = await _flashcardSetService.GetAllFlashcardSets();
                return result;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return new List<FlashcardSetDTO>();     // returns empty list if fails
        }

        private async Task LoadFlaschardSetsAsync()
        {
            var sets = await GetAllFlashcardSets();         // calls the method above
            FlashcardSets.Clear();

            foreach (var set in sets)
            {
                FlashcardSets.Add(set);
            }
        }






    }

}
