using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Orionde.MoreLookup;

using Xunit;

namespace Tests
{
    public class LookupEmptyTests
    {
        [Fact]
        public void When_requesting_empty_lookup_should_be_empty_and_singleton()
        {
            // Act
            var lookup = Lookup.Empty<int, string>();

            // Assert
            lookup.Should().BeEmpty();
            ReferenceEquals(lookup, Lookup.Empty<int, string>()).Should().BeTrue();
        }
    }
}