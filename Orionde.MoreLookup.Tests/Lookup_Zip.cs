using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using Orionde.MoreLookup;
using Tests.Utils;
using FluentAssertions;

namespace Tests
{
    public class LookupZipTests
    {
        [Fact]
        public void When_zipping_lookups_should_zip_according_to_selector()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

            // Act
            var zipped = lookup.Zip(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y);

            // Assert
            zipped.Single().Key.Should().Be(2);
            zipped[2].ShouldContainExactly("3e", "4d");
        }

        [Fact]
        public void When_zipping_lookups_with_key_comparer_should_respect_comparer()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey("one", new[] { 1, 2 })
                .WithKey("ONE", new[] { 3 }).Build();

            // Act
            var zipped = lookup.Zip(Lookup.Builder
                .WithKey("two", new[] { "a", "b" })
                .WithKey("TWO", new[] { "c", "d" }).Build(), (x, y) => x + y, new StringLengthComparer());

            // Assert
            zipped.Single().Key.Should().Be("one");
            zipped["two"].ShouldContainExactly("1a", "2b", "3c");
        }

        [Fact]
        public void When_zipping_null_with_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            ILookup<int, string> lookup = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Zip(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y));
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_zipping_lookup_with_null_should_throw_ArgumentNullException()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Zip<int, string, int, string>(null, (x, y) => x + y));
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_zipping_lookups_with_null_result_selector_should_throw_ArgumentNullException()
        {
            // Arrange
            var lookup = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => lookup.Zip(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (Func<int, string, string>) null));
            exception.Should().BeOfType<ArgumentNullException>();
        }
    }
}