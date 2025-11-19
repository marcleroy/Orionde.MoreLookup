using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Produces the set intersection of two lookups by using the default equality comparer for keys and values.
        /// Values for common keys are intersected, returning only values that exist in both lookups for that key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <param name="first">An <see cref="ILookup{TKey, TValue}"/> whose distinct elements that also appear in <paramref name="second"/> will be returned.</param>
        /// <param name="second">An <see cref="ILookup{TKey, TValue}"/> whose distinct elements that also appear in <paramref name="first"/> will be returned.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <param name="valueComparer">An <see cref="IEqualityComparer{T}"/> to compare values. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the elements that form the set intersection of the two input lookups.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> Intersect<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second,
            IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return IntersectImpl(first, second, keyComparer, valueComparer).ToLookup(keyComparer);
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> IntersectImpl<TKey, TValue>(
            ILookup<TKey, TValue> first, ILookup<TKey, TValue> second,
            IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            var firstKeys = first.Keys(keyComparer);
            var secondKeys = second.Keys(keyComparer);

            return firstKeys.Where(x => secondKeys.Remove(x))
                .Select(x => Intersect_ValuesForKey(x, first.ValuesForKey(x, keyComparer), second, keyComparer, valueComparer));
        }

        private static KeyValuePair<TKey, IEnumerable<TValue>> Intersect_ValuesForKey<TKey, TValue>(
            TKey key, IEnumerable<TValue> current, IEnumerable<IGrouping<TKey, TValue>> second,
            IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            return Pair.Of(key, current.Intersect(second.ValuesForKey(key, keyComparer), valueComparer));
        }
    }
}