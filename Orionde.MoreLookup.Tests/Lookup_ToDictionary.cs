using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using Orionde.MoreLookup;
using Tests.Utils;
using FluentAssertions;

namespace Tests
{
    public class LookupToDictionaryTests
    {
        [Fact]
        public void When_converting_lookup_to_IDictionary_should_create_proper_dictionary()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

            // Act
            var dictionary = lookup.ToDictionary();

            // Assert
            dictionary.Count.Should().Be(2);
            dictionary[1].ShouldContainExactly("a", "b");
            dictionary[2].ShouldContainExactly("c", "d");
        }
        
        [Fact]
        public void When_converting_lookup_to_IDictionary_with_comparer_should_respect_comparer()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey("one", new[] { "a", "b" })
                .WithKey("two", new[] { "c", "d" }).Build();

            // Act
            var dictionary = lookup.ToDictionary(new StringLengthComparer());

            // Assert
            dictionary.Count.Should().Be(1);
            dictionary["one"].ShouldContainExactly("a", "b", "c", "d");
            dictionary["two"].ShouldContainExactly("a", "b", "c", "d");
        }

        [Fact]
        public void When_converting_null_to_IDictionary_should_throw_ArgumentNullException()
        {
            // Arrange
            ILookup<int, string> lookup = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.ToDictionary());
            exception.Should().BeOfType<ArgumentNullException>();
        }
    }
}