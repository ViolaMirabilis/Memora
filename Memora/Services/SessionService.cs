using Memora.Model;
using Memora.Model.StudyModes;
namespace Memora.Services;

/// <summary>
/// WIP. The service will store information related to the current session, including currently used/requested flashcard folders, sets, progress, etc.
/// </summary>
public class SessionService
{
    // initialises an empty session
    public Session CurrentSession { get; set; } = null!;        // cannot be null

    public void NewSession(List<Flashcard> flashcards)
    {
        CurrentSession = new Session { FlashcardsCollection = flashcards };
    }

    public void NewSession(FlashcardSet flashcardSet)
    {
        CurrentSession = new Session { FlashcardSet = flashcardSet };
    }

    public void NewSession(List<Flashcard> flashcards, FlashcardSet flashcardSet)
    {
        CurrentSession = new Session { FlashcardsCollection = flashcards, FlashcardSet = flashcardSet };
    }

    public void NewResult(Result result)
    {
        CurrentSession.Result = result;
    }

    // to do
    // Clear current session
}
