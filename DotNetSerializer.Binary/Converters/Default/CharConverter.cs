using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for char values.
    /// </summary>
    public class CharConverter : BinaryConverter<char>
    {
        /// <summary>
        /// Reads a char value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read char value.</returns>
        public override char ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadChar();
        }

        /// <summary>
        /// Gets the fixed size of a char.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to 1.</param>
        /// <returns>Always true as byte has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext metaData, out int size)
        {
            size = 1;
            return true;
        }

        /// <summary>
        /// Writes a char value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary wruter to write to.</param>
        /// <param name="value">The byte value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>

        public override void WriteValue(BinaryWriter writer, char value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
