using DotNetSerializer.Base.Processes;
using System.IO;

namespace DotNetSerializer.Base
{
    /// <summary>
    /// Base class for serializers
    /// </summary>
    public abstract class Serializer
    {
        protected IDeserializationProcess _deserializationProcess;
        protected ISerializationProcess _serializationProcess;

        protected abstract IDeserializationProcess GetDeserializationProcess();
        protected abstract ISerializationProcess GetSerializationProcess();

        /// <summary>
        /// Deserializes an object of the specified type from the file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="EndOfStreamException">Thrown when an incomplete data stream is passed, deserialization fails in the converter, or a deserializable type has incorrect structure.</exception> 
        /// <exception cref="CollectionHandlerNotFoundException">Thrown when cannot find a collection handler in process.</exception> 
        /// <exception cref="ConverterNotFoundException">Thrown when cannot find a converter in process.</exception> 
        /// <exception cref="StreamEndNotReachedException">Thrown when an object deserialized, but data still remains in the stream.</exception> 
        /// <exception cref="TransientValueNotFoundException">Thrown when cannot find transient value in process.</exception> 
        /// <exception cref="UnsupportedVersionException">Thrown when the data contains unsupported version value.</exception> 
        /// <exception cref="VersionPropertyNotFoundException">Thrown when an object does not contain a version property.</exception> 
        public T Deserialize<T>(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Deserialize<T>(file);
            }
        }

        /// <summary>
        /// Deserializes an object of the specified type from the stream.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="stream">The stream contains the deserialization data.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="EndOfStreamException">Thrown when an incomplete data stream is passed, deserialization fails in the converter, or a deserializable type has incorrect structure.</exception> 
        /// <exception cref="CollectionHandlerNotFoundException">Thrown when cannot find a collection handler in process.</exception> 
        /// <exception cref="ConverterNotFoundException">Thrown when cannot find a converter in process.</exception> 
        /// <exception cref="StreamEndNotReachedException">Thrown when an object deserialized, but data still remains in the stream.</exception> 
        /// <exception cref="TransientValueNotFoundException">Thrown when cannot find transient value in process.</exception> 
        /// <exception cref="UnsupportedVersionException">Thrown when the data contains unsupported version value.</exception> 
        /// <exception cref="VersionPropertyNotFoundException">Thrown when an object does not contain a version property.</exception> 
        public T Deserialize<T>(Stream stream)
        {
            var objDeserializer = GetDeserializationProcess();

            return objDeserializer.Deserialize<T>(stream);
        }

        /// <summary>
        /// Serializes the specified object to the file.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="obj">The object to be serialize.</param>
        /// <exception cref="CollectionHandlerNotFoundException">Thrown when cannot find a collection handler in process.</exception> 
        /// <exception cref="ConverterNotFoundException">Thrown when cannot find a converter in process.</exception> 
        /// <exception cref="TransientValueNotFoundException">Thrown when cannot find transient value in process.</exception> 
        /// <exception cref="UnsupportedVersionException">Thrown when the data contains unsupported version value.</exception> 
        /// <exception cref="VersionPropertyNotFoundException">Thrown when an object does not contain a version property.</exception> 
        public void Serialize<T>(string filePath, T obj)
        {
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                Serialize<T>(file, obj);
            }
        }

        /// <summary>
        /// Serializes the specified object to the stream.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="obj">The object to be serialize.</param>
        /// <exception cref="CollectionHandlerNotFoundException">Thrown when cannot find a collection handler in process.</exception> 
        /// <exception cref="ConverterNotFoundException">Thrown when cannot find a converter in process.</exception> 
        /// <exception cref="TransientValueNotFoundException">Thrown when cannot find transient value in process.</exception> 
        /// <exception cref="UnsupportedVersionException">Thrown when the data contains unsupported version value.</exception> 
        /// <exception cref="VersionPropertyNotFoundException">Thrown when an object does not contain a version property.</exception> 
        public void Serialize<T>(Stream stream, T obj)
        {
            var objSerializer = GetSerializationProcess();

            objSerializer.Serialize<T>(stream, obj);
        }
    }
}
