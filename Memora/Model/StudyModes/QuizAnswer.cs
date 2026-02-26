namespace Memora.Model.StudyModes;

/// <summary>
/// Used for the quiz mode, where each flashcard has one correct answer and three incorrect answers
/// </summary>
public class QuizAnswer
{
    public int Index { get; set; }
    // Holds a question (the front of the flashcard)
    public string Question { get; set; } = string.Empty;
    // answer (back of the flashcard)
    public string CorrectAnswer { get; set; } = string.Empty;
    // three incorrect answers + the correct one
    public string[] IncorrectAnswers { get; set; } = new string[4];
    public string SelectedAnswer { get; set; } = string.Empty;
}
