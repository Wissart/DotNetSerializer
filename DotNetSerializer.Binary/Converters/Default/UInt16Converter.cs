using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for ushort values.
    /// </summary>
    public class UInt16Converter : BinaryConverter<ushort>
    {
        /// <summary>
        /// Reads a ushort value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read ushort value.</returns>
        public override ushort ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadUInt16();
        }

        /// <summary>
        /// Gets the fixed size of a ushort.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(ushort).</param>
        /// <returns>Always true as ushort has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(ushort);
            return true;
        }

        /// <summary>
        /// Writes a ushort value to binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The ushort value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, ushort value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
