using Memora.Core;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Xml;

namespace Memora.ViewModels;

public class MyFlashcardSetDataViewModel : ViewModel
{
    private event Action OnCountChanged;        // when a flashcard is added/deleted, the event is fired and the count is recalculated
    private int _setId { get; set; }
    private int _flashcardsCount;   // bindable property for flashcard count
    public int FlashcardsCount
    {
        get { return _flashcardsCount; }
        set { _flashcardsCount = value; OnPropertyChanged();
        }
    }
    private readonly FlashcardApiService _flashcardApiService;
    // stores all the flashcards fetched from the API.
    private List<Flashcard> _fetchedFlashcards = new List<Flashcard>();     // contains flashcards that are fetched from the API ("the original flashcards")
    public ObservableCollection<Flashcard> ModifiedFlashcards { get; set; } = new ObservableCollection<Flashcard>();   // all the operations are done on this one, in order to compare them to the original and update only the changed flashcards

    public RelayCommand AddFlashcardCommand { get; set; }
    public RelayCommand RemoveFlashcardCommand { get; set; }
    public RelayCommand SaveFlashcardsAsyncCommand { get; set; }
    //public RelayCommand RemoveFlashcardAsyncCommand { get; set; }
    //public RelayCommand SaveAllFlashcardsAsyncCommand { get; set; }

    public MyFlashcardSetDataViewModel(FlashcardApiService flashcardApiService)
    {
        _flashcardApiService = flashcardApiService;
        OnCountChanged += IncreaseCount;
        AddFlashcardCommand = new RelayCommand(_ => AddEmptyFlashcardToList(), _ => true);
        // checks if the parameter is a flashcard, then removes it from the list
        RemoveFlashcardCommand = new RelayCommand(f =>
        {
            if (f is not Flashcard flashcard) return;
            RemoveFlashcardFromList(flashcard);
        }, _ => CanRemoveFlashcardFromList());
    }       

    #region Event Logic
    public void IncreaseCount()
    {
        FlashcardsCount = ModifiedFlashcards.Count;
    }
    #endregion

    #region Add and Delete logic 
    // Appends the list with an empty flashcard, where the user can input data.
    // The saving of the flashcard is handled with another command.

    private void AddEmptyFlashcardToList()
    {
        ModifiedFlashcards.Add(new Flashcard() {FlashcardSetId=_setId, Front="", Back=""});
        OnCountChanged?.Invoke();
    }

    private void RemoveFlashcardFromList(Flashcard flashcard)
    {
        ModifiedFlashcards.Remove(flashcard);
        OnCountChanged?.Invoke();
    }

    private bool CanRemoveFlashcardFromList()
    {
        return ModifiedFlashcards.Count > 1;
    }
    #endregion

    #region Updating and saving the flashcard/flashcard sets
    /// <summary>
    /// Compares the "modified" list with the original and looks for any differences.
    /// If a difference is made (even a single letter change), the flashcard is added to this list.
    /// This list is a basis for updating the flashcards via an API and prevents going through all the flashcards and updating them, despite not changes being made.
    /// </summary>
    /// <returns></returns>
    private List<Flashcard> GetModifiedFlashcards()
    {
        // if modified front and back is not equal the fetched (original), add to the list.
        var modifiedList = ModifiedFlashcards.Where(f => !_fetchedFlashcards.Any(m => m.Front == f.Front && m.Back == f.Back)).ToList();
        return modifiedList;
    }
    /// <summary>
    /// returns the amount of modified flashcards. Will solve nice as a pop up that informs about the amount of flashcards modified.
    /// TO BE IMPLEMENTED.
    /// </summary>
    /// <returns></returns>
    private int GetModifiedFlashcardsCount()
    {
        return GetModifiedFlashcards().Count;
    }
    private async Task SaveFlashcardAsync()
    {

    }
    #endregion


    public async Task LoadFlaschardsByIdAsync(int id)
    {
        _setId = id;        // sets the ID, then calls the method
        var flashcards = await GetAllFlashcardsById();
        _fetchedFlashcards.Clear();      // clears the set, so they don't get duplicated
        ModifiedFlashcards.Clear();      // clears the set, so they don't get duplicated

        foreach (var flashcard in flashcards)
        {
            _fetchedFlashcards.Add(flashcard);      // adds flashcards to the observ
            ModifiedFlashcards.Add(flashcard);     // adds flashcards to the comparable list, so we can compare them later when we want to update them 
        }

        OnCountChanged?.Invoke();
    }

    private async Task<List<Flashcard>> GetAllFlashcardsById()
    {
        try
        {
            var result = await _flashcardApiService.GetAllFlashcardsByIdAsync(_setId);
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

        return new List<Flashcard>();     // returns empty list if fails
    }
}
