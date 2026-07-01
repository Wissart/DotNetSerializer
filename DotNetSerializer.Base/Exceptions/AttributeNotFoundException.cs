namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when a required attriute is not found on a type or member.
    /// </summary>
    public class AttributeNotFoundException : AttributeException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeNotFoundException"/> class with the attribute name and context.
        /// </summary>
        /// <param name="attributeName">The attribute name.</param>
        /// <param name="context">The serialization context when exception occured.</param>
        public AttributeNotFoundException(string attributeName, SerializationContext context)
            : base($"Attribute '{attributeName}' was not found.", attributeName, context)
        {
        }
    }
}
