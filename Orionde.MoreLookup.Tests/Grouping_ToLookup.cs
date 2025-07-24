using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using Orionde.MoreLookup;
using Tests.Utils;
using FluentAssertions;

namespace Tests
{
    public class GroupingToLookupTests
    {
        [Fact]
        public void When_converting_IEnumerable_of_IGrouping_to_lookup_should_create_proper_lookup()
        {
            // Arrange
            var grouping = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

            // Act
            var lookup = grouping.ToLookup();

            // Assert
            lookup.Count.Should().Be(2);
            lookup["a"].ShouldContainExactly(1, 3);
            lookup["b"].ShouldContainExactly(2, 4);
        }
        
        [Fact]
        public void When_converting_IEnumerable_of_IGrouping_to_lookup_with_comparer_should_respect_comparer()
        {
            // Arrange
            var grouping = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

            // Act
            var lookup = grouping.ToLookup(new StringLengthComparer());

            // Assert
            lookup.Count.Should().Be(1);
            lookup["a"].ShouldContainExactly(1, 3, 2, 4);
            lookup["b"].ShouldContainExactly(1, 3, 2, 4);
        }
        
        [Fact]
        public void When_converting_null_IEnumerable_of_IGrouping_to_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            IEnumerable<IGrouping<string, int>> grouping = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => grouping.ToLookup());
            exception.Should().BeOfType<ArgumentNullException>();
        }
    }
}