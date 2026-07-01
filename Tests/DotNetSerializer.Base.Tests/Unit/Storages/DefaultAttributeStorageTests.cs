using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetSerializer.Base.Tests.Unit.Storages
{
    [TestClass]
    public class DefaultAttributeStorageTests
    {
        private class CustomStringFormatAttribute : StringFormatAttribute
        {
            public CustomStringFormatAttribute() : base("ASCII", StringSizeType.Fixed, 32) { }
        }

        [TestMethod]
        public void Get_Generic_StringFormatAttribute_ShouldReturnStringFormatAttributeWithDefaultProperties()
        {
            var storage = new DefaultAttributeStorage();

            var result = storage.Get<StringFormatAttribute>();

            Assert.IsNotNull(result);
            Assert.AreEqual("UTF-8", result.EncodingName);
            Assert.AreEqual(StringSizeType.Prefix, result.SizeType);
            Assert.AreEqual(0, result.Size);
        }

        [TestMethod]
        public void Get_NonGeneric_StringFormatAttribute_ShouldReturnStringFormatAttributeWithDefaultProperties()
        {
            var storage = new DefaultAttributeStorage();

            var result = (StringFormatAttribute)storage.Get(typeof(StringFormatAttribute));

            Assert.IsNotNull(result);
            Assert.AreEqual("UTF-8", result.EncodingName);
            Assert.AreEqual(StringSizeType.Prefix, result.SizeType);
            Assert.AreEqual(0, result.Size);
        }

        [TestMethod]
        public void Get_NotSupportAttribute_ShouldThrowDotNetSerializerException()
        {
            var storage = new DefaultAttributeStorage();

            Assert.ThrowsException<DotNetSerializerException>(
                () => storage.Get(typeof(ConverterAttribute)));
        }

        [TestMethod]
        public void Set_Generic_CustomStringFormatAttribute_ShouldSetLikeStringFormatAttribute()
        {
            var storage = new DefaultAttributeStorage();

            storage.Set<CustomStringFormatAttribute>();
            var attribute = storage.Get<StringFormatAttribute>();


            Assert.IsNotNull(attribute);
            Assert.AreEqual("ASCII", attribute.EncodingName);
            Assert.AreEqual(StringSizeType.Fixed, attribute.SizeType);
            Assert.AreEqual(32, attribute.Size);
        }

        [TestMethod]
        public void Set_Generic_NotSupportAttribute_ShouldThrowDotNetSerializerException()
        {
            var storage = new DefaultAttributeStorage();

            Assert.ThrowsException<DotNetSerializerException>(
                () => storage.Set<ConverterAttribute>(typeof(int)));
        }
    }
}
