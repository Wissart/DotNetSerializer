using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Storages;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.Common.Base;
using System;
using System.IO;

namespace DotNetSerializer.Binary.Processes.DefaultProcess
{
    internal class DefaultDeserializationProcess : DeserializationProcessBase
    {
        public DefaultDeserializationProcess(BinaryConfiguration configuration) 
            : base(configuration)
        {
        }

        protected override object DeserializeCollection(BinaryReader reader, Type type, Type[] elementTypes, BinaryContext context)
        {
            return DeserializePrefixCollection(reader, type, elementTypes, context);
        }

        protected override object DeserializePropertyCollection(BinaryReader reader, Type propertyType, Type[] elementTypes, BinaryContext context)
        {
            var attribute = AttributeUtilities.GetCollectionAttribute(context.MetaData.Property);

            if (attribute == null)
                attribute = Options.DefaultAttributes.Get<CollectionAttribute>();

            if (attribute.SizeType == CollectionSizeType.Prefix)
                return DeserializePrefixCollection(reader, propertyType, elementTypes, context);
            else
                return DeserializeFixedCollection(reader, propertyType, elementTypes, attribute.Shape, context);
        }

        private object DeserializePrefixCollection(BinaryReader reader, Type type, Type[] elementTypes, BinaryContext context)
        {
            var collectionType = CollectionUtilities.GetCollectionType(type);

            var handler = Options.CollectionHandlers.Get(collectionType);
            var rank = handler.GetRank(type);
            var elementType = handler.GetElementType(elementTypes);
            var shape = new int[rank];

            for (int i = 0; i < shape.Length; i++)
            {
                shape[i] = reader.ReadInt32();
            }

            var collection = handler.CreateCollection(elementTypes, shape);

            DeserializeCollection(reader, handler, collection, shape, elementType, context);

            return collection;
        }

        private object DeserializeFixedCollection(BinaryReader reader, Type type, Type[] elementTypes, int[] shape, BinaryContext context)
        {
            var collectionType = CollectionUtilities.GetCollectionType(type);

            var handler = Options.CollectionHandlers.Get(collectionType);
            var elementType = handler.GetElementType(elementTypes);

            var collection = handler.CreateCollection(elementTypes, shape);

            DeserializeCollection(reader, handler, collection, shape, elementType, context);

            return collection;
        }

        private void DeserializeCollection(BinaryReader reader, ICollectionHandler handler, object collection, int[] shape, Type itemType, BinaryContext context)
        {
            var deserializeItemMethod = CreateCollectionItemDeserializationMethod(itemType);

            switch (shape.Length)
            {
                case 1:
                    Deserialize1DCollection(reader, handler, collection, shape, deserializeItemMethod, context);
                    break;
                case 2:
                    Deserialize2DCollection(reader, handler, collection, shape, deserializeItemMethod, context);
                    break;
                case 3:
                    Deserialize3DCollection(reader, handler, collection, shape, deserializeItemMethod, context);
                    break;
                default:
                    throw new DotNetSerializerException($"Unsupported collection rank: {shape.Length}");
            }
        }

        private Func<BinaryReader, BinaryContext, object> CreateCollectionItemDeserializationMethod(Type valueType)
        {
            if (CollectionUtilities.TryGetElementTypes(valueType, out Type[] elementTypes))
            {
                return CreateNestedDeserializationMethod(valueType, elementTypes);
            }
            else
            {
                return CreateTypeDeserializationMethod(valueType);
            }
        }

        private Func<BinaryReader, BinaryContext, object> CreateNestedDeserializationMethod(Type declareCollectionType, Type[] elementTypes)
        {
            return (reader, context) =>
            {
                return DeserializePrefixCollection(reader, declareCollectionType, elementTypes, context);
            };
        }

        private Func<BinaryReader, BinaryContext, object> CreateTypeDeserializationMethod(Type itemType)
        {
            if (SerializationUtilities.IsClass(itemType))
            {
                if (TypeInfoStorage.Get(itemType).IsVersionable)
                {
                    if (Options.Converters.Items.TryGetValue(itemType, out BinaryConverter converter))
                        return (reader, context) =>
                        {
                            return VersionableSerializer.DeserializeWithConverter(reader, converter, context, DeserializeVersionableObject);
                        };
                    else
                        return (reader, context) =>
                        {
                            return VersionableSerializer.Deserialize(reader, itemType, context, DeserializeVersionableObject);
                        };
                }
                else
                {
                    if (Options.Converters.Items.TryGetValue(itemType, out BinaryConverter converter))
                        return (reader, context) =>
                        {
                            return ClassSerializer.DeserializeWithConverter(reader, converter, context, DeserializeClassObject);
                        };
                    else
                        return (reader, context) =>
                        {
                            return ClassSerializer.Deserialize(reader, itemType, context, DeserializeClassObject);
                        };
                }
            }
            else
            {
                var converter = Options.Converters.Get(itemType);
                return (reader, context) =>
                {
                    return NonClassSerializer.Deserialize(reader, converter, context);
                };
            }
        }

        private static void Deserialize1DCollection(BinaryReader reader, ICollectionHandler handler, object collection, int[] shape, Func<BinaryReader, BinaryContext, object> method, BinaryContext context)
        {
            for (int i = 0; i < shape[0]; i++)
            {
                var value = method.Invoke(reader, context);
                handler.AddItem(collection, value, i);
            }
        }

        private static void Deserialize2DCollection(BinaryReader reader, ICollectionHandler handler, object collection, int[] shape, Func<BinaryReader, BinaryContext, object> method, BinaryContext context)
        {
            for (int i = 0; i < shape[0]; i++)
            {
                for(int j = 0; j < shape[1]; j++)
                {
                    var value = method.Invoke(reader, context);
                    handler.AddItem(collection, value, i, j);
                }
            }
        }

        private static void Deserialize3DCollection(BinaryReader reader, ICollectionHandler handler, object collection, int[] shape, Func<BinaryReader, BinaryContext, object> method, BinaryContext context)
        {
            for (int i = 0; i < shape[0]; i++)
            {
                for (int j = 0; j < shape[1]; j++)
                {
                    for(int k = 0; k < shape[2]; k++)
                    {
                        var value = method.Invoke(reader, context);
                        handler.AddItem(collection, value, i, j, k);
                    }
                }
            }
        }
    }
}
