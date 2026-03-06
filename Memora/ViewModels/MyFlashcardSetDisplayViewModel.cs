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
    // placeholder
    private int index = 1;
    // temporary variable to store user's text from the searchbox
    private string _textSearch;
    public string TextSearch
    {
        get { return _textSearch; }
        set {
            _textSearch = value;
            OnPropertyChanged(TextSearch);

            // messagebox WORKS
            //MessageBox.Show($"{_textSearch}");
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

    private readonly FlashcardSetApiService _flashcardSetService;
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
    public RelayCommand AddNewFlashcardSet { get; set; }
    public RelayCommand DisplayNewName { get; set; }

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
        FlashcardSetsView = CollectionViewSource.GetDefaultView(FlashcardSets);
        AddNewFlashcardSet = new RelayCommand(_ => AddSet(), _ => true);
        DisplayNewName = new RelayCommand(obj => DisplayNewSetName(obj), _ => true);

    }
    // placeholder

    private void DisplayNewSetName(object obj)
    {
        MessageBox.Show("asdasd");
        var f = obj as FlashcardSet;
        MessageBox.Show($"New name: {f.Name}\nId: {f.Id}");
    }
    private void AddSet()
    {
        FlashcardSets.Add(new FlashcardSet { Name = $"New set {index}" });
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
