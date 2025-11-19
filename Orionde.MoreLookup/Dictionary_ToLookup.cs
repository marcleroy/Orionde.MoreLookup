using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Orionde.MoreLookup
{
    /// <summary>
    /// Provides extension methods for converting dictionaries to <see cref="ILookup{TKey, TValue}"/> instances.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts a dictionary with <see cref="IEnumerable{T}"/> values to an <see cref="ILookup{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the collections.</typeparam>
        /// <param name="dictionary">The dictionary to convert.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the elements from the input dictionary.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, IEnumerable<TValue>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, IEnumerable<TValue>, TValue>(comparer);
        }

        /// <summary>
        /// Converts a dictionary with <see cref="IList{T}"/> values to an <see cref="ILookup{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the collections.</typeparam>
        /// <param name="dictionary">The dictionary to convert.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the elements from the input dictionary.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, IList<TValue>, TValue>(comparer);
        }

        /// <summary>
        /// Converts a dictionary with <see cref="ICollection{T}"/> values to an <see cref="ILookup{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the collections.</typeparam>
        /// <param name="dictionary">The dictionary to convert.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the elements from the input dictionary.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, ICollection<TValue>, TValue>(comparer);
        }

        /// <summary>
        /// Converts a dictionary with array values to an <see cref="ILookup{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the arrays.</typeparam>
        /// <param name="dictionary">The dictionary to convert.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys. If null, the default equality comparer is used.</param>
        /// <returns>An <see cref="ILookup{TKey, TValue}"/> that contains the elements from the input dictionary.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, TValue[]> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, TValue[], TValue>(comparer);
        }

        internal static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, IEnumerable<TValue>, TValue>(comparer);
        }

        private static ILookup<TKey, TValue> ToLookup<TKey, TCollection, TValue>(this IEnumerable<KeyValuePair<TKey, TCollection>> dictionary, IEqualityComparer<TKey> comparer = null)
            where TCollection : IEnumerable<TValue>
        {
            return dictionary.Where(x => !Equals(x.Value, default(TCollection)))
                .SelectMany(kv => kv.Value, (kv, v) => new { kv.Key, Value = v })
                .ToLookup(x => x.Key, x => x.Value, comparer);
        }
    }
}