namespace MemoraWPF.Model;

public class FlashcardSetContentResponse
{
    public List<SimpleFlashcardDTO> Flashcards { get; set; } = new List<SimpleFlashcardDTO>();
}

public class SimpleFlashcardDTO
{
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}
