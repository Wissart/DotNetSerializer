using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.Common.Base;
using System;
using System.Collections;
using System.IO;

namespace DotNetSerializer.Binary.Processes.DefaultProcess
{
    internal class DefaultSerializationProcess : SerializationProcessBase
    {
        public DefaultSerializationProcess(BinaryOptions options) 
            : base(options)
        {
        }

        protected override void SerializeCollection(BinaryWriter writer, object value, Type[] elementTypes, BinaryContext context)
        {
            SerializeCollection(writer, value, CollectionSizeType.Prefix, elementTypes, context);
        }

        protected override void SerializePropertyCollection(BinaryWriter writer, object collection, Type[] elementTypes, BinaryContext context)
        {
            var attribute = AttributeUtilities.GetCollectionAttribute(context.ObjectContext.Property);

            if (attribute == null)
                attribute = Options.DefaultAttributes.Get<CollectionAttribute>();

            SerializeCollection(writer, collection, attribute.SizeType, elementTypes, context);
        }

        private void SerializeCollection(BinaryWriter writer, object collection, CollectionSizeType sizeType, Type[] elementTypes, BinaryContext context)
        {
            var collectionType = CollectionUtilities.GetCollectionType(collection.GetType());

            var handler = Options.CollectionHandlers.Get(collectionType);
            var elementType = handler.GetElementType(elementTypes);

            if(sizeType == CollectionSizeType.Prefix)
            {
                var shape = handler.GetItemsCount(collection);
                for (int i = 0; i < shape.Length; i++)
                {
                    writer.Write(shape[i]);
                }
            }

            var serializationMethod = CreateCollectionItemDeserializationMethod(elementType);

            foreach(var item in (ICollection)collection)
            {
                serializationMethod(writer, item, context);
            }

        }

        private Action<BinaryWriter, object, BinaryContext> CreateCollectionItemDeserializationMethod(Type valueType)
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

        private Action<BinaryWriter, object, BinaryContext> CreateNestedDeserializationMethod(Type declareCollectionType, Type[] elementTypes)
        {
            return (writer, item, context) =>
            {
                SerializeCollection(writer, item, CollectionSizeType.Prefix, elementTypes, context);
            };
        }

        private Action<BinaryWriter, object, BinaryContext> CreateTypeDeserializationMethod(Type itemType)
        {
            if (SerializationUtilities.IsClass(itemType))
            {
                if (Options.TypeInfoStorage.Get(itemType).IsVersionable)
                {
                    if (Options.Converters.Items.TryGetValue(itemType, out BinaryConverter converter))
                        return (writer, item, context) =>
                        {
                            VersionableSerializer.SerializeWithConverter(writer, converter, item, context, SerializeVersionableObject);
                        };
                    else
                        return (writer, item, context) =>
                        {
                            VersionableSerializer.Serialize(writer, item, context, SerializeVersionableObject);
                        };
                }
                else
                {
                    if (Options.Converters.Items.TryGetValue(itemType, out BinaryConverter converter))
                        return (writer, item, context) =>
                        {
                            ClassSerializer.SerializeWithConverter(writer, converter, item, context, SerializeClassObject);
                        };
                    else
                        return (writer, item, context) =>
                        {
                            ClassSerializer.Serialize(writer, item, context, SerializeClassObject);
                        };
                }
            }
            else
            {
                var converter = Options.Converters.Get(itemType);
                return (writer, item, context) =>
                {
                    NonClassSerializer.Serialize(writer, converter, item, context);
                };
            }
        }
    }
}
