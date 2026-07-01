using System;
using System.IO;

namespace DotNetSerializer.Binary.Converters
{
    /// <summary>
    /// Base converter for factory-created types that only supports deserialization.
    /// </summary>
    /// <typeparam name="T">The type this converter creates, must be a reference type.</typeparam>
    public abstract class FactoryConverter<T> : BinaryConverter 
        where T : class
    {
        /// <summary>Disables write support for factory converters.</summary>
        internal override sealed bool CanWrite => false;

        /// <summary>Gets the type that the converter converts.</summary>
        public override Type ConvertedType => typeof(T);
        /// <summary>Gets the type for converter registration</summary>
        public override Type RegisteredType => typeof(T);


        /// <summary>
        /// Reads and creates an instance from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read data from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The created object instance.</returns>
        public override object Read(BinaryReader reader, BinaryContext context)
        {
            return Create(reader, context);
        }

        /// <summary>
        /// Creates a typed instance from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read data from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The created instance of type <typeparamref name="T"/>.</returns>
        public abstract T Create(BinaryReader reader, BinaryContext context);


        /// <summary>
        /// Write is not supported for factory converters.
        /// </summary>
        /// <param name="writer">The binary writer to write data to.</param>
        /// <param name="value">The value to write</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <exception cref="NotImplementedException">Always thrown as factory converters are read-only.</exception>
        public override void Write(BinaryWriter writer, object value, BinaryContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempts to retrieve the fixed size of the converted type, if applicable.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">When successful, the calculated type size; otherwise, -1.</param>
        /// <returns>False by default; override to provide fixed-size support.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }
}
