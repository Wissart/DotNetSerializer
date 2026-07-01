using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme;
using System;
using System.IO;

namespace DotNetSerializer.Binary.Processes.CachedProcess
{
    internal class CachedSerializationProcess : SerializationProcessBase
    {
        public ISerializerProvider<CachedProcessScheme> SerializerProvider { get; }

        public CachedSerializationProcess(BinaryOptions options, ISerializerProvider<CachedProcessScheme> serializerProvider) 
            : base(options)
        {
            SerializerProvider = serializerProvider;
        }


        protected override void SerializeCollection(BinaryWriter writer, object value, Type[] elementTypes, BinaryContext context)
        {
            SerializerProvider.GetCollectionSerializer(value.GetType(), elementTypes).Serialize(writer, value, context);
        }

        protected override void SerializePropertyCollection(BinaryWriter writer, object collection, Type[] elementTypes, BinaryContext context)
        {
            var scheme = SerializerProvider.Schemes.Get(context.ObjectContext.Object.GetType());
            var serializer = scheme.GetCollectionSerializer(elementTypes, context);
            serializer.Serialize(writer, collection, context);
        }
    }
}
