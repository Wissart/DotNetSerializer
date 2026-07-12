using DotNetSerializer.Base.Storages;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.Common.Base;
using System;
using System.IO;

namespace DotNetSerializer.Binary.Processes.CachedProcess.Parallel
{
    internal class ParallelDeserializationProcess : DeserializationProcessBase
    {
        public ISerializerProvider<ParallelProcessScheme> SerializerProvider { get; }

        public ParallelDeserializationProcess(BinaryConfiguration configuration, ISerializerProvider<ParallelProcessScheme> serializerProvider) 
            : base(configuration)
        {
            SerializerProvider = serializerProvider;
        }


        protected override T Deserialize<T>(BinaryReader reader)
        {
            var context = new BinaryContext(null, ParallelProcessPool.CreateProcess());
            var result = (T)Deserialize(reader, typeof(T), context);

            ParallelProcessPool.WaitProcess(context.ProcessID);

            return result;
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

        protected override object DeserializePropertyCollection(BinaryReader reader, Type propertyType, Type[] elementTypes, BinaryContext context)
        {
            var scheme = SerializerProvider.Schemes.Get(context.ObjectContext.Object.GetType());
            var serializer = scheme.GetCollectionSerializer(elementTypes, context);

            var collection = serializer.Deserialize(reader, context);
            return collection;
        }
    }
}
