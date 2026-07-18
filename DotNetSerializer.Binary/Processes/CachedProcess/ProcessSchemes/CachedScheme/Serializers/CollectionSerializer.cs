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
        protected ICollectionHandler _collectionHandler;
        protected IElementSerializer _elementSerializer;
        protected Type[] _elementTypes;

        protected abstract int Rank { get; }

        public CollectionSerializer(PropertyInfo property, ICollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer) : base(property)
        {
            _collectionHandler = collectionHandler;
            _elementTypes = elementTypes;
            _elementSerializer = elementSerializer;
        }

        protected IDisposable ElementContextScope(BinaryContext context, out BinaryContext prepContext)
        {
            var prep = _elementSerializer.ElementContext(context);
            prepContext = prep;
            return DisposableScope.Create(() =>
            {
                _elementSerializer.RemoveLastObjectContext(prep);
            });
        }

        protected object[] Deserialize1D(BinaryReader reader, int[] shape, BinaryContext context)
        {
            var items = new object[shape[0]];
            using (ElementContextScope(context, out BinaryContext elementContext))
            {
                for (int i = 0; i < shape[0]; i++)
                {
                    items[i] = _elementSerializer.DeserializeElement(reader, elementContext);
                }
            }
            return items;
        }

        protected object[,] Deserialize2D(BinaryReader reader, int[] shape, BinaryContext context)
        {
            var items = new object[shape[0], shape[1]];
            using (ElementContextScope(context, out BinaryContext elementContext))
            {
                for (int i = 0; i < shape[0]; i++)
                {
                    for (int j = 0; j < shape[1]; j++)
                    {
                        items[i, j] = _elementSerializer.DeserializeElement(reader, elementContext);
                    }
                }
            }
            return items;
        }
        protected object[,,] Deserialize3D(BinaryReader reader, int[] shape, BinaryContext context)
        {
            var items = new object[shape[0], shape[1], shape[2]];
            using (ElementContextScope(context, out BinaryContext elementContext))
            {
                for (int i = 0; i < shape[0]; i++)
                {
                    for (int j = 0; j < shape[1]; j++)
                    {
                        for (int k = 0; k < shape[2]; k++)
                        {
                            items[i, j, k] = _elementSerializer.DeserializeElement(reader, elementContext);
                        }
                    }
                }
            }
            return items;
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

    }
}
