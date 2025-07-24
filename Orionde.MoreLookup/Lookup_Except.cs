using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Produces the set difference of two lookups by using the default equality comparer for keys and values.
        /// Values for keys in the first lookup are returned, excluding any values that also appear in the second lookup for the same key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <param name="first">An <see cref="ILookup{TKey, TValue}"/> whose elements that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">An <see cref="ILookup{TKey, TValue}"/> whose elements that also occur in the first lookup will cause those elements to be removed from the returned sequence.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <param name="valueComparer">An <see cref="IEqualityComparer{T}"/> to compare values. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the set difference of the elements of two input lookups.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> Except<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, 
            IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first
                .Select(x => Pair.Of(x.Key, x.Except(second.ValuesForKey(x.Key, keyComparer), valueComparer)))
                .ToLookup(keyComparer);
        }
    }
}