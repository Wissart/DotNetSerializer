using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for decimal values.
    /// </summary>
    public class DecimalConverter : BinaryConverter<decimal>
    {
        /// <summary>
        /// Reads a decimal value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read decimal value.</returns>
        public override decimal ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadDecimal();
        }

        /// <summary>
        /// Gets the fixed size of a decimal.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output prarameter set to sizeof(decimal).</param>
        /// <returns>Always true as decimal has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(decimal);
            return true;
        }

        /// <summary>
        /// Writes a decimal value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The decimal value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, decimal value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
