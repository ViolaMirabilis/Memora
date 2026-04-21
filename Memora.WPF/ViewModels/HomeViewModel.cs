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
    private readonly SessionService _sessionService;

    #region Commands
    public RelayCommand NavigateFlashcardDataCommand { get; set; }
    #endregion

    public HomeViewModel(INavigationService navigation, FlashcardSetApiService flashcardService, SessionService sessionService)
    {
        _flashcardSetApiService = flashcardService;
        _sessionService = sessionService;
        _navigation = navigation;
        _ = LoadRecentFlaschardSetsAsync();

        NavigateFlashcardDataCommand = new RelayCommand(async obj => await SaveContextAndNavigate(obj), _ => true);

    }

    /// <summary>
    /// reused from MyFlashcardSetDisplayViewModel, maybe can be refactor to a helper class later on if too much duplication is made
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private async Task SaveContextAndNavigate(object obj)
    {
        if (obj is FlashcardSet set)
        {
            await _flashcardSetApiService.UpdateLastStudied(set.Id);
            _sessionService.CurrentSession.SetFlashcardSet(set);
            Navigation.NavigateTo<MyFlashcardSetDataViewModel>(vm => _ = vm.LoadFlaschardsByIdAsync(set.Id));
        }

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
