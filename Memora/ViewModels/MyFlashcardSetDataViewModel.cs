using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;


namespace Memora.ViewModels;

public class MyFlashcardSetDataViewModel : ViewModel
{
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
    private readonly SessionService _sessionService;

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

    #region Commands
    public RelayCommand AddFlashcardCommand { get; set; }
    public RelayCommand RemoveFlashcardCommand { get; set; }
    public RelayCommand SaveFlashcardsAsyncCommand { get; set; }
    public RelayCommand SaveChanges { get; set; }
    public RelayCommand NavigateRevisionModeCommand { get; set; }
    //public RelayCommand RemoveFlashcardAsyncCommand { get; set; }
    //public RelayCommand SaveAllFlashcardsAsyncCommand { get; set; }

    #endregion

    public MyFlashcardSetDataViewModel(INavigationService navService, FlashcardApiService flashcardApiService, SessionService sessionService)
    {
        _sessionService = sessionService;
        Navigation = navService;
        _flashcardApiService = flashcardApiService;
        OnCountChanged += IncreaseCount;
        AddFlashcardCommand = new RelayCommand(_ => AddEmptyFlashcardToList(), _ => true);
        // checks if the parameter is a flashcard, then removes it from the list
        RemoveFlashcardCommand = new RelayCommand(f =>
        {
            if (f is not Flashcard flashcard) return;
            RemoveFlashcardFromList(flashcard);
        }, _ => CanRemoveFlashcardFromList());
        SaveChanges = new RelayCommand(_ => SetSessionData(), o => true);
        NavigateRevisionModeCommand = new RelayCommand(o => { Navigation.NavigateTo<RevisionModeViewModel>(); }, o => true);        // Navigates to the Revision mode
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
        ModifiedFlashcards.Add(new Flashcard() {Id=0, FlashcardSetId=_setId, Front="", Back=""});
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
    /// Compares the 'modified' list with the original, to check if any flashcards were added or updated
    /// If so, append the list.
    /// Then, compares the original list with the modified, if there's any difference between the IDs.
    /// If there's an additional ID (that's not in the original), append the list.
    /// </summary>
    /// <returns></returns>
    private List<Flashcard> GetModifiedFlashcards()
    {
        var modifiedList = new List<Flashcard>();
        
        // checks if the ID, Front and Back are equal. If not - adds them to the list.
        var addedAndUpdated = ModifiedFlashcards.Where(f => !_fetchedFlashcards.Any(m => m.Id == f.Id &&  m.Front == f.Front && m.Back == f.Back)).ToList();
        modifiedList.AddRange(addedAndUpdated);

        // checks if ID match the original list - if there's any difference, it adds it to the list.
        var deletedFlashcards = _fetchedFlashcards.Where(m => !ModifiedFlashcards.Any(f => f.Id == m.Id));
        modifiedList.AddRange(deletedFlashcards);
        return modifiedList;        // returns a fully modified list, which can be passed to the API later on.
    }
    
    // TO BE DONE
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

            ModifiedFlashcards.Add(new Flashcard        // adds flashcards to the comparable list, so we can compare them later when we want to update them. Added separately in order to NOT pass a reference, but a copy.
            {
                Id = flashcard.Id,
                FlashcardSetId = flashcard.FlashcardSetId,
                Front = flashcard.Front,
                Back = flashcard.Back
            });

        }
        SetSessionData();       // sets session data here, because this method is called first (from another vm)
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

    // Assigns session data to the singleton SessionService.
    private void SetSessionData()
    {
        _sessionService.NewSession(ModifiedFlashcards.ToList());
    }


    #region Helpers
    /// <summary>
    /// returns the amount of modified flashcards. Will solve nice as a pop up that informs about the amount of flashcards modified.
    /// Pop-up to be implemented.
    /// </summary>
    /// <returns></returns>
    private int GetModifiedFlashcardsCount()
    {
        return GetModifiedFlashcards().Count();
    }

    #endregion
}
