namespace Memora.Extensions;

/// <summary>
/// Shuffles the contents of the IList<T>
/// </summary>

// @see: https://stackoverflow.com/questions/273313/randomize-a-listt
public static class ListExtension
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];      // basically Answer value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
