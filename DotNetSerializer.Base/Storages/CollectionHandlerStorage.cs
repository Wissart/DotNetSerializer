using DotNetSerializer.Base.Exceptions;
using System;

namespace DotNetSerializer.Base.Storages
{
    /// <summary>
    /// Storage for collection handlers.
    /// </summary>
    public class CollectionHandlerStorage : DictionaryWrapper<Type, CollectionHandler>
    {
        /// <summary>
        /// Determines  whether the specified type is contained.
        /// </summary>
        /// <param name="collectionType">The type of the collection handler.</param>
        /// <returns>True if the collection type is contained, otherwise, false.</returns>
        public bool Contains(Type collectionType)
        {
            return _storage.ContainsKey(collectionType);
        }

        /// <summary>
        /// Gets the <see cref="CollectionHandler"/> for the specified type.
        /// </summary>
        /// <param name="key">The type to retrieve collection handler for.</param>
        /// <returns>Yhe collection handler, or throws an exception if the collection handler does not exist.</returns>
        /// <exception cref="CollectionHandlerNotFoundException">Thrown when the collection handler does not exist.</exception>
        public override CollectionHandler Get(Type key)
        {
            if (!_storage.ContainsKey(key))
                throw new CollectionHandlerNotFoundException(key);

            return _storage[key];
        }

        /// <summary>
        /// Adds the collection handler.
        /// </summary>
        /// <typeparam name="T">The type of collection handler derived from <see cref="CollectionHandler"/>.</typeparam>
        public void Add<T>() where T: CollectionHandler
        {
            var handler = (T)Activator.CreateInstance(typeof(T));
            Add(handler);
        }

        /// <summary>
        /// Adds the collection handler.
        /// </summary>
        /// <param name="handler">The collection handler.</param>
        public void Add(CollectionHandler handler)
        {
            Add(handler.CollectionType, handler);
        }

        /// <summary>
        /// Adds or sets the collection handler.
        /// </summary>
        /// <param name="handler">The collection handler.</param>
        public void Set(CollectionHandler handler)
        {
            Set(handler.CollectionType, handler);
        }
    }
}
