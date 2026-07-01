using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class DoubleConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new DoubleConverter();

            Assert.AreEqual(typeof(double), converter.ConvertedType);
            Assert.AreEqual(typeof(double), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturn12551261()
        {
            var data = new byte[] { 0x00, 0x00, 0x00, 0xA0, 0x8B, 0xF0, 0x67, 0x41 };
            var converter = new DoubleConverter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (double)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<double>(12551261, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            double value = 12551261;
            var converter = new DoubleConverter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0xA0, 0x8B, 0xF0, 0x67, 0x41 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut8()
        {
            var converter = new DoubleConverter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(8, size);
        }
    }
}
