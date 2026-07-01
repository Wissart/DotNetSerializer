using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for long values.
    /// </summary>
    public class Int64Converter : BinaryConverter<long>
    {
        /// <summary>
        /// Reads a long value from the binary stream.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read long value.</returns>
        public override long ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadInt64();
        }

        /// <summary>
        /// Gets the fixed size of a long.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(long).</param>
        /// <returns>Always true as long has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(long);
            return true;
        }

        /// <summary>
        /// Writes a long value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The long value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, long value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
