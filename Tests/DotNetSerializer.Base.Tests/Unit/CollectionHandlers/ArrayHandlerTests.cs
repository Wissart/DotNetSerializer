using DotNetSerializer.Base.CollectionHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetSerializer.Base.Tests.Unit.CollectionHandlers
{
    [TestClass]
    public class ArrayHandlerTests
    {

        [TestMethod]
        public void Initilize_ShouldInitilizePropertyWithTypeArray()
        {
            var handler = new ArrayHandler();

            var result = handler.CollectionType;

            Assert.AreEqual(typeof(Array), result);
        }

        [TestMethod]
        public void CreateCollection_WithTypeInt_Size5_ShouldReturnIntArray1DWith5Elements()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var sizes = new int[] { 5 };

            var result = (int[])handler.CreateCollection(types, sizes);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.GetLength(0));
        }

        [TestMethod]
        public void CreateCollection_WithTypeInt_Size5And2_ShouldReturnIntArray1DWith5Elements()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var sizes = new int[] { 5, 2 };

            var result = (int[,])handler.CreateCollection(types, sizes);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.GetLength(0));
            Assert.AreEqual(2, result.GetLength(1));
        }

        [TestMethod]
        public void CreateCollection_WithTypeInt_Size5And2And3_ShouldReturnIntArray1DWith5Elements()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var sizes = new int[] { 5, 2, 3 };

            var result = (int[,,])handler.CreateCollection(types, sizes);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.GetLength(0));
            Assert.AreEqual(2, result.GetLength(1));
            Assert.AreEqual(3, result.GetLength(2));
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithTypeInt_ItemsIsObjectArray_ShouldReturnIntArray1DWith3Elements()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[] { 1, 3, 5 };
            var sizes = new int[] { 3 };

            var result = (int[])handler.CreateCollectionWithItems(types, items, sizes);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new int[] { 1, 3, 5 }, result);
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithTypeInt_ItemsIsObjectArray_Sizes_5_ShouldReturnIntArray1DWith5Elements()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[] { 1, 2, 3, 4, 5 };
            var sizes = new int[] { 5 };

            var result = (int[])handler.CreateCollectionWithItems(types, items, sizes);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5 }, result);
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithTypeInt_ItemsIsObjectArray_Sizes_2_3_ShouldReturnIntArray2DWith6Elements()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[,] { { 4, 4, 1 }, { 2, 15, 2} };
            var sizes = new int[] { 2, 3 };

            var result = (int[,])handler.CreateCollectionWithItems(types, items, sizes);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new int[,] { { 4, 4, 1 }, { 2, 15, 2 } }, result);
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithTypeInt_ItemsIsObjectArray_Sizes_2_1_2_ShouldReturnIntArray3DWith4Elements()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[,,] { { { 1, 2} }, { { 5, 2} } };
            var sizes = new int[] { 2, 1, 2 };

            var result = (int[,,])handler.CreateCollectionWithItems(types, items, sizes);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new int[,,] { { { 1, 2 } }, { { 5, 2 } } }, result);
        }

        [TestMethod]
        public void CreateCollectionWithItems_WithItems3DAndSizesFor1D_ShouldThrowRankException()
        {
            var handler = new ArrayHandler();
            var types = new Type[] { typeof(int) };
            var items = new object[,,] { { { 1, 2} }, { { 5, 2} } };
            var sizes = new int[] { 2 };

            Assert.ThrowsException<RankException>(
                () => handler.CreateCollectionWithItems(types, items, sizes));
        }

        [TestMethod]
        public void GetRank_WithValidArrayType_ShouldReturnCorrectRank()
        {
            var handler = new ArrayHandler();
            var arrayType = typeof(int[,]);

            var result = handler.GetRank(arrayType);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetElementType_WithValidElementTypes_ShouldReturnFirstElement()
        {
            var handler = new ArrayHandler();
            var elementTypes = new[] { typeof(string), typeof(int) };

            var result = handler.GetElementType(elementTypes);

            Assert.AreEqual(typeof(string), result);
        }

        [TestMethod]
        public void GetCollectionCapacity_WithOneDimensionalArray_ShouldReturnLengthArray()
        {
            var handler = new ArrayHandler();
            var array = new int[5];

            var result = handler.GetCapacity(array);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(5, result[0]);
        }

        [TestMethod]
        public void GetCollectionCapacity_WithTwoDimensionalArray_ShouldReturnLengthArray()
        {
            var handler = new ArrayHandler();
            var array = new int[3, 4];

            var result = handler.GetCapacity(array);

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(3, result[0]);
            Assert.AreEqual(4, result[1]);
        }

        [TestMethod]
        public void GetCollectionCapacity_WithThreeDimensionalArray_ShouldReturnLengthArray()
        {
            var handler = new ArrayHandler();
            var array = new int[2, 3, 4];

            var result = handler.GetCapacity(array);

            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(4, result[2]);
        }

        [TestMethod]
        public void GetItemsCount_WithOneDimensionalArray_ShouldReturnLengths()
        {
            var handler = new ArrayHandler();
            var array = new string[5];

            var result = handler.GetItemsCount(array);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(5, result[0]);
        }

        [TestMethod]
        public void GetItemsCount_WithTwoDimensionalArray_ShouldReturnLengthArray()
        {
            var handler = new ArrayHandler();
            var array = new double[2, 3];

            var result = handler.GetItemsCount(array);

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(3, result[1]);
        }

        [TestMethod]
        public void AddItem_WithInt3DCollection_ItemIs5_ShouldSet5ByIndex()
        {
            var handler = new ArrayHandler();
            var collection = new int[,,] { { { 1, 2 }, { 3, 4 } }, { { -1, 6 }, { 7, 8 } } };
            var item = 5;

            handler.AddItem(collection, item, 1, 0, 0);

            Assert.AreEqual(5, collection[1,0,0]);
        }
    }
}
