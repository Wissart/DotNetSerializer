using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Infrastructure;
using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal abstract class CollectionSerializer : PropertySerializer
    {
        protected delegate object CollectionDeserializeFunc(BinaryReader reader, int[] shape, Func<BinaryReader, BinaryContext, object> elementDeserializeFunc, BinaryContext context);

        private static readonly CollectionDeserializeFunc[] _deserializeFuncs;

        protected ICollectionHandler _collectionHandler;
        protected IElementSerializer _elementSerializer;
        protected int _rank;
        protected Type[] _elementTypes;
        protected CollectionDeserializeFunc _deserializeCollection;

        static CollectionSerializer()
        {
            _deserializeFuncs = new CollectionDeserializeFunc[3]
            {
                Deserialize1D,
                Deserialize2D,
                Deserialize3D,
            };
        }

        public CollectionSerializer(PropertyInfo property, 
            ICollectionHandler collectionHandler, 
            int rank, 
            Type[] elementTypes, 
            IElementSerializer elementSerializer) 
            : base(property)
        {
            _collectionHandler = collectionHandler;
            _rank = rank;
            _elementTypes = elementTypes;
            _elementSerializer = elementSerializer;

            _deserializeCollection = _deserializeFuncs[rank - 1];
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var shape = GetShape(reader);

            return DeserializeCollection(reader, shape, context);
        }

        protected abstract int[] GetShape(BinaryReader reader);

        protected IDisposable ElementContextScope(BinaryContext context, out BinaryContext prepContext)
        {
            var prep = _elementSerializer.ElementContext(context);
            prepContext = prep;
            return DisposableScope.Create(() =>
            {
                _elementSerializer.RemoveLastObjectContext(prep);
            });
        }

        protected virtual object DeserializeCollection(BinaryReader reader, int[] shape, BinaryContext context)
        {
            using (ElementContextScope(context, out BinaryContext elementContext))
            {
                var elements = _deserializeCollection(reader, shape, _elementSerializer.DeserializeElement, elementContext);
                var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, elements, shape);
                return collection;
            }
        }

        protected void SerializeCollection(BinaryWriter writer, object collection, BinaryContext context)
        {
            using (ElementContextScope(context, out BinaryContext elementContext))
            {
                foreach (var item in (ICollection)collection)
                {
                    _elementSerializer.SerializeElement(writer, item, elementContext);
                }
            }
        }

        protected static object[] Deserialize1D(BinaryReader reader, int[] shape, Func<BinaryReader, BinaryContext, object> deserializeElement, BinaryContext elementContext)
        {
            var elements = new object[shape[0]];
            for (int i = 0; i < shape[0]; i++)
            {
                elements[i] = deserializeElement(reader, elementContext);
            }
            return elements;
        }

        protected static object[,] Deserialize2D(BinaryReader reader, int[] shape, Func<BinaryReader, BinaryContext, object> deserializeElement, BinaryContext elementContext)
        {
            var elements = new object[shape[0], shape[1]];
            for (int i = 0; i < shape[0]; i++)
            {
                for (int j = 0; j < shape[1]; j++)
                {
                    elements[i, j] = deserializeElement(reader, elementContext);
                }
            }
            return elements;
        }

        protected static object[,,] Deserialize3D(BinaryReader reader, int[] shape, Func<BinaryReader, BinaryContext, object> deserializeElement, BinaryContext elementContext)
        {
            var elements = new object[shape[0], shape[1], shape[2]];
            for (int i = 0; i < shape[0]; i++)
            {
                for (int j = 0; j < shape[1]; j++)
                {
                    for (int k = 0; k < shape[2]; k++)
                    {
                        elements[i, j, k] = deserializeElement(reader, elementContext);
                    }
                }
            }
            return elements;
        }
    }
}
