using DotNetSerializer.Base.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetSerializer.Base.Tests.Unit.Attributes
{
    [TestClass]
    public class StringFormatAttributeTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldSetEncodingUTF8AndSizeTypeIsPrefix()
        {
            var attribute = new StringFormatAttribute();

            Assert.AreEqual("UTF-8", attribute.EncodingName);
            Assert.AreEqual(StringSizeType.Prefix, attribute.SizeType);
        }

        [TestMethod]
        public void Constructor_WithSize_ShouldSetEncodingUTF8AndSizeTypeIsFixedAndSetSize()
        {
            var attribute = new StringFormatAttribute(32);

            Assert.AreEqual("UTF-8", attribute.EncodingName);
            Assert.AreEqual(StringSizeType.Fixed, attribute.SizeType);
            Assert.AreEqual(32, attribute.Size);
        }

        [TestMethod]
        public void Constructor_WithPrefixSizeType_ShouldSetEncodingUTF8AndSizeTypeIsPrefix()
        {
            var attribute = new StringFormatAttribute(StringSizeType.Prefix);

            Assert.AreEqual("UTF-8", attribute.EncodingName);
            Assert.AreEqual(StringSizeType.Prefix, attribute.SizeType);
        }

        [TestMethod]
        public void Constructor_WithSignEndSizeType_ShouldSetEncodingUTF8AndSizeTypeIsSignEnd()
        {
            var attribute = new StringFormatAttribute(StringSizeType.SignEnd);

            Assert.AreEqual("UTF-8", attribute.EncodingName);
            Assert.AreEqual(StringSizeType.SignEnd, attribute.SizeType);
        }

        [TestMethod]
        public void Constructor_WithFixedSizeType_ShouldThrowArgumentException()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => new StringFormatAttribute(StringSizeType.Fixed));

            Assert.AreEqual("size", exception.ParamName);
        }

        [TestMethod]
        public void Constructor_WithEncodingName_ShouldSetEncodingNameAndStringSizeTypeIsPrefix()
        {
            var attribute = new StringFormatAttribute("ASCII");

            Assert.AreEqual("ASCII", attribute.EncodingName);
            Assert.AreEqual(StringSizeType.Prefix, attribute.SizeType);
        }

        [TestMethod]
        public void Constructor_WithEncodingNameAndSize_ShouldSetEncodingNameAndSizeAndStringSizeTypeIsFixed()
        {
            var attribute = new StringFormatAttribute("ASCII", StringSizeType.Fixed, 32);

            Assert.AreEqual("ASCII", attribute.EncodingName);
            Assert.AreEqual(StringSizeType.Fixed, attribute.SizeType);
            Assert.AreEqual(32, attribute.Size);
        }
    }
}
