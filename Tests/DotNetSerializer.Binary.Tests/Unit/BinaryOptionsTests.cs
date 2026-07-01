using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetSerializer.Binary.Tests.Unit
{
    [TestClass]
    public class BinaryOptionsTests
    {
        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizeWithDefaultConverters()
        {
            var options = new BinaryOptions();

            Assert.AreEqual(14, options.Converters.Items.Count);
            Assert.IsNotNull(options.Converters.Items[typeof(bool)]);
            Assert.IsNotNull(options.Converters.Items[typeof(byte)]);
            Assert.IsNotNull(options.Converters.Items[typeof(sbyte)]);
            Assert.IsNotNull(options.Converters.Items[typeof(short)]);
            Assert.IsNotNull(options.Converters.Items[typeof(int)]);
            Assert.IsNotNull(options.Converters.Items[typeof(long)]);
            Assert.IsNotNull(options.Converters.Items[typeof(ushort)]);
            Assert.IsNotNull(options.Converters.Items[typeof(uint)]);
            Assert.IsNotNull(options.Converters.Items[typeof(ulong)]);
            Assert.IsNotNull(options.Converters.Items[typeof(float)]);
            Assert.IsNotNull(options.Converters.Items[typeof(double)]);
            Assert.IsNotNull(options.Converters.Items[typeof(decimal)]);
            Assert.IsNotNull(options.Converters.Items[typeof(char)]);
            Assert.IsNotNull(options.Converters.Items[typeof(string)]);
        }
    }
}
