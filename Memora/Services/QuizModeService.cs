using Memora.Model;

namespace Memora.Services;
/// <summary>
/// Logic for Quiz Mode as a separate service.
/// Each flashcard has four answers, one of which is correct.
/// </summary>
public class QuizModeService
{
    // Imported, original flashcards
    private List<Flashcard> _flashcards = new List<Flashcard>();
    private Dictionary<int, QuizModeFlashcard> _questions = new Dictionary<int, QuizModeFlashcard>();
    public int TotalQuestions { get; private set; }
    public string CurrentAnswer { get; private set; } = string.Empty;
    public string AnswerA { get; private set; } = string.Empty;
    public string AnswerB { get; private set; } = string.Empty;
    public string AnswerC { get; private set; } = string.Empty;
    public string AnswerD { get; private set; } = string.Empty;

    /// <summary>
    /// Initialises list of flashcards and assigns the total amount of questions
    /// </summary>
    /// <param name="flashcards"></param>
    public void InitialiseFlashcards(ICollection<Flashcard> flashcards)
    {
        _flashcards = flashcards.ToList();
        TotalQuestions = flashcards.ToList().Count;
    }

    public ICollection<Flashcard> GetQuestionsCollection()
    {
        return _flashcards;
    }
    public void SetQuestions(ICollection<Flashcard> flashcards)
    {
        int questionNumber = 1;
        foreach (var flashcard in flashcards)
        {
            // Assigns a flashcard to a QuizModeFlashcard
            var quizModeFlashcard = (QuizModeFlashcard)flashcard;
            // Assigns an answer to the QuizModeFlashcard
            quizModeFlashcard.CorrectAnswer = flashcard.Back;
            _questions.Add(questionNumber, new QuizModeFlashcard{ });
            questionNumber++;
        }
    }

    public void DisplayAllQuestions(Dictionary<int, Flashcard> questions)
    {
        foreach (var question in questions)
        {
            Console.WriteLine($"Id: {question.Key}, Question:{question.Value.Front}");
        }
    }

}
