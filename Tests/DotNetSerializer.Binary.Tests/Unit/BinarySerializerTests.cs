using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit
{
    [TestClass]
    public class BinarySerializerTests
    {
        #region Test Data Setup

        private static IEnumerable<object[]> GetTestOptions()
        {
            yield return new object[] { ProcessType.Default, CachedProcessType.Single, CachingTargets.All };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Single, CachingTargets.Collections };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Single, CachingTargets.All };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.Collections };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All };
        }

        private static IEnumerable<object[]> GetBasicTestOptions()
        {
            yield return new object[] { ProcessType.Default, CachedProcessType.Single, CachingTargets.All };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Single, CachingTargets.All };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All };
        }

        private static IEnumerable<object[]> GetCollectionTestOptions()
        {
            yield return new object[] { ProcessType.Default, CachedProcessType.Single, CachingTargets.All };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Single, CachingTargets.Collections };
            yield return new object[] { ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.Collections };
        }

        private BinarySerializer CreateSerializer(ProcessType processType, CachedProcessType cachedType,
            CachingTargets targets, params BinaryConverter[] converterTypes)
        {
            var options = new BinaryOptions
            {
                ProcessType = processType
            };
            options.CachedProcessSettings.ProcessType = cachedType;
            options.CachedProcessSettings.CachingTargets = targets;

            foreach (var converterType in converterTypes)
            {
                options.Converters.Add(converterType);
            }

            return new BinarySerializer(options);
        }

        private void AssertTestClass(TestClass obj)
        {
            Assert.IsNotNull(obj);
            Assert.AreEqual(12, obj.ID);
            AssertPrimitiveContainer(obj.Primitives);
            AssertStringContainer(obj.Strings);
            AssertCollectionContainer(obj.Collections);
        }

        private void AssertPrimitiveContainer(PrimitiveContainer obj)
        {
            Assert.AreEqual('a', obj.CharProperty);
            Assert.AreEqual((byte)222, obj.ByteProperty);
            Assert.AreEqual((sbyte)(-51), obj.SByteProperty);
            Assert.IsTrue(obj.BoolProperty);
            Assert.AreEqual((short)(-24142), obj.ShortProperty);
            Assert.AreEqual((ushort)43835, obj.UShortProperty);
            Assert.AreEqual(-128123, obj.IntProperty);
            Assert.AreEqual(1003793211U, obj.UintProperty);
            Assert.AreEqual(-155512321512355123L, obj.LongProperty);
            Assert.AreEqual(172129014483561275UL, obj.ULongProperty);
            Assert.AreEqual(2.551f, obj.FloatProperty, 0.001f);
            Assert.AreEqual(1.512331, obj.DoubleProperty, 0.000001);
            Assert.AreEqual(79228162514264337593543950335M, obj.DecimalProperty);
        }

        private void AssertStringContainer(StringContainer obj)
        {
            Assert.AreEqual("Fixed string", obj.FixedString.FixedString);
            Assert.AreEqual("Prefix string", obj.PrefixString.PrefixString);
            Assert.AreEqual("Sign string", obj.SignEndString.SignEndString);
        }

        private void AssertCollectionContainer(CollectionContainer obj)
        {
            CollectionAssert.AreEqual(new byte[] { 0xEB, 0xFF, 0x1C, 0x7A, 0xFE, 0xD8 }, obj.Fixed1DCollection.Fixed1DCollection);
            CollectionAssert.AreEqual(new byte[2, 3] { { 0xBB, 0xCD, 0x00 }, { 0xFE, 0x10, 0x02 } }, obj.Fixed2DCollection.Fixed2DCollection);
            CollectionAssert.AreEqual(new byte[1, 2, 3] { { { 0x03, 0x04, 0x03 }, { 0x63, 0x77, 0x2C } } }, obj.Fixed3DCollection.Fixed3DCollection);

            CollectionAssert.AreEqual(new List<byte> { 0x01, 0x02, 0x03 }, obj.Prefix1DCollection.Prefix1DCollection);
            CollectionAssert.AreEqual(new byte[3, 2] { { 0x00, 0xC6 }, { 0x1C, 0x0D }, { 0x9A, 0x11 } }, obj.Prefix2DCollection.Prefix2DCollection);
            CollectionAssert.AreEqual(new byte[3, 2, 1] { { { 0xDD }, { 0xC4 } }, { { 0x43 }, { 0x8E } }, { { 0xB1 }, { 0xC6 } } }, obj.Prefix3DCollection.Prefix3DCollection);

            Assert.AreEqual(2, obj.Nested1DCollection.Nested1DCollection.Length);
            CollectionAssert.AreEqual(new byte[] { 12, 125, 55 }, obj.Nested1DCollection.Nested1DCollection[0]);
            CollectionAssert.AreEqual(new byte[] { 112, 11 }, obj.Nested1DCollection.Nested1DCollection[1]);
            Assert.AreEqual(2, obj.Nested2DCollection.Nested2DCollection.Length);
            CollectionAssert.AreEqual(new byte[3, 2] { { 2, 25 }, { 171, 192 }, { 11, 123 } }, obj.Nested2DCollection.Nested2DCollection[0]);
            CollectionAssert.AreEqual(new byte[3, 1] { { 1 }, { 121 }, { 12 } }, obj.Nested2DCollection.Nested2DCollection[1]);
            Assert.AreEqual(2, obj.Nested3DCollection.Nested3DCollection.Length);
            CollectionAssert.AreEqual(new byte[2, 2, 2] { { { 2, 25 }, { 171, 192 } }, { { 4, 152 }, { 11, 250 } } }, obj.Nested3DCollection.Nested3DCollection[0]);
            CollectionAssert.AreEqual(new byte[2, 1, 2] { { { 4, 51 } }, { { 12, 42 } } }, obj.Nested3DCollection.Nested3DCollection[1]);
        }
        #endregion

        #region Class Tests

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Class_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(TestClass.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestClass>(data);

            // Assert
            AssertTestClass(obj);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Class_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestClass.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestClass.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region Class With Converter Tests

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_ClassWithConverter_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets,
                new PrimitiveContainerInAttributeConverter(),
                new PrimitiveContainerFactoryConverter());
            var data = new MemoryStream(TestClassWithConverter.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestClassWithConverter>(data);

            // Assert
            Assert.IsNotNull(obj);
            AssertPrimitiveContainer(obj.Primitives1);
            AssertPrimitiveContainer(obj.Primitives2);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_ClassWithConverter_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets,
                new PrimitiveContainerInAttributeConverter(),
                new PrimitiveContainerFactoryConverter());
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestClassWithConverter.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestClassWithConverter.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_ClassWithConverter_WhenConvertersMissing_ShouldThrowConverterNotFoundException(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(TestClassWithConverter.GetDefaultBinaryData());

            // Act & Assert
            Assert.ThrowsException<ConverterNotFoundException>(
                () => serializer.Deserialize<TestClassWithConverter>(data));
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_ClassWithConverter_WhenConvertersMissing_ShouldThrowConverterNotFoundException(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act & Assert
            Assert.ThrowsException<ConverterNotFoundException>(
                () => serializer.Serialize(data, TestClassWithConverter.GetDefault()));
        }

        #endregion

        #region Versionable Tests

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Versionable_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(TestVersionable.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestVersionable>(data);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(101, obj.ID);
            Assert.AreEqual(10U, obj.Version);

            AssertStringContainer(obj.SubVersionable.SupportedByVersion);
            Assert.IsNull(obj.SubVersionable.NotSupportedByVersion);
            AssertPrimitiveContainer(obj.SubVersionable.NotMarkedProperty);

            AssertCollectionContainer(obj.SubClass.SupportedByVersion);
            Assert.IsNull(obj.SubClass.NotSupportedByVersion);
            AssertStringContainer(obj.SubClass.NotMarkedProperty);

            AssertPrimitiveContainer(obj.SupportedByVersion);
            Assert.IsNull(obj.NotSupportedByVersion);
            AssertCollectionContainer(obj.NotMarkedProperty);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Versionable_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestVersionable.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestVersionable.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region Versionable With Converter Tests

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_VersionableWithConverter_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets,
                new SubVersionableInAttributeConverter(),
                new SubVersionableFactoryConverter());
            var data = new MemoryStream(TestVersionableWithConverter.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestVersionableWithConverter>(data);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(101, obj.ID);
            Assert.AreEqual(10U, obj.Version);

            AssertStringContainer(obj.SubVersionable1.SupportedByVersion);
            Assert.IsNull(obj.SubVersionable1.NotSupportedByVersion);
            AssertPrimitiveContainer(obj.SubVersionable1.NotMarkedProperty);

            AssertStringContainer(obj.SubVersionable2.SupportedByVersion);
            Assert.IsNull(obj.SubVersionable2.NotSupportedByVersion);
            AssertPrimitiveContainer(obj.SubVersionable2.NotMarkedProperty);

            AssertCollectionContainer(obj.SubClass.SupportedByVersion);
            Assert.IsNull(obj.SubClass.NotSupportedByVersion);
            AssertStringContainer(obj.SubClass.NotMarkedProperty);

            AssertPrimitiveContainer(obj.SupportedByVersion);
            Assert.IsNull(obj.NotSupportedByVersion);
            AssertCollectionContainer(obj.NotMarkedProperty);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_VersionableWithConverter_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets,
                new SubVersionableInAttributeConverter(),
                new SubVersionableFactoryConverter());
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestVersionableWithConverter.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestVersionableWithConverter.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_VersionableWithConverter_WhenConvertersMissing_ShouldThrowConverterNotFoundException(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(TestVersionableWithConverter.GetDefaultBinaryData());

            // Act & Assert
            Assert.ThrowsException<ConverterNotFoundException>(
                () => serializer.Deserialize<TestVersionableWithConverter>(data));
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_VersionableWithConverter_WhenConvertersMissing_ShouldThrowConverterNotFoundException(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act & Assert
            Assert.ThrowsException<ConverterNotFoundException>(
                () => serializer.Serialize(data, TestVersionableWithConverter.GetDefault()));
        }

        #endregion

        #region Primitive Types Tests

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Primitives_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(PrimitiveContainer.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<PrimitiveContainer>(data);

            // Assert
            AssertPrimitiveContainer(obj);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Primitives_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, PrimitiveContainer.GetDefault());

            // Assert
            CollectionAssert.AreEqual(PrimitiveContainer.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region String Types Tests

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_FixedString_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(FixedStringWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<FixedStringWrapper>(data);

            // Assert
            Assert.AreEqual("Fixed string", obj.FixedString);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_FixedString_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, FixedStringWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(FixedStringWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_PrefixString_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(PrefixStringWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<PrefixStringWrapper>(data);

            // Assert
            Assert.AreEqual("Prefix string", obj.PrefixString);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_PrefixString_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, PrefixStringWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(PrefixStringWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_SignEndString_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(SignEndStringWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<SignEndStringWrapper>(data);

            // Assert
            Assert.AreEqual("Sign string", obj.SignEndString);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_SignEndString_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, SignEndStringWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(SignEndStringWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region Primitive Value Tests

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Char_ShouldReturnValue(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0x61 });

            // Act
            var value = serializer.Deserialize<char>(data);

            // Assert
            Assert.AreEqual('a', value);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Char_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, 'a');

            // Assert
            CollectionAssert.AreEqual(new byte[] { 0x61 }, data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Byte_ShouldReturnValue(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0xDE });

            // Act
            var value = serializer.Deserialize<byte>(data);

            // Assert
            Assert.AreEqual((byte)222, value);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Int_ShouldReturnValue(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0x85, 0x0B, 0xFE, 0xFF });

            // Act
            var value = serializer.Deserialize<int>(data);

            // Assert
            Assert.AreEqual(-128123, value);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Decimal_ShouldReturnValue(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00 });

            // Act
            var value = serializer.Deserialize<decimal>(data);

            // Assert
            Assert.AreEqual(79228162514264337593543950335M, value);
        }

        #endregion

        #region Collection Tests

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Fixed1DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Fixed1DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Fixed1DCollectionWrapper>(data);

            // Assert
            CollectionAssert.AreEqual(new byte[] { 0xEB, 0xFF, 0x1C, 0x7A, 0xFE, 0xD8 }, obj.Fixed1DCollection);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Fixed1DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Fixed1DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Fixed1DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Fixed2DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Fixed2DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Fixed2DCollectionWrapper>(data);

            // Assert
            CollectionAssert.AreEqual(new byte[2, 3] { { 0xBB, 0xCD, 0x00 }, { 0xFE, 0x10, 0x02 } }, obj.Fixed2DCollection);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Fixed2DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Fixed2DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Fixed2DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Fixed3DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Fixed3DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Fixed3DCollectionWrapper>(data);

            // Assert
            CollectionAssert.AreEqual(new byte[1, 2, 3] { { { 0x03, 0x04, 0x03 }, { 0x63, 0x77, 0x2C } } }, obj.Fixed3DCollection);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Fixed3DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Fixed3DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Fixed3DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Prefix1DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Prefix1DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Prefix1DCollectionWrapper>(data);

            // Assert
            Assert.IsInstanceOfType(obj.Prefix1DCollection, typeof(List<byte>));
            CollectionAssert.AreEqual(new List<byte> { 0x01, 0x02, 0x03 }, obj.Prefix1DCollection);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Prefix1DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Prefix1DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Prefix1DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Prefix2DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Prefix2DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Prefix2DCollectionWrapper>(data);

            // Assert
            CollectionAssert.AreEqual(new byte[3, 2] { { 0x00, 0xC6 }, { 0x1C, 0x0D }, { 0x9A, 0x11 } }, obj.Prefix2DCollection);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Prefix2DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Prefix2DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Prefix2DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Prefix3DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Prefix3DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Prefix3DCollectionWrapper>(data);

            // Assert
            CollectionAssert.AreEqual(new byte[3, 2, 1] { { { 0xDD }, { 0xC4 } }, { { 0x43 }, { 0x8E } }, { { 0xB1 }, { 0xC6 } } }, obj.Prefix3DCollection);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Prefix3DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Prefix3DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Prefix3DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Nested1DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Nested1DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Nested1DCollectionWrapper>(data);

            // Assert
            Assert.AreEqual(2, obj.Nested1DCollection.Length);
            CollectionAssert.AreEqual(
                new byte[] { 12, 125, 55 },
                obj.Nested1DCollection[0]);
            CollectionAssert.AreEqual(
                new byte[] { 112, 11 },
                obj.Nested1DCollection[1]);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Nested1DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Nested1DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Nested1DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Nested2DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Nested2DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Nested2DCollectionWrapper>(data);

            // Assert
            Assert.AreEqual(2, obj.Nested2DCollection.Length);
            CollectionAssert.AreEqual(
                new byte[3, 2] { { 2, 25 }, { 171, 192 }, { 11, 123 } },
                obj.Nested2DCollection[0]);
            CollectionAssert.AreEqual(
                new byte[3, 1] { { 1 }, { 121 }, { 12 } },
                obj.Nested2DCollection[1]);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Nested2DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Nested2DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Nested2DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_Nested3DCollection_ShouldReturnDeserializedObject(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(Nested3DCollectionWrapper.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<Nested3DCollectionWrapper>(data);

            // Assert
            Assert.AreEqual(2, obj.Nested3DCollection.Length);
            CollectionAssert.AreEqual(
                new byte[2, 2, 2] { { { 2, 25 }, { 171, 192 } }, { { 4, 152 }, { 11, 250 } } },
                obj.Nested3DCollection[0]);
            CollectionAssert.AreEqual(
                new byte[2, 1, 2] { { { 4, 51 } }, { { 12, 42 } } },
                obj.Nested3DCollection[1]);
        }

        [TestMethod]
        [DynamicData(nameof(GetCollectionTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_Nested3DCollection_ShouldSerializeToBinaryData(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, Nested3DCollectionWrapper.GetDefault());

            // Assert
            CollectionAssert.AreEqual(Nested3DCollectionWrapper.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region Prallelable Collection Tests

        [TestMethod]
        public void Deserialize_PrallelableCollection_ShouldReturnDeserializedObject()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.Collections);
            var data = new MemoryStream(TestPrallelableCollection.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestPrallelableCollection>(data);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual((long)1234567890, obj.ParallelableCollection[16383]);
            CollectionAssert.AreEqual(new byte[] { 0xA2, 0xC8, 0x00, 0x01 }, obj.NotParallelableCollection);
        }

        [TestMethod]
        public void Serialize_PrallelableCollection_ShouldSerializeToBinaryData()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.Collections);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestPrallelableCollection.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestPrallelableCollection.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region Parallelable Class Tests

        [TestMethod]
        public void Deserialize_ParallelableClass_ShouldReturnDeserializedObject()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All);
            var data = new MemoryStream(TestParallelableClass.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestParallelableClass>(data);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(11000, obj.Parallelable.PrimitiveCollection.Length);
            AssertPrimitiveContainer(obj.Parallelable.PrimitiveCollection[10999]);
            AssertCollectionContainer(obj.NotParallelable);
        }

        [TestMethod]
        public void Serialize_ParallelableClass_ShouldSerializeToBinaryData()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestParallelableClass.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestParallelableClass.GetDefaultBinaryData(), data.ToArray());
        }

        [TestMethod]
        public void Deserialize_ParallelableClassWithConverter_ShouldReturnDeserializedObject()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All,
                    new ParallelableClassBinaryConverter());
            var data = new MemoryStream(TestParallelableClass.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestParallelableClass>(data);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(11000, obj.Parallelable.PrimitiveCollection.Length);
            AssertPrimitiveContainer(obj.Parallelable.PrimitiveCollection[10999]);
            AssertCollectionContainer(obj.NotParallelable);
        }

        [TestMethod]
        public void Serialize_ParallelableClassWithConverter_ShouldSerializeToBinaryData()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All,
                    new ParallelableClassBinaryConverter());
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestParallelableClass.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestParallelableClass.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region Parallel Versionable Tests

        [TestMethod]
        public void Deserialize_ParallelVersionable_ShouldReturnDeserializedObject()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All);
            var data = new MemoryStream(TestParallelVersionable.GetDefaultBinaryData());

            // Act
            var obj = serializer.Deserialize<TestParallelVersionable>(data);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual((uint)10, obj.Versionable.Version);
            Assert.AreEqual(11000, obj.Versionable.Parallelable.PrimitiveCollection.Length);
            AssertPrimitiveContainer(obj.Versionable.Parallelable.PrimitiveCollection[10999]);
            AssertStringContainer(obj.Versionable.NotParallelable);
        }

        [TestMethod]
        public void Serialize_ParallelVersionable_ShouldSerializeToBinaryData()
        {
            // Arrange
            var serializer = CreateSerializer(ProcessType.Cached, CachedProcessType.Parallel, CachingTargets.All);
            var data = new MemoryStream();

            // Act
            serializer.Serialize(data, TestParallelVersionable.GetDefault());

            // Assert
            CollectionAssert.AreEqual(TestParallelVersionable.GetDefaultBinaryData(), data.ToArray());
        }

        #endregion

        #region Edge Cases Tests

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_EmptyCollection_ShouldReturnEmpty(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            // Act
            var value = serializer.Deserialize<List<int>>(data);

            // Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(0, value.Count);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_EmptyString_ShouldReturnEmptyString(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            // Act
            var value = serializer.Deserialize<string>(data);

            // Assert
            Assert.AreEqual(string.Empty, value);
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_WhenStreamLongerThenData_ShouldThrowStreamEndNotReachedExceptionn(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0x00, 0x00 });

            // Act
            var exception = Assert.ThrowsException<StreamEndNotReachedException>(
                () => serializer.Deserialize<byte>(data));
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Deserialize_InvalidData_ShouldThrowEndOfStreamException(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            var data = new MemoryStream(new byte[] { 0x01, 0x02 }); // Invalid length

            // Act & Assert
            Assert.ThrowsException<EndOfStreamException>(
                () => serializer.Deserialize<int>(data));
        }

        [TestMethod]
        [DynamicData(nameof(GetBasicTestOptions), DynamicDataSourceType.Method)]
        public void Serialize_NullObject_ShouldThrowSerializationException(
            ProcessType processType, CachedProcessType cachedType, CachingTargets targets)
        {
            // Arrange
            var serializer = CreateSerializer(processType, cachedType, targets);
            TestClass obj = null;
            var data = new MemoryStream();

            // Act
            Assert.ThrowsException<SerializationException>(
                () => serializer.Serialize(data, obj));
        }

        #endregion

    }
}
