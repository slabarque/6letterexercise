using System.Diagnostics;
using System.IO;
using System.Linq;
using WhatsInAWord.Core;

namespace WhatsInAWord.Console {
  internal class Program {
    private static void Main(string[] args) {
      var stopwatch = Stopwatch.StartNew();
      var combinations = new BruteForceWordFinder(new Settings() /*, System.Console.WriteLine*/)
        .FindWordCombinations(File.ReadAllLines("input.txt")).ToList();

      //foreach (var combination in combinations) {
      //  System.Console.WriteLine(combination);
      //}

      System.Console.WriteLine($"DONE! Found {combinations.Count()} combinations (took: {stopwatch.ElapsedMilliseconds}ms)");
      System.Console.ReadKey();
    }
  }

  internal class Settings : IWordFinderSettings {
    public int WordLengthToMatch => 6;
  }
}