using DotNetSerializer.Base.CollectionHandlers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal class Prefix1DSerializer : PrefixSerializer
    {
        protected override int Rank => 1;

        public Prefix1DSerializer(PropertyInfo property, ICollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer)
            : base(property, collectionHandler, elementTypes, elementSerializer)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);

            var items = Deserialize1D(reader, shape, context);

            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, shape);

            return collection;
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);
            var items = new object[shape[0]];
            for (int i = 0; i < shape[0]; i++)
            {
                items[i] = _elementSerializer.DeserializeElement(reader, context);
            }
            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items, shape);

            return collection;
        }
    }
}
