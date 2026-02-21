using Memora.Model;
using Memora.Model.StudyModes;
using System.Windows;

namespace Memora.Services;
/// <summary>
/// Logic for Quiz Mode as a separate service.
/// Each flashcard has four answers, one of which is correct.
/// </summary>
public class QuizModeService
{
    // Imported, original flashcards
    private List<Flashcard> _flashcards = new List<Flashcard>();
    // holds questions and answers for the quiz mode.
    private List<QuizAnswer> _quizAnswers = new List<QuizAnswer>();
    public int TotalQuestions { get; private set; }
    public string CurrentAnswer { get; private set; } = string.Empty;

    /// <summary>
    /// Initialises list of flashcards and assigns the total amount of questions
    /// </summary>
    /// <param name="flashcards"></param>
    public void InitialiseFlashcards(ICollection<Flashcard> flashcards)
    {
        _flashcards = flashcards.ToList();
        TotalQuestions = flashcards.ToList().Count;
    }

    public void InitialiseQuizData()
    {
        // initialises the quizData list from the fetched flashcards
        SetQuestionsWithCorrectAnswers(_flashcards);
        // adds wrong answers to the quiz data list, so each question has 1 correct and 3 wrong answers
        SetWrongAnswers(_quizAnswers);
    }

    public List<QuizAnswer> GetQuizAnswers()
    {
        return _quizAnswers;
    }

    public void SetQuestionsWithCorrectAnswers(ICollection<Flashcard> flashcards)
    {
        // Adds questions and correct answers to the quiz data list (which serves as the main logic for the quiz)
        foreach (var flashcard in flashcards)
        {
            _quizAnswers.Add(new QuizAnswer { Question = flashcard.Front, CorrectAnswer = flashcard.Back });
        }
    }
    // Adds wrong answers to the general quiz answer in the entire list
    public void SetWrongAnswers(List<QuizAnswer> quizAnswers)
    {
        if (quizAnswers.Count < 6)
        {
            // MessageBox is temporary
            MessageBox.Show("The quiz mode needs at least 6 flashcards!");
            return;  // returns an empty list to prevent further errors, since we need at least 6 flashcards to have 3 wrong answers and 1 correct answer for each question.
        }

        foreach (var answer in quizAnswers)
        {
            answer.IncorrectAnswers = SetWrongAnswerPerFlashcard(quizAnswers, answer);
        }
    }

    // returns a string[3] with 3 wrong answers. Needs to be called on each QuizAnswer individually
    public string[] SetWrongAnswerPerFlashcard(List<QuizAnswer> quizAnswers, QuizAnswer originalAnswer)
    {
        // creates an array to hold 3 "wrong" answers
        string[] wrongAnswers = new string[3];
        // gets a copy of the flashcards list so we dont erase data after each iteration
        var flashcardsCopy = quizAnswers.ToList();
        // shuffling the list of Flashcards, so we can freely take elements 0, 1, 2 without using random here
        flashcardsCopy.Shuffle();
        int i = 0;
        // Runs while we have less than 3 wrong answers AND the list with answers isn't empty.
        foreach (var randomFlashcard in flashcardsCopy)
        {
            if (i >= 3) break;
            // if random answer is equal to correct answer (random compared to the original) OR wrong answers already ocntians the answer OR the answer is null = continue;
            if (randomFlashcard.CorrectAnswer == originalAnswer.CorrectAnswer || wrongAnswers.Contains(randomFlashcard.CorrectAnswer))
                continue;

            // if no duplicate was found, we assign a "wrong" answer to the array
            wrongAnswers[i] = randomFlashcard.CorrectAnswer;
            i++;
        }

        return wrongAnswers;
    }

}
