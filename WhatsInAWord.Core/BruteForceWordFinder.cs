using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatsInAWord.Core {
  public class BruteForceWordFinder : IWordFinder {
    private readonly IWordFinderSettings _settings;
    private readonly Action<string> _logger;

    public BruteForceWordFinder(IWordFinderSettings settings, Action<string> logger) {
      _settings = settings;
      _logger = logger;
    }

    public IEnumerable<string> FindWordCombinations(IEnumerable<string> words) {
      var wordsByLength = words.GroupBy(w => w.Length).ToList();

      var result = MatchWords(
        wordsByLength.SingleOrDefault(x => x.Key == _settings.WordLengthToMatch)?.ToArray() ?? Array.Empty<string>(),
        wordsByLength.Where(g => g.Key < _settings.WordLengthToMatch).ToArray());

      return result.Select(x=>x.ToString());
    }

    private IEnumerable<WordMatch> MatchWords(string[] words, IGrouping<int, string>[] parts) {
      return words.SelectMany(word => {
        var results = new List<WordMatch>();
        CombineParts(results, new WordMatch(word, _logger), parts, 0);
        return results;
      });
    }

    private void CombineParts(List<WordMatch> acc, WordMatch wordMatch, IGrouping<int, string>[] parts, int level) {
      foreach (var part in parts.SelectMany(x=>x)) {
        _logger($"LEVEL {level}");
        if(!wordMatch.IsComplete && wordMatch.Match(part))
          if (wordMatch.IsComplete)
            acc.Add(wordMatch);
          else
            CombineParts(acc, wordMatch, parts
              .Where(g => g.Key <= wordMatch.RemainingCharCount)
              .ExcludePart(part)
              .ToArray(), level++);
      }
    }
  }

  internal static class Extensions {
    public static IEnumerable<IGrouping<int, string>> ExcludePart(this IEnumerable<IGrouping<int, string>> original, string part) {
      return original
        .SelectMany(g => g)
        .Where(x=>x!=part)
        .GroupBy(x => x.Length);
    }
  }

  internal class WordMatch {
    private readonly List<string> _parts;
    private string _remainder;
    private readonly Action<string> _logger;

    public WordMatch(string word, Action<string> logger) {
      Word = word;
      _remainder = word;
      _logger = logger;
      _parts = new List<string>();
    }

    public string Word { get; }

    public IReadOnlyList<string> Parts => _parts;

    public bool Match(string part) {
      _logger($"{GetHashCode()}: {Word} => matching [{part,-6}] with [{_remainder,-6}]");//TODO: dynamic alignment according to Word.Length
      if (_remainder.StartsWith(part)) {
        _logger("MATCH!");
        _parts.Add(part);
        _remainder = _remainder.Substring(part.Length, RemainingCharCount-part.Length);
        return true;
      }
      _logger("no");

      return false;
    }

    public bool IsComplete => string.IsNullOrEmpty(_remainder);

    public int RemainingCharCount => _remainder.Length;

    public override string ToString() => $"{string.Join("+", Parts)}={Word}";
  }
}