using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for sbyte values.
    /// </summary>
    public class SByteConverter : BinaryConverter<sbyte>
    {
        /// <summary>
        /// Reads a sbyte value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read sbyte value.</returns>
        public override sbyte ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadSByte();
        }

        /// <summary>
        /// Gets the fixed size of a sbyte.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(sbyte).</param>
        /// <returns>Always true as sbyte has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(sbyte);
            return true;
        }

        /// <summary>
        /// Writes a sbyte value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The sbyte value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, sbyte value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
