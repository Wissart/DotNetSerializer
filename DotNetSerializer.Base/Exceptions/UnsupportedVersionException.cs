using System;

namespace DotNetSerializer.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when the object type contains a version that is not supported.
    /// </summary>
    public class UnsupportedVersionException : DotNetSerializerException
    {
        /// <summary>Gets the version value that caused this error.</summary>
        public uint Version { get; }

        /// <summary>Gets the type that caused this error.</summary>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedVersionException"/> class with the version and object type.
        /// </summary>
        /// <param name="version">The unsupported vesrion.</param>
        /// <param name="objectType">The type containing the version value.</param>
        public UnsupportedVersionException(uint version, Type objectType) 
            : base($"Version {version} is not supported by type '{objectType}'")
        {
            Version = version;
            Type = objectType;
        }
    }
}
