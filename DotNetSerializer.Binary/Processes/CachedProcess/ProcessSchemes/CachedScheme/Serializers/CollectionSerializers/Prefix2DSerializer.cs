using DotNetSerializer.Base;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal class Prefix2DSerializer : PrefixSerializer
    {
        protected override int Rank => 2;

        public Prefix2DSerializer(PropertyInfo property, CollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer) 
            : base(property, collectionHandler, elementTypes, elementSerializer)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);
            var items = Deserialize2D(reader, shape, context);

            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, shape);

            return collection;
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);
            var items = new object[shape[0], shape[1]];
            for (int i = 0; i < shape[0]; i++)
            {
                for (int j = 0; j < shape[1]; j++)
                {
                    items[i, j] = _elementSerializer.DeserializeElement(reader, context);
                }
            }
            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, shape);

            return collection;
        }
    }
}
