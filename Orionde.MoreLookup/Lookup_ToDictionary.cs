using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Orionde.MoreLookup
{
    public static partial class LookupExtensions
    {
        /// <summary>
        /// Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="ILookup{TKey, TValue}"/> 
        /// where each key maps to a <see cref="List{T}"/> containing all values for that key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <param name="lookup">The lookup to convert to a dictionary.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains keys and lists of values from the input lookup.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lookup"/> is <c>null</c>.</exception>
        [Pure]
        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(
            this ILookup<TKey, TValue> lookup, IEqualityComparer<TKey> comparer = null)
        {
            if (lookup == null)
                throw new ArgumentNullException("lookup");

            var dictionary = new Dictionary<TKey, List<TValue>>(comparer);
            foreach (var group in lookup)
            {
                List<TValue> values;
                if (!dictionary.TryGetValue(group.Key, out values))
                {
                    values = new List<TValue>();
                    dictionary[group.Key] = values;
                }
                values.AddRange(group);
            }
            return dictionary;
        }
    }
}