using DotNetSerializer.Base;
using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Converters.Default;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers;
using System;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme
{
    internal class CachedSerializerProvider : ISerializerProvider<CachedProcessScheme>, ISchemeMaker
    {
        private readonly BinaryConfiguration _configuration;
        public SchemeStorage<CachedProcessScheme> Schemes { get; }

        public CachedSerializerProvider(BinaryConfiguration configuration)
        {
            _configuration = configuration;
            Schemes = new SchemeStorage<CachedProcessScheme>(configuration.TypeInfoStorage, this);
        }

        public CollectionSerializer GetCollectionSerializer(Type declareCollectionType, Type[] elementTypes)
        {
            return CreateCollectionSerializer(declareCollectionType, elementTypes);
        }

        private PrefixSerializer CreateCollectionSerializer(Type declareCollectionType, Type[] elementTypes)
        {
            var collectionType = CollectionUtilities.GetCollectionType(declareCollectionType);

            var handler = _configuration.Options.CollectionHandlers.Get(collectionType);
            var rank = handler.GetRank(declareCollectionType);
            var elementType = handler.GetElementType(elementTypes);
            var elementSerializer = CreateElementSerializer(elementType);

            return new PrefixSerializer(null, handler, rank, elementTypes, elementSerializer);
        }

        private IElementSerializer CreateElementSerializer(Type valueType)
        {
            if (CollectionUtilities.TryGetElementTypes(valueType, out Type[] elementTypes))
            {
                return CreateCollectionSerializer(valueType, elementTypes);
            }
            else
            {
                return CreateTypeSerializer(null, valueType, valueType);
            }
        }



        public PropertySerializer GetSerializerByProperty(PropertyInfo property, BinaryContext context = null)
        {
            if (CollectionUtilities.TryGetElementTypes(property.PropertyType, out Type[] elementTypes))
            {
                return CreateCollectionSerializerByProperty(property, property.PropertyType, elementTypes);
            }
            else
            {
                return CreateTypeSerializer(property, property.PropertyType);
            }
        }

        public CollectionSerializer GetCollectionSerializerByProperty(Type[] elementTypes, BinaryContext context)
        {
            var property = context.MetaData.Property;
            var declareCollectionType = property.PropertyType;

            return CreateCollectionSerializerByProperty(property, declareCollectionType, elementTypes);
        }

        private CollectionSerializer CreateCollectionSerializerByProperty(PropertyInfo property, Type declareCollectionType, Type[] elementTypes)
        {
            var attribute = AttributeUtilities.GetCollectionAttribute(property);
            if (attribute == null)
                attribute = _configuration.Options.DefaultAttributes.Get<CollectionAttribute>();

            var sizeType = attribute.SizeType;

            var collectionType = CollectionUtilities.GetCollectionType(declareCollectionType);

            var handler = _configuration.Options.CollectionHandlers.Get(collectionType);
            var rank = handler.GetRank(declareCollectionType);
            var elementType = handler.GetElementType(elementTypes);
            var elementSerializer = CreateElementSerializer(property, elementType);

            if (sizeType == CollectionSizeType.Fixed)
            {
                var shape = attribute.Shape;
                return new FixedSerializer(property, handler, rank, elementTypes, elementSerializer, shape);
            }
            else
                return new PrefixSerializer(property, handler, rank, elementTypes, elementSerializer);
        }

        public IElementSerializer CreateElementSerializer(PropertyInfo property, Type valueType)
        {
            if (CollectionUtilities.TryGetElementTypes(valueType, out Type[] elementTypes))
            {
                return CreateNestedSerializer(property, valueType, elementTypes);
            }
            else
            {
                return CreateTypeSerializer(property, valueType);
            }
        }

        private IElementSerializer CreateNestedSerializer(PropertyInfo property, Type declareCollectionType, Type[] elementTypes)
        {
            var collectionType = CollectionUtilities.GetCollectionType(declareCollectionType);

            var handler = _configuration.Options.CollectionHandlers.Get(collectionType);
            var rank = handler.GetRank(declareCollectionType);
            var elementType = handler.GetElementType(elementTypes);
            var elementSerializer = CreateElementSerializer(property, elementType);

            return new PrefixSerializer(property, handler, rank, elementTypes, elementSerializer);
        }

        public TypeSerializer CreateTypeSerializer(PropertyInfo property, Type valueType)
        {
            Type converterType = valueType;
            var converterAttribute = AttributeUtilities.GetConverterAttribute(property);
            if (converterAttribute != null)
            {
                converterType = converterAttribute.ConverterType;
                if (!_configuration.Options.Converters.Contains(converterType))
                    throw new ConverterNotFoundException(converterType);
            }

            return CreateTypeSerializer(property, valueType, converterType);
        }

        private TypeSerializer CreateTypeSerializer(PropertyInfo property, Type valueType, Type converterType)
        {
            if (SerializationUtilities.IsClass(valueType))
            {
                var scheme = Schemes.Get(valueType);

                if (_configuration.TypeInfoStorage.Get(valueType).IsVersionable)
                {
                    if (_configuration.Options.Converters.Items.TryGetValue(converterType, out BinaryConverter converter))
                        return new CachedVersionableSerializerWithConverter(property, scheme, converter);
                    else
                        return new CachedVersionableSerializer(property, scheme);
                }
                else
                {
                    if (_configuration.Options.Converters.Items.TryGetValue(converterType, out BinaryConverter converter))
                        return new CachedClassSerializerWithConverter(property, scheme, converter);
                    else
                        return new CachedClassSerializer(property, scheme);
                }
            }
            else
            {
                var converter = _configuration.Options.Converters.Get(converterType);
                return CreateNonClassSerializer(property, converter);
            }
        }

        private TypeSerializer CreateNonClassSerializer(PropertyInfo property, BinaryConverter converter)
        {
            if (converter.RegisteredType == typeof(string))
            {
                StringFormatAttribute attriute;
                if (property == null)
                    attriute = _configuration.Options.DefaultAttributes.Get<StringFormatAttribute>();
                else
                {
                    attriute = AttributeUtilities.GetStringFormatAttribute(property);
                    if (attriute == null)
                        attriute = _configuration.Options.DefaultAttributes.Get<StringFormatAttribute>();
                }
                    

                return CreateStringSerializer(property, (StringConverter)converter, attriute);
            }
            else
                return new CachedNonClassSerializer(property, converter);
        }

        private StringSerializer CreateStringSerializer(PropertyInfo property, StringConverter converter, StringFormatAttribute attribute)
        {
            switch (attribute.SizeType)
            {
                case StringSizeType.Fixed:
                    return new CachedFixedStringSerializer(property, converter, attribute.EncodingName, attribute.Size);
                case StringSizeType.Prefix:
                    return new CachedPrefixStringSerializer(property, converter, attribute.EncodingName);
                case StringSizeType.SignEnd:
                    return new CachedSignEndStringSerializer(property, converter, attribute.EncodingName);
                default:
                    throw new DotNetSerializerException($"Unknown string size type: {attribute.SizeType}");
            }
        }
    }
}
