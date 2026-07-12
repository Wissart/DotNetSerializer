using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DotNetSerializer.Base.Tests.Unit
{
    [TestClass]
    public class SerializerOptionsTests
    {
        public class SerializerOptionsObject : SerializerOptions { }

        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizePropertiesAndRegisterDefaultHandlers()
        {
            var options = new SerializerOptionsObject();

            Assert.IsNotNull(options.CollectionHandlers);
            Assert.AreEqual(2, options.CollectionHandlers.Items.Count);
            Assert.IsNotNull(options.CollectionHandlers.Items[typeof(Array)]);
            Assert.IsNotNull(options.CollectionHandlers.Items[typeof(List<>)]);
        }
    }
}
