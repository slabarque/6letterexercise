using System;
using System.IO;
using WhatsInAWord.Core;

namespace WhatsInAWord.Console
{
    class Program
    {
        static void Main(string[] args) {
          foreach (var combination in new BruteForceWordFinder(new Settings(), str => { } /*System.Console.WriteLine*/).FindWordCombinations(
            File.ReadAllLines("input.txt"))) {
            System.Console.WriteLine(combination);
          }

          System.Console.ReadKey();

        }

  }

    internal class Settings:IWordFinderSettings {
      public int WordLengthToMatch => 6;
    }
}
