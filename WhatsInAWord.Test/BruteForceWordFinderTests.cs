using WhatsInAWord.Core;
using Xunit.Abstractions;

namespace WhatsInAWord.Test {
  public class BruteForceWordFinderTests : WordFinderTests {
    private readonly ITestOutputHelper _output;

    public BruteForceWordFinderTests(ITestOutputHelper output) {
      _output = output;
    }

    protected override IWordFinder CreateWordFinder() {
      return new BruteForceWordFinder(TestSettings.Default, _output.WriteLine);
    }
  }
}