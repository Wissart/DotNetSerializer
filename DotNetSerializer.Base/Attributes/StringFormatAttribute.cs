using System;

namespace DotNetSerializer.Base.Attributes
{
    /// <summary>
    /// Determines how string size is processed
    /// </summary>
    public enum StringSizeType
    {
        /// <summary>Fixed size is defined by <see cref="StringFormatAttribute.Size"/>.</summary>
        Fixed,
        /// <summary>Size is prefixed in the serialized data.</summary>
        Prefix,
        /// <summary>The end of the string is defined by the character</summary>
        SignEnd
    }

    /// <summary>
    /// Defines how the string property is processed
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class StringFormatAttribute : DotNetSerializerAttribute
    {
        /// <summary>Gets the encoding used for handling.</summary>
        public string EncodingName { get; }
        /// <summary>Gets the size determination strategy.</summary>
        public StringSizeType SizeType { get; }
        /// <summary>Gets the fixed length when <see cref="SizeType"/> is <see cref="StringSizeType.Fixed"/>.</summary>
        public int Size { get; }

        /// <summary>
        /// Initializes the string format with 'UTF-8' encoding, the 'Prefix' size strategy, and a fixed size of 0.
        /// </summary>
        public StringFormatAttribute() 
            : this("UTF-8", StringSizeType.Prefix, 0) { }

        /// <summary>
        /// Initializes string format with encoding, size strategy, and optional fixed size.
        /// </summary>
        /// <param name="encoding">Encoding name. Default: "UTF-8".</param>
        /// <param name="sizeType">Size determination strategy. Default: Prefix.</param>
        /// <param name="size">Fixed length required when <paramref name="sizeType"/> is <see cref="StringSizeType.Fixed"/>. Default: 0.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sizeType"> is <see cref="StringSizeType.Fixed"/> and <paramref name="size"/> is less than or equal to 0.</exception>
        public StringFormatAttribute(string encoding = "UTF-8", StringSizeType sizeType = StringSizeType.Prefix, int size = 0)
        {
            if (sizeType == StringSizeType.Fixed && size <= 0)
                throw new ArgumentException("Fixed requires positive size.", nameof(size));

            EncodingName = encoding;
            SizeType = sizeType;
            Size = size;
        }

        /// <summary>
        /// Initializes string format with default encoding, size strategy, and fixed size equal 0.
        /// </summary>
        /// <param name="sizeType">Size determination strategy</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sizeType"> is <see cref="StringSizeType.Fixed"/>.</exception>
        public StringFormatAttribute(StringSizeType sizeType)
            : this(sizeType: sizeType, size: 0) { }

        /// <summary>
        /// Initializes string format with default encoding, fixed size strategy, and fixed size.
        /// </summary>
        /// <param name="size">Fixed length</param>
        public StringFormatAttribute(int size)
            : this(sizeType: StringSizeType.Fixed, size: size) { }
    }
}
