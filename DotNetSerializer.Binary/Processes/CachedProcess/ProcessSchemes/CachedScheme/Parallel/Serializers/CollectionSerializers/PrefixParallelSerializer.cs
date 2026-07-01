using DotNetSerializer.Base;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers
{
    internal abstract class PrefixParallelSerializer : CollectionSerializer
    {
        protected readonly int _elementSize;
        protected PrefixParallelSerializer(PropertyInfo property, 
            CollectionHandler collectionHandler, 
            Type[] elementTypes, 
            IElementSerializer elementSerializer,
            int elementSize) 
            : base(property, collectionHandler, elementTypes, elementSerializer)
        {
            _elementSize = elementSize;
        }

        protected int[] ReadShape(BinaryReader reader)
        {
            var _shape = new int[Rank];

            for (int i = 0; i < Rank; i++)
            {
                _shape[i] = reader.ReadInt32();
            }

            return _shape;
        }

        protected void WriteShape(BinaryWriter writer, object collection)
        {
            var itemsCount = _collectionHandler.GetItemsCount(collection);

            for (int i = 0; i < itemsCount.Length; i++)
            {
                writer.Write(itemsCount[i]);
            }
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            var collection = value;

            WriteShape(writer, collection);

            SerializeCollection(writer, collection, context);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }
}
