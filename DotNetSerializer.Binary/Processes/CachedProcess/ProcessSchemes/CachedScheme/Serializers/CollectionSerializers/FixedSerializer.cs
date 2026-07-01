using DotNetSerializer.Base;
using DotNetSerializer.Base.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal abstract class FixedSerializer : CollectionSerializer
    {
        protected int[] _shape;

        protected FixedSerializer(PropertyInfo property, 
                                CollectionHandler collectionHandler, 
                                Type[] elementTypes,
                                IElementSerializer elementSerializer, 
                                int[] shape)
            : base(property, collectionHandler, elementTypes, elementSerializer)
        {
            if (shape.Length < Rank)
                throw new AttributeException($"Wrong collection shape for {Property?.Name ?? "Value"}. Require rank {Rank}, but was {shape.Length}.");

            _shape = shape;
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
