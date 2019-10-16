using System.Collections.Generic;

namespace WhatsInAWord.Core {
  public interface IWordFinder {
    IEnumerable<string> FindWordCombinations(IEnumerable<string> words);
  }
}