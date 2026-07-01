using System;
using System.Collections;

namespace DotNetSerializer.Base.Utilities
{
    /// <summary>
    /// Utility methods for collection handling during the process.
    /// </summary>
    public static class CollectionUtilities
    {
        /// <summary>
        /// Attempts to retrieve element types from a collection type.
        /// </summary>
        /// <param name="declareCollectionType">The collection type to analyze.</param>
        /// <param name="elementTypes">When successful, the element type(s) of the collection.</param>
        /// <returns>True if element types were found; otherwise, false.</returns>
        public static bool TryGetElementTypes(Type declareCollectionType, out Type[] elementTypes)
        {
            elementTypes = GetElementTypes(declareCollectionType);
            return elementTypes != null;
        }

        /// <summary>
        /// Gets the element type(s) from a collection type.
        /// </summary>
        /// <param name="declareCollectionType">The collection type to analyze.</param>
        /// <returns>
        /// Array of element types, or null if type is not a recognized collection.
        /// Returns null for string (treated as non-collection).
        /// Returns element type for arrays.
        /// Returns generic arguments for generic collections (first interface).
        /// Returns object[] for non-generic IEnumerable.
        /// </returns>
        public static Type[] GetElementTypes(Type declareCollectionType)
        {
            if (declareCollectionType == typeof(string))
                return null;

            if (declareCollectionType.IsArray)
                return new Type[] { declareCollectionType.GetElementType() };

            if (declareCollectionType.IsGenericType)
            {
                foreach (var interfaceType in declareCollectionType.GetInterfaces())
                {
                    if (interfaceType.IsGenericType)
                    {
                        return interfaceType.GetGenericArguments();
                    }
                }
            }

            return typeof(IEnumerable).IsAssignableFrom(declareCollectionType) ? new Type[] { typeof(object) } : null;
        }

        /// <summary>
        /// Gets the generic type definition or base type for a collection.
        /// </summary>
        /// <param name="type">The collection type to analyze.</param>
        /// <returns>typeof(Array) for arrays, or the generic type definition for generic collections.</returns>
        /// <exception cref="ArgumentException">Thrown when the type is not a collection.</exception>
        public static Type GetCollectionType(Type type)
        {
            if (type.IsArray)
                return typeof(Array);

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return type.GetGenericTypeDefinition();

            throw new ArgumentException("Type was not a collection type.", nameof(type));
        }
    }
}
