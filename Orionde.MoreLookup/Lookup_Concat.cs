using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
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