using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DotNetSerializer.Base.Tests.Unit.Utilities
{
    [TestClass]
    public class CollectionUtilitiesTests
    {
        [TestMethod]
        public void TryGetElementTypes_WhenIntArray_ShouldReturnTrueAndOutCorrectArray()
        {
            var type = typeof(int[]);

            var result = CollectionUtilities.TryGetElementTypes(type, out Type[] elementTypes);

            Assert.IsTrue(result);
            Assert.IsNotNull(elementTypes);
            Assert.AreEqual(1, elementTypes.Length);
            Assert.AreEqual(typeof(int), elementTypes[0]);
        }

        [TestMethod]
        public void TryGetElementTypes_WhenIntList_ShouldReturnTrueAndOutCorrectArray()
        {
            var type = typeof(List<int>);

            var result = CollectionUtilities.TryGetElementTypes(type, out Type[] elementTypes);

            Assert.IsTrue(result);
            Assert.IsNotNull(elementTypes);
            Assert.AreEqual(1, elementTypes.Length);
            Assert.AreEqual(typeof(int), elementTypes[0]);
        }

        [TestMethod]
        public void TryGetElementTypes_WhenDictionaryStringInt_ShouldReturnTrueAndOutCorrectArray()
        {
            var type = typeof(Dictionary<string, int>);

            var result = CollectionUtilities.TryGetElementTypes(type, out Type[] elementTypes);

            Assert.IsTrue(result);
            Assert.IsNotNull(elementTypes);
            Assert.AreEqual(2, elementTypes.Length);
            Assert.AreEqual(typeof(string), elementTypes[0]);
            Assert.AreEqual(typeof(int), elementTypes[1]);
        }

        [TestMethod]
        public void TryGetElementTypes_Whenstring_ShouldReturnFalseAndOutNull()
        {
            var type = typeof(string);

            var result = CollectionUtilities.TryGetElementTypes(type, out Type[] elementTypes);

            Assert.IsFalse(result);
            Assert.IsNull(elementTypes);
        }

        [TestMethod]
        public void TryGetElementTypes_WhenNonCollectionType_ShouldReturnFalseAndOutNull()
        {
            var type = typeof(int);

            var result = CollectionUtilities.TryGetElementTypes(type, out Type[] elementTypes);

            Assert.IsFalse(result);
            Assert.IsNull(elementTypes);
        }

        [TestMethod]
        public void GetElementTypes_WhenIntArray_ShouldReturnCorrectArray()
        {
            var type = typeof(int[]);

            var elementTypes = CollectionUtilities.GetElementTypes(type);

            Assert.IsNotNull(elementTypes);
            Assert.AreEqual(1, elementTypes.Length);
            Assert.AreEqual(typeof(int), elementTypes[0]);
        }

        [TestMethod]
        public void GetElementTypes_WhenIntList_ShouldReturnCorrectArray()
        {
            var type = typeof(List<int>);

            var elementTypes = CollectionUtilities.GetElementTypes(type);

            Assert.IsNotNull(elementTypes);
            Assert.AreEqual(1, elementTypes.Length);
            Assert.AreEqual(typeof(int), elementTypes[0]);
        }

        [TestMethod]
        public void GetElementTypes_WhenDictionaryStringInt_ShouldReturnCorrectArray()
        {
            var type = typeof(Dictionary<string, int>);

            var elementTypes = CollectionUtilities.GetElementTypes(type);

            Assert.IsNotNull(elementTypes);
            Assert.AreEqual(2, elementTypes.Length);
            Assert.AreEqual(typeof(string), elementTypes[0]);
            Assert.AreEqual(typeof(int), elementTypes[1]);
        }

        [TestMethod]
        public void GetElementTypes_WhenString_ShouldReturnNull()
        {
            var type = typeof(string);

            var elementTypes = CollectionUtilities.GetElementTypes(type);

            Assert.IsNull(elementTypes);
        }

        [TestMethod]
        public void GetElementTypes_WhenNonCollectionType_ShouldReturnNull()
        {
            var type = typeof(int);

            var elementTypes = CollectionUtilities.GetElementTypes(type);

            Assert.IsNull(elementTypes);
        }

        [TestMethod]
        public void GetCollectionType_WhenArray_ShouldReturnTypeArray()
        {
            var type = typeof(int[]);

            var collectionType = CollectionUtilities.GetCollectionType(type);

            Assert.AreEqual(typeof(Array), collectionType);
        }

        [TestMethod]
        public void GetCollectionType_WhenList_ShouldReturnTypeList()
        {
            var type = typeof(List<int>);

            var collectionType = CollectionUtilities.GetCollectionType(type);

            Assert.AreEqual(typeof(List<>), collectionType);
        }

        [TestMethod]
        public void GetCollectionType_WhenDictionary_ShouldReturnTypeDictionary()
        {
            var type = typeof(Dictionary<string, int>);

            var collectionType = CollectionUtilities.GetCollectionType(type);

            Assert.AreEqual(typeof(Dictionary<,>), collectionType);
        }

        [TestMethod]
        public void GetCollectionType_WhenNonCollectionType_ShouldThrowArgumentException()
        {
            var type = typeof(int);

            var exception = Assert.ThrowsException<ArgumentException>(
                () => CollectionUtilities.GetCollectionType(type));
        }
    }
}
