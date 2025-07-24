using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    /// <summary>
    /// Provides extension methods for enhanced <see cref="ILookup{TKey, TValue}"/> operations.
    /// </summary>
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Projects each value of the lookup into a new form by incorporating the value's index.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="lookup">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lookup"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TResult> Select<TKey, TValue, TResult>(
            this ILookup<TKey, TValue> lookup, Func<TValue, TResult> selector)
        {
            return lookup.OnEachKey(x => x.Select(selector));
        }
    }
}
