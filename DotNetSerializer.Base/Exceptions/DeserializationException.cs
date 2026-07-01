using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Base exception for all deserialization errors
    /// </summary>
    public class DeserializationException : ContextableException
    {
        /// <inheritdoc/>
        public DeserializationException(string message) 
            : base(message)
        {
        }

        /// <inheritdoc/>
        public DeserializationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <inheritdoc/>
        public DeserializationException(string message, SerializationContext context) 
            : base(message, context)
        {
        }
    }
}
