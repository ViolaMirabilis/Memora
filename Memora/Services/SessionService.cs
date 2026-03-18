using Memora.Controls;
using Memora.Model;
using Memora.Model.StudyModes;
namespace Memora.Services;

/// <summary>
/// WIP. The service will store information related to the current session, including currently used/requested flashcard folders, sets, progress, etc.
/// </summary>
public class SessionService
{
    // initialises an empty session
    public Session CurrentSession { get; set; } = new Session();

    public void NewSession(List<Flashcard> flashcards)
    {
        //CurrentSession.FlashcardsCollection = flashcards;
    }

    public void NewFlashcardSet(Memora.Model.FlashcardSet set)
    {
        //CurrentSession.FlashcardSet = set;
    }

    public void NewResult(Result result)
    {
        //CurrentSession.Result = result;
    }

    // to do
    // Clear current session
}
