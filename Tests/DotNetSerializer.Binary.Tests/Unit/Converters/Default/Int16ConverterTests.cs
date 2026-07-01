using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class Int16ConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new Int16Converter();

            Assert.AreEqual(typeof(short), converter.ConvertedType);
            Assert.AreEqual(typeof(short), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturnNegative6656()
        {
            var data = new byte[] { 0x00, 0xE6 };
            var converter = new Int16Converter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (short)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<short>(-6656, result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            short value = -6656;
            var converter = new Int16Converter();
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
            var converter = new Int16Converter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(2, size);
        }
    }
}
