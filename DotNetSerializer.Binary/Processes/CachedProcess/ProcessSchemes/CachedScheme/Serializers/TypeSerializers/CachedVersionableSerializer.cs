using DotNetSerializer.Binary.Processes.Common.Base;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers
{
    internal class CachedVersionableSerializer : ContainerSerializer
    {
        public CachedVersionableSerializer(PropertyInfo property, IProcessScheme serializationScheme) 
            : base(property, serializationScheme)
        {
        }

        public override BinaryContext ElementContext(BinaryContext context)
        {
            var newMetaData = new BinaryContext(context);
            newMetaData.CreateMetaData(null);
            return newMetaData;
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            return VersionableSerializer.Deserialize(reader, _serializationScheme.TypeInfo.Type, context, DeserializeVersionableObject);
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var obj = Activator.CreateInstance(_serializationScheme.TypeInfo.Type);

            DeserializeElement(reader, obj, context);

            return obj;
        }

        protected void DeserializeElement(BinaryReader reader, object obj, BinaryContext context)
        {
            context.MetaData.Object = obj;
            DeserializeVersionableObject(reader, context);
        }

        protected void DeserializeVersionableObject(BinaryReader reader, BinaryContext context)
        {
            foreach (var serializer in _serializationScheme.GetVersionSerializers())
            {
                context.MetaData.Property = serializer.Property;
                var value = serializer.Deserialize(reader, context);
                //serializer.Property.SetValue(context.ObjectData.Object, value);
                context.MetaData.SetValue(value);
            }

            var versionProperty = _serializationScheme.TypeInfo.GetVersionProperty();
            var version = (uint)versionProperty.GetValue(context.MetaData.Object);

            context.Version = version;

            foreach (var serializer in _serializationScheme.GetSerializers(version))
            {
                context.MetaData.Property = serializer.Property;
                var value = serializer.Deserialize(reader, context);
                //serializer.Property.SetValue(context.ObjectData.Object, value);
                context.MetaData.SetValue(value);
            }
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            VersionableSerializer.Serialize(writer, value, context, SerializeVersionableObject);
        }

        public override void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            context.MetaData.Object = element;
            SerializeVersionableObject(writer, context);
        }

        protected void SerializeVersionableObject(BinaryWriter writer, BinaryContext context)
        {
            var versionProperty = _serializationScheme.TypeInfo.GetVersionProperty();
            var version = (uint)versionProperty.GetValue(context.MetaData.Object);

            context.Version = version;

            foreach (var serializer in _serializationScheme.GetVersionSerializers())
            {
                context.MetaData.Property = serializer.Property;
                var value = context.MetaData.GetValue();
                serializer.Serialize(writer, value, context);
            }

            foreach (var serializer in _serializationScheme.GetSerializers(version))
            {
                context.MetaData.Property = serializer.Property;
                var value = context.MetaData.GetValue();
                serializer.Serialize(writer, value, context);
            }
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }
}
