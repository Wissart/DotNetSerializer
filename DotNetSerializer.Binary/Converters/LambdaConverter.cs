using System;
using System.IO;

namespace DotNetSerializer.Binary.Converters
{
    /// <summary>
    /// Converter that uses lambda delegates for custom serialization logic.
    /// </summary>
    /// <typeparam name="T">The type this converter handles.</typeparam>
    public class LambdaConverter<T> : BinaryConverter<T>
    {
        public delegate bool TryGetSizeDelegate(BinaryContext metaData, out int size);

        private readonly Func<BinaryReader, BinaryContext, T> _readMethod;
        private readonly Action<BinaryWriter, T, BinaryContext> _writeMethod;
        private readonly TryGetSizeDelegate _tryGetSize;

        private readonly bool _canWrite;
        internal override bool CanWrite => _canWrite;

        /// <summary>
        /// Initializes a new instance with custom serialization delegates.
        /// </summary>
        /// <param name="readMethod">Delegate for deserializing a value of type <typeparamref name="T">.</param>
        /// <param name="writeMethod">Delegate for serializing a value of type <typeparamref name="T"> (optional).</param>
        /// <param name="tryGetSize">Delegate for retrieving the fixed size (optional).</param>
        public LambdaConverter(Func<BinaryReader, BinaryContext, T> readMethod, 
            Action<BinaryWriter, T, BinaryContext> writeMethod = null, 
            TryGetSizeDelegate tryGetSize = null)
        {
            _readMethod = readMethod;

            _canWrite = writeMethod == null;
            _writeMethod = writeMethod;

            _tryGetSize = tryGetSize;
            if (_tryGetSize == null)
                _tryGetSize = DefaultGetSize;
        }

        /// <summary>
        /// Reads a value using the configured read delegate
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read value of type <typeparamref name="T"/>.</returns>
        public override T ReadValue(BinaryReader reader, BinaryContext context)
        {
            return _readMethod.Invoke(reader, context);
        }

        /// <summary>
        /// Writes a value using the configured write delegate.
        /// </summary>
        /// <param name="writer">The binary writer to write data to.</param>
        /// <param name="value">The value of type <typeparamref name="T"/> to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, T value, BinaryContext context)
        {
            _writeMethod.Invoke(writer, value, context);
        }

        /// <summary>
        /// Attempts to retrieve the fixed size using configured delegate.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">When successful, the calculated type size.</param>
        /// <returns>True if size retrieval succeeded; otherwise, false.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            return _tryGetSize(context, out size);
        }

        private bool DefaultGetSize(BinaryContext metaData, out int size)
        {
            size = -1;
            return false;
        }
    }
}
