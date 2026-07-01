using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Base exception for all errors that occur during deserialization.
    /// </summary>
    public class SerializationException : ContextableException
    {
        /// <inheritdoc/>
        public SerializationException(string message) 
            : base(message)
        {
        }

        /// <inheritdoc/>
        public SerializationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <inheritdoc/>
        public SerializationException(string message, SerializationContext context) 
            : base(message, context)
        {
        }

        /// <inheritdoc/>
        public SerializationException(string message, SerializationContext context, Exception innerException) 
            : base(message, context, innerException)
        {
        }
    }
}
