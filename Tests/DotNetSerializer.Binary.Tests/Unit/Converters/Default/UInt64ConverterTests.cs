using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class UInt64ConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new UInt64Converter();

            Assert.AreEqual(typeof(ulong), converter.ConvertedType);
            Assert.AreEqual(typeof(ulong), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturn18446744064709551616()
        {
            var data = new byte[] { 0x00, 0xE6, 0x8E, 0xE7, 0xFD, 0xFF, 0xFF, 0xFF };
            var converter = new UInt64Converter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (ulong)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<ulong>(18446744064709551616, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            ulong value = 18446744064709551616;
            var converter = new UInt64Converter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x00, 0xE6, 0x8E, 0xE7, 0xFD, 0xFF, 0xFF, 0xFF }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut8()
        {
            var converter = new UInt64Converter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(8, size);
        }
    }
}
