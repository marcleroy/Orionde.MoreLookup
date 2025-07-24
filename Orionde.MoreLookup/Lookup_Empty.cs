using System;
using System.Collections.Generic;
using System.Linq;

namespace Orionde.MoreLookup
{
    public static partial class Lookup
    {
        private class EmptyHolder<TKey, TValue>
        {
            public static readonly ILookup<TKey, TValue> Instance = 
                Enumerable.Empty<int>().ToLookup(x => default(TKey), x => default(TValue));
        }

        /// <summary>
        /// Returns an empty <see cref="ILookup{TKey, TValue}"/> instance. This method uses a singleton pattern to ensure the same instance is returned for each type combination.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the lookup.</typeparam>
        /// <typeparam name="TValue">The type of the values in the lookup.</typeparam>
        /// <returns>An empty <see cref="ILookup{TKey, TValue}"/> instance.</returns>
        public static ILookup<TKey, TValue> Empty<TKey, TValue>()
        {
            return EmptyHolder<TKey, TValue>.Instance;
        }
    }
}
