using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class LookupOnEachKeyTests
{
    [Fact]
    public void When_applying_function_on_each_lookup_key_should_transform_values()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act
        ILookup<string, string>? transformed =
            lookup.OnEachKey(x => x.Select(e => x.Key + e).OrderByDescending(e => e));

        // Assert
        transformed.Count.Should().Be(2);
        transformed["a"].ShouldContainExactly("a3", "a1");
        transformed["b"].ShouldContainExactly("b4", "b2");
    }

    [Fact]
    public void When_applying_function_on_null_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<string, int> lookup = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            lookup.OnEachKey(x => x.Select(e => x.Key + e).OrderByDescending(e => e)));
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_applying_null_function_on_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<string, int>? lookup = Lookup.LookupBuilder.WithKey("a", new[] { 1, 3 })
            .WithKey("b", new[] { 2, 4 }).Build();

        // Act & Assert
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(() => lookup.OnEachKey<string, int, int>(null));
        exception.Should().BeOfType<ArgumentNullException>();
    }
}