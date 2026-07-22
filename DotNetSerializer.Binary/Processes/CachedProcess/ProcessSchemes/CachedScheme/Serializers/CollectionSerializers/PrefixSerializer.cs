using DotNetSerializer.Base.CollectionHandlers;
using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal class PrefixSerializer : CollectionSerializer, IElementSerializer
    {
        public PrefixSerializer(PropertyInfo property, 
            ICollectionHandler collectionHandler, 
            int rank, Type[] elementTypes, 
            IElementSerializer elementSerializer) 
            : base(property, collectionHandler, rank, elementTypes, elementSerializer)
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

        public BinaryContext ElementContext(BinaryContext context) => context;
        public void RemoveLastObjectContext(BinaryContext context) { }

        public object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var shape = GetShape(reader);

            var elements = _deserializeCollection(reader, shape, _elementSerializer.DeserializeElement, context);
            var collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, elements, shape);
            return collection;
        }

        public void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            WriteShape(writer, element);

            foreach (var item in (ICollection)element)
            {
                _elementSerializer.SerializeElement(writer, item, context);
            }
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }

    }
}
