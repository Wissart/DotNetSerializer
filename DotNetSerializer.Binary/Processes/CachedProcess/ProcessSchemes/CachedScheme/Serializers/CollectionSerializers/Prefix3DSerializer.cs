using DotNetSerializer.Base.CollectionHandlers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal class Prefix3DSerializer : PrefixSerializer
    {
        protected override int Rank => 3;

        public Prefix3DSerializer(PropertyInfo property, ICollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer) 
            : base(property, collectionHandler, elementTypes, elementSerializer)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);
            var items = Deserialize3D(reader, shape, context);

            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, shape);

            return collection;
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);
            var items = new object[shape[0], shape[1], shape[2]];
            for (int i = 0; i < shape[0]; i++)
            {
                for (int j = 0; j < shape[1]; j++)
                {
                    for (int k = 0; k < shape[2]; k++)
                    {
                        items[i, j, k] = _elementSerializer.DeserializeElement(reader, context);
                    }
                }
            }
            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, shape);

            return collection;
        }
    }
}
