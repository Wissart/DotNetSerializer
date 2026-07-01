using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class SByteConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new SByteConverter();

            Assert.AreEqual(typeof(sbyte), converter.ConvertedType);
            Assert.AreEqual(typeof(sbyte), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturnNegative74()
        {
            var data = new byte[] { 0xB6 };
            var converter = new SByteConverter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (sbyte)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<sbyte>(-74, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            sbyte value = -74;
            var converter = new SByteConverter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0xB6 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut1()
        {
            var converter = new SByteConverter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(1, size);
        }
    }
}
