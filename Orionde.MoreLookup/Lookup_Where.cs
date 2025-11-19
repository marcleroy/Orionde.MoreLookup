using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Filters the values in each grouping of a lookup based on a predicate. 
        /// Keys with no values that satisfy the predicate will not appear in the result.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <param name="lookup">An <see cref="ILookup{TKey, TValue}"/> to filter.</param>
        /// <param name="predicate">A function to test each value for a condition.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains elements from the input lookup that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lookup"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> Where<TKey, TValue>(
            this ILookup<TKey, TValue> lookup, Func<TValue, bool> predicate)
        {
            return lookup.OnEachKey(x => x.Where(predicate));
        }
    }
}