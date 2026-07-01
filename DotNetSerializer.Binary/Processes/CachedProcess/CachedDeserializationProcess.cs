using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme;
using System;
using System.IO;
using DotNetSerializer.Binary.Processes.Common.Base;

namespace DotNetSerializer.Binary.Processes.CachedProcess
{
    internal class CachedDeserializationProcess : DeserializationProcessBase
    {
        public ISerializerProvider<CachedProcessScheme> SerializerProvider { get; }

        public CachedDeserializationProcess(BinaryOptions options, ISerializerProvider<CachedProcessScheme> serializerProvider) 
            : base(options)
        {
            SerializerProvider = serializerProvider;
        }


        protected override object Deserialize(BinaryReader reader, Type type, BinaryContext context)
        {
            if (CollectionUtilities.TryGetElementTypes(type, out Type[] elementTypes))
            {
                return DeserializeCollection(reader, type, elementTypes, context);
            }
            else
            {
                if (Options.CachedProcessSettings.CachingTargets == CachingTargets.All)
                    return DeserializeCachedValue(reader, type, type, context);
                else
                    return DeserializeValue(reader, type, type, context);
            }
        }

        protected override object DeserializeCollection(BinaryReader reader, Type type, Type[] elementTypes, BinaryContext context)
        {
            var serializer = SerializerProvider.GetCollectionSerializer(type, elementTypes);
            var collection = serializer.Deserialize(reader, context);

            return collection;
        }

        protected override object DeserializePropertyCollection(BinaryReader reader, Type propertyType, Type[] elementTypes, BinaryContext context)
        {
            var scheme = SerializerProvider.Schemes.Get(context.ObjectContext.Object.GetType());
            var serializer = scheme.GetCollectionSerializer(elementTypes, context);
            var collection = serializer.Deserialize(reader, context);

            return collection;
        }

        private object DeserializeCachedValue(BinaryReader reader, Type valueType, Type converterType, BinaryContext context)
        {
            if (SerializationUtilities.IsClass(valueType))
            {
                var scheme = SerializerProvider.Schemes.Get(valueType);
                return scheme.Deserialize(reader, context);
            }
            else
            {
                var converter = Options.Converters.Get(converterType);
                return NonClassSerializer.Deserialize(reader, converter, context);
            }
        }
    }
}
