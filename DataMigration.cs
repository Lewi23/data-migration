using System.Collections.Concurrent;
using Fastenshtein;
using LocationDataMigration;



MultiLocationDataMigration multiLocationDataMigration = new MultiLocationDataMigration();
await multiLocationDataMigration.TestMethod();

/*var townData = TownDataSet.Towns;
var userTownData = UserTownDataSet.Towns;

Console.WriteLine("Towns ingested: " + userTownData.Count);

var townNames = new HashSet<string>();
var townNameLengths = new Dictionary<string, int>();

foreach (var town in townData)
{
    townNames.Add(town);
    townNameLengths.TryAdd(town, town.Length);
}

var resultDictionary = new ConcurrentDictionary<int, int>();

Parallel.ForEach(userTownData, userTown =>
{
    var trimmedTown = userTown.Trim();

    // **Exact Match Optimization**
    if (townNames.Contains(trimmedTown))
    {
        resultDictionary.AddOrUpdate(0, 1, (_, v) => v + 1);
        return;
    }

    // **Precompute Length Differences to Reduce Comparisons**
    string closestTown = townNames
        .Where(town => Math.Abs(trimmedTown.Length - townNameLengths[town]) <= 2) // Small threshold
        .OrderBy(town => Levenshtein.Distance(trimmedTown, town))
        .FirstOrDefault();

    if (closestTown != null)
    {
        int distance = Levenshtein.Distance(trimmedTown, closestTown);
        resultDictionary.AddOrUpdate(distance, 1, (_, v) => v + 1);
    }

});

Helper.PrintResult(resultDictionary);*/