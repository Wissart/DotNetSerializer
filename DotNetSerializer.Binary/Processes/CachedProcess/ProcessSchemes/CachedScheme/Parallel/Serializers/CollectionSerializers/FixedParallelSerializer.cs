using DotNetSerializer.Base;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers
{
    internal abstract class FixedParallelSerializer : CollectionSerializer
    {
        protected readonly int[] _shape;
        protected readonly int _elementSize;

        protected FixedParallelSerializer(PropertyInfo property, 
            CollectionHandler collectionHandler, 
            Type[] elementTypes, 
            IElementSerializer elementSerializer, 
            int[] shape,
            int elementSize) 
            : base(property, collectionHandler, elementTypes, elementSerializer)
        {
            _shape = shape;
            _elementSize = elementSize;
        }

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
                for (int i = 0; i < Rank; i++)
                    size *= _shape[i];

                return true;
            }

            size = -1;
            return false;
        }
    }
}
