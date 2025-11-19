using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests
{
    public class LookupConcatTests
    {
        [Fact]
        public void When_concatenating_lookups_should_create_lookup_with_concatenated_values()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d", "d" }).Build();

            // Act
            var concatenated = lookup.Concat(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build());

            // Assert
            concatenated.Count.Should().Be(3);
            concatenated[1].ShouldContainExactly("a", "b");
            concatenated[2].ShouldContainExactly("c", "d", "d", "e", "d");
            concatenated[3].ShouldContainExactly("f", "g");
        }

        [Fact]
        public void When_concatenating_lookups_with_key_comparer_should_respect_comparer()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey("one", new[] { "a", "b" })
                .WithKey("ONE", new[] { "c" }).Build();

            // Act
            var concatenated = lookup.Concat(Lookup.Builder
                .WithKey("two", new[] { "b", "c" })
                .WithKey("TWO", new[] { "d" }).Build(), new StringLengthComparer());

            // Assert
            concatenated.Count.Should().Be(1);
            concatenated["two"].ShouldContainExactly("a", "b", "c", "b", "c", "d");
        }

        [Fact]
        public void When_concatenating_null_with_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            ILookup<int, string> lookup = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Concat(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build()));
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_concatenating_lookup_with_null_should_throw_ArgumentNullException()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Concat(null));
            exception.Should().BeOfType<ArgumentNullException>();
        }
    }
}