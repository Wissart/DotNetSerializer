using System.IO;

namespace DotNetSerializer.Base.Processes
{
    /// <summary>
    /// Defines the contract for deserialization operations.
    /// </summary>
    public interface IDeserializationProcess
    {
        /// <summary>
        /// Deserializes an object of th specified from a stream.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="stream">The stream containing the serialization data.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        T Deserialize<T>(Stream stream);
    }
}
