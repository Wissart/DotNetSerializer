using System;
using System.IO;

namespace DotNetSerializer.Binary.Converters
{
    /// <summary>
    /// Base class for binary serialization converters.
    /// </summary>
    public abstract class BinaryConverter
    {
        /// <summary>Gets a value that indicates whether the converter can write.</summary>
        internal virtual bool CanWrite => true;

        /// <summary>Gets the type for converter registration.</summary>
        public abstract Type RegisteredType { get; }
        /// <summary>Gets the type that the converter converts.</summary>
        public abstract Type ConvertedType { get; }
        /// <summary>Gets a value indicating whether the conversion process is complete.</summary>
        /// Override to signal that the conversion should be considered finished,
        /// typically used for special handling of reference types or complex objects.
        public virtual bool IsComplete => false;


        /// <summary>
        /// Reads a value as an object from the binary stream using the specified context.
        /// </summary>
        /// <param name="reader">The binary reader to read data from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The value as an object.</returns>
        public abstract object Read(BinaryReader reader, BinaryContext context);

        /// <summary>
        /// Writes a value to the binary stream using the specified context.
        /// </summary>
        /// <param name="writer">The binary writer to write data to.</param>
        /// <param name="value">The value to write</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public abstract void Write(BinaryWriter writer, object value, BinaryContext context);

        /// <summary>
        /// Attempts to retrieve the fixed size of the converted type.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">When successful, the calculated type size.</param>
        /// <returns>True if size retrieval succeeded; otherwise, false.</returns>
        public abstract bool TryGetSize(BinaryContext context, out int size);
    }

    /// <summary>
    /// Generic base class for binary serialization converters.
    /// </summary>
    /// <typeparam name="T">The type this conveter handles.</typeparam>
    public abstract class BinaryConverter<T> : BinaryConverter
    {
        /// <summary>Gets the type for converter registration.</summary>
        public override Type RegisteredType => typeof(T);
        /// <summary>Gets the type that the converter converts.</summary>
        public override Type ConvertedType => typeof(T);

        /// <summary>
        /// Reads a value as an object from the binary stream using the specified context.
        /// </summary>
        /// <param name="reader">The binary reader to read data from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The value as an object.</returns>
        public override object Read(BinaryReader reader, BinaryContext context)
        {
            return ReadValue(reader, context);
        }

        /// <summary>
        /// Read a typed value form the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read data from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read value of type <typeparamref name="T"/>.</returns>
        public abstract T ReadValue(BinaryReader reader, BinaryContext context);

        /// <summary>
        /// Writes a value to the binary stream using the specified context.
        /// </summary>
        /// <param name="writer">The binary writer to write data to.</param>
        /// <param name="value">The value to write</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <exception cref="InvalidCastException"> Thrown when the value is not of the type <typeparamref name="T"/>.</exception>
        public override void Write(BinaryWriter writer, object value, BinaryContext context)
        {
            if (!(value is T))
                throw new InvalidCastException($"Wrong value type: {value.GetType()}. {ConvertedType} is required!");

            WriteValue(writer, (T)value, context);
        }

        /// <summary>
        /// Writes a typed value to the binary stream using the specified context.
        /// </summary>
        /// <param name="writer">The binary writer to write data to.</param>
        /// <param name="value">The value of type <typeparamref name="T"/> to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public abstract void WriteValue(BinaryWriter writer, T value, BinaryContext context);

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
