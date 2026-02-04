using Memora.Core;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;

namespace Memora.ViewModels;

public class MyFlashcardSetDataViewModel : ViewModel
{
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
    public RelayCommand SaveFlashcardAsyncCommand { get; set; }
    
    public MyFlashcardSetDataViewModel(FlashcardApiService flashcardApiService)
    {
        _flashcardApiService = flashcardApiService;
        AddFlashcardCommand = new RelayCommand(_ => AddEmptyFlashcardToList(), _ => true);

    }

    #region Buttons Logic 
    // Appends the list with an empty flashcard, where the user can input data.
    // The saving of the flashcard is handled with another command.
    private void AddEmptyFlashcardToList()
    {
        Flashcards.Add(new Flashcard() {FlashcardSetId=_setId, Front="", Back=""});
        //OnPropertyChanged("Flashcards");
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
        FlashcardsCount = flashcards.Count; // updates the count property
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
