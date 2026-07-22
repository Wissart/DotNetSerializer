using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.TypeSerializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers;
using System;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel
{
    internal class ParallelSerializerProvider : ISerializerProvider<ParallelProcessScheme>, ISchemeMaker
    {
        private readonly BinaryConfiguration _configuration;
        private readonly CachedSerializerProvider _cachedSerializerProvider;

        public SchemeStorage<ParallelProcessScheme> Schemes { get; }

        public ParallelSerializerProvider(BinaryConfiguration configuration,
            CachedSerializerProvider cachedSerializerProvider)
        {
            _configuration = configuration;
            _cachedSerializerProvider = cachedSerializerProvider;
            Schemes = new ParallelSchemeStorage(configuration.TypeInfoStorage, this, cachedSerializerProvider.Schemes);
        }

        public CollectionSerializer GetCollectionSerializer(Type declareCollectionType, Type[] elementTypes)
        {
            return _cachedSerializerProvider.GetCollectionSerializer(declareCollectionType, elementTypes);
        }

        public PropertySerializer GetSerializerByProperty(PropertyInfo property, BinaryContext context)
        {
            if (CollectionUtilities.TryGetElementTypes(property.PropertyType, out Type[] elementTypes))
            {
                return CreateCollectionSerializerByProperty(elementTypes, context);
            }
            else
            {
                return CreateTypeSerializer(property, context);
            }
        }

        public CollectionSerializer GetCollectionSerializerByProperty(Type[] elementTypes, BinaryContext context)
        {
            return CreateCollectionSerializerByProperty(elementTypes, context);
        }

        private CollectionSerializer CreateCollectionSerializerByProperty(Type[] elementTypes, BinaryContext context)
        {
            var property = context.MetaData.Property;
            var declareCollectionType = property.PropertyType;

            var attribute = AttributeUtilities.GetCollectionAttribute(property);
            if (attribute == null)
                attribute = _configuration.Options.DefaultAttributes.Get<CollectionAttribute>();

            var sizeType = attribute.SizeType;

            var collectionType = CollectionUtilities.GetCollectionType(declareCollectionType);

            var handler = _configuration.Options.CollectionHandlers.Get(collectionType);
            var rank = handler.GetRank(declareCollectionType);
            var elementType = handler.GetElementType(elementTypes);
            var elementSerializer = _cachedSerializerProvider.CreateElementSerializer(property, elementType);

            if (elementSerializer.TryGetSize(context, out int elementSize))
            {
                if (sizeType == CollectionSizeType.Fixed)
                {
                    var shape = attribute.Shape;
                    return new FixedParallelSerializer(property, handler, rank, elementTypes, elementSerializer, shape, elementSize);
                }
                else
                    return new PrefixParallelSerializer(property, handler, rank, elementTypes, elementSerializer, elementSize);
            }

            if (elementSerializer is ContainerSerializer containerSerializer)
            {
                containerSerializer.SerializationScheme = Schemes.Get(elementType);
            }

            if (sizeType == CollectionSizeType.Fixed)
            {
                var shape = attribute.Shape;
                return new FixedSerializer(property, handler, rank, elementTypes, elementSerializer, shape);
            }
            else
                return new PrefixSerializer(property, handler, rank, elementTypes, elementSerializer);
        }

        private PropertySerializer CreateTypeSerializer(PropertyInfo property, BinaryContext context)
        {
            var cachedSerializer = _cachedSerializerProvider.CreateTypeSerializer(property, property.PropertyType);

            if (!(cachedSerializer is ContainerSerializer containerSerializer))
                return cachedSerializer;

            if (containerSerializer is CachedVersionableSerializer versionableSerializer)
                versionableSerializer.SerializationScheme = Schemes.Get(property.PropertyType);

            if (!cachedSerializer.TryGetSize(context, out int size) || size < ParallelProcessPool.PARALLELABLE_MIN_DATA_SIZE)
                return cachedSerializer;

            if (containerSerializer is CachedClassSerializer classSerializer)
            {
                if (classSerializer is CachedClassSerializerWithConverter withConverter)
                    return new ClassParallelSerializerWithConverter(property, classSerializer.SerializationScheme, size, withConverter.Converter);

                return new ClassParallelSerializer(property, classSerializer.SerializationScheme, size);
            }

            return cachedSerializer;
        }
    }
}
