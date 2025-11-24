# Orionde.MoreLookup

[![NuGet Version](https://img.shields.io/nuget/v/Orionde.MoreLookup.svg)](https://www.nuget.org/packages/Orionde.MoreLookup/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Orionde.MoreLookup.svg)](https://www.nuget.org/packages/Orionde.MoreLookup/)
[![Build Status](https://github.com/marcleroy/Orionde.MoreLookup/workflows/CI%20Build%20and%20Test/badge.svg)](https://github.com/marcleroy/Orionde.MoreLookup/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE.txt)

A powerful LINQ extension library that enhances `ILookup<TKey, TValue>` with additional operations like Union,
Intersect, Except, Join, Zip, and more.

## Features

- **Enhanced Lookup Operations**: Union, Intersect, Except, Join, Zip, Concat
- **LINQ Query Syntax Support**: Works seamlessly with LINQ query expressions
- **Builder Pattern**: Fluent API for constructing lookups
- **Custom Comparers**: Support for custom key and value comparers
- **Multi-Framework Support**: .NET Framework 4.8, .NET 6, 7, 8, 9, and .NET Standard 2.0

## Installation

```bash
dotnet add package Orionde.MoreLookup
```

## Quick Start

```csharp
using Orionde.MoreLookup;

// Create lookups using the builder
var lookup1 = Lookup.Builder
    .WithKey("fruits", new[] { "apple", "banana" })
    .WithKey("colors", new[] { "red", "yellow" })
    .Build();

var lookup2 = Lookup.Builder
    .WithKey("fruits", new[] { "orange", "apple" })
    .WithKey("animals", new[] { "cat", "dog" })
    .Build();

// Union lookups
var union = lookup1.Union(lookup2);
// Result: fruits -> [apple, banana, orange], colors -> [red, yellow], animals -> [cat, dog]

// Intersect lookups
var intersection = lookup1.Intersect(lookup2);
// Result: fruits -> [apple]

// Filter values
var filtered = lookup1.Where(value => value.StartsWith("a"));
// Result: fruits -> [apple], colors -> []

// Transform values
var transformed = lookup1.Select(value => value.ToUpper());
// Result: fruits -> [APPLE, BANANA], colors -> [RED, YELLOW]
```

## Available Operations

### Set Operations

- `Union(other)` - Combines lookups, merging values for common keys
- `Intersect(other)` - Returns only values that exist in both lookups for common keys
- `Except(other)` - Returns values from first lookup excluding those in second
- `Concat(other)` - Concatenates all values for matching keys

### Transformations

- `Select(selector)` - Projects each value using the selector function
- `Where(predicate)` - Filters values based on the predicate
- `OnEachKey(func)` - Applies a transformation function to each key's values

### Joins

- `Join(inner, resultSelector)` - Performs an inner join between lookups
- `Zip(other, resultSelector)` - Combines corresponding elements from both lookups

### Conversions

- `ToDictionary()` - Converts lookup to `Dictionary<TKey, List<TValue>>`
- `ToLookup()` - Extension for `IEnumerable<IGrouping>` and dictionaries

### Utilities

- `Lookup.Empty<TKey, TValue>()` - Creates an empty lookup (singleton)
- `Lookup.Builder` - Fluent builder for creating lookups

## Advanced Usage

### Custom Comparers

```csharp
// Case-insensitive string comparer
var lookup = Lookup.Builder
    .WithComparer(StringComparer.OrdinalIgnoreCase)
    .WithKey("Test", new[] { "value1" })
    .WithKey("TEST", new[] { "value2" })  // Will merge with previous key
    .Build();

// Union with custom key comparer
var result = lookup1.Union(lookup2, keyComparer: StringComparer.OrdinalIgnoreCase);
```

### LINQ Query Syntax

```csharp
var result = from value in lookup
             where value.Length > 3
             select value.ToUpper();
```

### Dictionary Conversions

```csharp
// Convert dictionary to lookup
var dict = new Dictionary<string, string[]>
{
    ["fruits"] = new[] { "apple", "banana" },
    ["colors"] = new[] { "red", "blue" }
};
var lookup = dict.ToLookup();

// Convert lookup back to dictionary
var backToDict = lookup.ToDictionary();
```

## Inspiration and Acknowledgments

This library was inspired by the original [NOtherLookup](https://github.com/NOtherDev/NOtherLookup) project
by [NOtherDev](https://github.com/NOtherDev). Special thanks to the author for the foundational concepts and the
excellent blog
post: ["NOtherLookup - even better lookups"](https://notherdev.blogspot.com/2014/01/NOtherLookup-even-better-lookups.html).

While maintaining compatibility with modern .NET versions, this library builds upon those original ideas with:

- Multi-framework support (.NET Framework 4.8 through .NET 9)
- Enhanced builder pattern
- Improved test coverage
- Additional operations and customization options

## Comparison with Other Libraries

### vs. MoreLinq

[MoreLinq](https://github.com/morelinq/MoreLinq) is an excellent general-purpose LINQ extension library, but it focuses
primarily on `IEnumerable<T>` operations rather than `ILookup<TKey, TValue>` specifically. Key differences:

- **MoreLinq**: Provides 100+ general LINQ operators for sequences
- **Orionde.MoreLookup**: Specialized for lookup/grouping operations with set theory methods

**Use MoreLinq when**: You need general sequence operations  
**Use Orionde.MoreLookup when**: You're working extensively with lookups and need operations like Union, Intersect,
Except on grouped data

### Unique Value Proposition

This library fills a specific gap by providing:

- Set operations specifically for `ILookup<TKey, TValue>`
- Fluent builder pattern for lookup construction
- Dictionary-to-Lookup conversion utilities
- Operations that preserve the lookup structure while transforming contents

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.