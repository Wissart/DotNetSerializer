using DotNetSerializer.Base;
using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.TypeSerializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers;
using System;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel
{
    internal class ParallelSerializerProvider : ISerializerProvider<ParallelProcessScheme>, ISchemeMaker
    {
        private readonly BinaryOptions _options;
        private readonly CachedSerializerProvider _cachedSerializerProvider;

        public SchemeStorage<ParallelProcessScheme> Schemes { get; }

        public ParallelSerializerProvider(BinaryOptions options,
            CachedSerializerProvider cachedSerializerProvider)
        {
            _options = options;
            _cachedSerializerProvider = cachedSerializerProvider;
            Schemes = new ParallelSchemeStorage(options, this, cachedSerializerProvider.Schemes);
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
            var property = context.ObjectContext.Property;
            var declareCollectionType = property.PropertyType;

            var attribute = AttributeUtilities.GetCollectionAttribute(property);
            if (attribute == null)
                attribute = _options.DefaultAttributes.Get<CollectionAttribute>();

            var sizeType = attribute.SizeType;

            var collectionType = CollectionUtilities.GetCollectionType(declareCollectionType);

            var handler = _options.CollectionHandlers.Get(collectionType);
            var rank = handler.GetRank(declareCollectionType);
            var elementType = handler.GetElementType(elementTypes);
            var elementSerializer = _cachedSerializerProvider.CreateElementSerializer(property, elementType);

            if (elementSerializer.TryGetSize(context, out int elementSize))
            {
                if (sizeType == CollectionSizeType.Fixed)
                {
                    var shape = attribute.Shape;
                    return CreateFixedParallelSerializer(property, handler, rank, shape, elementTypes, elementSerializer, elementSize);
                }
                else
                    return CreatePrefixParallelSerializer(property, handler, rank, elementTypes, elementSerializer, elementSize);
            }

            if (elementSerializer is ContainerSerializer containerSerializer)
            {
                containerSerializer.SerializationScheme = Schemes.Get(elementType);
            }

            if (sizeType == CollectionSizeType.Fixed)
            {
                var shape = attribute.Shape;
                return _cachedSerializerProvider.CreateFixedSerializer(property, handler, rank, shape, elementTypes, elementSerializer);
            }
            else
                return _cachedSerializerProvider.CreatePrefixSerializer(property, handler, rank, elementTypes, elementSerializer);
        }

        private FixedParallelSerializer CreateFixedParallelSerializer(PropertyInfo property,
            ICollectionHandler handler,
            int rank,
            int[] shape,
            Type[] elementTypes,
            IElementSerializer elementSerializer,
            int elementSize)
        {
            switch (rank)
            {
                case 1:
                    return new Fixed1DParallelSerializer(property, handler, elementTypes, elementSerializer, shape, elementSize);
                case 2:
                    return new Fixed2DParallelSerializer(property, handler, elementTypes, elementSerializer, shape, elementSize);
                case 3:
                    return new Fixed3DParallelSerializer(property, handler, elementTypes, elementSerializer, shape, elementSize);
                default:
                    throw new DotNetSerializerException($"Unsupported collection rank: {shape.Length}");
            }
        }

        private PrefixParallelSerializer CreatePrefixParallelSerializer(PropertyInfo property,
            ICollectionHandler handler,
            int rank,
            Type[] elementTypes,
            IElementSerializer elementSerializer,
            int elementSize)
        {
            switch (rank)
            {
                case 1:
                    return new Prefix1DParallelSerializer(property, handler, elementTypes, elementSerializer, elementSize);
                case 2:
                    return new Prefix2DParallelSerializer(property, handler, elementTypes, elementSerializer, elementSize);
                case 3:
                    return new Prefix3DParallelSerializer(property, handler, elementTypes, elementSerializer, elementSize);
                default:
                    throw new DotNetSerializerException($"Unsupported collection rank: {rank}");
            }
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
