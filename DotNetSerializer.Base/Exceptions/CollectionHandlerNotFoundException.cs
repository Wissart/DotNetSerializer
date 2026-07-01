using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when the collection handler is not found in the serialization options
    /// </summary>
    public class CollectionHandlerNotFoundException : DotNetSerializerException
    {
        /// <summary>Gets the collection type that caused this error.</summary>
        public Type CollectionType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionHandlerNotFoundException"/> class with the collection type.
        /// </summary>
        /// <param name="collectionType">The collection type that caused this error</param>
        public CollectionHandlerNotFoundException(Type collectionType) 
            : base($"Collection handler was not found. Collection type: {collectionType}.")
        {
            CollectionType = collectionType;
        }
    }
}
