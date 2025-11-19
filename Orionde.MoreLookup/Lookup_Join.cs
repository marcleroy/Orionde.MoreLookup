using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Correlates the elements of two lookups based on matching keys and groups the results. 
        /// This method performs an inner join on the keys, combining values from both lookups using the specified result selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in both lookups.</typeparam>
        /// <typeparam name="TOuter">The type of the values in the outer lookup.</typeparam>
        /// <typeparam name="TInner">The type of the values in the inner lookup.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first lookup to join.</param>
        /// <param name="inner">The lookup to join to the first lookup.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that has elements of type <typeparamref name="TResult"/> that are obtained by performing an inner join on two lookups.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="outer"/>, <paramref name="inner"/>, or <paramref name="resultSelector"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TResult> Join<TKey, TOuter, TInner, TResult>(
            this ILookup<TKey, TOuter> outer, ILookup<TKey, TInner> inner,
            Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> keyComparer = null)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return outer.SelectMany(o => o, (o, e) => Join_ValuesForKey(o.Key, e, inner, resultSelector, keyComparer))
                .ToLookup(keyComparer);
        }

        private static KeyValuePair<TKey, IEnumerable<TResult>> Join_ValuesForKey<TKey, TOuter, TInner, TResult>(
            TKey key, TOuter current, IEnumerable<IGrouping<TKey, TInner>> inner,
            Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> keyComparer)
        {
            return Pair.Of(key, inner.ValuesForKey(key, keyComparer).Select(x => resultSelector(current, x)));
        }
    }
}