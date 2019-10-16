using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatsInAWord.Core {
  public class BruteForceWordFinder : IWordFinder {
    private readonly Action<string> _logger;
    private readonly IWordFinderSettings _settings;

    public BruteForceWordFinder(IWordFinderSettings settings, Action<string> logger=null) {
      _settings = settings;
      _logger = logger??(_ => { });
    }

    public IEnumerable<string> FindWordCombinations(IEnumerable<string> words) {
      var wordsByLength = words.Distinct().GroupBy(w => w.Length).ToList();

      var result = MatchWords(
        wordsByLength.SingleOrDefault(x => x.Key == _settings.WordLengthToMatch)?.ToArray() ?? Array.Empty<string>(),
        wordsByLength.Where(g => g.Key < _settings.WordLengthToMatch).ToArray());

      return result.Select(x => x.ToString()).Distinct();
    }

    private IEnumerable<WordMatch> MatchWords(string[] words, IGrouping<int, string>[] parts) {
      return words.SelectMany(word => CombineParts(new WordMatch(word, _logger), parts));
    }

    private IEnumerable<WordMatch> CombineParts(WordMatch wordMatch, IGrouping<int, string>[] parts) {
      foreach (var part in parts.SelectMany(x => x)) {
        if (!wordMatch.IsMatch(part)) continue;

        var clone = (WordMatch)wordMatch.Clone();
        clone.Match(part);
        if (clone.IsComplete)
          yield return clone;

        foreach (var match in CombineParts(clone, parts
          .Where(g => g.Key <= clone.RemainingCharCount)
          .ExcludePart(part)
          .ToArray()))
          yield return match;
      }
    }

    private class WordMatch : ICloneable {
      private readonly Action<string> _logger;
      private List<string> _parts;
      private string _remainder;

      public WordMatch(string word, Action<string> logger) {
        Word = word;
        _remainder = word;
        _logger = logger;
        _parts = new List<string>();
      }

      public string Word { get; }

      public IReadOnlyList<string> Parts => _parts;

      public bool IsComplete => string.IsNullOrEmpty(_remainder);

      public int RemainingCharCount => _remainder.Length;

      public object Clone() {
        var partsClone = new string[_parts.Count];
        _parts.CopyTo(partsClone);
        var clone = new WordMatch(Word, _logger) {
          _remainder = _remainder,
          _parts = partsClone.ToList()
        };
        _logger($"Cloning [{GetHashCode()}] to [{clone.GetHashCode()}]");
        return clone;
      }

      public bool IsMatch(string part) {
        //TODO: dynamic alignment according to Word.Length
        _logger($"{GetHashCode()}: {Word} => matching [{part,-6}] with [{_remainder,-6}]");
        return _remainder.StartsWith(part);
      }

      public void Match(string part) {
        _parts.Add(part);
        _remainder = _remainder.Substring(part.Length, RemainingCharCount - part.Length);
      }

      public override string ToString() {
        return $"{string.Join("+", Parts)}={Word}";
      }
    }
  }
}