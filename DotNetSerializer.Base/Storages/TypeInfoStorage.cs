using DotNetSerializer.Base.Reflection;
using System;
using System.Linq;

namespace DotNetSerializer.Base.Storages
{
    /// <summary>
    /// Lazy-loading cache that generates SerializableTypeInfo on demand for types.
    /// </summary>
    public class TypeInfoStorage : DictionaryWrapper<Type, SerializableTypeInfo>
    {
        /// <summary>
        /// Gets or generates SerializableTypeInfo for the specified type.
        /// </summary>
        /// <param name="key">The type to retrieve type information for.</param>
        /// <returns>Cached or newly generated type information.</returns>
        public override SerializableTypeInfo Get(Type key)
        {
            if (!_storage.ContainsKey(key))
                _storage[key] = MakeTypeInfo(key);

            return _storage[key];
        }

        private SerializableTypeInfo MakeTypeInfo(Type type)
        {
            var properties = type.GetProperties()
                                 .GroupBy(p => p.DeclaringType)
                                 .Reverse()
                                 .SelectMany(g => g)
                                 .ToArray();

            return new SerializableTypeInfo(type, properties);
        }
    }
}
