using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for short values.
    /// </summary>
    public class Int16Converter : BinaryConverter<short>
    {
        /// <summary>
        /// Reads a short value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read short value.</returns>
        public override short ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadInt16();
        }

        /// <summary>
        /// Gets the fixed size of a short.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(short).</param>
        /// <returns>Always true as short has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(short);
            return true;
        }

        /// <summary>
        /// Writes a short value to the binary stream.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The short value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, short value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
