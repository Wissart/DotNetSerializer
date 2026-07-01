using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for boolean values
    /// </summary>
    public class BoolConverter : BinaryConverter<bool>
    {
        /// <summary>
        /// Reads a boolean value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read boolean value.</returns>
        public override bool ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadBoolean();
        }


        /// <summary>
        /// Gets the fixed size of a boolean.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(bool).</param>
        /// <returns>Always true as bool has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(bool);
            return true;
        }

        /// <summary>
        /// Writes a boolean value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to</param>
        /// <param name="value">The boolean value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, bool value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
