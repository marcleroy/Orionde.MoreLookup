using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using Orionde.MoreLookup;
using Tests.Utils;
using FluentAssertions;

namespace Tests
{
    public class LookupUnionTests
    {
        [Fact]
        public void When_unionizing_lookups_should_create_lookup_with_unionized_values()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

            // Act
            var concatenated = lookup.Union(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build());

            // Assert
            concatenated.Count.Should().Be(3);
            concatenated[1].ShouldContainExactly("a", "b");
            concatenated[2].ShouldContainExactly("c", "d", "e");
            concatenated[3].ShouldContainExactly("f", "g");
        }
        
        [Fact]
        public void When_unionizing_lookups_with_key_comparer_should_respect_comparer()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey("one", new[] { "a", "b" })
                .WithKey("ONE", new[] { "c" }).Build();

            // Act
            var concatenated = lookup.Union(Lookup.Builder
                .WithKey("two", new[] { "c", "d" })
                .WithKey("TWO", new[] { "e" }).Build(), keyComparer: new StringLengthComparer());

            // Assert
            concatenated.Count.Should().Be(1);
            concatenated["two"].ShouldContainExactly("a", "b", "c", "d", "e");
        }

        [Fact]
        public void When_unionizing_lookups_with_value_comparer_should_respect_comparer()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(0, new[] { "one", "three" }).Build();

            // Act
            var concatenated = lookup.Union(Lookup.Builder
                .WithKey(0, new[] { "two", "four" }).Build(), valueComparer: new StringLengthComparer());

            // Assert
            concatenated[0].ShouldContainExactly("one", "three", "four");
        }

        [Fact]
        public void When_unionizing_null_with_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            ILookup<int, string> lookup = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Union(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build()));
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_unionizing_lookup_with_null_should_throw_ArgumentNullException()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Union(null));
            exception.Should().BeOfType<ArgumentNullException>();
        }
    }
}