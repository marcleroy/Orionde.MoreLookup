using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class LookupIntersectTests
{
    [Fact]
    public void When_intersecting_lookups_should_create_lookup_with_intersection()
    {
        // Arrange
        ILookup<int, string>? lookup = Lookup.LookupBuilder.WithKey(0, new[] { "a", "b" })
            .WithKey(1, new[] { "c", "d" })
            .WithKey(2, new[] { "e", "f", "f" }).Build();

        // Act
        ILookup<int, string>? intersection = lookup.Intersect(Lookup.LookupBuilder.WithKey(2, new[] { "f", "g", "h" })
            .WithKey(1, new[] { "a", "b" })
            .WithKey(3, new[] { "i", "j" }).Build());

        // Assert
        intersection.Count.Should().Be(1);
        intersection[2].ShouldContainExactly("f");
    }

    [Fact]
    public void When_intersecting_lookups_with_key_comparer_should_respect_comparer()
    {
        // Arrange
        ILookup<string, string>? lookup = Lookup.LookupBuilder.WithKey("one", new[] { "a", "b" })
            .WithKey("ONE", new[] { "c" }).Build();

        // Act
        ILookup<string, string>? intersection = lookup.Intersect(Lookup.LookupBuilder.WithKey("two", new[] { "b", "d" })
            .WithKey("TWO", new[] { "c" }).Build(), new StringLengthComparer());

        // Assert
        intersection.Count.Should().Be(1);
        intersection["one"].ShouldContainExactly("b", "c");
    }

    [Fact]
    public void When_intersecting_lookups_with_value_comparer_should_respect_comparer()
    {
        // Arrange
        ILookup<int, string>? lookup = Lookup.LookupBuilder.WithKey(0, new[] { "one", "three" }).Build();

        // Act
        ILookup<int, string>? intersection = lookup.Intersect(Lookup.LookupBuilder.WithKey(0, new[] { "two", "four" }).Build(), valueComparer: new StringLengthComparer());

        // Assert
        intersection[0].ShouldContainExactly("one");
    }

    [Fact]
    public void When_intersecting_null_with_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<int, string> lookup = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => lookup.Intersect(Lookup.LookupBuilder.WithKey(2, new[] { "e", "d" })
            .WithKey(3, new[] { "f", "g" }).Build()));
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_intersecting_lookup_with_null_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<int, string>? lookup = Lookup.LookupBuilder.WithKey(1, new[] { "a", "b" })
            .WithKey(2, new[] { "c", "d" }).Build();

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => lookup.Intersect(null));
        exception.Should().BeOfType<ArgumentNullException>();
    }
}