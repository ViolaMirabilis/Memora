using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using Memora.ViewModels.StudyModes;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;


namespace Memora.ViewModels;

public class MyFlashcardSetDataViewModel : ViewModel
{
    private string _sharingCode = string.Empty;
    public string SharingCode
    {
        get => _sharingCode;
        set { _sharingCode = value; OnPropertyChanged(); }
    }
    // PLACEHOLDER. Holds the separator character.
    private string _separator = string.Empty;
    public string Separator
    {
        get => _separator;
        set { _separator = value; OnPropertyChanged(); }
    }
    // PLACEHOLDER. Holds the IsOpen value needed for displaying the popup.
    private bool _isImportOpen;
    public bool IsImportOpen
    {
        get => _isImportOpen;
        set { _isImportOpen = value; OnPropertyChanged();}
    }
    // TO DO:
    // MOVE SET'S LOGIC TO A SERVICE TO SEPARATE CONCERNS
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

    private bool _isSharing;
    public bool IsSharing
    {
        get => _isSharing;
        set {_isSharing = value; OnPropertyChanged(); }
    }
    private int _setId { get; set; }
    private int _flashcardsCount;   // bindable property for flashcard count
    public int FlashcardsCount
    {
        get { return _flashcardsCount; }
        set { _flashcardsCount = value; OnPropertyChanged();
        }
    }
    private readonly ImportFlashcardFromTextService _importFromText;
    private readonly FlashcardApiService _flashcardApiService;
    private readonly FlashcardSetApiService _flashcardSetApiService;
    // stores all the flashcards fetched from the API.
    private List<Flashcard> _fetchedFlashcards = new List<Flashcard>();     // contains flashcards that are fetched from the API ("the original flashcards")
    public ObservableCollection<Flashcard> ModifiedFlashcards { get; set; } = new ObservableCollection<Flashcard>();   // all the operations are done on this one, in order to compare them to the original and update only the changed flashcards

    #region Commands
    public RelayCommand AddFlashcardCommand { get; set; }
    public RelayCommand RemoveFlashcardCommand { get; set; }
    // only one needed?
    public RelayCommand SaveFlashcardsAsyncCommand { get; set; }
    public RelayCommand SaveChanges { get; set; }
    public RelayCommand NavigateRevisionModeCommand { get; set; }
    public RelayCommand NavigateQuizModeCommand { get; set; }
    public RelayCommand ShareSetCommand { get; set; }
    public RelayCommand SwapFrontWithBackCommand { get; set; }
    public RelayCommand ToggleIsOpenCommand { get; set; }
    public RelayCommand ImportFlashcardsFromTextCommand { get; set; }
    //public RelayCommand RemoveFlashcardAsyncCommand { get; set; }
    //public RelayCommand SaveAllFlashcardsAsyncCommand { get; set; }

    #endregion

    public MyFlashcardSetDataViewModel(INavigationService navService,
        FlashcardApiService flashcardApiService,
        FlashcardSetApiService flashcardSetApiService,
        SessionService sessionService,
        ImportFlashcardFromTextService importFromText)
    {
        _importFromText = importFromText;
        _sessionService = sessionService;
        Navigation = navService;
        _flashcardApiService = flashcardApiService;
        _flashcardSetApiService = flashcardSetApiService;
        OnCountChanged += IncreaseCount;
        AddFlashcardCommand = new RelayCommand(_ => AddEmptyFlashcardToList(), _ => true);
        // checks if the parameter is a flashcard, then removes it from the list
        RemoveFlashcardCommand = new RelayCommand(f =>
        {
            if (f is not Flashcard flashcard) return;
            RemoveFlashcardFromList(flashcard);
        }, _ => CanRemoveFlashcardFromList());
        SaveChanges = new RelayCommand(_ => SetSessionData(), o => true);
        NavigateRevisionModeCommand = new RelayCommand(o => { Navigation.NavigateTo<RevisionModeViewModel>(); }, _ => true);        // Navigates to the Revision mode
        NavigateQuizModeCommand = new RelayCommand(_ => { Navigation.NavigateTo<QuizModeViewModel>(); }, _ => true);
        // toggles the sharing mode ON and OFF
        ShareSetCommand = new RelayCommand(async _ => await GetSharingCode(), _ => true);
        SwapFrontWithBackCommand = new RelayCommand(_ => SwapFrontWithBack(), _ => true);
        ToggleIsOpenCommand = new RelayCommand(_ => ToggleIsOpen(), _ => true);
        ImportFlashcardsFromTextCommand = new RelayCommand(obj => ImportFromText(obj), _ => true);

        InitializeSharingWindow();
    }

    #region Placeholders
    // method name to change
    /// <summary>
    /// Checks if the set is already marked as "Sharing". If it does, the user does not have to reopen the "share" window and the code is visible right away.
    /// </summary>
    private void InitializeSharingWindow()
    {
        var set = _sessionService.CurrentSession.FlashcardSet;
        // Initializes the IsSharing VM property
        if (set.IsSharing == true)
        {
            //VM property
            IsSharing = true;
            SharingCode = set.SharingCode;
        }
    }
    private async Task GetSharingCode()
    {
        try
        {
            // VM bindable property
            IsSharing = true;
            FlashcardSet set = _sessionService.CurrentSession.FlashcardSet!;
            // if set is not being marked as "shared", send an API request
            if (set.IsSharing == false)
            {
                await _flashcardSetApiService.ShareFlashcardSet(set.Id);
                set.IsSharing = true;
                // gets the sharing code
                var code = await _flashcardSetApiService.GetFlashcardSetSharingCode(set.Id);
                // sets the sharing code both on the FlaschardSet property AND the VM
                set.SharingCode = code;
                SharingCode = code;
                OnPropertyChanged(nameof(SharingCode));

            }
            else if (set.IsSharing == true)
            {
                var code = await _flashcardSetApiService.GetFlashcardSetSharingCode(set.Id);
                // sets the sharing code
                SharingCode = code;
                OnPropertyChanged(nameof(SharingCode));
            }
        } catch(HttpRequestException ex)
        {
            MessageBox.Show($"An error ocurred while obtaining the sharing code.\nError message: {ex.Message}");
        }
        
    }
    private void ImportFromText(object obj)
    {
        // converts the unformatted object to string
        string unformattedFlashcards = obj.ToString();
        // takes unformatted string of flashcards and a separator
        var formattedFlashcards = _importFromText.SplitFlashcards(unformattedFlashcards, Separator);
        _importFromText.AppendFlashcardList(ModifiedFlashcards, formattedFlashcards);
        OnPropertyChanged(nameof(ModifiedFlashcards));
        IsImportOpen = !IsImportOpen;
    }
    private void ToggleIsOpen()
    {
        IsImportOpen = !IsImportOpen;
    }
    // temporary solution
    private void SwapFrontWithBack()
    {
        // creating a new "swapped flashcard" and assigning it to the old collection.
        // the previous approach used a reference and operated on the same objects (flashcards)
        // but now with new Flashcard(...) we're creating a completely new object
        ObservableCollection<Flashcard> swappedCollection = new ObservableCollection<Flashcard>(
            ModifiedFlashcards.Select(f => new Flashcard
            {
                Front = f.Back,
                Back = f.Front
            }));

        ModifiedFlashcards = swappedCollection;
        OnPropertyChanged(nameof(ModifiedFlashcards));
    }
    #endregion
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
        try
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
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"An error ocurred while loading all flashcards.\nError message: {ex.Message}");
        }

    }

    private async Task<List<Flashcard>> GetAllFlashcardsById()
    {
        // exception handled in the GetAllFlashcardsByIdAsync method
        var result = await _flashcardApiService.GetAllFlashcardsByIdAsync(_setId);
        return result;
    }

    // Assigns session data to the singleton SessionService.
    private void SetSessionData()
    {
        _sessionService.CurrentSession.SetFlashcardCollection(ModifiedFlashcards.ToList());
        //_sessionService.NewSession(ModifiedFlashcards.ToList());
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
