using DotNetSerializer.Base.Storages;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel;
using System;
using System.IO;

namespace DotNetSerializer.Binary.Processes.CachedProcess.Parallel
{
    internal class ParallelSerializationProcess : SerializationProcessBase
    {
        public ISerializerProvider<ParallelProcessScheme> SerializerProvider { get; }

        public ParallelSerializationProcess(BinaryConfiguration configuration, ISerializerProvider<ParallelProcessScheme> serializerProvider) 
            : base(configuration)
        {
            SerializerProvider = serializerProvider;
        }


        protected override void SerializeCollection(BinaryWriter writer, object value, Type[] elementTypes, BinaryContext context)
        {
            SerializerProvider.GetCollectionSerializer(value.GetType(), elementTypes).Serialize(writer, value, context);
        }

        protected override void SerializePropertyCollection(BinaryWriter writer, object collection, Type[] elementTypes, BinaryContext context)
        {
            var scheme = SerializerProvider.Schemes.Get(context.MetaData.Object.GetType());
            var serializer = scheme.GetCollectionSerializer(elementTypes, context);
            serializer.Serialize(writer, collection, context);
        }
    }
}
