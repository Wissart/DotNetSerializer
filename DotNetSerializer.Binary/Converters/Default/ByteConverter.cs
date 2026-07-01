using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for byte values.
    /// </summary>
    public class ByteConverter : BinaryConverter<byte>
    {
        /// <summary>
        /// Reads a byte value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read byte value.</returns>
        public override byte ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadByte();
        }

        /// <summary>
        /// Gets the fixed size of a byte.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(byte).</param>
        /// <returns>Always true as byte has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(byte);
            return true;
        }

        /// <summary>
        /// Writes a byte value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary wruter to write to.</param>
        /// <param name="value">The byte value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, byte value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
