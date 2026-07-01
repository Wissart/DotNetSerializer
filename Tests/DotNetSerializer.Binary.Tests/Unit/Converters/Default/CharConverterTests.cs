using DotNetSerializer.Binary.Converters.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit.Converters.Default
{
    [TestClass]
    public class CharConverterTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeProperties()
        {
            var converter = new CharConverter();

            Assert.AreEqual(typeof(char), converter.ConvertedType);
            Assert.AreEqual(typeof(char), converter.RegisteredType);
        }

        [TestMethod]
        public void Read_WithMemoryStream_ShouldReturnA()
        {
            var data = new byte[] { 0x41 };
            var converter = new CharConverter();
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            var result = (char)converter.Read(reader, null);
            reader.Close();
            stream.Close();

            Assert.AreEqual<char>('A', result);
        }

        [TestMethod]
        public void Write_WithMemoryStream_ShouldCorrectWriteValue()
        {
            char value = 'A';
            var converter = new CharConverter();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            converter.WriteValue(writer, value, null);
            var data = stream.ToArray();
            writer.Close();
            stream.Close();

            CollectionAssert.AreEqual(new byte[] { 0x41 }, data);
        }

        [TestMethod]
        public void TryGetSize_WithNull_ShouldReturnTrueAndOut1()
        {
            var converter = new CharConverter();

            var result = converter.TryGetSize(null, out int size);

            Assert.IsTrue(result);
            Assert.AreEqual(1, size);
        }
    }
}
