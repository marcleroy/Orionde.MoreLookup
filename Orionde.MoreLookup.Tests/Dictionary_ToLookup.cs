using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class DictionaryToLookupTests
{
    [Fact]
    public void When_converting_IDictionary_of_IEnumerables_to_lookup_should_create_proper_lookup()
    {
        // Arrange
        Dictionary<int, IEnumerable<string>> dictionary = new()
        {
            { 1, new[] { "a", "b" } }, { 2, new[] { "c", "d" } }, { 3, new string[0] }
        };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Count.Should().Be(2);
        lookup[1].ShouldContainExactly("a", "b");
        lookup[2].ShouldContainExactly("c", "d");
        lookup[3].Should().BeEmpty();
        lookup[4].Should().BeEmpty();
    }

    [Fact]
    public void When_converting_IDictionary_of_ILists_to_lookup_should_create_proper_lookup()
    {
        // Arrange
        Dictionary<int, IList<string>> dictionary = new()
        {
            { 1, new[] { "a", "b" } }, { 2, new[] { "c", "d" } }, { 3, new string[0] }
        };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Count.Should().Be(2);
        lookup[1].ShouldContainExactly("a", "b");
        lookup[2].ShouldContainExactly("c", "d");
        lookup[3].Should().BeEmpty();
        lookup[4].Should().BeEmpty();
    }

    [Fact]
    public void When_converting_IDictionary_of_Lists_typed_as_ICollection_to_lookup_should_create_proper_lookup()
    {
        // Arrange
        Dictionary<int, ICollection<string>> dictionary = new()
        {
            { 1, new List<string> { "a", "b" } }, { 2, new List<string> { "c", "d" } }, { 3, new List<string>() }
        };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Count.Should().Be(2);
        lookup[1].ShouldContainExactly("a", "b");
        lookup[2].ShouldContainExactly("c", "d");
        lookup[3].Should().BeEmpty();
        lookup[4].Should().BeEmpty();
    }

    [Fact]
    public void When_converting_IDictionary_of_arrays_to_lookup_should_create_proper_lookup()
    {
        // Arrange
        Dictionary<int, string[]> dictionary = new()
        {
            { 1, new[] { "a", "b" } }, { 2, new[] { "c", "d" } }, { 3, new string[0] }
        };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Count.Should().Be(2);
        lookup[1].ShouldContainExactly("a", "b");
        lookup[2].ShouldContainExactly("c", "d");
        lookup[3].Should().BeEmpty();
        lookup[4].Should().BeEmpty();
    }

    [Fact]
    public void When_converting_IDictionary_of_IEnumerables_with_null_to_lookup_should_create_empty_lookup()
    {
        // Arrange
        Dictionary<int, IEnumerable<string>> dictionary = new() { { 0, null } };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Should().BeEmpty();
    }

    [Fact]
    public void When_converting_IDictionary_of_ILists_with_null_to_lookup_should_create_empty_lookup()
    {
        // Arrange
        Dictionary<int, IList<string>> dictionary = new() { { 0, null } };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Should().BeEmpty();
    }

    [Fact]
    public void When_converting_IDictionary_of_ICollection_with_null_to_lookup_should_create_empty_lookup()
    {
        // Arrange
        Dictionary<int, ICollection<string>> dictionary = new() { { 0, null } };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Should().BeEmpty();
    }

    [Fact]
    public void When_converting_IDictionary_of_arrays_with_null_to_lookup_should_create_empty_lookup()
    {
        // Arrange
        Dictionary<int, string[]> dictionary = new() { { 0, null } };

        // Act
        ILookup<int, string>? lookup = dictionary.ToLookup();

        // Assert
        lookup.Should().BeEmpty();
    }

    [Fact]
    public void When_converting_null_of_IEnumerables_to_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        IDictionary<int, IEnumerable<string>> dictionary = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_converting_null_of_ILists_to_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        IDictionary<int, IList<string>> dictionary = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_converting_null_of_ICollections_to_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        IDictionary<int, ICollection<string>> dictionary = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_converting_null_of_arrays_to_lookup_should_throw_ArgumentNullException()
    {
        // Arrange
        IDictionary<int, string[]> dictionary = null;

        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void When_converting_IDictionary_of_IEnumerables_to_lookup_with_comparer_should_respect_comparer()
    {
        // Arrange
        Dictionary<string, IEnumerable<string>> dictionary = new()
        {
            { "one", new[] { "a", "b" } }, { "two", new[] { "b", "c" } }
        };

        // Act
        ILookup<string, string>? lookup = dictionary.ToLookup(new StringLengthComparer());

        // Assert
        lookup.Count.Should().Be(1);
        lookup["one"].ShouldContainExactly("a", "b", "b", "c");
    }

    [Fact]
    public void When_converting_IDictionary_of_ILists_to_lookup_with_comparer_should_respect_comparer()
    {
        // Arrange
        Dictionary<string, IList<string>> dictionary = new()
        {
            { "one", new[] { "a", "b" } }, { "two", new[] { "b", "c" } }
        };

        // Act
        ILookup<string, string>? lookup = dictionary.ToLookup(new StringLengthComparer());

        // Assert
        lookup.Count.Should().Be(1);
        lookup["one"].ShouldContainExactly("a", "b", "b", "c");
    }

    [Fact]
    public void
        When_converting_IDictionary_of_Lists_typed_as_ICollection_to_lookup_with_comparer_should_respect_comparer()
    {
        // Arrange
        Dictionary<string, ICollection<string>> dictionary = new()
        {
            { "one", new[] { "a", "b" } }, { "two", new[] { "b", "c" } }
        };

        // Act
        ILookup<string, string>? lookup = dictionary.ToLookup(new StringLengthComparer());

        // Assert
        lookup.Count.Should().Be(1);
        lookup["one"].ShouldContainExactly("a", "b", "b", "c");
    }

    [Fact]
    public void When_converting_IDictionary_of_arrays_to_lookup_with_comparer_should_respect_comparer()
    {
        // Arrange
        Dictionary<string, string[]> dictionary = new()
        {
            { "one", new[] { "a", "b" } }, { "two", new[] { "b", "c" } }
        };

        // Act
        ILookup<string, string>? lookup = dictionary.ToLookup(new StringLengthComparer());

        // Assert
        lookup.Count.Should().Be(1);
        lookup["one"].ShouldContainExactly("a", "b", "b", "c");
    }
}