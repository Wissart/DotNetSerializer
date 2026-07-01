using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when the property is not found.
    /// </summary>
    public class VersionPropertyNotFoundException : DotNetSerializerException
    {
        /// <summary>Gets the property name that caused this error.</summary>
        public string PropertyName { get; }

        /// <summary>Gets the type that caused this error.</summary>
        public Type ObjectType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionPropertyNotFoundException"/> class with the property name and object type.
        /// </summary>
        /// <param name="propertyName">The name of the property that was not found.</param>
        /// <param name="objectType">The type from which to retrieve the property.</param>
        public VersionPropertyNotFoundException(string propertyName, Type objectType) 
            : base($"the version property '{propertyName}' was not found in the type '{objectType}'.")
        {
            PropertyName = propertyName;
            ObjectType = objectType;
        }
    }
}
