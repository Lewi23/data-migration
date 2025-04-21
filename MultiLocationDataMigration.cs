using System.Collections.Concurrent;
using Fastenshtein;

namespace LocationDataMigration;

public class MultiLocationDataMigration
{
    public async Task TestMethod()
    {
        var failedToProcessCount = 0;
        var failedToProcessTowns = new List<string[]>();

        var townNames = new HashSet<string>();
        var townNameLengths = new Dictionary<string, int>();

        foreach (var town in TownDataSet.Towns)
        {
            townNames.Add(town);
            townNameLengths.TryAdd(town, town.Length);
        }

        //var dummyData = new List<string> { "Fakirk, ", "Edinburgh | Stirling ", "Newcastle" };
        //var dummyData = UserMultiTownDataSet.MultiTowns;
        var dummyData = UserTownDataSet.Towns;

        var formattedData = new List<string>();
        foreach (var town in dummyData)
        {
            formattedData.Add(Helper.RemoveNonLetters(town));
        }

        var dict = new ConcurrentDictionary<int, int>();

        Parallel.ForEach(formattedData, userTown =>
        {
            var usersTowns = userTown.Split(" ");
            var usersTownsProcessed = 0;

            foreach (var town in usersTowns)
            {
                var trimmedTown = town.Trim();

                // **Exact Match Optimization**
                if (townNames.Contains(trimmedTown))
                {
                    dict.AddOrUpdate(0, 1, (_, v) => v + 1);
                    break;
                }

                // **Precompute Length Differences to Reduce Comparisons**
                string closestTown = townNames
                    .Where(town => Math.Abs(trimmedTown.Length - townNameLengths[town]) <= 2) // Small threshold
                    .OrderBy(town => Levenshtein.Distance(trimmedTown, town))
                    .FirstOrDefault();

                if (closestTown != null)
                {
                    int distance = Levenshtein.Distance(trimmedTown, closestTown);

                    if (distance == 0)
                    {
                        dict.AddOrUpdate(distance, 1, (_, v) => v + 1);
                        break;
                    }

                    usersTownsProcessed++;

                    if (usersTownsProcessed == usersTowns.Length)
                    {
                        failedToProcessCount++;
                        failedToProcessTowns.Add(usersTowns);
                    }
                    // check on last item if we get here we were unable to process

                }
            }
        });





        var count = dict.Sum(x => x.Value);
        Console.WriteLine("Exact match found for " + count + " users");
        Console.WriteLine("Failed to find any exact matches for " + failedToProcessCount + " users");

        // St Helens abc

        // St
        // St Helens // match
        // St Helens abc // match


        Console.WriteLine("Brute forcing failed matches, retrying " + failedToProcessCount + " entries");

        var foundMatch = 0;


        Parallel.ForEach(failedToProcessTowns, retryString =>
        {
            var combinations = Helper.GetAllCombinations(retryString);

            foreach (var combo in combinations)
            {
                var trimmedTown = combo.Trim();

                string closestTown = townNames
                    .Where(town => Math.Abs(trimmedTown.Length - townNameLengths[town]) <= 2)
                    .OrderBy(town => Levenshtein.Distance(trimmedTown, town))
                    .FirstOrDefault();

                if (closestTown != null)
                {
                    int distance = Levenshtein.Distance(trimmedTown, closestTown);

                    if (distance == 0)
                    {
                        Interlocked.Increment(ref foundMatch);
                        break;
                    }
                }
            }
        });

        Console.WriteLine(foundMatch);
        
    }
}