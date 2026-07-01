using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class UInt32ConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new UInt32Converter();

            Assert.AreEqual(typeof(uint), converter.ConvertedType);
            Assert.AreEqual(typeof(uint), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturn256661235()
        {
            var data = new byte[] { 0xF3, 0x56, 0x4C, 0x0F };
            var converter = new UInt32Converter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (uint)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<uint>(256661235, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            uint value = 256661235;
            var converter = new UInt32Converter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0xF3, 0x56, 0x4C, 0x0F }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut4()
        {
            var converter = new UInt32Converter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(4, size);
        }
    }
}
