using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using Orionde.MoreLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("ILookup.Join")]
    public class When_joining_lookups
    {
        Establish context = () =>
            outer = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

        Because of = () =>
            joined = outer.Join(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y);

        It should_create_lookup_with_keys_from_outer_lookup_that_has_matching_keys_in_inner_lookup = () =>
            joined.Single().Key.ShouldEqual(2);

        It should_have_elements_joined_according_to_selector = () => 
            joined[2].ShouldContainExactly("3e", "3d", "4e", "4d", "4e", "4d");
        
        private static ILookup<int, int> outer;
        private static ILookup<int, string> joined;
    }
    
    [Subject("ILookup.Join")]
    public class When_joining_lookups_with_key_comparer
    {
        Establish context = () =>
            outer = Lookup.Builder
                .WithKey("one", new[] { 1, 2 })
                .WithKey("ONE", new[] { 3 }).Build();

        Because of = () =>
            joined = outer.Join(Lookup.Builder
                .WithKey("two", new[] { "a", "b" })
                .WithKey("TWO", new[] { "c" }).Build(), (x, y) => x + y, new StringLengthComparer());

        It should_create_lookup_with_keys_from_outer_lookup_that_has_matching_keys_in_inner_lookup_respecting_comparer = () =>
            joined.Single().Key.ShouldEqual("one");

        It should_have_elements_joined_according_to_selector_respecting_comparer = () => 
            joined["two"].ShouldContainExactly("1a", "1b", "1c", "2a", "2b", "2c", "3a", "3b", "3c");
        
        private static ILookup<string, int> outer;
        private static ILookup<string, string> joined;
    }

    [Subject("ILookup.Join")]
    public class When_joining_null_with_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Join(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Join")]
    public class When_joining_lookup_with_null
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Join<int, string, int, string>(null, (x, y) => x + y));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Join")]
    public class When_joining_lookups_with_null_result_selector
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Join(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (Func<int, string, string>) null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, int> lookup;
        private static Exception exception;
    }
}