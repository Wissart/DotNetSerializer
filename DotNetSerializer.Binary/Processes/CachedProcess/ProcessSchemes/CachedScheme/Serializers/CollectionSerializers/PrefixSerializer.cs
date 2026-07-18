using DotNetSerializer.Base.CollectionHandlers;
using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.CollectionSerializers
{
    internal abstract class PrefixSerializer : CollectionSerializer, IElementSerializer
    {
        protected PrefixSerializer(PropertyInfo property, ICollectionHandler collectionHandler, Type[] elementTypes, IElementSerializer elementSerializer) 
            : base(property, collectionHandler, elementTypes, elementSerializer)
        {
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

        public virtual BinaryContext ElementContext(BinaryContext context) => context;
        public virtual void RemoveLastObjectContext(BinaryContext context) { }

        public abstract object DeserializeElement(BinaryReader reader, BinaryContext context);
        public void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            var collection = element;

            WriteShape(writer, collection);

            foreach (var item in (ICollection)collection)
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
