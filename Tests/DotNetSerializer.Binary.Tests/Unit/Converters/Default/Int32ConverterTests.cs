using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class Int32ConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new Int32Converter();

            Assert.AreEqual(typeof(int), converter.ConvertedType);
            Assert.AreEqual(typeof(int), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturnNegative410065408()
        {
            var data = new byte[] { 0x00, 0xE6, 0x8E, 0xE7 };
            var converter = new Int32Converter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (int)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<int>(-410065408, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            int value = -410065408;
            var converter = new Int32Converter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x00, 0xE6, 0x8E, 0xE7 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut4()
        {
            var converter = new Int32Converter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(4, size);
        }
    }
}
