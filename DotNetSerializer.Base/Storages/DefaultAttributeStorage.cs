using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Exceptions;
using System;
using System.Collections.Generic;

namespace DotNetSerializer.Base.Storages
{
    /// <summary>
    /// Lazy-loading cache for default serializer attributes with whitelist validation.
    /// </summary>
    public class DefaultAttributeStorage : DictionaryWrapper<Type, DotNetSerializerAttribute>
    {
        private readonly HashSet<Type> _allowAttributes;

        /// <summary>
        /// Initializes with the settled base default attributes (StringFormat, Collection).
        /// </summary>
        public DefaultAttributeStorage()
        {
            _allowAttributes = new HashSet<Type>()
            {
                typeof(StringFormatAttribute),
                typeof(CollectionAttribute),
            };
        }

        /// <summary>
        /// Gets the default attribute of type T, creating it if not cached.
        /// </summary>
        /// <typeparam name="T">The attribute type derived from <see cref="DotNetSerializerAttribute"/>.</typeparam>
        /// <returns>The default attribute instance.</returns>
        /// <exception cref="DotNetSerializerException">Thrown if T is not in the allow list.</exception>>
        public T Get<T>() where T : DotNetSerializerAttribute
        {
            return (T)Get(typeof(T));
        }

        /// <summary>
        /// Gets or creates a default attribute instance for the specified type.
        /// </summary>
        /// <param name="key">The attribute type to retrieve.</param>
        /// <returns>The cached or newly created attribute.</returns>
        /// <exception cref="DotNetSerializerException">Thrown if key is not in the allow list.</exception>
        public override DotNetSerializerAttribute Get(Type key)
        {
            if (!_allowAttributes.Contains(key))
                throw new DotNetSerializerException($"Attribute {key} cannot be default attribute.");

            if (!_storage.ContainsKey(key))
                _storage[key] = (DotNetSerializerAttribute)Activator.CreateInstance(key);

            return _storage[key];
        }

        /// <summary>
        /// Sets the specified attrbiute as the default.
        /// </summary>
        /// <param name="attribute">The attribute that is set as the default.</param>
        /// <exception cref="DotNetSerializerException">Thrown when the attribute is not of allowed type or is not derived from an allowed type.</exception>
        public void Set(DotNetSerializerAttribute attribute)
        {
            Type attributeType = attribute.GetType();
            Type rootAttributeType = attributeType;

            while (rootAttributeType.BaseType != typeof(DotNetSerializerAttribute))
                rootAttributeType = rootAttributeType.BaseType;

            if (!_allowAttributes.Contains(rootAttributeType))
                throw new DotNetSerializerException($"Attribute {rootAttributeType} cannot be default attribute. Attribute type: {attributeType}");

            Set(rootAttributeType, attribute);
        }
    }
}
