using System.Collections.Concurrent;
using System.Text;

namespace LocationDataMigration;

public static class Helper
{
    public static void PrintResult(ConcurrentDictionary<int, int> results)
    {
        var count = results.Sum(x => x.Value);
        Console.WriteLine("Towns processed: " + count);
        Console.WriteLine();
        Console.WriteLine("Distance\tTowns");
        Console.WriteLine("------------------------");

        foreach (var kvp in results.OrderBy(kvp => kvp.Key))
        {
            Console.WriteLine($"{kvp.Key}\t\t{kvp.Value}");
        }
    }

    public static string RemoveNonLetters(string input)
    {
        var output = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            if (char.IsLetter(c) || char.IsWhiteSpace(c))
            {
                output.Append(c);
            }
        }
        return output.ToString();
    }

    public static List<string> GetAllCombinations(string[] array)
    {
        var result = new List<string>();

        int comboCount = (1 << array.Length); // 2^n
        for (int i = 1; i < comboCount; i++) // start at 1 to skip the empty combo
        {
            var output = new StringBuilder();
            for (int j = 0; j < array.Length; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    output.Append(array[j] + " ");
                }
            }
            result.Add(output.ToString());
        }

        return result;
    }
}