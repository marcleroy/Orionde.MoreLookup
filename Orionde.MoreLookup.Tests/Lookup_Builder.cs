using FluentAssertions;

using Orionde.MoreLookup;

using Tests.Utils;

using Xunit;

namespace Tests;

public class LookupBuilderTests
{
    [Fact]
    public void When_building_lookup_should_handle_all_scenarios()
    {
        // Act
        ILookup<int, string>? lookup = Lookup.LookupBuilder.WithKey(1, new[] { "a", "b" })
            .WithKey(1, new[] { "e", "f" })
            .WithKey(2, new[] { "c", "d", "d" })
            .WithKey(3, null)
            .Build();

        // Assert
        lookup.Select(x => x.Key).ShouldContainExactly(1, 2);
        lookup[1].ShouldContainExactly("e", "f");
        lookup[2].ShouldContainExactly("c", "d", "d");
        lookup[3].Should().BeEmpty();
        lookup[4].Should().BeEmpty();
        lookup.Count.Should().Be(2);
        lookup.Contains(1).Should().BeTrue();
        lookup.Contains(2).Should().BeTrue();
        lookup.Contains(3).Should().BeFalse();
        lookup.Contains(4).Should().BeFalse();
    }

    [Fact]
    public void When_building_lookup_with_null_key_should_handle_null_properly()
    {
        // Act
        ILookup<string, string>? lookup = Lookup.LookupBuilder.WithKey("not null", new[] { "a", "b" })
            .WithKey(null, new[] { "e", "f" })
            .Build();

        // Assert
        lookup.Select(x => x.Key).ShouldContainExactly("not null", null);
        lookup[null].ShouldContainExactly("e", "f");
        lookup.Count.Should().Be(2);
        lookup.Contains(null).Should().BeTrue();
    }

    [Fact]
    public void When_modifying_source_collection_after_lookup_was_built_should_not_affect_lookup()
    {
        // Arrange
        List<string> source = new() { "a", "b" };
        ILookup<string, string>? lookup = Lookup.LookupBuilder.WithKey("key", source)
            .Build();

        // Act
        source.Add("c");

        // Assert
        lookup["key"].ShouldContainExactly("a", "b");
    }

    [Fact]
    public void When_building_lookup_with_custom_comparer_should_respect_comparer()
    {
        // Act
        ILookup<string, string>? lookup = Lookup.Builder
            .WithComparer(new StringLengthComparer())
            .WithKey("one", new[] { "a", "b" })
            .WithKey("two", new[] { "e", "f" })
            .WithKey("three", new[] { "c", "d", "d" })
            .Build();

        // Assert
        lookup.Select(x => x.Key).ShouldContainExactly("one", "three");
        lookup["one"].ShouldContainExactly("e", "f");
        lookup["two"].Should().BeSameAs(lookup["one"]);
        lookup["abc"].Should().BeSameAs(lookup["one"]);
        lookup.Count.Should().Be(2);
        lookup.Contains("one").Should().BeTrue();
        lookup.Contains("two").Should().BeTrue();
        lookup.Contains("abc").Should().BeTrue();
        lookup.Contains("four").Should().BeFalse();
    }

    [Fact]
    public void When_building_lookup_with_custom_comparer_that_handles_nulls_should_work_properly()
    {
        // Act
        ILookup<string, string>? lookup = Lookup.Builder
            .WithComparer(new StringLengthComparerWithStringifiedNull())
            .WithKey("", new[] { "a", "b" })
            .WithKey(null, new[] { "c", "d" })
            .Build();

        // Assert
        lookup.Select(x => x.Key).ShouldContainExactly("");
        lookup[""].ShouldContainExactly("c", "d");
        lookup[null].Should().BeSameAs(lookup[""]);
        lookup.Count.Should().Be(1);
        lookup.Contains("").Should().BeTrue();
        lookup.Contains(null).Should().BeTrue();
    }

    private class StringLengthComparerWithStringifiedNull : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return StringifyNullIfNeeded(x).Length == StringifyNullIfNeeded(y).Length;
        }

        public int GetHashCode(string obj)
        {
            return StringifyNullIfNeeded(obj).Length;
        }

        private static string StringifyNullIfNeeded(string obj)
        {
            return obj ?? string.Empty;
        }
    }
}