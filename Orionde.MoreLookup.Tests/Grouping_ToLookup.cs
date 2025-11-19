using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class GroupingToLookupTests
{
    [Fact]
    public void When_converting_IEnumerable_of_IGrouping_to_lookup_should_create_proper_lookup()
    {
        // Arrange
        ILookup<string, int>? grouping = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act
        ILookup<string, int>? lookup = grouping.ToLookup();

        // Assert
        lookup.Count.Should().Be(2);
        lookup["a"].ShouldContainExactly(1, 3);
        lookup["b"].ShouldContainExactly(2, 4);
    }

    [Fact]
    public void When_converting_IEnumerable_of_IGrouping_to_lookup_with_comparer_should_respect_comparer()
    {
        // Arrange
        ILookup<string, int>? grouping = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act
        ILookup<string, int>? lookup = grouping.ToLookup(new StringLengthComparer());

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
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => grouping.ToLookup());
        exception.Should().BeOfType<ArgumentNullException>();
    }
}