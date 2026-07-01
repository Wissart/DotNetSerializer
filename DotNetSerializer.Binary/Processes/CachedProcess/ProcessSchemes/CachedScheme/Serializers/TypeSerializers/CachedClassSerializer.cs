using DotNetSerializer.Binary.Processes.Common.Base;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers
{
    internal class CachedClassSerializer : ContainerSerializer
    {
        public CachedClassSerializer(PropertyInfo property, IProcessScheme serializationScheme) 
            : base(property, serializationScheme)
        {
        }


        public override BinaryContext PrepareContext(BinaryContext context)
        {
            context.CreateObjectContext(null);
            return context;
        }

        public override void FreeObjectContext(BinaryContext context)
        {
            context.RemoveObjectContext();
        }


        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            return ClassSerializer.Deserialize(reader, _serializationScheme.TypeInfo.Type, context, DeserializeClassObject);
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var obj = Activator.CreateInstance(_serializationScheme.TypeInfo.Type);

            context.ObjectContext.Object = obj;
            DeserializeClassObject(reader, context);

            return obj;
        }

        protected void DeserializeClassObject(BinaryReader reader, BinaryContext context)
        {
            foreach (var serializer in _serializationScheme.GetSerializers(context.Version))
            {
                context.ObjectContext.Property = serializer.Property;
                var value = serializer.Deserialize(reader, context);
                context.ObjectContext.SetValue(value);
            }
        }


        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            var obj = Property.GetValue(context.ObjectContext.Object);

            ClassSerializer.Serialize(writer, obj, context, SerializeClassObject);
        }

        public override void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            context.ObjectContext.Object = element;
            SerializeClassObject(writer, context);
        }

        protected void SerializeClassObject(BinaryWriter writer, BinaryContext context)
        {
            foreach (var serializer in _serializationScheme.GetSerializers(context.Version))
            {
                context.ObjectContext.Property = serializer.Property;
                var value = context.ObjectContext.GetValue();
                serializer.Serialize(writer, value, context);
            }
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            return _serializationScheme.TryGetSize(context, out size);
        }
    }
}
