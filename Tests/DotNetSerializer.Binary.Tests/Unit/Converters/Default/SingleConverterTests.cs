using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class SingleConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new SingleConverter();

            Assert.AreEqual(typeof(float), converter.ConvertedType);
            Assert.AreEqual(typeof(float), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturn12()
        {
            var data = new byte[] { 0x00, 0x00, 0x40, 0x41 };
            var converter = new SingleConverter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (float)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<float>(12.0f, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            float value = 12.0f;
            var converter = new SingleConverter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x00, 0x00, 0x40, 0x41 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut4()
        {
            var converter = new SingleConverter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(4, size);
        }
    }
}
