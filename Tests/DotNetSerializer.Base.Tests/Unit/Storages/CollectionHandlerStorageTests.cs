using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetSerializer.Base.Tests.Unit.Storages
{
    [TestClass]
    public class CollectionHandlerStorageTests
    {
        private class NewCollectionHandler : CollectionHandler
        {
            public override Type CollectionType => GetType();

            public override void AddItem(object collection, object item, params int[] indices)
            {
                throw new NotImplementedException();
            }

            public override object CreateCollection(Type[] elementTypes, int[] sizes)
            {
                throw new NotImplementedException();
            }

            public override object CreateCollectionWithItems(Type[] elementTypes, object[] items)
            {
                throw new NotImplementedException();
            }

            public override object CreateCollectionWithItems(Type[] elementTypes, object items, int[] sizes)
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

            public override int GetRank(Type declareCollectionType)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Add_Generic_WithNewCollectionHandler_ShouldAddNewHandler()
        {
            var storage = new CollectionHandlerStorage();

            storage.Add<NewCollectionHandler>();

            Assert.IsNotNull(storage.Items[typeof(NewCollectionHandler)]);
        }

        [TestMethod]
        public void Add_NonGeneric_WithNewCollectionHandler_ShouldAddNewHandler()
        {
            var storage = new CollectionHandlerStorage();

            storage.Add(new NewCollectionHandler());

            Assert.IsNotNull(storage.Items[typeof(NewCollectionHandler)]);
        }

        [TestMethod]
        public void Add_WhenSameHandlersAddTwice_ShouldThrowArgumentException()
        {
            var storage = new CollectionHandlerStorage();
            storage.Add(new NewCollectionHandler());

            Assert.ThrowsException<ArgumentException>(
                () => storage.Add(new NewCollectionHandler()));
        }

        [TestMethod]
        public void Set_WithNewCollectionHandler_ShouldAddNewHandler()
        {
            var storage = new CollectionHandlerStorage();

            storage.Set(new NewCollectionHandler());

            Assert.IsNotNull(storage.Items[typeof(NewCollectionHandler)]);
        }

        [TestMethod]

        public void Set_WhenSameHandlersSetTwice_ShouldResetCollectionHandler()
        {
            var storage = new CollectionHandlerStorage();
            var first = new NewCollectionHandler();
            var second = new NewCollectionHandler();
            storage.Set(first);

            storage.Set(second);

            Assert.AreSame(second, storage.Items[typeof(NewCollectionHandler)]);
        }

        [TestMethod]
        public void Get_WhenCollectionHandlerExists_ShouldReturnCollectionHandler()
        {
            var storage = new CollectionHandlerStorage();
            var handler = new NewCollectionHandler();
            storage.Add(handler);

            var result = storage.Get(typeof(NewCollectionHandler));

            Assert.IsNotNull(result);
            Assert.AreSame(handler, result);
        }

        [TestMethod]
        public void Get_WhenCollectionHandlerNoExist_ShouldThrowCollectionHandlerNotFoundException()
        {
            var storage = new CollectionHandlerStorage();

            Assert.ThrowsException<CollectionHandlerNotFoundException>(
                () => storage.Get(typeof(NewCollectionHandler)));
        }

        [TestMethod]
        public void Contains_WhenCollectionHandlerExists_ShouldReturnTrue()
        {
            var storage = new CollectionHandlerStorage();
            storage.Add(new NewCollectionHandler());

            var result = storage.Contains(typeof(NewCollectionHandler));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_WhenCollectionHandlerNoExist_ShouldReturnFalse()
        {
            var storage = new CollectionHandlerStorage();

            var result = storage.Contains(typeof(NewCollectionHandler));

            Assert.IsFalse(result);
        }
    }
}
