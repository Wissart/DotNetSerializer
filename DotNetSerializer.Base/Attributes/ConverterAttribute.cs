using System;

namespace DotNetSerializer.Base.Attributes
{
    /// <summary>
    /// Defines which converter is used to handle a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConverterAttribute : DotNetSerializerAttribute
    {
        /// <summary>Gets the type of converter used for handling</summary>
        public Type ConverterType { get; }

        /// <summary>
        /// Initializes with a converter type
        /// </summary>
        /// <param name="converterType"></param>
        public ConverterAttribute(Type converterType) => ConverterType = converterType;
    }
}
