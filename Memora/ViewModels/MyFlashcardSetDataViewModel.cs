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
    public ObservableCollection<Flashcard> Flashcards { get; set; } = new ObservableCollection<Flashcard>();

    public RelayCommand AddFlashcardCommand { get; set; }
    public RelayCommand RemoveFlashcardCommand { get; set; }
    //public RelayCommand SaveFlashcardAsyncCommand { get; set; }
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
        }, _ => true);
    }       

    #region Event Logic
    public void IncreaseCount()
    {
        FlashcardsCount = Flashcards.Count;
    }
    #endregion

    #region Add and Delete logic 
    // Appends the list with an empty flashcard, where the user can input data.
    // The saving of the flashcard is handled with another command.
    private void AddEmptyFlashcardToList()
    {
        Flashcards.Add(new Flashcard() {FlashcardSetId=_setId, Front="", Back=""});
        OnCountChanged?.Invoke();
    }

    private void RemoveFlashcardFromList(Flashcard flashcard)
    {
        Flashcards.Remove(flashcard);
        OnCountChanged?.Invoke();
    }


    #endregion


    public async Task LoadFlaschardsByIdAsync(int id)
    {
        _setId = id;        // sets the ID, then calls the method
        var flashcards = await GetAllFlashcardsById();
        Flashcards.Clear();

        foreach (var flashcard in flashcards)
        {
            Flashcards.Add(flashcard);
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
