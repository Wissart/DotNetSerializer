namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Base exception for all attribute-related errors.
    /// </summary>
    public class AttributeException : ContextableException
    {
        /// <summary>Gets the attribute name that caused exception.</summary>
        public string AttributeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeException"/> class with the specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public AttributeException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeException"/> class with the specified error message and attribute name.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="attributeName">The attribute name.</param>
        public AttributeException(string message, string attributeName) 
            : base(message)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeException"/> class with the specified error message and attribute name and context.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="attributeName">The attribute name.</param>
        /// <param name="context">The serialization context when exception occured.</param>
        public AttributeException(string message, string attributeName, SerializationContext context)
            : base(message, context)
        {
            AttributeName = attributeName;
        }
    }
}
