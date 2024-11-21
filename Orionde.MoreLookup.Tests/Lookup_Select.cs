using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Orionde.MoreLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("ILookup.Select")]
    public class When_projecting_lookup
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            projected = lookup.Select(x => x + "!");

        It should_project_values_according_to_selector = () =>
        {
            projected.Count.ShouldEqual(2);
            projected["a"].ShouldContainExactly("1!", "3!");
            projected["b"].ShouldContainExactly("2!", "4!");
        };

        private static ILookup<string, int> lookup;
        private static ILookup<string, string> projected;
    }

    [Subject("ILookup.Select")]
    public class When_projecting_lookup_using_query_syntax
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            projected = from x in lookup 
                       select x + "!";

        It should_project_values_according_to_selector = () =>
        {
            projected.Count.ShouldEqual(2);
            projected["a"].ShouldContainExactly("1!", "3!");
            projected["b"].ShouldContainExactly("2!", "4!");
        };

        private static ILookup<string, int> lookup;
        private static ILookup<string, string> projected;
    }

    [Subject("ILookup.Select")]
    public class When_projecting_null_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Select(x => x + "!"));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Select")]
    public class When_projecting_lookup_with_null_selector
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            exception = Catch.Exception(() => lookup.Select((Func<int, string>)null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }    
}
