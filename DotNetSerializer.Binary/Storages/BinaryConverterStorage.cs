using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Storages;
using DotNetSerializer.Binary.Converters;
using System;
using System.Collections.Generic;

namespace DotNetSerializer.Binary.Storages
{
    /// <summary>
    /// Storage for binary converters.
    /// </summary>
    public class BinaryConverterStorage : DictionaryWrapper<Type, BinaryConverter>
    {
        /// <summary>
        /// Determines whether the specified converter type is contained.
        /// </summary>
        /// <param name="converterType">The type of converter.</param>
        /// <returns></returns>
        public bool Contains(Type converterType)
        {
            return _storage.ContainsKey(converterType);
        }

        /// <summary>
        /// Gets a <see cref="BinaryConverter"/> for the specified type.
        /// </summary>
        /// <param name="key">The type to retrieve converter for.</param>
        /// <returns>The binary converter, or throws an exception if converter does not exists.</returns>
        /// <exception cref="ConverterNotFoundException">Thrown when the converter does not exists.</exception>
        public override BinaryConverter Get(Type key)
        {
            if (!_storage.ContainsKey(key))
                throw new ConverterNotFoundException(key);

            return _storage[key];
        }

        /// <summary>
        /// Adds a binary converter to the registry.
        /// </summary>
        /// <typeparam name="T">The type of converter derived from <see cref="BinaryConverter"/>.</typeparam>
        public void Add<T>() where T : BinaryConverter
        {
            var converter = (T)Activator.CreateInstance(typeof(T));
            Add(converter);
        }

        /// <summary>
        /// Adds a binary converter to the registry.
        /// </summary>
        /// <param name="converter">The binary converter.</param>
        public void Add(BinaryConverter converter)
        {
            Add(converter.RegisteredType, converter);
        }

        /// <summary>
        /// Adds a collection of converters to the registry.
        /// </summary>
        /// <param name="converters">The enumerable collection of converters to add.</param>
        public void Add(IEnumerable<BinaryConverter> converters)
        {
            foreach (var converter in converters)
                Add(converter);
        }

        /// <summary>
        /// Adds or sets a binary converter to the registry.
        /// </summary>
        /// <typeparam name="T">The type of converter derived from <see cref="BinaryConverter"/>.</typeparam>
        public void Set<T>() where T : BinaryConverter
        {
            var converter = (T)Activator.CreateInstance(typeof(T));
            Set(converter);
        }

        /// <summary>
        /// Adds or sets a binary converter to the registry.
        /// </summary>
        /// <param name="converter">The binary converter.</param>
        public void Set(BinaryConverter converter)
        {
            Set(converter.RegisteredType, converter);
        }

        /// <summary>
        /// Adds or sets a collection of converters to the registry.
        /// </summary>
        /// <param name="converters">The enumerable collection of converters to add.</param>

        public void Set(IEnumerable<BinaryConverter> converters)
        {
            foreach (var converter in converters)
                Set(converter);
        }
    }
}
