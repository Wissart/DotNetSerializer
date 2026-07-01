using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for int values.
    /// </summary>
    public class Int32Converter : BinaryConverter<int>
    {
        /// <summary>
        /// Reads a int value from the binary stream.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read int value.</returns>
        public override int ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadInt32();
        }

        /// <summary>
        /// Gets the fixed size of a int.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(int).</param>
        /// <returns>Always true as int has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(int);
            return true;
        }

        /// <summary>
        /// Writes a int value to the binary stream.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The int value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, int value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
