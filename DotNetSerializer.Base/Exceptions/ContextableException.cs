using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Base exception for all errors requiring context.
    /// </summary>
    public class ContextableException : DotNetSerializerException
    {
        /// <summary>Gets the serialization context when exception occurred</summary>
        public SerializationContext SerializationContext { get; }


        /// <inheritdoc />
        public ContextableException(string message)
            : base(message) { }

        /// <inheritdoc />
        public ContextableException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Creates exception with message and serialization context.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="context">The serialization context when exception occured.</param>
        public ContextableException(string message, SerializationContext context)
            : base(message)
        {
            SerializationContext = context;
        }

        /// <summary>
        /// Creates exception with message, context, and innner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="context">The serialization context when exception occured.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public ContextableException(string message, SerializationContext context, Exception innerException)
            : base(message, innerException)
        {
            SerializationContext = context;
        }
    }
}
