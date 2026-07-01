using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when the converter is not found in the serialization options for a type.
    /// </summary>
    public class ConverterNotFoundException : DotNetSerializerException
    {
        /// <summary>Gets the type for which the converter is registered, which caused this error.</summary>
        public Type ConverterType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterNotFoundException"/> class with the type for which the converter is registered.
        /// </summary>
        /// <param name="converterType">The type for which the converter is registered</param>
        public ConverterNotFoundException(Type converterType) 
            : base($"Converter '{converterType}' was not found.")
        {
            ConverterType = converterType;
        }
    }
}
