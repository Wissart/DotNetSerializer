using DotNetSerializer.Base.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetSerializer.Base.Tests.Unit
{
    [TestClass]
    public class SerializationContextTests
    {
        public class TestSerializationContext : SerializationContext<TestSerializationContext>
        {
            public TestSerializationContext(TestSerializationContext context) : base(context) { }
        }

        private class TestObject
        {
            public int TestProperty { get; set; }
        }

        [TestMethod]
        public void Constructor_WithNull_ShouldInitilizeProperties()
        {
            var context = new TestSerializationContext(null);

            Assert.AreEqual<uint>(0, context.Version);
            Assert.IsNull(context.MetaData);
            Assert.IsNotNull(context.TransientValues);
        }

        [TestMethod]
        public void Constructor_Withcontext_ShouldCorrectInitilizeProperties()
        {
            var obj = new TestObject();
            var property = typeof(TestObject).GetProperty(nameof(TestObject.TestProperty));
            var context = new TestSerializationContext(null)
            {
                Version = 12,
                MetaData = new SerialiationMetaData(null)
                {
                    Object = obj,
                    Property = property,
                },
            };
            var result = new TestSerializationContext(context);

            Assert.AreEqual<uint>(0, result.Version);
            Assert.IsNotNull(result.MetaData);
            Assert.AreSame(context, result.Prev);
            Assert.AreSame(context.MetaData, result.MetaData);
            Assert.AreSame(context.TransientValues, result.TransientValues);
        }

        [TestMethod]
        public void AddTransientValue_WhenNameIsTransientAndValueTrue_ShouldAddNameValue()
        {
            var context = new TestSerializationContext(null);

            context.AddTransientValue("Transient", true);

            Assert.AreEqual(true, context.TransientValues["Transient"]);
        }

        [TestMethod]
        public void AddTransientValue_WhenValueIsExist_ShouldThrowArgumentException()
        {
            var context = new TestSerializationContext(null);

            context.AddTransientValue("Transient", true);

            Assert.ThrowsException<ArgumentException>(
                () => context.AddTransientValue("Transient", false));
        }

        [TestMethod]
        public void SetTransientValue_WhenValueWasSet_ShouldChangeValue()
        {
            var context = new TestSerializationContext(null);

            context.SetTransientValue("Version", 123);
            context.SetTransientValue("Version", 156);

            Assert.AreEqual(156, context.TransientValues["Version"]);
        }

        [TestMethod]
        public void GetTransientValue_WhenValueExists_ShouldReturnValue()
        {
            var context = new TestSerializationContext(null);
            context.SetTransientValue("Version", 176);

            var result = context.GetTransientValue<int>("Version");

            Assert.AreEqual(176, result);
        }

        [TestMethod]
        public void GetTransientValue_WhenValueNotExist_ShouldThrowTransientValueNotFoundException()
        {
            var context = new TestSerializationContext(null)
            {
                Version = 12,
                MetaData = new SerialiationMetaData(null)
                {
                    Object = new TestObject(),
                    Property = typeof(TestObject).GetProperty(nameof(TestObject.TestProperty)),
                },
            };

            var exception = Assert.ThrowsException<TransientValueNotFoundException>(
                () => context.GetTransientValue<int>("Version"));

            Assert.AreEqual("Version", exception.ValueName);
        }
    }
}
