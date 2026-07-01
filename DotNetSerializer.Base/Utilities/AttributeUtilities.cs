using DotNetSerializer.Base.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetSerializer.Base.Utilities
{
    /// <summary>
    /// Utility methods for working with attribute during the process.
    /// </summary>
    public static class AttributeUtilities
    {
        /// <summary>
        /// Determines if the property has a <see cref="ConverterAttribute"/> applied.
        /// </summary>
        /// <param name="property">The property to inspect for the attribute.</param>
        /// <returns>True if the property conatins a <see cref="ConverterAttribute"/>; otherwise, false.</returns>
        public static bool ContainConverterAttribute(PropertyInfo property)
        {
            return IsPropertyContainAttribute<ConverterAttribute>(property);
        }

        /// <summary>
        /// Retrieves the <see cref="ConverterAttribute"/> from the specified property, if present.
        /// </summary>
        /// <param name="property">The property to retrieve the attribute from.</param>
        /// <returns>The <see cref="ConverterAttribute"/> instance if found; otherwise, null.</returns>
        public static ConverterAttribute GetConverterAttribute(PropertyInfo property)
        {
            return property.GetCustomAttribute<ConverterAttribute>();
        }
        
        /// <summary>
        /// Retrieves the <see cref="CollectionAttribute"/> from the specified property, if present.
        /// </summary>
        /// <param name="property">The property to retrive the attribute from.</param>
        /// <returns>The <see cref="CollectionAttribute"/> instance if found; otherwise, null.</returns>
        public static CollectionAttribute GetCollectionAttribute(PropertyInfo property)
        {
            return property.GetCustomAttribute<CollectionAttribute>();
        }

        /// <summary>
        /// Retrieves the <see cref="StringFormatAttribute"/> from the specified property, if present.
        /// </summary>
        /// <param name="property">The property to retrive the attribute from.</param>
        /// <returns>The <see cref="StringFormatAttribute"/> instance if found; otherwise, null.</returns>
        public static StringFormatAttribute GetStringFormatAttribute(PropertyInfo property)
        {
            return property.GetCustomAttribute<StringFormatAttribute>();
        }

        /// <summary>
        /// Determines if the specified type has the given attribute applied.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to check for.</typeparam>
        /// <param name="objectType">The type to inspect for the attribute.</param>
        /// <returns>True if the attribute is present; otherwise, false.</returns>
        public static bool IsTypeContainAttribute<TAttribute>(Type objectType)
            where TAttribute : Attribute
        {
            return objectType.GetCustomAttribute<TAttribute>(true) != null;
        }

        /// <summary>
        /// Determines if the specified type has the given attribute applied (including inherited).
        /// </summary>
        /// <param name="objectType">The type to inspect for the attribute.</param>
        /// <param name="attributeType">The attribute type to check for.</param>
        /// <returns>True if the attribute is present; otherwise, false.</returns>
        public static bool IsTypeContainAttribute(Type objectType, Type attributeType)
        {
            return objectType.GetCustomAttribute(attributeType, true) != null;
        }

        /// <summary>
        /// Determines if the specified property has the given attribute applied.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to check for.</typeparam>
        /// <param name="property">THe property to inspect fot the attribute.</param>
        /// <returns>True if the attribute is present; otherwise, false.</returns>
        public static bool IsPropertyContainAttribute<TAttribute>(PropertyInfo property)
            where TAttribute : Attribute
        {
            return property.GetCustomAttribute<TAttribute>(true) != null;
        }

        /// <summary>
        /// Determines if the specified property has the given attribute applied (including inherited).
        /// </summary>
        /// <param name="property">The property to inspect for the attribute.</param>
        /// <param name="attributeType">The attribute type to check for.</param>
        /// <returns>True if the attribute is present; otherwise, false.</returns>
        public static bool IsPropertyContainAttribute(PropertyInfo property, Type attributeType)
        {
            return property.GetCustomAttribute(attributeType, true) != null;
        }

        /// <summary>
        /// Retrievs all attributes from the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetAttributes(PropertyInfo property)
        {
            return property.GetCustomAttributes();
        }

        /// <summary>
        /// Attempts to retrieve the specified attribute from a property (including inherited).
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to retrieve.</typeparam>
        /// <param name="property">The property to inspect.</param>
        /// <param name="attribute">When successful, the retrieved attribute instance.</param>
        /// <returns>True if the attribute exists; otherwise, false.</returns>
        public static bool TryGetPropertyAttribute<TAttribute>(PropertyInfo property, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = property.GetCustomAttribute<TAttribute>(true);
            return attribute != null;
        }

        /// <summary>
        /// Attempts to retrieve the specified attributes from a type. (including inherited).
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to retrieve.</typeparam>
        /// <param name="type">The type to inspect.</param>
        /// <param name="attribute">When successful, the retrieved attribute instance.</param>
        /// <returns>True if the attribute exists; otherwise, false.</returns>
        public static bool TryGetTypeAttribute<TAttribute>(Type type, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = type.GetCustomAttribute<TAttribute>(true);
            return attribute != null;
        }
    }
}
