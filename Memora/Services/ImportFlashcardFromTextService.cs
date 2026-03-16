using Memora.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Memora.Services;

public class ImportFlashcardFromTextService
{
    public List<Flashcard> SplitFlashcards(string importedFlashcards, string separator)
    {
        List<Flashcard> splitFlashcards = new List<Flashcard>();
        if (string.IsNullOrEmpty(importedFlashcards) || string.IsNullOrEmpty(separator))
            return splitFlashcards;

        // if separator is \\t, replace it with \t
        separator = separator == "\\t" ? "\t" : separator;

        // splits the entire string by new lines
        // e.g. front   separator   back is one "line" now.
        string[] lines = importedFlashcards.Split('\n');
        foreach (var line in lines)
        {
            // splits current line, e.g WordOne SEPARATOR WordTwo by the separator
            // puts wordOne into subs[0] and wordTwo into subs[1]
            string[] subs = line.Split(separator);
            if (subs.Length >= 2)
            {
                // additionally trimming the whitespaces
                splitFlashcards.Add(new Flashcard { Front = subs[0].Trim(), Back = subs[1].Trim() });
            }
        }
        return splitFlashcards;
    }

    public void AppendFlashcardList(ICollection<Flashcard> original, List<Flashcard> imported)
    {
        foreach (var flashcard in imported)
        {
            original.Add(flashcard);
        }
    }



}
