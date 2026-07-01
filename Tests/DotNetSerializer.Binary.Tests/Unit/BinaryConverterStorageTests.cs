using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DotNetSerializer.Binary.Tests.Unit
{
    [TestClass]
    public class BinaryConverterStorageTests
    {
        private class CustomConverterByte : BinaryConverter<byte>
        {
            public override Type RegisteredType => typeof(CustomConverterByte);

            public override byte ReadValue(BinaryReader reader, BinaryContext data)
            {
                throw new NotImplementedException();
            }

            public override bool TryGetSize(BinaryContext metaData, out int size)
            {
                throw new NotImplementedException();
            }

            public override void WriteValue(BinaryWriter writer, byte value, BinaryContext data)
            {
                throw new NotImplementedException();
            }
        }
        private class CustomConverterInt : BinaryConverter<int>
        {
            public override Type RegisteredType => typeof(CustomConverterInt);

            public override int ReadValue(BinaryReader reader, BinaryContext data)
            {
                throw new NotImplementedException();
            }

            public override bool TryGetSize(BinaryContext metaData, out int size)
            {
                throw new NotImplementedException();
            }

            public override void WriteValue(BinaryWriter writer, int value, BinaryContext data)
            {
                throw new NotImplementedException();
            }
        }

        public class NewStringConverter : BinaryConverter<string>
        {
            public override Type RegisteredType => typeof(string);

            public override string ReadValue(BinaryReader reader, BinaryContext data)
            {
                throw new NotImplementedException();
            }

            public override bool TryGetSize(BinaryContext metaData, out int size)
            {
                throw new NotImplementedException();
            }

            public override void WriteValue(BinaryWriter writer, string value, BinaryContext data)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Contains_WhenConverterExists_ShouldReturnTrue()
        {
            var storage = new BinaryConverterStorage();
            storage.Add<CustomConverterInt>();

            var result = storage.Contains(typeof(CustomConverterInt));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_WhenConverterNoExists_ShouldReturnFalse()
        {
            var storage = new BinaryConverterStorage();

            var result = storage.Contains(typeof(CustomConverterInt));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_WhenConverterExists_ShouldRetunConverter()
        {
            var storage = new BinaryConverterStorage();
            storage.Add<CustomConverterInt>();

            var converter = storage.Get(typeof(CustomConverterInt));

            Assert.IsNotNull(converter);
            Assert.AreEqual(typeof(CustomConverterInt), converter.GetType());
        }

        [TestMethod]
        public void Get_WhenConverterNoExists_ShouldThrowConverterNotFoundException()
        {
            var storage = new BinaryConverterStorage();

            Assert.ThrowsException<ConverterNotFoundException>(
                () => storage.Get(typeof(CustomConverterInt)));
        }

        [TestMethod]
        public void Add_Generic_WithNewConverter_ShouldAddCustomConverter()
        {
            var storage = new BinaryConverterStorage();

            storage.Add<CustomConverterInt>();

            Assert.IsNotNull(storage.Items[typeof(CustomConverterInt)]);
        }

        [TestMethod]
        public void Add_NonGeneric_WithNewConverter_ShouldAddCustomConverter()
        {
            var storage = new BinaryConverterStorage();

            storage.Add(new CustomConverterInt());

            Assert.IsNotNull(storage.Items[typeof(CustomConverterInt)]);
        }

        [TestMethod]
        public void Add_NonGeneric_WhenConverterExists_ShouldThrowArgumentException()
        {
            var storage = new BinaryConverterStorage();

            storage.Add(new CustomConverterInt());

            Assert.ThrowsException<ArgumentException>(
                () => storage.Add(new CustomConverterInt()));
        }

        [TestMethod]
        public void Add_WithConverterCollection_ShouldAddConverters()
        {
            var storage = new BinaryConverterStorage();
            var converters = new BinaryConverter[]
            {
                new CustomConverterByte(),
                new CustomConverterInt(),
            };

            storage.Add(converters);

            Assert.IsNotNull(storage.Items[typeof(CustomConverterByte)]);
            Assert.IsNotNull(storage.Items[typeof(CustomConverterInt)]);
        }

        [TestMethod]
        public void Set_Generic_WithNewConverter_ShouldAddConverter()
        {
            var storage = new BinaryConverterStorage();

            storage.Set<CustomConverterByte>();

            Assert.IsNotNull(storage.Items[typeof(CustomConverterByte)]);
        }

        [TestMethod]
        public void Set_Generic_WhenConverterExists_ShouldResetOldConverterToNew()
        {
            var storage = new BinaryConverterStorage();
            var newConverter = new NewStringConverter();

            storage.Set(newConverter);

            Assert.AreSame(newConverter, storage.Items[typeof(string)]);
        }

        [TestMethod]
        public void Set_NonGeneric_WithNewConverter_ShouldAddConverter()
        {
            var storage = new BinaryConverterStorage();

            storage.Set(new CustomConverterByte());

            Assert.IsNotNull(storage.Items[typeof(CustomConverterByte)]);
        }

        [TestMethod]
        public void Set_NonGeneric_WhenConverterExists_ShouldResetOldConverterToNew()
        {
            var storage = new BinaryConverterStorage();
            var newConverter = new NewStringConverter();

            storage.Set(newConverter);

            Assert.AreSame(newConverter, storage.Items[typeof(string)]);
        }

        [TestMethod]
        public void Set_WithConverterCollection_ShouldResetOldAndAddNewConverters()
        {
            var storage = new BinaryConverterStorage();
            var oldStringConverter = new NewStringConverter();
            storage.Add(oldStringConverter);
            var converters = new BinaryConverter[]
            {
                new CustomConverterByte(),
                new NewStringConverter(),
            };

            storage.Set(converters);

            Assert.IsNotNull(storage.Items[typeof(CustomConverterByte)]);
            Assert.AreNotSame(oldStringConverter, storage.Items[typeof(string)]);
        }
    }
}
