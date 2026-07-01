using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class UInt16ConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new UInt16Converter();

            Assert.AreEqual(typeof(ushort), converter.ConvertedType);
            Assert.AreEqual(typeof(ushort), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturn58880()
        {
            var data = new byte[] { 0x00, 0xE6};
            var converter = new UInt16Converter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (ushort)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<ushort>(58880, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            ushort value = 58880;
            var converter = new UInt16Converter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x00, 0xE6 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut2()
        {
            var converter = new UInt16Converter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(2, size);
        }
    }
}
