using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Storages;
using DotNetSerializer.Base.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetSerializer.Binary.Converters.Default
{
    /// <summary>
    /// Converter for string values.
    /// </summary>
    public class StringConverter : BinaryConverter<string>
    {
        private readonly DefaultAttributeStorage _defaultAttribute;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringConverter"/> class with the default attribute storage.
        /// </summary>
        /// <param name="defaultAttribute"></param>
        public StringConverter(DefaultAttributeStorage defaultAttribute)
        {
            _defaultAttribute = defaultAttribute;
        }

        /// <summary>
        /// Reads a string value from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <returns>The read string value.</returns>
        public override string ReadValue(BinaryReader reader, BinaryContext context)
        {
            var attribute = context.MetaData != null ? AttributeUtilities.GetStringFormatAttribute(context.MetaData.Property) : null;

            // if property not marked StringFormatAttribute use default attribute
            if (attribute == null)
                attribute = _defaultAttribute.Get<StringFormatAttribute>();

            return ReadStringBySizeType(reader, attribute);
        }

        private string ReadStringBySizeType(BinaryReader reader, StringFormatAttribute attribute)
        {
            var encoding = Encoding.GetEncoding(attribute.EncodingName);
            switch (attribute.SizeType)
            {
                case StringSizeType.Fixed:
                    return ReadFixedSizeString(reader, encoding, attribute.Size);
                case StringSizeType.Prefix:
                    return ReadPrefixSizeString(reader, encoding);
                case StringSizeType.SignEnd:
                    return ReadSignEndString(reader, encoding);
                default:
                    throw new DotNetSerializerException($"Undefined string size type: {attribute.SizeType}");
            }
        }

        /// <summary>
        /// Reads a fixed-size string value using the specified encoding and size from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="encoding">The encoding of the string value.</param>
        /// <param name="size">The string size in bytes.</param>
        /// <returns>The read string value.</returns>
        public static string ReadFixedSizeString(BinaryReader reader, Encoding encoding, int size)
        {
            var buffer = reader.ReadBytes(size);
            return encoding.GetString(buffer).TrimEnd('\0');
        }

        /// <summary>
        /// Reads a prefixed-size string value using the specified encoding from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="encoding">The encoding of the string value.</param>
        /// <returns>The read string value.</returns>
        public static string ReadPrefixSizeString(BinaryReader reader, Encoding encoding)
        {
            var size = reader.ReadInt32();
            var buffer = reader.ReadBytes(size);
            return encoding.GetString(buffer);
        }

        /// <summary>
        /// Reads a sign end string value using the specified encoding from the binary stream.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="encoding">The encoding of the string value.</param>
        /// <returns>The read string value.</returns>
        public static string ReadSignEndString(BinaryReader reader, Encoding encoding)
        {
            var byteList = new List<byte>();
            var c = reader.ReadByte();
            while (c != '\0')
            {
                byteList.Add(c);
                c = reader.ReadByte();
            }
            return encoding.GetString(byteList.ToArray());
        }

        /// <summary>
        /// Writes a string value to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The string value to write.</param>
        /// <param name="context">The serialization context containing type and processing information.</param>
        public override void WriteValue(BinaryWriter writer, string value, BinaryContext context)
        {
            var attribute = context.MetaData != null ? AttributeUtilities.GetStringFormatAttribute(context.MetaData.Property) : null;

            // if property not marked StringFormatAttribute use default attribute
            if (attribute == null)
                attribute = _defaultAttribute.Get<StringFormatAttribute>();

            WriteStringBySizeType(writer, value, attribute);
        }

        private void WriteStringBySizeType(BinaryWriter writer, string value, StringFormatAttribute attribute)
        {
            var encoding = Encoding.GetEncoding(attribute.EncodingName);
            switch (attribute.SizeType)
            {
                case StringSizeType.Fixed:
                    WriteFixedString(writer, value, encoding, attribute.Size);
                    break;
                case StringSizeType.Prefix:
                    WritePrefixString(writer, value, encoding);
                    break;
                case StringSizeType.SignEnd:
                    WriteSignEndString(writer, value, encoding);
                    break;
                default:
                    throw new DotNetSerializerException($"Undefined string size type: {attribute.SizeType}");
            }
        }

        /// <summary>
        /// Writes a prefixed-size string value using the specified encoding to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The string value to write.</param>
        /// <param name="encoding">The encoding of the string value.</param>
        public static void WritePrefixString(BinaryWriter writer, string value, Encoding encoding)
        {
            var strBuffer = encoding.GetBytes(value);
            writer.Write(strBuffer.Length);
            writer.Write(strBuffer);
        }

        /// <summary>
        /// Writes a sign end string value using the specified encoding to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The string value to write.</param>
        /// <param name="encoding">The encoding of the string value</param>
        public static void WriteSignEndString(BinaryWriter writer, string value, Encoding encoding)
        {
            var strBuffer = encoding.GetBytes(value);
            writer.Write(strBuffer);
            writer.Write('\0');
        }

        /// <summary>
        /// Writes a fixed-size string value using the specified encoding to the binary stream.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The string value to write.</param>
        /// <param name="encoding">The encoding of the string value.</param>
        /// <param name="size">The string size in bytes.</param>
        public static void WriteFixedString(BinaryWriter writer, string value, Encoding encoding, int size)
        {
            var strBuffer = encoding.GetBytes(value);
            strBuffer = FitToFixedBuffer(strBuffer, size);
            writer.Write(strBuffer);
        }

        private static byte[] FitToFixedBuffer(byte[] valueBuffer, int size)
        {
            byte[] buffer = new byte[size];

            if (buffer.Length > valueBuffer.Length)
            {
                Array.Copy(valueBuffer, buffer, valueBuffer.Length);
            }
            else
            {
                Array.Copy(valueBuffer, buffer, buffer.Length);
            }

            return buffer;
        }

        /// <summary>
        /// Attempts to retrieve the fixed size for a string based on its format attribute.
        /// </summary>
        /// <param name="context">The serialization context containing type and processing information.</param>
        /// <param name="size">When successful, the fixed string size; otherwise, -1.</param>
        /// <returns>True if the string has a fixed size and is retrieveable; otherwise, false.</returns>
        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            var attribute = AttributeUtilities.GetStringFormatAttribute(context.MetaData.Property) ?? _defaultAttribute.Get<StringFormatAttribute>();

            if (attribute.SizeType != StringSizeType.Fixed)
                return false;

            size = attribute.Size;
            return true;
        }
    }
}
