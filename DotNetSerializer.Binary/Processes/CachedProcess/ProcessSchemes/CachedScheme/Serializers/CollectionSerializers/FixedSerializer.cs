using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal class FixedSerializer : CollectionSerializer
    {
        protected int[] _shape;


        public FixedSerializer(PropertyInfo property,
            ICollectionHandler collectionHandler,
            int rank,
            Type[] elementTypes,
            IElementSerializer elementSerializer, 
            int[] shape)
            : base(property, collectionHandler, rank, elementTypes, elementSerializer)
        {
            if (shape.Length != _rank)
                throw new AttributeException($"Wrong collection shape for {Property?.Name ?? "Value"}. Require rank {_rank}, but was {shape.Length}.");

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
