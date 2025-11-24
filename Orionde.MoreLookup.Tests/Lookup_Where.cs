using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class LookupWhereTests
{
    [Fact]
    public void When_filtering_lookup_should_filter_values_according_to_predicate()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act
        ILookup<string, int>? filtered = lookup.Where(x => x > 3);

        // Assert
        filtered.Count.Should().Be(1);
        filtered["b"].ShouldContainExactly(4);
    }

    [Fact]
    public void When_filtering_lookup_using_query_syntax_should_filter_values_according_to_predicate()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act
        ILookup<string, int> filtered = from x in lookup
                                        where x > 3
                                        select x;

        // Assert
        filtered.Count.Should().Be(1);
        filtered["b"].ShouldContainExactly(4);
    }

    [Fact]
    public void When_filtering_null_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<string, int> lookup = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => lookup.Where(x => x > 3));
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_filtering_lookup_with_null_predicate_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => lookup.Where(null));
        exception.Should().BeOfType<ArgumentNullException>();
    }
}