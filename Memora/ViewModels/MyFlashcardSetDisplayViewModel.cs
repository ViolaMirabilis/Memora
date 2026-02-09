using Memora.Core;
using Memora.Interfaces;
using Memora.Services;
using System.Net.Http;
using System.Windows;
using Memora.Model;

using System.Collections.ObjectModel;

namespace Memora.ViewModels;

public class MyFlashcardSetDisplayViewModel : ViewModel
{
    private readonly FlashcardSetApiService _flashcardSetService;
    public ObservableCollection<FlashcardSet> FlashcardSets { get; set; } = new ObservableCollection<FlashcardSet>();

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

    public RelayCommand NavigateFlashcardDataCommand { get; set; }

    public MyFlashcardSetDisplayViewModel(INavigationService navService, FlashcardSetApiService flashcardSetService)
    {
        _navigation = navService;
        _flashcardSetService = flashcardSetService;

        // We're using the overloaded method from NavigationService. vm => _ = vm... is set to the TViewModel instance
        // and we're just using the method this way
        NavigateFlashcardDataCommand = new RelayCommand(o =>
        {
            if (o is not FlashcardSet set) return;

            Navigation.NavigateTo<MyFlashcardSetDataViewModel>(
                vm => _ = vm.LoadFlaschardsByIdAsync(set.Id)); }, _ => true
        );
         
        _ = LoadFlaschardSetsAsync();      // fire and forget with the "discard" operator
    }

    private async Task<List<FlashcardSet>> GetAllFlashcardSets()
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

        return new List<FlashcardSet>();     // returns empty list if fails
    }

    /// <summary>
    /// Loads already fetched flashcards into the list, which is then displayed in the list view
    /// </summary>
    /// <returns></returns>
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
