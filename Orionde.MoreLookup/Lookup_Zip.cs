using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        ///     Applies a specified function to the corresponding elements of two lookups with matching keys,
        ///     producing a lookup of the results. The operation is performed element-wise, stopping when the shorter sequence
        ///     ends.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in both lookups.</typeparam>
        /// <typeparam name="TFirst">The type of the values in the first lookup.</typeparam>
        /// <typeparam name="TSecond">The type of the values in the second lookup.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="first">The first lookup to merge.</param>
        /// <param name="second">The second lookup to merge.</param>
        /// <param name="resultSelector">A function that specifies how to merge the elements from the two lookups.</param>
        /// <param name="keyComparer">
        ///     An <see cref="IEqualityComparer{T}" /> to compare keys. If null, the default equality
        ///     comparer is used.
        /// </param>
        /// <returns>An <see cref="ILookup{TKey, TValue}" /> that contains merged elements of two input lookups.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="first" />, <paramref name="second" />, or
        ///     <paramref name="resultSelector" /> is <c>null</c>.
        /// </exception>
        [Pure]
        public static ILookup<TKey, TResult> Zip<TKey, TFirst, TSecond, TResult>(
            this ILookup<TKey, TFirst> first, ILookup<TKey, TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector, IEqualityComparer<TKey> keyComparer = null)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }

            return first.Keys(keyComparer)
                .Select(key => Zip_ValuesForKey(key, first, second, resultSelector, keyComparer))
                .ToLookup(keyComparer);
        }

        private static KeyValuePair<TKey, IEnumerable<TResult>> Zip_ValuesForKey<TKey, TFirst, TSecond, TResult>(
            TKey key, IEnumerable<IGrouping<TKey, TFirst>> first, IEnumerable<IGrouping<TKey, TSecond>> second,
            Func<TFirst, TSecond, TResult> resultSelector, IEqualityComparer<TKey> keyComparer)
        {
            using (IEnumerator<TFirst> iterator1 = first.ValuesForKey(key, keyComparer).GetEnumerator())
            {
                List<TResult> values = new List<TResult>();

                using (IEnumerator<TSecond> iterator2 = second.ValuesForKey(key, keyComparer).GetEnumerator())
                {
                    while (iterator1.MoveNext() && iterator2.MoveNext())
                    {
                        values.Add(resultSelector(iterator1.Current, iterator2.Current));
                    }
                }

                return Pair.Of(key, values);
            }
        }
    }
}