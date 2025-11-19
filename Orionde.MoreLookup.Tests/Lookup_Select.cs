using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class LookupSelectTests
{
    [Fact]
    public void When_projecting_lookup_should_project_values_according_to_selector()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act
        ILookup<string, string>? projected = lookup.Select(x => x + "!");

        // Assert
        projected.Count.Should().Be(2);
        projected["a"].ShouldContainExactly("1!", "3!");
        projected["b"].ShouldContainExactly("2!", "4!");
    }

    [Fact]
    public void When_projecting_lookup_using_query_syntax_should_project_values_according_to_selector()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act
        ILookup<string, string> projected = from x in lookup
            select x + "!";

        // Assert
        projected.Count.Should().Be(2);
        projected["a"].ShouldContainExactly("1!", "3!");
        projected["b"].ShouldContainExactly("2!", "4!");
    }

    [Fact]
    public void When_projecting_null_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<string, int> lookup = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => lookup.Select(x => x + "!"));
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_projecting_lookup_with_null_selector_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act & Assert
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(() => lookup.Select((Func<int, string>)null));
        exception.Should().BeOfType<ArgumentNullException>();
    }
}