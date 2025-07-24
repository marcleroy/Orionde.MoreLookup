using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using Orionde.MoreLookup;
using Tests.Utils;
using FluentAssertions;

namespace Tests
{
    public class DictionaryToLookupTests
    {
        [Fact]
        public void When_converting_IDictionary_of_IEnumerables_to_lookup_should_create_proper_lookup()
        {
            // Arrange
            var dictionary = new Dictionary<int, IEnumerable<string>>()
            {
                { 1, new[] { "a", "b" }},
                { 2, new[] { "c", "d" }},
                { 3, new string[0] }
            };

            // Act
            var lookup = dictionary.ToLookup();

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
            var dictionary = new Dictionary<int, IList<string>>()
            {
                { 1, new[] { "a", "b" }},
                { 2, new[] { "c", "d" }},
                { 3, new string[0] }
            };

            // Act
            var lookup = dictionary.ToLookup();

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
            var dictionary = new Dictionary<int, ICollection<string>>()
            {
                { 1, new List<string>() { "a", "b" }},
                { 2, new List<string>() { "c", "d" }},
                { 3, new List<string>() }
            };

            // Act
            var lookup = dictionary.ToLookup();

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
            var dictionary = new Dictionary<int, string[]>()
            {
                { 1, new[] { "a", "b" }},
                { 2, new[] { "c", "d" }},
                { 3, new string[0] }
            };

            // Act
            var lookup = dictionary.ToLookup();

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
            var dictionary = new Dictionary<int, IEnumerable<string>>()
            {
                { 0, null }
            };

            // Act
            var lookup = dictionary.ToLookup();

            // Assert
            lookup.Should().BeEmpty();
        }

        [Fact]
        public void When_converting_IDictionary_of_ILists_with_null_to_lookup_should_create_empty_lookup()
        {
            // Arrange
            var dictionary = new Dictionary<int, IList<string>>()
            {
                { 0, null }
            };

            // Act
            var lookup = dictionary.ToLookup();

            // Assert
            lookup.Should().BeEmpty();
        }

        [Fact]
        public void When_converting_IDictionary_of_ICollection_with_null_to_lookup_should_create_empty_lookup()
        {
            // Arrange
            var dictionary = new Dictionary<int, ICollection<string>>()
            {
                { 0, null }
            };

            // Act
            var lookup = dictionary.ToLookup();

            // Assert
            lookup.Should().BeEmpty();
        }

        [Fact]
        public void When_converting_IDictionary_of_arrays_with_null_to_lookup_should_create_empty_lookup()
        {
            // Arrange
            var dictionary = new Dictionary<int, string[]>()
            {
                { 0, null }
            };

            // Act
            var lookup = dictionary.ToLookup();

            // Assert
            lookup.Should().BeEmpty();
        }

        [Fact]
        public void When_converting_null_of_IEnumerables_to_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            IDictionary<int, IEnumerable<string>> dictionary = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_converting_null_of_ILists_to_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            IDictionary<int, IList<string>> dictionary = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_converting_null_of_ICollections_to_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            IDictionary<int, ICollection<string>> dictionary = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_converting_null_of_arrays_to_lookup_should_throw_ArgumentNullException()
        {
            // Arrange
            IDictionary<int, string[]> dictionary = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => dictionary.ToLookup());
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_converting_IDictionary_of_IEnumerables_to_lookup_with_comparer_should_respect_comparer()
        {
            // Arrange
            var dictionary = new Dictionary<string, IEnumerable<string>>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            // Act
            var lookup = dictionary.ToLookup(new StringLengthComparer());

            // Assert
            lookup.Count.Should().Be(1);
            lookup["one"].ShouldContainExactly("a", "b", "b", "c");
        }

        [Fact]
        public void When_converting_IDictionary_of_ILists_to_lookup_with_comparer_should_respect_comparer()
        {
            // Arrange
            var dictionary = new Dictionary<string, IList<string>>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            // Act
            var lookup = dictionary.ToLookup(new StringLengthComparer());

            // Assert
            lookup.Count.Should().Be(1);
            lookup["one"].ShouldContainExactly("a", "b", "b", "c");
        }

        [Fact]
        public void When_converting_IDictionary_of_Lists_typed_as_ICollection_to_lookup_with_comparer_should_respect_comparer()
        {
            // Arrange
            var dictionary = new Dictionary<string, ICollection<string>>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            // Act
            var lookup = dictionary.ToLookup(new StringLengthComparer());

            // Assert
            lookup.Count.Should().Be(1);
            lookup["one"].ShouldContainExactly("a", "b", "b", "c");
        }

        [Fact]
        public void When_converting_IDictionary_of_arrays_to_lookup_with_comparer_should_respect_comparer()
        {
            // Arrange
            var dictionary = new Dictionary<string, string[]>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            // Act
            var lookup = dictionary.ToLookup(new StringLengthComparer());

            // Assert
            lookup.Count.Should().Be(1);
            lookup["one"].ShouldContainExactly("a", "b", "b", "c");
        }
    }
}