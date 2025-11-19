using System.Collections.Generic;
using System.Linq;

namespace Orionde.MoreLookup
{
    /// <summary>
    ///     Provides utilities for creating and working with <see cref="ILookup{TKey, TValue}" /> instances.
    /// </summary>
    public static partial class Lookup
    {
        /// <summary>
        ///     Gets a builder instance for fluently constructing <see cref="ILookup{TKey, TValue}" /> objects.
        /// </summary>
        /// <value>A new <see cref="LookupBuilder" /> instance.</value>
        public static LookupBuilder Builder => new LookupBuilder();

        /// <summary>
        ///     Provides a fluent interface for building <see cref="ILookup{TKey, TValue}" /> instances.
        /// </summary>
        public class LookupBuilder
        {
            internal LookupBuilder()
            {
            }

            /// <summary>
            ///     Adds a key-value collection pair to the lookup being built.
            /// </summary>
            /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
            /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
            /// <param name="key">The key to associate with the values.</param>
            /// <param name="values">The collection of values to associate with the key.</param>
            /// <returns>A typed builder that can be used to add more key-value pairs or build the final lookup.</returns>
            public static LookupBuilder<TKey, TValue> WithKey<TKey, TValue>(TKey key, IEnumerable<TValue> values)
            {
                return new LookupBuilder<TKey, TValue>(EqualityComparer<TKey>.Default).WithKey(key, values);
            }

            /// <summary>
            ///     Specifies a custom equality comparer for keys in the lookup being built.
            /// </summary>
            /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
            /// <param name="comparer">The equality comparer to use for key comparisons.</param>
            /// <returns>A key-typed builder that uses the specified comparer.</returns>
            public LookupBuilder<TKey> WithComparer<TKey>(IEqualityComparer<TKey> comparer)
            {
                return new LookupBuilder<TKey>(comparer);
            }
        }

        /// <summary>
        ///     Provides a fluent interface for building <see cref="ILookup{TKey, TValue}" /> instances with a specific key type
        ///     and comparer.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        public class LookupBuilder<TKey>
        {
            private readonly IEqualityComparer<TKey> _comparer;

            /// <summary>
            ///     Initializes a new instance of the <see cref="LookupBuilder{TKey}" /> class with the specified comparer.
            /// </summary>
            /// <param name="comparer">The equality comparer to use for key comparisons.</param>
            public LookupBuilder(IEqualityComparer<TKey> comparer)
            {
                _comparer = comparer;
            }

            /// <summary>
            ///     Adds a key-value collection pair to the lookup being built.
            /// </summary>
            /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
            /// <param name="key">The key to associate with the values.</param>
            /// <param name="values">The collection of values to associate with the key.</param>
            /// <returns>A fully typed builder that can be used to add more key-value pairs or build the final lookup.</returns>
            public LookupBuilder<TKey, TValue> WithKey<TValue>(TKey key, IEnumerable<TValue> values)
            {
                return new LookupBuilder<TKey, TValue>(_comparer).WithKey(key, values);
            }
        }

        /// <summary>
        ///     Provides a fluent interface for building <see cref="ILookup{TKey, TValue}" /> instances with specific key and value
        ///     types.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        public class LookupBuilder<TKey, TValue>
        {
            private readonly Dictionary<TKey, IEnumerable<TValue>> _data;
            private TKey _firstAppearedNullKey;

            private bool _hasNullKey;
            private IEnumerable<TValue> _valuesForNullKey;

            internal LookupBuilder(IEqualityComparer<TKey> comparer)
            {
                _data = new Dictionary<TKey, IEnumerable<TValue>>(comparer);
            }

            private IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> NullKeyEntries
            {
                get
                {
                    if (_hasNullKey)
                    {
                        yield return new KeyValuePair<TKey, IEnumerable<TValue>>(_firstAppearedNullKey,
                            _valuesForNullKey);
                    }
                }
            }

            /// <summary>
            ///     Adds a key-value collection pair to the lookup being built. If the key already exists, the new values replace the
            ///     previous ones.
            /// </summary>
            /// <param name="key">The key to associate with the values. Can be null for reference types.</param>
            /// <param name="values">
            ///     The collection of values to associate with the key. If null, the key will not be included in the
            ///     final lookup.
            /// </param>
            /// <returns>This builder instance for method chaining.</returns>
            public LookupBuilder<TKey, TValue> WithKey(TKey key, IEnumerable<TValue> values)
            {
                // ILookup supports null keys, IDictionary does not, so value for null key need to be kept separately.
                // Null literal cannot be used here as TKey may be a value type, in which case Comparer cannot be called properly.
                // But default(TKey) will produce null for every reference type, and it may be used safely here.
                if (typeof(TKey).IsClass && _data.Comparer.Equals(key, default))
                {
                    // Null key must be also stored, because when some odd comparers make null equal to some non-null value,
                    // the key which appeared first should be used; when non-null key that is equal to null using provided comparer
                    // appear first, it has to be used as a key for all equal-to-null values.
                    if (!_hasNullKey)
                    {
                        _firstAppearedNullKey = key;
                        _hasNullKey = true;
                    }

                    _valuesForNullKey = values;
                }
                else
                {
                    _data[key] = values;
                }

                return this;
            }

            /// <summary>
            ///     Builds the final <see cref="ILookup{TKey, TValue}" /> instance from the accumulated key-value pairs.
            /// </summary>
            /// <returns>An <see cref="ILookup{TKey, TValue}" /> containing all the key-value pairs added to this builder.</returns>
            public ILookup<TKey, TValue> Build()
            {
                return _data.Concat(NullKeyEntries).ToLookup(_data.Comparer);
            }
        }
    }
}