using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WhatsInAWord.Core;

namespace WhatsInAWord.Console
{
    class Program
    {
        static void Main(string[] args) {
          var stopwatch = Stopwatch.StartNew();
          foreach (var combination in new BruteForceWordFinder(new Settings(), str => { } /*System.Console.WriteLine*/).FindWordCombinations(
            File.ReadAllLines("input.txt"))) {
            System.Console.WriteLine(combination);
          }

          System.Console.WriteLine($"DONE! (took: {stopwatch.ElapsedMilliseconds}ms)");
          System.Console.ReadKey();

        }

  }

    internal class Settings:IWordFinderSettings {
      public int WordLengthToMatch => 5;
    }
}
