using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        ///     Produces the set union of two lookups by using the default equality comparer for keys and values.
        ///     Values for common keys are combined using the Union operation, removing duplicates.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <param name="first">
        ///     An <see cref="ILookup{TKey, TValue}" /> whose distinct elements that also appear in
        ///     <paramref name="second" /> will be returned.
        /// </param>
        /// <param name="second">
        ///     An <see cref="ILookup{TKey, TValue}" /> whose distinct elements that also appear in
        ///     <paramref name="first" /> will be returned.
        /// </param>
        /// <param name="keyComparer">
        ///     An <see cref="IEqualityComparer{T}" /> to compare keys. If null, the default equality
        ///     comparer is used.
        /// </param>
        /// <param name="valueComparer">
        ///     An <see cref="IEqualityComparer{T}" /> to compare values. If null, the default equality
        ///     comparer is used.
        /// </param>
        /// <returns>
        ///     An <see cref="ILookup{TKey, TValue}" /> that contains the elements that form the set union of the two input
        ///     lookups.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="first" /> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> Union<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second,
            IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            return UnionImpl(first, second, keyComparer, valueComparer).ToLookup(keyComparer);
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> UnionImpl<TKey, TValue>(
            ILookup<TKey, TValue> first, ILookup<TKey, TValue> second,
            IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            ISet<TKey> firstKeys = first.Keys(keyComparer);
            ISet<TKey> secondKeys = second.Keys(keyComparer);

            foreach (TKey key in firstKeys)
            {
                secondKeys.Remove(key);
                yield return Pair.Of(key,
                    first.ValuesForKey(key, keyComparer).Union(second.ValuesForKey(key, keyComparer), valueComparer));
            }

            foreach (TKey newKey in secondKeys)
            {
                yield return Pair.Of(newKey, second.ValuesForKey(newKey, keyComparer));
            }
        }
    }
}