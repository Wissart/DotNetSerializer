namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when deserialization is completed but the end of the stream is not reached.
    /// </summary>
    public class StreamEndNotReachedException : DeserializationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEndNotReachedException"/> class with the default error message.
        /// </summary>
        public StreamEndNotReachedException()
        : base("Stream contains data after read") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEndNotReachedException"/> class with the specified remaining bytes count.
        /// </summary>
        /// <param name="remainingBytes">The remaining bytes.</param>
        public StreamEndNotReachedException(long remainingBytes)
            : base($"Stream end not reached; remaining {remainingBytes} bytes.") { }
    }
}
