using Memora.Model;
namespace Memora.Services;

/// <summary>
/// WIP. The service will store information related to the current session, including currently used/requested flashcard folders, sets, progress, etc.
/// </summary>
public class SessionService
{
    // all can be nullable, because everything is optional
    public Flashcard? flashcard { get; set; }
    public FlashcardSet? flashcardSet { get; set; }
    public FlashcardFolder? flashcardFolder { get; set; }
}
