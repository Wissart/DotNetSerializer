using DotNetSerializer.Base;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal class Fixed2DSerializer : FixedSerializer
    {
        protected override int Rank => 2;

        public Fixed2DSerializer(PropertyInfo property, CollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer, int[] shape) 
            : base(property, collectionHandler, elementTypes, elementSerializer, shape)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var items = Deserialize2D(reader, _shape, context);

            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, _shape);

            return collection;
        }
    }
}
