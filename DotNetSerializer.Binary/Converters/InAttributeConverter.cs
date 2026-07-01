using System;

namespace DotNetSerializer.Binary.Converters
{
    /// <summary>
    /// Base converter for types handled via attribute-based serialization.
    /// </summary>
    /// <typeparam name="T">The type this conveter handles.</typeparam>
    public abstract class InAttributeConverter<T> : BinaryConverter<T>
    {
        /// <summary>Gets the type for converter registration (the implementing converter type).</summary>
        public override Type RegisteredType => GetType();
        /// <summary>Gets a value indicating that conversion is always complete fir attribute-based converters.</summary>
        public override bool IsComplete => true;
    }
}
