using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DotNetSerializer.Base.Tests.Unit.CollectionHandlers
{
    [TestClass]
    public class ListHandlerTests
    {
        [TestMethod]
        public void Initilize_ShouldInitilizePropertyWithTypeList()
        {
            var handler = new ListHandler();

            var result = handler.CollectionType;

            Assert.AreEqual(typeof(List<>), result);
        }

        [TestMethod]
        public void CreateCollection_WithIntType_Size5_ShouldReturnListWithCapacity5()
        {
            var handler = new ListHandler();
            var types = new Type[] { typeof(int) };
            var sizes = new int[] { 5 };

            var result = (List<int>)handler.CreateCollection(types, sizes);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Capacity);
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithTypeInt_ItemsIsObjectArray_ShouldReturnIntListWith3Elements()
        {
            var handler = new ListHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[] { 1, 3, 5 };
            var sizes = new int[] { 3 };

            var result = (List<int>)handler.CreateCollectionWithItems(types, items, sizes);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new int[] { 1, 3, 5 }, result.ToArray());
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithTypeInt_ItemsIsObjectArray_Sizes_5_ShouldReturnIntListWith5Elements()
        {
            var handler = new ListHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[] { 1, 2, 3, 4, 5 };
            var sizes = new int[] { 5 };

            var result = (List<int>)handler.CreateCollectionWithItems(types, items, sizes);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5 }, result.ToArray());
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithSizesForRank2_ShouldThrowDotNetSerializerException()
        {
            var handler = new ListHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[] { 1, 2, 3, 4, 5 };
            var sizes = new int[] { 2, 3 };

            Assert.ThrowsException<DotNetSerializerException>(
                () => handler.CreateCollectionWithItems(types, items, sizes));
        }

        [TestMethod]
        public void GetRank_WithValidTypedList_ShouldReturn1()
        {
            var handler = new ListHandler();
            var listType = typeof(List<int>);

            var result = handler.GetRank(listType);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetRank_WithNullCollectionType_ShouldReturn1()
        {
            var handler = new ListHandler();

            var result = handler.GetRank(null);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetElementType_WithValidElementTypes_ShouldReturnFirstElement()
        {
            var handler = new ListHandler();
            var elementTypes = new[] { typeof(string), typeof(int) };

            var result = handler.GetElementType(elementTypes);

            Assert.AreEqual(typeof(string), result);
        }

        [TestMethod]
        public void GetCollectionCapacity_WithValidTypedList_ShouldReturnCapacity()
        {
            var handler = new ListHandler();
            var list = new List<int>(5);

            var result = handler.GetCapacity(list);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(5, result[0]);
        }

        [TestMethod]
        public void GetItemsCount_WithValidTypedList_ShouldReturnLengthArray()
        {
            var handler = new ListHandler();
            var list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var result = handler.GetItemsCount(list);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(10, result[0]);
        }

        [TestMethod]
        public void AddItem_WithIntList_ItemIs5_ShouldSet5ByIndex()
        {
            var handler = new ListHandler();
            var list = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            handler.AddItem(list, 5, 5);

            Assert.AreEqual(5, list[5]);
        }
    }
}
