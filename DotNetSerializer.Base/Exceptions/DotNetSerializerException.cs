using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>Base exception for all serialization-related errors.</summary>
    public class DotNetSerializerException : Exception
    {
        /// <summary>
        /// Creates exception with message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public DotNetSerializerException(string message) 
            : base(message) { }

        /// <summary>
        /// Creates exception with message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public DotNetSerializerException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
