using DotNetSerializer.Base;
using DotNetSerializer.Base.Infrastructure;
using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal abstract class CollectionSerializer : PropertySerializer
    {
        protected CollectionHandler _collectionHandler;
        protected IElementSerializer _elementSerializer;
        protected Type[] _elementTypes;

        protected abstract int Rank { get; }

        public CollectionSerializer(PropertyInfo property, CollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer) : base(property)
        {
            _collectionHandler = collectionHandler;
            _elementTypes = elementTypes;
            _elementSerializer = elementSerializer;
        }

        protected IDisposable PrepContextScope(BinaryContext context, out BinaryContext prepContext)
        {
            var prep = _elementSerializer.PrepareContext(context);
            prepContext = prep;
            return DisposableScope.Create(() =>
            {
                _elementSerializer.FreeObjectContext(prep);
            });
        }

        protected object[] Deserialize1D(BinaryReader reader, int[] shape, BinaryContext context)
        {
            var items = new object[shape[0]];
            using (PrepContextScope(context, out BinaryContext prepContext))
            {
                for (int i = 0; i < shape[0]; i++)
                {
                    items[i] = _elementSerializer.DeserializeElement(reader, prepContext);
                }
            }
            return items;
        }
        protected object[,] Deserialize2D(BinaryReader reader, int[] shape, BinaryContext context)
        {
            var items = new object[shape[0], shape[1]];
            using (PrepContextScope(context, out BinaryContext prepContext))
            {
                for (int i = 0; i < shape[0]; i++)
                {
                    for (int j = 0; j < shape[1]; j++)
                    {
                        items[i, j] = _elementSerializer.DeserializeElement(reader, prepContext);
                    }
                }
            }
            return items;
        }
        protected object[,,] Deserialize3D(BinaryReader reader, int[] shape, BinaryContext context)
        {
            var items = new object[shape[0], shape[1], shape[2]];
            using (PrepContextScope(context, out BinaryContext prepContext))
            {
                for (int i = 0; i < shape[0]; i++)
                {
                    for (int j = 0; j < shape[1]; j++)
                    {
                        for (int k = 0; k < shape[2]; k++)
                        {
                            items[i, j, k] = _elementSerializer.DeserializeElement(reader, prepContext);
                        }
                    }
                }
            }
            return items;
        }

        protected void SerializeCollection(BinaryWriter writer, object collection, BinaryContext context)
        {
            using (PrepContextScope(context, out BinaryContext prepContext))
            {
                foreach (var item in (ICollection)collection)
                {
                    _elementSerializer.SerializeElement(writer, item, prepContext);
                }
            }
        }

    }
}
