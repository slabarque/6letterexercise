using System.Diagnostics;
using System.IO;
using WhatsInAWord.Core;

namespace WhatsInAWord.Console {
  internal class Program {
    private static void Main(string[] args) {
      var stopwatch = Stopwatch.StartNew();
      foreach (var combination in new BruteForceWordFinder(new Settings()/*, System.Console.WriteLine*/)
        .FindWordCombinations(
          File.ReadAllLines("input.txt"))) {
        System.Console.WriteLine(combination);
      }

      System.Console.WriteLine($"DONE! (took: {stopwatch.ElapsedMilliseconds}ms)");
      System.Console.ReadKey();
    }
  }

  internal class Settings : IWordFinderSettings {
    public int WordLengthToMatch => 6;
  }
}