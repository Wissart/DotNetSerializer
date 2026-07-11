using DotNetSerializer.Base.CollectionHandlers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal class Fixed3DSerializer : FixedSerializer
    {
        protected override int Rank => 3;

        public Fixed3DSerializer(PropertyInfo property, ICollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer, int[] shape) 
            : base(property, collectionHandler, elementTypes, elementSerializer, shape)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var items = Deserialize3D(reader, _shape, context);

            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, _shape);

            return collection;
        }
    }
}
