namespace Memora.Model.StudyModes;

/// <summary>
/// Stores data about the result of one session.
/// Includes total/correct/incorrect answers.
/// Keeps track of what flashcards the user hasn't learnt yet, so they are able to revise only these later on.
/// </summary>
public class Result
{
    public int TotalAnswers { get; set; }
    public int CorrectAnswers { get; set; } = 0;        // nullable in case the mode is without providing correct/incorrect answers.
    public int IncorrectAnswers { get; set; } = 0;
    // Change this to a TemporaryList, which can hold:
    // still learning flashcards (revision mode)
    // incorrectly answered flashacrds (quiz mode)
    // incorrectly answered flaschards (typing mode)
    public List<Flashcard>? StillLearning { get; set; } = new List<Flashcard>(); // stores flashcards that the user marked as "don't know". Initialises as an empty list.
}
