using System.IO;

namespace DotNetSerializer.Base.Processes
{
    /// <summary>
    /// Defines the contract for serialization operations
    /// </summary>
    public interface ISerializationProcess
    {
        /// <summary>
        /// Serializes an object of the specified type to a stream
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="obj">The serialize object of type <typeparamref name="T"/>.</param>
        void Serialize<T>(Stream stream, T obj);
    }
}
