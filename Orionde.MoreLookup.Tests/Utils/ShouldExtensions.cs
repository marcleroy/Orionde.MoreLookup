using System.Linq;
using System.Collections.Generic;
using FluentAssertions;

namespace Tests.Utils
{
    public static class ShouldExtensions
    {
        public static void ShouldContainExactly<T>(this IEnumerable<T> actual, params T[] expected)
        {
            actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }
    }
}