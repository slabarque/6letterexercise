using System.Collections.Generic;
using System.Linq;

namespace WhatsInAWord.Core
{
  internal static class Extensions {
    public static IEnumerable<IGrouping<int, string>> ExcludePart(this IEnumerable<IGrouping<int, string>> original,
      string part) {
      return original
        .SelectMany(g => g)
        .Where(x => x != part)
        .Concat(original.SelectMany(g => g).Where(x => x == part)
          .Skip(1)) //only remove 1 occurrence of part (not all of them)
        .GroupBy(x => x.Length);
    }
  }
}