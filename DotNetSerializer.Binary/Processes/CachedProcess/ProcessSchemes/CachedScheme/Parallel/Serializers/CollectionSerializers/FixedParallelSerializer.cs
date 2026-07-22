using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers
{
    internal class FixedParallelSerializer : ParallelCollectionSerializer
    {
        protected readonly int[] _shape;

        public FixedParallelSerializer(PropertyInfo property, 
            ICollectionHandler collectionHandler,
            int rank,
            Type[] elementTypes, 
            IElementSerializer elementSerializer, 
            int[] shape,
            int elementSize) 
            : base(property, collectionHandler, rank, elementTypes, elementSerializer, elementSize)
        {
            _shape = shape;
        }

        protected override int[] GetShape(BinaryReader reader) => _shape;

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            var collection = value;

            SerializeCollection(writer, collection, context);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            if (_elementSerializer.TryGetSize(context, out int elementSize))
            {
                size = elementSize;
                for (int i = 0; i < _rank; i++)
                    size *= _shape[i];

                return true;
            }

            size = -1;
            return false;
        }
    }
}
