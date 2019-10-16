using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhatsInAWord.Core;
using Xunit;
using Xunit.Abstractions;

namespace WhatsInAWord.Test {
  public abstract class WordFinderTests {
    [Fact]
    public void CanFindCombinationOf2() {
      ForWords("foobar", "foo", "bar")
        .Expect("foo+bar=foobar");
    }

    [Fact]
    public void CanFindCombinationOf3() {
      ForWords("foobar", "o", "fo", "bar")
        .Expect("fo+o+bar=foobar");
    }

    [Fact]
    public void DisallowDoubleUseOfLetter() {
      ForWords("succes", "uc", "ce", "s")
        .ExpectNothing();
    }

    [Fact]
    public void UseDoubleLettersAsStart() {
      ForWords("succes", "uc", "s", "ce", "s")
        .Expect("s+uc+ce+s=succes");
    }

    [Fact]
    public void UseDoubleLettersInTheMiddle() {
      ForWords("ucssce", "uc", "s", "ce", "s")
        .Expect("uc+s+s+ce=ucssce");
    }

    [Fact]
    public void FindMultipleWords() {
      ForWords("rifraf", "rif", "raf", "rafrif")
        .Expect("rif+raf=rifraf", "raf+rif=rafrif");
    }

    [Fact]
    public void FindMultipleCombinationsFor1Word() {
      ForWords("soccer", "soc", "ce", "r", "so", "cc", "er")
        .Expect("soc+ce+r=soccer", "so+cc+er=soccer");
    }

    [Fact]
    public void FindPermutationsOfChars() {
      ForWords("t", "actors", "c", "r", "castor", "a", "s", "costar", "o")
        .Expect("a+c+t+o+r+s=actors", "c+a+s+t+o+r=castor", "c+o+s+t+a+r=costar");
    }

    protected WordFinderTestSetup ForWords(params string[] words) {
      return new WordFinderTestSetup(CreateWordFinder(), words);
    }

    public class WordFinderTestSetup {
      private readonly IWordFinder _wordFinder;
      private readonly IEnumerable<string> _words;

      public WordFinderTestSetup(IWordFinder wordFinder, params string[] words) {
        _wordFinder = wordFinder;
        _words = words;
      }

      public void Expect(params string[] combinations) {
        Assert.Equal(combinations, _wordFinder.FindWordCombinations(_words));
      }

      public void ExpectNothing() {
        Assert.Equal(Enumerable.Empty<string>(), _wordFinder.FindWordCombinations(_words));
      }
    }

    protected abstract IWordFinder CreateWordFinder();
  }

  public class BruteForceWordFinderTests : WordFinderTests
  {
    private readonly ITestOutputHelper _output;

    public BruteForceWordFinderTests(ITestOutputHelper output ) {
      _output = output;
    }
    protected override IWordFinder CreateWordFinder() {
      return new BruteForceWordFinder(TestSettings.Default, _output.WriteLine);
    }
  }

  public class TestSettings:IWordFinderSettings {
    public int WordLengthToMatch { get; set; }

    public static TestSettings Default => new TestSettings {
      WordLengthToMatch = 6
    };
  }
}
