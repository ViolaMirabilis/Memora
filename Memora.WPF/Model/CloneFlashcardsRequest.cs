namespace Memora.Model;

public class CloneFlashcardsRequest
{
    public int SetId { get; set; }
    public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}
