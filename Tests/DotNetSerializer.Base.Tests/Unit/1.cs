using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetSerializer.Base.Tests.Unit
{
    [TestClass]
    public class SerializerOptionsTests
    {
        public class SerializerOptionsObject : SerializerOptions { }
        private class CollectionHandlerStub : CollectionHandler
        {
            public override Type CollectionType => typeof(CollectionHandlerStub);

            public override void AddItem(object collection, object item, params int[] indices)
            {
                throw new NotImplementedException();
            }

            public override object CreateCollection(Type[] elementTypes, int[] sizes)
            {
                throw new NotImplementedException();
            }

            public override int[] GetCapacity(object collection)
            {
                throw new NotImplementedException();
            }

            public override Type GetElementType(Type[] elementTypes)
            {
                throw new NotImplementedException();
            }

            public override int[] GetItemsCount(object collection)
            {
                throw new NotImplementedException();
            }

            public override int GetRank(Type collectionType)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Constructor_Initilize_ShouldInitilizePropertiesAndRegisterDefaultHandlers()
        {
            var options = new SerializerOptionsObject();

            Assert.IsNotNull(options.TypeInfoManager);
            Assert.IsNotNull(options.CollectionHandlers);
            Assert.AreEqual(2, options.CollectionHandlers.Count);
            Assert.IsNotNull(options.CollectionHandlers[typeof(Array)]);
            Assert.IsNotNull(options.CollectionHandlers[typeof(List<>)]);
        }

        [TestMethod]
        public void AddCollectionHandler_Generic_CollectionHandlerStub_ShouldRegisterNewHandler()
        {
            var options = new SerializerOptionsObject();

            options.AddCollectionHandler<CollectionHandlerStub>();

            Assert.AreEqual(3, options.CollectionHandlers.Count);
            Assert.IsNotNull(options.CollectionHandlers[typeof(CollectionHandlerStub)]);
        }

        [TestMethod]
        public void AddCollectionHandler_NonGeneric_CollectionHandlerStub_ShouldRegisterNewHandler()
        {
            var options = new SerializerOptionsObject();
            var handler = new CollectionHandlerStub();

            options.AddCollectionHandler(handler);

            Assert.AreEqual(3, options.CollectionHandlers.Count);
            Assert.IsNotNull(options.CollectionHandlers[typeof(CollectionHandlerStub)]);
        }
    }
}
