using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converer for ulong values.
    /// </summary>
    public class UInt64Converter : BinaryConverter<ulong>
    {
        /// <summary>
        /// Reads a ulong value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read ulong value.</returns>
        public override ulong ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadUInt64();
        }

        /// <summary>
        /// Gets the fixed size of a ulong.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(ulong).</param>
        /// <returns>Always true as ulong has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(ulong);
            return true;
        }

        /// <summary>
        /// Writes a ulong value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The uong value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, ulong value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
