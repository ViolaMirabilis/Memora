using Memora.Core;
using Memora.Interfaces;
using Memora.Services;
using System.Net.Http;
using System.Windows;
using Memora.Model;

using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;

namespace Memora.ViewModels;

public class MyFlashcardSetDisplayViewModel : ViewModel
{
    #region placeholders
    private string _code = string.Empty;
    public string Code
    {
        get => _code;
        set { _code = value; OnPropertyChanged(); }
    }
    // placeholder
    private int index = 1;
    // temporary variable to store user's text from the searchbox
    private string _textSearch;
    public string TextSearch
    {
        get { return _textSearch; }
        set {
            _textSearch = value;
            OnPropertyChanged(nameof(TextSearch));

            // using a predicate to filter the collection
            if (!string.IsNullOrEmpty(_textSearch))
            {
                // casting hte object to FlashcardSet to access its "name" property
                // both name and the input are checked as lowercase.
                FlashcardSetsView.Filter = new Predicate<object>(o => ((FlashcardSet)o).Name.Contains(TextSearch, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                // removes the filter if empty
                FlashcardSetsView?.Filter = null;
            }

            
        }
    }
    #endregion
    private readonly FlashcardApiService _flashcardApiService;
    private readonly FlashcardSetApiService _flashcardSetService;
    private readonly SessionService _sessionService;
    public ObservableCollection<FlashcardSet> FlashcardSets { get; set; } = new ObservableCollection<FlashcardSet>();
    // @see: https://stackoverflow.com/questions/37385532/implenting-listcollectionview-from-observablecollection
    public ICollectionView FlashcardSetsView { get; set; }

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
    public RelayCommand CreateNewFlashcardSetAsync { get; set; }
    public RelayCommand DisplayNewName { get; set; }
    public RelayCommand ImportFromCodeCommandAsync { get; set; }
    public RelayCommand UpdateNameCommandAsync { get; set; }
    public RelayCommand DeleteFlashcardSetCommandAsync { get; set; }
    public RelayCommand ToggleIsOpenCommand { get; set; }
    private bool _isImportOpen;
    public bool IsImportOpen
    {
        get => _isImportOpen;
        set { _isImportOpen = value; OnPropertyChanged(); }
    }

    public MyFlashcardSetDisplayViewModel(INavigationService navService,
        FlashcardApiService flashcardApiService,
        FlashcardSetApiService flashcardSetService,
        SessionService session)
    {
        _flashcardApiService = flashcardApiService;
        _navigation = navService;
        _flashcardSetService = flashcardSetService;
        _sessionService = session;

        
        NavigateFlashcardDataCommand = new RelayCommand(async obj => await SaveContextAndNavigate(obj), _ => true);
        // OLD
        // We're using the overloaded method from NavigationService. vm => _ = vm... is set to the TViewModel instance
        // and we're just using the method this way
        //{
        /*if (o is not FlashcardSet set) return;

        Navigation.NavigateTo<MyFlashcardSetDataViewModel>(
            vm => _ = vm.LoadFlaschardsByIdAsync(set.Id)); }, _ => true*/
        //);
        _ = LoadFlaschardSetsAsync();      // fire and forget with the "discard" operator
        FlashcardSetsView = CollectionViewSource.GetDefaultView(FlashcardSets);
        CreateNewFlashcardSetAsync = new RelayCommand(async _ => await CreateFlashcardSetAsync(), _ => true);
        ImportFromCodeCommandAsync = new RelayCommand(async _ => await ImportFromCodeAsync(), _ => true);
        UpdateNameCommandAsync = new RelayCommand(async obj => await UpdateFlashcardSetNameAsync(obj), _ => true);
        DeleteFlashcardSetCommandAsync = new RelayCommand(async obj => await DeleteFlashcardSetAsync(obj), _ => true);
        ToggleIsOpenCommand = new RelayCommand(_ => ToggleIsOpen(), _ => true);

    }

    private void ToggleIsOpen()
    {
        // toggles the bool
        IsImportOpen = !IsImportOpen;
        // clears the code
        Code = string.Empty;
    }
    private async Task SaveContextAndNavigate(object obj)
    {
        if (obj is FlashcardSet set)
        {
            await _flashcardSetService.UpdateLastStudied(set.Id);
            _sessionService.CurrentSession.SetFlashcardSet(set);
            Navigation.NavigateTo<MyFlashcardSetDataViewModel>(vm => _ = vm.LoadFlaschardsByIdAsync(set.Id));
        }
        
    }

    private async Task ImportFromCodeAsync()
    {
        try
        {
            IsImportOpen = !IsImportOpen;
            // retrieves the flashcard set by a sharing code
            FlashcardSet set = await _flashcardSetService.GetFlashcardSetByCode(Code);
            // gets all the flashcards from a shared set
            List<Flashcard> flashcardsFromSharedSet = await _flashcardApiService.GetAllSharedFlaschardsByIdAsync(set.Id);

            // adds the empty set to the user's database via API
            await _flashcardSetService.CreateFlashcardSet(set);
            // gets the last flashcard set from the DB
            FlashcardSet newSet = await _flashcardSetService.GetLastFlashcard();
            // clones the flashcard from the original to the copy
            await _flashcardApiService.CloneFlashcardsToNewSet(newSet.Id, flashcardsFromSharedSet);

            // adds it to the local list
            FlashcardSets.Add(newSet);
        } catch (HttpRequestException ex)
        {
            MessageBox.Show($"An error ocurred while importing the flashcard set from code.\nError message: {ex.Message}");
        }
        

    }

    /// <summary>
    /// Updates the flashcard set's name in the collection AND in the db via an API
    /// </summary>
    /// <returns></returns>
    private async Task DeleteFlashcardSetAsync(object obj)
    {
        if (obj is FlashcardSet set)
        {
            var setId = set.Id;
            await _flashcardSetService.DeleteFlashcardSet(setId);
            FlashcardSets.Remove(set);
        }
    }


    private async Task UpdateFlashcardSetNameAsync(object obj)
    {
        if (obj is FlashcardSet set)
        {
            var updatedName = new UpdatedNameFlashcardSet {Id = set.Id, Name = set.Name };
            await _flashcardSetService.UpdateFlashcardSetName(updatedName);
        }
    }

    /// <summary>
    /// Adds the flashcard set both to the observable collection and to the DB via an API
    /// </summary>
    /// <returns></returns>
    private async Task CreateFlashcardSetAsync()
    {
        // creates a temporary object for the visual side
        var tmpSet = new FlashcardSet { Name = $"New set {index}" };
        // POST set to API
        // the API assigns the ID, its unknown here, because it's auto incremented in the API
        await _flashcardSetService.CreateFlashcardSet(tmpSet);
        var newSet = await _flashcardSetService.GetLastFlashcard();
        // Adds set to the observable collection
        FlashcardSets.Add(newSet);
        // Index keeps track of "newly created" sets, e.g. Set(1), Set(2), Set(3), etc.
        index++;
        
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
