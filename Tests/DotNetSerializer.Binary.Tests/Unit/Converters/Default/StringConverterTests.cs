using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Storages;
using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class StringConverterTests
    {
        private class TestStringsClass
        {
            [StringFormat(16)]
            public string FixedString { get; set; }
            [StringFormat(StringSizeType.Prefix)]
            public string PrefixString { get; set; }
            [StringFormat(StringSizeType.SignEnd)]
            public string SignString { get; set; }
            public string DefaultString { get; set; }
        }

        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);

            Assert.AreEqual(typeof(string), converter.ConvertedType);
            Assert.AreEqual(typeof(string), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStreamAndSizeTypeIsFixed_ShouldReturnCorrectString()
        {
            var data = new byte[] { 0x46, 0x69, 0x78, 0x65, 0x64, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x00, 0x00, 0x00, 0x00 };
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);
            var serializationData = new BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.FixedString));

            var result = (string)converter.Read(reader, serializationData);
            reader.Close();
            stream.Close();

            Assert.AreEqual<string>("Fixed string", result);
        }

        [TestMethod]
        public void Write_WithMemoryStreamAndSizeTypeIsFixed_ShouldCorrectWriteString()
        {
            string value = "Fixed string";
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.FixedString));

            converter.WriteValue(writer, value, serializationData);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x46, 0x69, 0x78, 0x65, 0x64, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x00, 0x00, 0x00, 0x00 }, data);
        }

        [TestMethod]
        public void Read_WithMemoryStreamAndSizeTypeIsPrefix_ShouldReturnCorrectString()
        {
            var data = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x50, 0x72, 0x65, 0x66, 0x69, 0x78, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67 };
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.PrefixString));

            var result = (string)converter.Read(reader, serializationData);
            reader.Close();
            stream.Close();

            Assert.AreEqual<string>("Prefix string", result);
        }

        [TestMethod]
        public void Write_WithMemoryStreamAndSizeTypeIsPrefix_ShouldCorrectWriteString()
        {
            string value = "Prefix string";
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.PrefixString));

            converter.WriteValue(writer, value, serializationData);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x50, 0x72, 0x65, 0x66, 0x69, 0x78, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67 }, data);
        }

        [TestMethod]
        public void Read_WithMemoryStreamAndSizeTypeIsSign_ShouldReturnCorrectString()
        {
            var data = new byte[] { 0x53, 0x69, 0x67, 0x6E, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x00 };
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.SignString));

            var result = (string)converter.Read(reader, serializationData);
            reader.Close();
            stream.Close();

            Assert.AreEqual<string>("Sign string", result);
        }

        [TestMethod]
        public void Write_WithMemoryStreamAndSizeTypeIsSign_ShouldCorrectWriteString()
        {
            string value = "Sign string";
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.SignString));

            converter.WriteValue(writer, value, serializationData);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x53, 0x69, 0x67, 0x6E, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x00 }, data);
        }

        [TestMethod]
        public void Read_WithMemoryStreamAndSizeTypeSetByDefault_ShouldReturnCorrectString()
        {
            var data = new byte[] { 0x0E, 0x00, 0x00, 0x00, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6C, 0x74, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67 };
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.PrefixString));

            var result = (string)converter.Read(reader, serializationData);
            reader.Close();
            stream.Close();

            Assert.AreEqual<string>("Default string", result);
        }

        [TestMethod]
        public void Write_WithMemoryStreamAndSizeTypeSetByDefault_ShouldCorrectWriteString()
        {
            string value = "Default string";
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.PrefixString));

            converter.WriteValue(writer, value, serializationData);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x0E, 0x00, 0x00, 0x00, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6C, 0x74, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithFixedProperty_ShouldReturnTrueAndOut16()
        {
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.FixedString));

            var result = converter.TryGetSize(serializationData, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(16, size);
        }

        [TestMethod]
        public void TryGetSize_WithPrefixProperty_ShouldReturnFalseAndOutNegative1()
        {
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.PrefixString));

            var result = converter.TryGetSize(serializationData, out int size);

            Assert.IsFalse(result);
            Assert.AreEqual(-1, size);
        }
        
        [TestMethod]
        public void TryGetSize_WithSignProperty_ShouldReturnFalseAndOutNegative1()
        {
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.SignString));

            var result = converter.TryGetSize(serializationData, out int size);

            Assert.IsFalse(result);
            Assert.AreEqual(-1, size);
        }
        
        [TestMethod]
        public void TryGetSize_WithDefaultProperty_ShouldReturnFalseAndOutNegative1()
        {
            var defaultAttributes = new DefaultAttributeStorage();
            var converter = new StringConverter(defaultAttributes);
            var serializationData = new Binary.BinaryContext(null, null);
            serializationData.ObjectContext.Property = typeof(TestStringsClass).GetProperty(nameof(TestStringsClass.DefaultString));

            var result = converter.TryGetSize(serializationData, out int size);

            Assert.IsFalse(result);
            Assert.AreEqual(-1, size);
        }
    }
}
