using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes
{
    internal interface ISchemeMaker
    {
        CollectionSerializer GetCollectionSerializerByProperty(Type[] elementTypes, BinaryContext context);
        PropertySerializer GetSerializerByProperty(PropertyInfo property, BinaryContext context = null);
    }
}
