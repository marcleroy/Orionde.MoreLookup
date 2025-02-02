using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
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