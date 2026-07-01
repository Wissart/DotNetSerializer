using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes
{
    internal interface ISerializerProvider<TProcessScheme> where TProcessScheme : IProcessScheme
    {
        SchemeStorage<TProcessScheme> Schemes { get; }
        CollectionSerializer GetCollectionSerializer(Type declareCollectionType, Type[] elementTypes);
    }
}
