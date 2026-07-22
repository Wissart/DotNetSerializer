using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers
{
    internal class PrefixParallelSerializer : ParallelCollectionSerializer
    {
        public PrefixParallelSerializer(PropertyInfo property, 
            ICollectionHandler collectionHandler, 
            int rank,
            Type[] elementTypes, 
            IElementSerializer elementSerializer,
            int elementSize) 
            : base(property, collectionHandler, rank, elementTypes, elementSerializer, elementSize)
        {
        }

        protected override int[] GetShape(BinaryReader reader)
        {
            var _shape = new int[_rank];

            for (int i = 0; i < _rank; i++)
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
            WriteShape(writer, value);

            SerializeCollection(writer, value, context);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }
}
