using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class ByteConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new ByteConverter();

            Assert.AreEqual(typeof(byte), converter.ConvertedType);
            Assert.AreEqual(typeof(byte), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturn255()
        {
            var data = new byte[] { 0xFF };
            var converter = new ByteConverter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (byte)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<byte>(255, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            byte value = 255;
            var converter = new ByteConverter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0xFF }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut1()
        {
            var converter = new ByteConverter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(1, size);
        }
    }
}
