using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;

namespace Memora.ViewModels;

public class HomeViewModel : ViewModel
{
    public ObservableCollection<FlashcardSet> FlashcardSets { get; } = new ObservableCollection<FlashcardSet>();

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

    private readonly FlashcardSetApiService _flashcardSetApiService;

    public HomeViewModel(INavigationService navigation, FlashcardSetApiService flashcardService)
    {
        _flashcardSetApiService = flashcardService;
        _navigation = navigation;
        _ = LoadRecentFlaschardSetsAsync();

    }

    /// <summary>
    ///  retrieves flashcards by the API request
    /// </summary>
    /// <returns></returns>
    private async Task<List<FlashcardSet>> GetRecentFlashcardSetsAsync()
    {
        try
        {
            var result = await _flashcardSetApiService.GetRecentFlashcardSets();
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

        return new List<FlashcardSet>();     // returns empty list if fails
    }

    /// <summary>
    /// Assigns the flashcards to the observable collection as new objects
    /// </summary>
    /// <returns></returns>
    private async Task LoadRecentFlaschardSetsAsync()
    {
        var sets = await GetRecentFlashcardSetsAsync();         // calls the method above
        FlashcardSets.Clear();

        foreach (var set in sets)
        {
            FlashcardSets.Add(set);
        }
    }

}
