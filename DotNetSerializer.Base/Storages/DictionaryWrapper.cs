using System.Collections.Generic;

namespace DotNetSerializer.Base.Storages
{
    /// <summary>
    /// /// Base wrapper for a dictionary with controlled read/write access.
    /// </summary>
    /// <typeparam name="TKey">Dictionary key type.</typeparam>
    /// <typeparam name="TValue">Dictionary value type.</typeparam>
    public abstract class DictionaryWrapper<TKey, TValue>
    {
        protected readonly Dictionary<TKey, TValue> _storage;

        /// <summary>
        /// Read-only view of the underlying dictionary.
        /// </summary>
        public IReadOnlyDictionary<TKey, TValue> Items => _storage;

        /// <summary>
        /// Initializes an empty dictionary wrapper.
        /// </summary>
        public DictionaryWrapper()
        {
            _storage = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Retrieves a value by key using custom logic.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <returns>The associated value.</returns>
        public abstract TValue Get(TKey key);

        protected void Add(TKey key, TValue value)
        {
            _storage.Add(key, value);
        }
        protected void Set(TKey key, TValue value)
        {
            _storage[key]= value;
        }

    }
}
