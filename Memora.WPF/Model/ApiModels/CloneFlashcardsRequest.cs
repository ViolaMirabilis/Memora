using Memora.Model;

namespace MemoraWPF.Model.ApiModels;

public class CloneFlashcardsRequest
{
    public int SetId { get; set; }
    public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}
