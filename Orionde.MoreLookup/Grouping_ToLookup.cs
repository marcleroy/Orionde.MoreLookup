using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    /// <summary>
    /// Provides extension methods for converting <see cref="IGrouping{TKey, TValue}"/> collections to <see cref="ILookup{TKey, TValue}"/> instances.
    /// </summary>
    public static partial class GroupingExtensions
    {
        /// <summary>
        /// Converts a collection of <see cref="IGrouping{TKey, TValue}"/> instances to an <see cref="ILookup{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the groupings.</typeparam>
        /// <typeparam name="TValue">The type of the values in the groupings.</typeparam>
        /// <param name="groupings">The collection of groupings to convert.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the elements from the input groupings.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="groupings"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings, IEqualityComparer<TKey> comparer = null)
        {
            return groupings.Select(x => Pair.Of(x.Key, x)).ToLookup(comparer);
        }
    }
}