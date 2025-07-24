using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Applies a transformation function to the values of each key in a lookup, 
        /// allowing access to both the key and its associated values during transformation.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the input lookup.</typeparam>
        /// <typeparam name="TResult">The type of the values in the result lookup.</typeparam>
        /// <param name="lookup">An <see cref="ILookup{TKey, TValue}"/> to transform.</param>
        /// <param name="transformer">A transform function to apply to each grouping. The function receives an <see cref="IGrouping{TKey, TValue}"/> and returns an <see cref="IEnumerable{T}"/> of results.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> whose values are the result of invoking the transform function on each grouping of the source lookup.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lookup"/> or <paramref name="transformer"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TResult> OnEachKey<TKey, TValue, TResult>(
            this ILookup<TKey, TValue> lookup, Func<IGrouping<TKey, TValue>, IEnumerable<TResult>> transformer)
        {
            if (transformer == null)
                throw new ArgumentNullException("transformer");

            return lookup.Select(x => Pair.Of(x.Key, transformer(x))).ToLookup();
        }
    }
}
