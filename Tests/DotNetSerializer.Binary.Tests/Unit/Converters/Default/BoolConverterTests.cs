using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class BoolConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new BoolConverter();

            Assert.AreEqual(typeof(bool), converter.ConvertedType);
            Assert.AreEqual(typeof(bool), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturnTrue()
        {
            var data = new byte[] { 0x01 };
            var converter = new BoolConverter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (bool)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            var value = true;
            var converter = new BoolConverter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x01 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut1()
        {
            var converter = new BoolConverter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(1, size);
        }
    }
}
