using WhatsInAWord.Core;

namespace WhatsInAWord.Test {
  internal class TestSettings : IWordFinderSettings {
    public static TestSettings Default => new TestSettings {
      WordLengthToMatch = 6
    };

    public int WordLengthToMatch { get; set; }
  }
}