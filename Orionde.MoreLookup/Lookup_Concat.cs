using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Concatenates the values of two lookups. Values for common keys are concatenated, preserving duplicates.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <param name="first">The first lookup to concatenate.</param>
        /// <param name="second">The lookup to concatenate to the first lookup.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the concatenated elements of the two input lookups.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> Concat<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> keyComparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return ConcatImpl(first, second).ToLookup(keyComparer);
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> ConcatImpl<TKey, TValue>(
            IEnumerable<IGrouping<TKey, TValue>> first, ILookup<TKey, TValue> second)
        {
            var secondKeys = second.Keys();
            foreach (var grouping in first)
            {
                secondKeys.Remove(grouping.Key);
                yield return Pair.Of(grouping.Key, grouping.Concat(second[grouping.Key]));
            }

            foreach (var newKey in secondKeys)
            {
                yield return Pair.Of(newKey, second[newKey]);
            }
        }
    }
}