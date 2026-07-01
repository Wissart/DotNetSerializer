using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Converters.Default;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetSerializer.Binary.Tests.TestData
{
    #region String Wrappers

    //  Fixed
    internal class FixedStringWrapper
    {
        [StringFormat(32)]
        public string FixedString { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x46, 0x69, 0x78, 0x65, 0x64, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            return _defaultBinaryData;
        }

        private static FixedStringWrapper _default;
        public static FixedStringWrapper GetDefault()
        {
            if (_default == null)
                _default = new FixedStringWrapper()
                {
                    FixedString = "Fixed string"
                };

            return _default;
        }
    }

    //  Prefix
    internal class PrefixStringWrapper
    {
        public string PrefixString { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x50, 0x72, 0x65, 0x66, 0x69, 0x78, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67 };

            return _defaultBinaryData;
        }

        private static PrefixStringWrapper _default;
        public static PrefixStringWrapper GetDefault()
        {
            if (_default == null)
                _default = new PrefixStringWrapper()
                {
                    PrefixString = "Prefix string"
                };

            return _default;
        }
    }

    //  SignEnd
    internal class SignEndStringWrapper
    {
        [StringFormat(StringSizeType.SignEnd)]
        public string SignEndString { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x53, 0x69, 0x67, 0x6E, 0x20, 0x73, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x00 };

            return _defaultBinaryData;
        }

        private static SignEndStringWrapper _default;
        public static SignEndStringWrapper GetDefault()
        {
            if (_default == null)
                _default = new SignEndStringWrapper()
                {
                    SignEndString = "Sign string",
                };

            return _default;
        }
    }

    #endregion

    #region Collection Wrappers

    //  Fixed
    internal class Fixed1DCollectionWrapper
    {
        [Collection(6)]
        public byte[] Fixed1DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0xEB, 0xFF, 0x1C, 0x7A, 0xFE, 0xD8 };


            return _defaultBinaryData;
        }

        private static Fixed1DCollectionWrapper _default;
        public static Fixed1DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Fixed1DCollectionWrapper()
                {
                    Fixed1DCollection = new byte[6] { 0xEB, 0xFF, 0x1C, 0x7A, 0xFE, 0xD8, }
                };

            return _default;
        }
    }

    internal class Fixed2DCollectionWrapper
    {
        [Collection(2, 3)]
        public byte[,] Fixed2DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0xBB, 0xCD, 0x00, 0xFE, 0x10, 0x02 };


            return _defaultBinaryData;
        }

        private static Fixed2DCollectionWrapper _default;
        public static Fixed2DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Fixed2DCollectionWrapper()
                {
                    Fixed2DCollection = new byte[2, 3] { { 0xBB, 0xCD, 0x00 }, { 0xFE, 0x10, 0x02 } }
                };

            return _default;
        }
    }

    internal class Fixed3DCollectionWrapper
    {
        [Collection(1, 2, 3)]
        public byte[,,] Fixed3DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x03, 0x04, 0x03, 0x63, 0x77, 0x2C };


            return _defaultBinaryData;
        }

        private static Fixed3DCollectionWrapper _default;
        public static Fixed3DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Fixed3DCollectionWrapper()
                {
                    Fixed3DCollection = new byte[1, 2, 3] { { { 0x03, 0x04, 0x03 }, { 0x63, 0x77, 0x2C } } }
                };

            return _default;
        }
    }

    //  Prefix
    internal class Prefix1DCollectionWrapper
    {
        // For collection above use default CollectionAttribute from options
        public List<byte> Prefix1DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03 };


            return _defaultBinaryData;
        }

        private static Prefix1DCollectionWrapper _default;
        public static Prefix1DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Prefix1DCollectionWrapper()
                {
                    Prefix1DCollection = new List<byte>(new byte[3] { 0x01, 0x02, 0x03 })
                };

            return _default;
        }
    }

    internal class Prefix2DCollectionWrapper
    {
        public byte[,] Prefix2DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xC6, 0x1C, 0x0D, 0x9A, 0x11 };


            return _defaultBinaryData;
        }

        private static Prefix2DCollectionWrapper _default;
        public static Prefix2DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Prefix2DCollectionWrapper()
                {
                    Prefix2DCollection = new byte[3, 2] { { 0x00, 0xC6 }, { 0x1C, 0x0D }, { 0x9A, 0x11 } }
                };

            return _default;
        }
    }

    internal class Prefix3DCollectionWrapper
    {
        public byte[,,] Prefix3DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xDD, 0xC4, 0x43, 0x8E, 0xB1, 0xC6 };


            return _defaultBinaryData;
        }

        private static Prefix3DCollectionWrapper _default;
        public static Prefix3DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Prefix3DCollectionWrapper()
                {
                    Prefix3DCollection = new byte[3, 2, 1] { { { 0xDD }, { 0xC4 } }, { { 0x43 }, { 0x8E } }, { { 0xB1 }, { 0xC6 } } }
                };

            return _default;
        }
    }

    //  Nested
    internal class Nested1DCollectionWrapper
    {
        public byte[][] Nested1DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x0C, 0x7D, 0x37, 0x02, 0x00, 0x00, 0x00, 0x70, 0x0B };


            return _defaultBinaryData;
        }

        private static Nested1DCollectionWrapper _default;
        public static Nested1DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Nested1DCollectionWrapper()
                {
                    Nested1DCollection = new byte[][] { new byte[] { 12, 125, 55 }, new byte[] { 112, 11 }, }
                };

            return _default;
        }
    }

    internal class Nested2DCollectionWrapper
    {
        public byte[][,] Nested2DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x19, 0xAB, 0xC0, 0x0B, 0x7B, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x79, 0x0C };


            return _defaultBinaryData;
        }

        private static Nested2DCollectionWrapper _default;
        public static Nested2DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Nested2DCollectionWrapper()
                {
                    Nested2DCollection = new byte[][,] { new byte[3, 2] { { 2, 25 }, { 171, 192 }, { 11, 123 } }, new byte[3, 1] { { 1 }, { 121 }, { 12 } }, }
                };

            return _default;
        }
    }

    internal class Nested3DCollectionWrapper
    {
        public byte[][,,] Nested3DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x19, 0xAB, 0xC0, 0x04, 0x98, 0x0B, 0xFA, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x33, 0x0C, 0x2A };


            return _defaultBinaryData;
        }

        private static Nested3DCollectionWrapper _default;
        public static Nested3DCollectionWrapper GetDefault()
        {
            if (_default == null)
                _default = new Nested3DCollectionWrapper()
                {
                    Nested3DCollection = new byte[][,,] { new byte[2, 2, 2] { { { 2, 25 }, { 171, 192 } }, { { 4, 152 }, { 11, 250 } } }, new byte[2, 1, 2] { { { 4, 51 } }, { { 12, 42 } } }, }
                };

            return _default;
        }
    }

    #endregion

    #region NonClass Containers

    internal class PrimitiveContainer
    {
        public char CharProperty { get; set; }
        public byte ByteProperty { get; set; }
        public sbyte SByteProperty { get; set; }
        public bool BoolProperty { get; set; }
        public short ShortProperty { get; set; }
        public ushort UShortProperty { get; set; }
        public int IntProperty { get; set; }
        public uint UintProperty { get; set; }
        public long LongProperty { get; set; }
        public ulong ULongProperty { get; set; }
        public float FloatProperty { get; set; }
        public double DoubleProperty { get; set; }

        public decimal DecimalProperty { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
                _defaultBinaryData = new byte[] { 0x61,
                                                      0xDE,
                                                      0xCD,
                                                      0x01,
                                                      0xB2, 0xA1,
                                                      0x3B, 0xAB,
                                                      0x85, 0x0B, 0xFE, 0xFF,
                                                      0x3B, 0xAB, 0xD4, 0x3B,
                                                      0xCD, 0x4A, 0x0F, 0x16, 0x5E, 0x82, 0xD7, 0xFD,
                                                      0x3B, 0xAB, 0xD4, 0x3B, 0x6D, 0x86, 0x63, 0x02,
                                                      0x96, 0x43, 0x23, 0x40,
                                                      0xB2, 0xA1, 0x9B, 0xFD, 0x81, 0x32, 0xF8, 0x3F,
                                                      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, };

            return _defaultBinaryData;
        }

        private static PrimitiveContainer _default;
        public static PrimitiveContainer GetDefault()
        {
            if (_default == null)
                _default = new PrimitiveContainer()
                {
                    CharProperty = 'a',
                    ByteProperty = 222,
                    SByteProperty = -51,
                    BoolProperty = true,
                    ShortProperty = -24142,
                    UShortProperty = 43835,
                    IntProperty = -128123,
                    UintProperty = 1003793211,
                    LongProperty = -155512321512355123,
                    ULongProperty = 172129014483561275,
                    FloatProperty = 2.551f,
                    DoubleProperty = 1.512331,

                    DecimalProperty = 79228162514264337593543950335M,
                };

            return _default;
        }
    }

    internal class StringContainer
    {
        public FixedStringWrapper FixedString { get; set; }
        public PrefixStringWrapper PrefixString { get; set; }
        public SignEndStringWrapper SignEndString { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>();
                dataList.AddRange(FixedStringWrapper.GetDefaultBinaryData());
                dataList.AddRange(PrefixStringWrapper.GetDefaultBinaryData());
                dataList.AddRange(SignEndStringWrapper.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static StringContainer _default;
        public static StringContainer GetDefault()
        {
            if (_default == null)
                _default = new StringContainer()
                {
                    FixedString = FixedStringWrapper.GetDefault(),
                    PrefixString = PrefixStringWrapper.GetDefault(),
                    SignEndString = SignEndStringWrapper.GetDefault(),
                };

            return _default;
        }
    }

    internal class CollectionContainer
    {
        public Fixed1DCollectionWrapper Fixed1DCollection { get; set; }
        public Fixed2DCollectionWrapper Fixed2DCollection { get; set; }
        public Fixed3DCollectionWrapper Fixed3DCollection { get; set; }

        public Prefix1DCollectionWrapper Prefix1DCollection { get; set; }
        public Prefix2DCollectionWrapper Prefix2DCollection { get; set; }
        public Prefix3DCollectionWrapper Prefix3DCollection { get; set; }

        public Nested1DCollectionWrapper Nested1DCollection { get; set; }
        public Nested2DCollectionWrapper Nested2DCollection { get; set; }
        public Nested3DCollectionWrapper Nested3DCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>();
                dataList.AddRange(Fixed1DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Fixed2DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Fixed3DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Prefix1DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Prefix2DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Prefix3DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Nested1DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Nested2DCollectionWrapper.GetDefaultBinaryData());
                dataList.AddRange(Nested3DCollectionWrapper.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }


            return _defaultBinaryData;
        }

        private static CollectionContainer _default;
        public static CollectionContainer GetDefault()
        {
            if (_default == null)
                _default = new CollectionContainer()
                {
                    Fixed1DCollection = Fixed1DCollectionWrapper.GetDefault(),
                    Fixed2DCollection = Fixed2DCollectionWrapper.GetDefault(),
                    Fixed3DCollection = Fixed3DCollectionWrapper.GetDefault(),

                    Prefix1DCollection = Prefix1DCollectionWrapper.GetDefault(),
                    Prefix2DCollection = Prefix2DCollectionWrapper.GetDefault(),
                    Prefix3DCollection = Prefix3DCollectionWrapper.GetDefault(),

                    Nested1DCollection = Nested1DCollectionWrapper.GetDefault(),
                    Nested2DCollection = Nested2DCollectionWrapper.GetDefault(),
                    Nested3DCollection = Nested3DCollectionWrapper.GetDefault(),
                };

            return _default;
        }
    }

    #endregion


    #region Test Class

    internal class TestClass
    {
        public int ID { get; set; }
        public PrimitiveContainer Primitives { get; set; }
        public StringContainer Strings { get; set; }
        public CollectionContainer Collections { get; set; }




        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>(new byte[] { 0x0C, 0x00, 0x00, 0x00 });
                dataList.AddRange(PrimitiveContainer.GetDefaultBinaryData());
                dataList.AddRange(StringContainer.GetDefaultBinaryData());
                dataList.AddRange(CollectionContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static TestClass _default;
        public static TestClass GetDefault()
        {
            if (_default == null)
                _default = new TestClass()
                {
                    ID = 12,
                    Primitives = PrimitiveContainer.GetDefault(),
                    Strings = StringContainer.GetDefault(),
                    Collections = CollectionContainer.GetDefault(),
                };

            return _default;
        }
    }

    #endregion

    #region Test Class With Converter

    internal class TestClassWithConverter
    {
        [Converter(typeof(PrimitiveContainerInAttributeConverter))]
        public PrimitiveContainer Primitives1 { get; set; }
        public PrimitiveContainer Primitives2 { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>();
                dataList.AddRange(PrimitiveContainer.GetDefaultBinaryData());
                dataList.AddRange(PrimitiveContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static TestClassWithConverter _default;
        public static TestClassWithConverter GetDefault()
        {
            if (_default == null)
                _default = new TestClassWithConverter()
                {
                    Primitives1 = PrimitiveContainer.GetDefault(),
                    Primitives2 = PrimitiveContainer.GetDefault(),
                };

            return _default;
        }
    }

    
    internal class PrimitiveContainerInAttributeConverter : InAttributeConverter<PrimitiveContainer>
    {
        public override PrimitiveContainer ReadValue(BinaryReader reader, BinaryContext data)
        {
            return new PrimitiveContainer()
            {
                CharProperty = reader.ReadChar(),
                ByteProperty = reader.ReadByte(),
                SByteProperty = reader.ReadSByte(),
                BoolProperty = reader.ReadBoolean(),
                ShortProperty = reader.ReadInt16(),
                UShortProperty = reader.ReadUInt16(),
                IntProperty = reader.ReadInt32(),
                UintProperty = reader.ReadUInt32(),
                LongProperty = reader.ReadInt64(),
                ULongProperty = reader.ReadUInt64(),
                FloatProperty = reader.ReadSingle(),
                DoubleProperty = reader.ReadDouble(),
                DecimalProperty = reader.ReadDecimal(),
            };
        }

        public override void WriteValue(BinaryWriter writer, PrimitiveContainer value, BinaryContext data)
        {
            writer.Write(value.CharProperty);
            writer.Write(value.ByteProperty);
            writer.Write(value.SByteProperty);
            writer.Write(value.BoolProperty);
            writer.Write(value.ShortProperty);
            writer.Write(value.UShortProperty);
            writer.Write(value.IntProperty);
            writer.Write(value.UintProperty);
            writer.Write(value.LongProperty);
            writer.Write(value.ULongProperty);
            writer.Write(value.FloatProperty);
            writer.Write(value.DoubleProperty);
            writer.Write(value.DecimalProperty);
        }

        public override bool TryGetSize(BinaryContext metaData, out int size)
        {
            size = 60;
            return true;
        }
    }

    internal class PrimitiveContainerFactoryConverter : FactoryConverter<PrimitiveContainer>
    {
        public override PrimitiveContainer Create(BinaryReader reader, BinaryContext data)
        {
            return new PrimitiveContainer();
        }
    }

    #endregion

    #region Common Versionable

    [Versionable]
    internal class SubVersionable
    {
        public uint Version { get; set; }

        [RequireVersion(21)]
        public StringContainer SupportedByVersion { get; set; }
        [RequireVersion(34)]
        public CollectionContainer NotSupportedByVersion { get; set; }
        public PrimitiveContainer NotMarkedProperty { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>(new byte[] { 0x1E, 0x00, 0x00, 0x00 });
                dataList.AddRange(StringContainer.GetDefaultBinaryData());
                dataList.AddRange(PrimitiveContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static SubVersionable _default;
        public static SubVersionable GetDefault()
        {
            if (_default == null)
                _default = new SubVersionable()
                {
                    Version = 30,
                    SupportedByVersion = StringContainer.GetDefault(),
                    NotSupportedByVersion = CollectionContainer.GetDefault(),
                    NotMarkedProperty = PrimitiveContainer.GetDefault(),
                };

            return _default;
        }
    }

    internal class SubClass
    {
        [RequireVersion(10)]
        public CollectionContainer SupportedByVersion { get; set; }
        [RequireVersion(11)]
        public PrimitiveContainer NotSupportedByVersion { get; set; }
        public StringContainer NotMarkedProperty { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>();
                dataList.AddRange(CollectionContainer.GetDefaultBinaryData());
                dataList.AddRange(StringContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static SubClass _default;
        public static SubClass GetDefault()
        {
            if (_default == null)
                _default = new SubClass()
                {
                    SupportedByVersion = CollectionContainer.GetDefault(),
                    NotSupportedByVersion = PrimitiveContainer.GetDefault(),
                    NotMarkedProperty = StringContainer.GetDefault(),
                };

            return _default;
        }
    }

    #endregion

    #region Test Vesrionable

    [Versionable]
    internal class TestVersionable
    {
        public int ID { get; set; }
        public uint Version { get; set; }
        public SubVersionable SubVersionable { get; set; }
        public SubClass SubClass { get; set; }

        [RequireVersion(5)]
        public PrimitiveContainer SupportedByVersion { get; set; }
        [RequireVersion(15)]
        public StringContainer NotSupportedByVersion { get; set; }
        public CollectionContainer NotMarkedProperty { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>(new byte[] { 0x65, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00 });
                dataList.AddRange(SubVersionable.GetDefaultBinaryData());
                dataList.AddRange(SubClass.GetDefaultBinaryData());
                dataList.AddRange(PrimitiveContainer.GetDefaultBinaryData());
                dataList.AddRange(CollectionContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static TestVersionable _default;
        public static TestVersionable GetDefault()
        {
            if (_default == null)
                _default = new TestVersionable()
                {
                    ID = 101,
                    Version = 10,
                    SubVersionable = SubVersionable.GetDefault(),
                    SubClass = SubClass.GetDefault(),
                    SupportedByVersion = PrimitiveContainer.GetDefault(),
                    NotSupportedByVersion = StringContainer.GetDefault(),
                    NotMarkedProperty = CollectionContainer.GetDefault(),
                };

            return _default;
        }
    }

    #endregion

    #region Test Versionable With Converter

    [Versionable]
    internal class TestVersionableWithConverter
    {
        public int ID { get; set; }
        public uint Version { get; set; }
        [Converter(typeof(SubVersionableInAttributeConverter))]
        public SubVersionable SubVersionable1 { get; set; }
        public SubVersionable SubVersionable2 { get; set; }
        public SubClass SubClass { get; set; }

        [RequireVersion(5)]
        public PrimitiveContainer SupportedByVersion { get; set; }
        [RequireVersion(15)]
        public StringContainer NotSupportedByVersion { get; set; }
        public CollectionContainer NotMarkedProperty { get; set; }



        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>(new byte[] { 0x65, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00 });
                dataList.AddRange(SubVersionable.GetDefaultBinaryData());
                dataList.AddRange(SubVersionable.GetDefaultBinaryData());
                dataList.AddRange(SubClass.GetDefaultBinaryData());
                dataList.AddRange(PrimitiveContainer.GetDefaultBinaryData());
                dataList.AddRange(CollectionContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static TestVersionableWithConverter _default;
        public static TestVersionableWithConverter GetDefault()
        {
            if (_default == null)
                _default = new TestVersionableWithConverter()
                {
                    ID = 101,
                    Version = 10,
                    SubVersionable1 = SubVersionable.GetDefault(),
                    SubVersionable2 = SubVersionable.GetDefault(),
                    SubClass = SubClass.GetDefault(),
                    SupportedByVersion = PrimitiveContainer.GetDefault(),
                    NotSupportedByVersion = StringContainer.GetDefault(),
                    NotMarkedProperty = CollectionContainer.GetDefault(),
                };

            return _default;
        }
    }

    internal class SubVersionableInAttributeConverter : InAttributeConverter<SubVersionable>
    {
        public override SubVersionable ReadValue(BinaryReader reader, BinaryContext data)
        {
            var obj = new SubVersionable()
            {
                Version = reader.ReadUInt32(),
            };
            if (obj.Version >= 21)
                obj.SupportedByVersion = new StringContainer()
                {
                    FixedString = new FixedStringWrapper()
                    {
                        FixedString = StringConverter.ReadFixedSizeString(reader, Encoding.UTF8, 32),
                    },
                    PrefixString = new PrefixStringWrapper()
                    {
                        PrefixString = StringConverter.ReadPrefixSizeString(reader, Encoding.UTF8),
                    },
                    SignEndString = new SignEndStringWrapper()
                    {
                        SignEndString = StringConverter.ReadSignEndString(reader, Encoding.UTF8),
                    },
                };
            if (obj.Version >= 34)
                throw new System.Exception("Not supported version");

            obj.NotMarkedProperty = new PrimitiveContainer()
            {
                CharProperty = reader.ReadChar(),
                ByteProperty = reader.ReadByte(),
                SByteProperty = reader.ReadSByte(),
                BoolProperty = reader.ReadBoolean(),
                ShortProperty = reader.ReadInt16(),
                UShortProperty = reader.ReadUInt16(),
                IntProperty = reader.ReadInt32(),
                UintProperty = reader.ReadUInt32(),
                LongProperty = reader.ReadInt64(),
                ULongProperty = reader.ReadUInt64(),
                FloatProperty = reader.ReadSingle(),
                DoubleProperty = reader.ReadDouble(),
                DecimalProperty = reader.ReadDecimal(),
            };

            return obj;
        }

        public override void WriteValue(BinaryWriter writer, SubVersionable value, BinaryContext data)
        {
            writer.Write(value.Version);
            if (value.Version >= 21)
            {
                StringConverter.WriteFixedString(writer, value.SupportedByVersion.FixedString.FixedString, Encoding.UTF8, 32);
                StringConverter.WritePrefixString(writer, value.SupportedByVersion.PrefixString.PrefixString, Encoding.UTF8);
                StringConverter.WriteSignEndString(writer, value.SupportedByVersion.SignEndString.SignEndString, Encoding.UTF8);
            }
            if (value.Version >= 34)
                throw new System.Exception("Not supported version");

            writer.Write(value.NotMarkedProperty.CharProperty);
            writer.Write(value.NotMarkedProperty.ByteProperty);
            writer.Write(value.NotMarkedProperty.SByteProperty);
            writer.Write(value.NotMarkedProperty.BoolProperty);
            writer.Write(value.NotMarkedProperty.ShortProperty);
            writer.Write(value.NotMarkedProperty.UShortProperty);
            writer.Write(value.NotMarkedProperty.IntProperty);
            writer.Write(value.NotMarkedProperty.UintProperty);
            writer.Write(value.NotMarkedProperty.LongProperty);
            writer.Write(value.NotMarkedProperty.ULongProperty);
            writer.Write(value.NotMarkedProperty.FloatProperty);
            writer.Write(value.NotMarkedProperty.DoubleProperty);
            writer.Write(value.NotMarkedProperty.DecimalProperty);
        }
    }

    internal class SubVersionableFactoryConverter : FactoryConverter<SubVersionable>
    {
        public override SubVersionable Create(BinaryReader reader, BinaryContext data)
        {
            return new SubVersionable();
        }
    }

    #endregion


    #region Test Prallelable Collection

    internal class TestPrallelableCollection
    {
        [Collection(16384)]
        public long[] ParallelableCollection { get; set; }
        [Collection(4)]
        public byte[] NotParallelableCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>();
                var longBinaryBuffer = BitConverter.GetBytes((long)1234567890);
                for (int i = 0; i < 16384; i++)
                {
                    dataList.AddRange(longBinaryBuffer);
                }
                dataList.AddRange(new byte[] { 0xA2, 0xC8, 0x00, 0x01 });
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static TestPrallelableCollection _default;
        public static TestPrallelableCollection GetDefault()
        {
            if (_default == null)
            {
                var parallelableCollection = new long[16384];
                for (int i = 0; i < parallelableCollection.Length; i++)
                {
                    parallelableCollection[i] = 1234567890;
                }
                _default = new TestPrallelableCollection()
                {
                    ParallelableCollection = parallelableCollection,
                    NotParallelableCollection = new byte[] { 0xA2, 0xC8, 0x00, 0x01 },
                };
            }

            return _default;
        }
    }

    #endregion

    #region Test Parallelable Class

    internal class TestParallelableClass
    {
        public ParallelableClass Parallelable { get; set; }
        public CollectionContainer NotParallelable { get; set; }



        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>();
                dataList.AddRange(ParallelableClass.GetDefaultBinaryData());
                dataList.AddRange(CollectionContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static TestParallelableClass _default;
        public static TestParallelableClass GetDefault()
        {
            if (_default == null)
            {
                _default = new TestParallelableClass()
                {
                    Parallelable = ParallelableClass.GetDefault(),
                    NotParallelable = CollectionContainer.GetDefault(),
                };
            }

            return _default;
        }
    }

    internal class ParallelableClass
    {
        [Collection(11000)]
        public PrimitiveContainer[] PrimitiveCollection { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>();
                for(int i = 0; i < 11000; i++)
                {
                    dataList.AddRange(PrimitiveContainer.GetDefaultBinaryData());
                }
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static ParallelableClass _default;
        public static ParallelableClass GetDefault()
        {
            if (_default == null)
            {
                var primitiveCollection = new PrimitiveContainer[11000];
                for (int i = 0; i < 11000; i++)
                {
                    primitiveCollection[i] = PrimitiveContainer.GetDefault();
                }
                _default = new ParallelableClass()
                {
                    PrimitiveCollection = primitiveCollection,
                };
            }

            return _default;
        }
    }

    internal class ParallelableClassBinaryConverter : BinaryConverter<ParallelableClass>
    {
        public override bool IsComplete => true;

        public override ParallelableClass ReadValue(BinaryReader reader, BinaryContext data)
        {
            var obj = new ParallelableClass
            {
                PrimitiveCollection = new PrimitiveContainer[11000]
            };
            for (int i = 0; i < obj.PrimitiveCollection.Length; i++)
            {
                obj.PrimitiveCollection[i] = new PrimitiveContainer()
                {
                    CharProperty = reader.ReadChar(),
                    ByteProperty = reader.ReadByte(),
                    SByteProperty = reader.ReadSByte(),
                    BoolProperty = reader.ReadBoolean(),
                    ShortProperty = reader.ReadInt16(),
                    UShortProperty = reader.ReadUInt16(),
                    IntProperty = reader.ReadInt32(),
                    UintProperty = reader.ReadUInt32(),
                    LongProperty = reader.ReadInt64(),
                    ULongProperty = reader.ReadUInt64(),
                    FloatProperty = reader.ReadSingle(),
                    DoubleProperty = reader.ReadDouble(),
                    DecimalProperty = reader.ReadDecimal(),
                };
            }

            return obj;
        }

        public override void WriteValue(BinaryWriter writer, ParallelableClass value, BinaryContext data)
        {
            var primitiveCollection = value.PrimitiveCollection;
            foreach(var primitive in primitiveCollection)
            {
                writer.Write(primitive.CharProperty);
                writer.Write(primitive.ByteProperty);
                writer.Write(primitive.SByteProperty);
                writer.Write(primitive.BoolProperty);
                writer.Write(primitive.ShortProperty);
                writer.Write(primitive.UShortProperty);
                writer.Write(primitive.IntProperty);
                writer.Write(primitive.UintProperty);
                writer.Write(primitive.LongProperty);
                writer.Write(primitive.ULongProperty);
                writer.Write(primitive.FloatProperty);
                writer.Write(primitive.DoubleProperty);
                writer.Write(primitive.DecimalProperty);
            }
        }

        public override bool TryGetSize(BinaryContext metaData, out int size)
        {
            size = 60 * 11000;
            return true;
        }
    }

    #endregion

    #region Test Parallel Versionable

    internal class TestParallelVersionable
    {
        public ParallelVersionable Versionable { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                _defaultBinaryData = ParallelVersionable.GetDefaultBinaryData();
            }

            return _defaultBinaryData;
        }

        private static TestParallelVersionable _default;
        public static TestParallelVersionable GetDefault()
        {
            if (_default == null)
            {
                _default = new TestParallelVersionable()
                {
                    Versionable = ParallelVersionable.GetDefault(),
                };
            }

            return _default;
        }
    }

    [Versionable]

    internal class ParallelVersionable
    {
        public uint Version { get; set; }
        public ParallelableClass Parallelable { get; set; }
        public StringContainer NotParallelable { get; set; }


        private static byte[] _defaultBinaryData;
        public static byte[] GetDefaultBinaryData()
        {
            if (_defaultBinaryData == null)
            {
                var dataList = new List<byte>(new byte[] { 0x0A, 0x00, 0x00, 0x00 });
                dataList.AddRange(ParallelableClass.GetDefaultBinaryData());
                dataList.AddRange(StringContainer.GetDefaultBinaryData());
                _defaultBinaryData = dataList.ToArray();
            }

            return _defaultBinaryData;
        }

        private static ParallelVersionable _default;
        public static ParallelVersionable GetDefault()
        {
            if (_default == null)
            {
                _default = new ParallelVersionable()
                {
                    Version = 10,
                    Parallelable = ParallelableClass.GetDefault(),
                    NotParallelable = StringContainer.GetDefault(),
                };
            }

            return _default;
        }
    }

    #endregion
}
