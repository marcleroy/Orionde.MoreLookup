using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class LookupExceptTests
{
    [Fact]
    public void When_creating_lookups_difference_should_create_lookup_with_difference()
    {
        // Arrange
        ILookup<int, string>? lookup = Lookup.LookupBuilder.WithKey(0, new[] { "a", "b", "a" })
            .WithKey(1, new[] { "c", "d" })
            .WithKey(2, new[] { "e", "f", "f" }).Build();

        // Act
        ILookup<int, string>? difference = lookup.Except(Lookup.LookupBuilder.WithKey(2, new[] { "f", "g", "e" })
            .WithKey(1, new[] { "c", "b" })
            .WithKey(3, new[] { "i", "j" }).Build());

        // Assert
        difference.Count.Should().Be(2);
        difference[0].ShouldContainExactly("a", "b");
        difference[1].ShouldContainExactly("d");
    }

    [Fact]
    public void When_creating_lookups_difference_with_key_comparer_should_respect_comparer()
    {
        // Arrange
        ILookup<string, string>? lookup = Lookup.LookupBuilder.WithKey("one", new[] { "a", "c" })
            .WithKey("ONE", new[] { "b" }).Build();

        // Act
        ILookup<string, string>? difference = lookup.Except(Lookup.LookupBuilder.WithKey("two", new[] { "d", "c" })
            .WithKey("TWO", new[] { "b" }).Build(), new StringLengthComparer());

        // Assert
        difference.Count.Should().Be(1);
        difference["one"].ShouldContainExactly("a");
    }

    [Fact]
    public void When_creating_lookups_difference_with_value_comparer_should_respect_comparer()
    {
        // Arrange
        ILookup<int, string>? lookup = Lookup.LookupBuilder.WithKey(0, new[] { "one", "three" }).Build();

        // Act
        ILookup<int, string>? difference = lookup.Except(Lookup.LookupBuilder.WithKey(0, new[] { "two", "four" }).Build(), valueComparer: new StringLengthComparer());

        // Assert
        difference[0].ShouldContainExactly("three");
    }

    [Fact]
    public void When_creating_difference_of_null_and_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<int, string> lookup = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => lookup.Except(Lookup.LookupBuilder.WithKey(2, new[] { "e", "d" })
            .WithKey(3, new[] { "f", "g" }).Build()));
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_creating_difference_of_lookup_and_null_should_throw_ArgumentNullException()
    {
        // Arrange
        ILookup<int, string>? lookup = Lookup.LookupBuilder.WithKey(1, new[] { "a", "b" })
            .WithKey(2, new[] { "c", "d" }).Build();

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => lookup.Except(null));
        exception.Should().BeOfType<ArgumentNullException>();
    }
}