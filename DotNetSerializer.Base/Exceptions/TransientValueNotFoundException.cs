namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when the transient value does not exist. 
    /// </summary>
    public class TransientValueNotFoundException : DotNetSerializerException
    {
        /// <summary>Gets the transient value name that caused this error.</summary>
        public string ValueName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientValueNotFoundException"/> class with the transient value name and context.
        /// </summary>
        /// <param name="valueName">The ransient value name.</param>
        public TransientValueNotFoundException(string valueName)
            : base($"Transient value was not found. Value name: {valueName}.")
        {
            ValueName = valueName;
        }
    }
}
