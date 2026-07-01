using System.IO;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for float values.
    /// </summary>
    public class SingleConverter : BinaryConverter<float>
    {
        /// <summary>
        /// Reads a float value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read float value.</returns>
        public override float ReadValue(BinaryReader reader, BinaryContext context)
        {
            return reader.ReadSingle();
        }

        /// <summary>
        /// Gets the fixed size of a float.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">Output parameter set to sizeof(float).</param>
        /// <returns>Always true as float has a fixed size.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = sizeof(float);
            return true;
        }

        /// <summary>
        /// Wirtes a float value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The float value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, float value, BinaryContext context)
        {
            writer.Write(value);
        }
    }
}
