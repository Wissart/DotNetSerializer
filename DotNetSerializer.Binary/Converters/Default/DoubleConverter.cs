using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for double values.
    /// </summary>
    public class DoubleConverter : BinaryConverter<double>
    {
        /// <summary>
        /// Reads a double value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read double value.</returns>
        public override double ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadDouble();
        }

        /// <summary>
        /// Gets the fixed size of a double.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(double).</param>
        /// <returns>Always true as double has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(double);
            return true;
        }

        /// <summary>
        /// Writes a double value to the binary stream.
        /// </summary>
        /// <param name="writer">The binart writer to write to</param>
        /// <param name="value">The double value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, double value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
