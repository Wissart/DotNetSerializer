using DotNetSerializer.Base.Reflection;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes
{
    internal interface IProcessScheme
    {
        SerializableTypeInfo TypeInfo { get; }

        PropertySerializer GetCollectionSerializer(Type[] elementTypes, BinaryContext context);
        IEnumerable<PropertySerializer> GetVersionSerializers();
        IEnumerable<PropertySerializer> GetSerializers(uint version = 0);
        bool TryGetSize(BinaryContext context, out int size);

        object Deserialize(BinaryReader reader, BinaryContext context);
    }
}
