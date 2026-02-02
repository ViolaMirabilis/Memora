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
    private readonly FlashcardApiService _flashcardApiService;
    public ObservableCollection<Flashcard> Flashcards { get; set; } = new ObservableCollection<Flashcard>();
    
    public MyFlashcardSetDataViewModel(FlashcardApiService flashcardApiService)
    {
        _flashcardApiService = flashcardApiService;
    }


    public async Task LoadFlaschardsByIdAsync(int id)
    {
        _setId = id;        // sets the ID, then calls the method
        var flashcards = await GetAllFlashcardsById();
        Flashcards.Clear();

        foreach (var flashcard in flashcards)
        {
            Flashcards.Add(flashcard);
        }
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
