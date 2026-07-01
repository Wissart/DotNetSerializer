using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for uint values.
    /// </summary>
    public class UInt32Converter : BinaryConverter<uint>
    {
        /// <summary>
        /// Reads a uint value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read uint value.</returns>
        public override uint ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadUInt32();
        }

        /// <summary>
        /// Gets the fixed size of a uint.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(uint).</param>
        /// <returns>Always true as uint has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(uint);
            return true;
        }

        /// <summary>
        /// Writes a uint value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The uint value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, uint value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
