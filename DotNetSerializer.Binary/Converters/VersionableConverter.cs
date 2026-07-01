using System;
using System.IO;

namespace DotNetSerializer.Binary.Converters
{
    /// <summary>
    /// Base converter for versionable types that separates version and data serialization.
    /// </summary>
    /// <typeparam name="T">The type this converter handles, must be a reference type.</typeparam>
    public abstract class VersionableConverter<T> : BinaryConverter<T>
        where T : class
    {
        /// <summary>
        /// Reads a versionable object by creating an instance and populating it.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read object instance.</returns>
        public override T ReadValue(BinaryReader reader, BinaryContext context)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));

            ReadVersion(reader, obj, context);
            ReadObject(reader, obj, context);

            return obj;
        }

        /// <summary>
        /// Read the version information from binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="obj">The target object instance.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public abstract void ReadVersion(BinaryReader reader, T obj, BinaryContext context);

        /// <summary>
        /// Reads the object data from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="obj">The target object instance.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public abstract void ReadObject(BinaryReader reader, T obj, BinaryContext context);

        /// <summary>
        /// Writes a versionable object by serializing version and data separately.
        /// </summary>
        /// <param name="writer">The binary writer to write to</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, T value, BinaryContext context)
        {
            WriteVersion(writer, value, context);
            WriteObject(writer, value, context);
        }

        /// <summary>
        /// Writes the version information to binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public abstract void WriteVersion(BinaryWriter writer, T value, BinaryContext context);

        /// <summary>
        /// Writes the object data to binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public abstract void WriteObject(BinaryWriter writer, T value, BinaryContext context);
    }
}
