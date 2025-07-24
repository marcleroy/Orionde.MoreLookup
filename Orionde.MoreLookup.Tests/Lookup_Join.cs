using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using Orionde.MoreLookup;
using Tests.Utils;
using FluentAssertions;

namespace Tests
{
    public class LookupJoinTests
    {
        [Fact]
        public void When_joining_lookups_should_join_according_to_selector()
        {
            // Arrange
            var outer = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

            // Act
            var joined = outer.Join(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y);

            // Assert
            joined.Single().Key.Should().Be(2);
            joined[2].ShouldContainExactly("3e", "3d", "4e", "4d", "4e", "4d");
        }
        
        [Fact]
        public void When_joining_lookups_with_key_comparer_should_respect_comparer()
        {
            // Arrange
            var outer = Lookup.Builder
                .WithKey("one", new[] { 1, 2 })
                .WithKey("ONE", new[] { 3 }).Build();

            // Act
            var joined = outer.Join(Lookup.Builder
                .WithKey("two", new[] { "a", "b" })
                .WithKey("TWO", new[] { "c" }).Build(), (x, y) => x + y, new StringLengthComparer());

            // Assert
            joined.Single().Key.Should().Be("one");
            joined["two"].ShouldContainExactly("1a", "1b", "1c", "2a", "2b", "2c", "3a", "3b", "3c");
        }

        [Fact]
        public void When_joining_null_with_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            ILookup<int, string> lookup = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Join(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y));
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_joining_lookup_with_null_should_throw_ArgumentNullException()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Join<int, string, int, string>(null, (x, y) => x + y));
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_joining_lookups_with_null_result_selector_should_throw_ArgumentNullException()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Join(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (Func<int, string, string>) null));
            exception.Should().BeOfType<ArgumentNullException>();
        }
    }
}